using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Interface
{
    public interface IPermissionService
    {
        void CheckLocationPermissionStatus();
        void AskLocationPermission();
    }
}
