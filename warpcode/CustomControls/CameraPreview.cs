using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.CustomControls
{
    public class CameraPage : ContentView
    {
        public delegate void PhotoResultEventHandler(PhotoResultEventArgs result);
        public event PhotoResultEventHandler OnPhotoResult;
        public void SetPhotoResult(byte[] image, int width = -1, int height = -1)
        {
            OnPhotoResult?.Invoke(new PhotoResultEventArgs(image, width, height));
        }
        public void Cancel()
        {
            OnPhotoResult?.Invoke(new PhotoResultEventArgs());
        }

    }
    public class PhotoResultEventArgs : EventArgs
    {
        public PhotoResultEventArgs()
        {
            Success = false;
        }
        public PhotoResultEventArgs(byte[] image, int width, int height)
        {
            Success = true;
            Image = image;
            Width = width;
            Height = height;
        }
        public byte[] Image { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool Success { get; private set; }
    }
}
