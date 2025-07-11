using System;
namespace CCIMIGRATION.Interface
{
    public interface ILogService
    {
        void CreateLogFile();
        void WriteLog();
    }
    public interface IMyService
    {
        void StartService();
        void StopServ();
    }
}
