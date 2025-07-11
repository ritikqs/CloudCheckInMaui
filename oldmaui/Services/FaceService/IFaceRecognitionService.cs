using System;
using System.IO;
using System.Threading.Tasks;

namespace CloudCheckInMaui.Services.FaceService
{
    public interface IFaceRecognitionService
    {
        Task<bool> IsFaceExistForEmail(string email);
        Task<string> AddNewFace(string username, Stream photo);
        Task<bool> MatchFace(Stream photo);
        Task<bool> IsFaceIdentified(string username, Stream photo);
        Task DeleteGroup();
        Task RemoveExistingFace(Guid userId);
        Task<bool> IsConfigurationValid();
    }
} 