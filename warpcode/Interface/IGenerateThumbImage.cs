using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.Interface
{
    public interface IGenerateThumbImage
    {
        ImageSource GenerateThumbImage(string url, long usecond);
    }
}
