using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Interface
{
    public interface ILocationService
    {
        void OpenSettings();
        void OpenLocationPage();
        bool IsGpsAvailable();
    }
}
