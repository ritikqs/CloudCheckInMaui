using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using CloudCheckInMaui.ConstantHelper;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudCheckInMaui.Services.FaceService
{
    public class FaceRecognitionService : IFaceRecognitionService
    {
        const string _personGroupId = "persongroupid";
        const string _personGroupName = "CloudCheckIn";

        private static readonly Lazy<FaceClient> _faceApiClientHolder = new Lazy<FaceClient>(() =>
            new FaceClient(
                new ApiKeyServiceClientCredentials(Constants.FacialRecognitionAPIKey),
                new HttpClient(),
                false)
            { Endpoint = Constants.FaceApiBaseUrl });

        private static FaceClient FaceApiClient => _faceApiClientHolder.Value;

        public async Task<bool> IsConfigurationValid()
        {
            try
            {
                await FaceApiClient.PersonGroup.ListAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Face API config error: {ex.Message}");
                return false;
            }
        }

        public async Task DeleteGroup()
        {
            try
            {
                await FaceApiClient.PersonGroup.DeleteAsync(_personGroupId).ConfigureAwait(false);
                var trainingStatus = await TrainPersonGroup(_personGroupId);
                if (trainingStatus.Status == TrainingStatusType.Failed)
                    throw new Exception(trainingStatus.Message);
            }
            catch (APIErrorException e) when (e.Response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                Debug.WriteLine("Person Group does not exist");
            }
        }

        public async Task RemoveExistingFace(Guid userId)
        {
            try
            {
                await FaceApiClient.PersonGroupPerson.DeleteAsync(_personGroupId, userId).ConfigureAwait(false);
            }
            catch (APIErrorException e) when (e.Response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                Debug.WriteLine("Person does not exist");
            }
        }

        public async Task<bool> IsFaceExistForEmail(string email)
        {
            try
            {
                var personGroupList = await FaceApiClient.PersonGroupPerson.ListAsync(_personGroupId).ConfigureAwait(false);
                return personGroupList.Any(x => x.Name.Equals(email, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking face existence: {ex.Message}");
                return false;
            }
        }

        public async Task<string> AddNewFace(string username, Stream photo)
        {
            try
            {
                await CreatePersonGroup();
                var createPersonResult = await FaceApiClient.PersonGroupPerson.CreateAsync(_personGroupId, username).ConfigureAwait(false);
                await FaceApiClient.PersonGroupPerson.AddFaceFromStreamAsync(_personGroupId, createPersonResult.PersonId, photo).ConfigureAwait(false);

                var trainingStatus = await TrainPersonGroup(_personGroupId);
                if (trainingStatus.Status == TrainingStatusType.Failed)
                {
                    Debug.WriteLine($"Training failed: {trainingStatus.Message}");
                    return null;
                }

                Debug.WriteLine($"Added new face for user '{username}' with PersonId: {createPersonResult.PersonId}");
                return createPersonResult.PersonId.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddNewFace error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsFaceIdentified(string username, Stream photo)
        {
            try
            {
                Debug.WriteLine($"Starting face identification for user: {username}");

                if (photo.CanSeek) photo.Position = 0;

                var trainingStatus = await FaceApiClient.PersonGroup.GetTrainingStatusAsync(_personGroupId);
                if (trainingStatus.Status != TrainingStatusType.Succeeded)
                {
                    App.FaceMathError = $"Person group training status: {trainingStatus.Status}";
                    Debug.WriteLine(App.FaceMathError);
                    return false;
                }

                var personGroupListTask = FaceApiClient.PersonGroupPerson.ListAsync(_personGroupId);

                var facesDetected = await FaceApiClient.Face.DetectWithStreamAsync(photo).ConfigureAwait(false);
                var faceDetectedIds = facesDetected.Where(f => f.FaceId != null).Select(f => (Guid)f.FaceId).ToList();

                if (!faceDetectedIds.Any())
                {
                    App.FaceMathError = "No face detected in photo.";
                    Debug.WriteLine(App.FaceMathError);
                    return false;
                }

                var facesIdentified = await FaceApiClient.Face.IdentifyAsync(faceDetectedIds, _personGroupId).ConfigureAwait(false);
                var candidateList = facesIdentified.SelectMany(x => x.Candidates).ToList();

                var personGroupList = await personGroupListTask.ConfigureAwait(false);
                var matchingPersonList = personGroupList.Where(p => p.Name.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                bool isMatched = candidateList.Select(c => c.PersonId).Intersect(matchingPersonList.Select(p => p.PersonId)).Any();

                if (!isMatched)
                {
                    App.FaceMathError = "Face not matched with registered faces.";
                    Debug.WriteLine($"Face verification failed for user: {username}");
                }
                else
                {
                    App.FaceMathError = null;
                    Debug.WriteLine($"Face verification successful for user: {username}");
                }

                // Extra debug
                Debug.WriteLine($"Debug: Detected faces: {facesDetected.Count}, Candidates: {candidateList.Count}, Persons in group: {personGroupList.Count()}");

                return isMatched;
            }
            catch (APIErrorException apiEx)
            {
                App.FaceMathError = $"Face API error: {apiEx.Message}";
                Debug.WriteLine(App.FaceMathError);
                return false;
            }
            catch (Exception ex)
            {
                App.FaceMathError = ex.Message;
                Debug.WriteLine($"IsFaceIdentified error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> MatchFace(Stream photo)
        {
            try
            {
                if (photo.CanSeek) photo.Position = 0;

                var trainingStatus = await FaceApiClient.PersonGroup.GetTrainingStatusAsync(_personGroupId);
                if (trainingStatus.Status != TrainingStatusType.Succeeded)
                {
                    Debug.WriteLine($"Person group training status: {trainingStatus.Status}");
                    return false;
                }

                var personGroupListTask = FaceApiClient.PersonGroupPerson.ListAsync(_personGroupId);
                var facesDetected = await FaceApiClient.Face.DetectWithStreamAsync(photo).ConfigureAwait(false);
                var faceDetectedIds = facesDetected.Where(f => f.FaceId != null).Select(f => (Guid)f.FaceId).ToList();

                if (!faceDetectedIds.Any())
                {
                    Debug.WriteLine("No face detected in photo.");
                    return false;
                }

                var facesIdentified = await FaceApiClient.Face.IdentifyAsync(faceDetectedIds, _personGroupId).ConfigureAwait(false);
                var candidateList = facesIdentified.SelectMany(x => x.Candidates).ToList();
                var personGroupList = await personGroupListTask.ConfigureAwait(false);

                var matchingPersonList = personGroupList.Where(p => p.Name.Equals("Test", StringComparison.InvariantCultureIgnoreCase));
                bool isMatched = candidateList.Select(c => c.PersonId).Intersect(matchingPersonList.Select(p => p.PersonId)).Any();

                Debug.WriteLine($"Debug: Detected faces: {facesDetected.Count}, Candidates: {candidateList.Count}, Persons in group: {personGroupList.Count()}");

                return isMatched;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MatchFace error: {ex.Message}");
                return false;
            }
        }

        private async Task CreatePersonGroup()
        {
            try
            {
                await FaceApiClient.PersonGroup.CreateAsync(_personGroupId, _personGroupName).ConfigureAwait(false);
                Debug.WriteLine("Person group created successfully.");
            }
            catch (APIErrorException e) when (e.Response.StatusCode == HttpStatusCode.Conflict)
            {
                Debug.WriteLine("Person group already exists.");
            }
        }

        private async Task<TrainingStatus> TrainPersonGroup(string personGroupId)
        {
            await FaceApiClient.PersonGroup.TrainAsync(personGroupId).ConfigureAwait(false);
            var status = await FaceApiClient.PersonGroup.GetTrainingStatusAsync(personGroupId).ConfigureAwait(false);
            Debug.WriteLine($"Training status: {status.Status}");
            return status;
        }
    }
}
