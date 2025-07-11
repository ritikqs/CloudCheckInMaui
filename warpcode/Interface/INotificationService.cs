using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Interface
{
    public interface INotificationService
    {
        void SendNotification(string title, string message);
    }
}
