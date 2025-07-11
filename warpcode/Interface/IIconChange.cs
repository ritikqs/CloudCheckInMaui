using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Interface
{
    public interface IIconChange
    {
        bool IsSelected { get; set; }
        string CurrentIcon { get; }
    }
}
