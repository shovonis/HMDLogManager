using System;
using UnityEngine;

namespace Log
{
    public class FrameCaptureAPI
    {
        public void SaveImageToFile(String path, int frameCount, String time)
        {
            // Both Eye
            String imageFileName = "Frame-" + frameCount + "-" + time +".png";
            
            ScreenCapture.CaptureScreenshot(path + "/" + imageFileName,
                ScreenCapture.StereoScreenCaptureMode.BothEyes); /// IO operation 
        }
    }
}