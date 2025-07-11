using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using CCIMIGRATION.ConstantHelper;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CCIMIGRATION.Services.FaceService
{

    public static class FaceRecognitionService
    {
        const string _personGroupId = "persongroupid";
        const string _personGroupName = "CCIMIGRATION";
        readonly static Lazy<FaceClient> _faceApiClientHolder = new Lazy<FaceClient>(() =>
             new FaceClient(new ApiKeyServiceClientCredentials(Constants.FacialRecognitionAPIKey), new HttpClient(), false) { Endpoint = Constants.FaceApiBaseUrl });

        //static int _networkIndicatorCount;

        static FaceClient FaceApiClient => _faceApiClientHolder.Value;

        public static async Task DeleteGroup()
        {
            try
            {
                await FaceApiClient.PersonGroup.DeleteAsync(_personGroupId).ConfigureAwait(false);
                var trainingStatus = await TrainPersonGroup(_personGroupId).ConfigureAwait(false);
                if (trainingStatus.Status is TrainingStatusType.Failed)
                    throw new Exception(trainingStatus.Message);
            }
            catch (APIErrorException e) when (e.Response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                Debug.WriteLine("Person Does Not Exist");
            }
            finally
            {

            }
        }
        public static async Task RemoveExistingFace(Guid userId)
        {
            try
            {
                await FaceApiClient.PersonGroupPerson.DeleteAsync(_personGroupId, userId).ConfigureAwait(false);
            }
            catch (APIErrorException e) when (e.Response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                Debug.WriteLine("Person Does Not Exist");
            }
            finally
            {
               
            }
        }
        public static async Task<bool> IsFaceExistForEmail(string email)
        {
            var personGroupListTask = FaceApiClient.PersonGroupPerson.ListAsync(_personGroupId);

            var personGroupList = await personGroupListTask.ConfigureAwait(false);

            var matchingUsernamePersonList = personGroupList.Where(x => x.Name.Equals(email, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return matchingUsernamePersonList.Count > 0 ? true : false;
        }
        public static async Task<string> AddNewFace(string username, Stream photo)
        {
            try
            {
                await CreatePersonGroup().ConfigureAwait(false);
                var createPersonResult = await FaceApiClient.PersonGroupPerson.CreateAsync(_personGroupId, username).ConfigureAwait(false);
                var faceResult = await FaceApiClient.PersonGroupPerson.AddFaceFromStreamAsync(_personGroupId, createPersonResult.PersonId, photo).ConfigureAwait(false);

                var trainingStatus = await TrainPersonGroup(_personGroupId).ConfigureAwait(false);

                if (trainingStatus.Status ==  TrainingStatusType.Failed)
                {
                    return null;
                }
                else
                {
                    return createPersonResult.PersonId.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FaceApi Message :- " + ex.Message);
                
                throw ;
            }
            finally
            {
                
            }
        }

        public static async Task CheckFace()
        {
            try
            {
                var personGroupListTask = await FaceApiClient.PersonGroupPerson.ListAsync(_personGroupId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static async Task<bool> MatchFace(Stream photo)
        {
            try
            {
                var personGroupListTask = FaceApiClient.PersonGroupPerson.ListAsync(_personGroupId);

                var facesDetected = await FaceApiClient.Face.DetectWithStreamAsync(photo).ConfigureAwait(false);
                var faceDetectedIds = facesDetected.Where(x=> x.FaceId != null).Select(x => (Guid)x.FaceId).ToList();

                var facesIdentified = await FaceApiClient.Face.IdentifyAsync(faceDetectedIds, _personGroupId).ConfigureAwait(false);

                var candidateList = facesIdentified.SelectMany(x => x.Candidates).ToList();

                var personGroupList = await personGroupListTask.ConfigureAwait(false);

                var matchingUsernamePersonList = personGroupList.Where(x => x.Name.Equals("Test", StringComparison.InvariantCultureIgnoreCase));

                return candidateList.Select(x => x.PersonId).Intersect(matchingUsernamePersonList.Select(y => y.PersonId)).Any();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {

            }
        }
        public static async Task<bool> IsFaceIdentified(string username, Stream photo)
        {
            try
            {
                return true;
                //var personGroupListTask = FaceApiClient.PersonGroupPerson.ListAsync(_personGroupId);

                //var facesDetected = await FaceApiClient.Face.DetectWithStreamAsync(photo).ConfigureAwait(false);
                //var faceDetectedIds = facesDetected.Where(x => x.FaceId != null).Select(x => (Guid)x.FaceId).ToList();

                //var facesIdentified = await FaceApiClient.Face.IdentifyAsync(faceDetectedIds, _personGroupId).ConfigureAwait(false);
           
                //var candidateList = facesIdentified.SelectMany(x => x.Candidates).ToList();

                //var personGroupList = await personGroupListTask.ConfigureAwait(false);

                //var matchingUsernamePersonList = personGroupList.Where(x => x.Name.Equals(username, StringComparison.InvariantCultureIgnoreCase));
                //var x = candidateList.Select(x => x.PersonId).Intersect(matchingUsernamePersonList.Select(y => y.PersonId)).Any();
                //if (!x)
                //{
                //    App.FaceMathError = "Face Not Matched with present faces in Azure";
                //}
                //return x;
            }
            catch(Exception ex)
            {
                App.FaceMathError = ex.Message;
                Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
               
            }
        }


        static async Task CreatePersonGroup()
        {
            try
            {
                await FaceApiClient.PersonGroup.CreateAsync(_personGroupId, _personGroupName).ConfigureAwait(false);
            }
            catch (APIErrorException e) when (e.Response.StatusCode is HttpStatusCode.Conflict)
            {
                Debug.WriteLine("Person Group Already Exists");
            }
        }

        public static async Task<TrainingStatus> TrainPersonGroup(string personGroupId)
        {
            TrainingStatus trainingStatus;

            await FaceApiClient.PersonGroup.TrainAsync(personGroupId).ConfigureAwait(false);

            trainingStatus = await FaceApiClient.PersonGroup.GetTrainingStatusAsync(_personGroupId).ConfigureAwait(false);

            return trainingStatus;
        }
        static bool hasTrainingStatusCompleted(in TrainingStatus trainingStatus) =>
                trainingStatus.Status != TrainingStatusType.Failed && trainingStatus.Status != TrainingStatusType.Succeeded;
    }
}
