using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Interface
{
    public interface IAppStateService 
    {
        public bool IsInBackground();
        public void SetBackgroundState(bool isBackground);
    }
}
