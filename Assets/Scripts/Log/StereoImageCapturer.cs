using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StereoImageCapturer : MonoBehaviour
{
    public String participantName;
    private String _imageDirectory = "DemoParticipant";

    private void Start()
    {
        _imageDirectory = Application.dataPath + "/Capture/" + participantName; // Image path
        Debug.Log("Image Saving Path: " + _imageDirectory);

        if (!Directory.Exists(_imageDirectory))
        {
            Directory.CreateDirectory(_imageDirectory);
            Debug.Log("Directory Created : " + _imageDirectory);
        }
    }

    private void FixedUpdate()
    {
        CaptureImages();
    }

    private void CaptureImages()
    {
        int currentFrame = Time.frameCount;
        String leftEyeImage = _imageDirectory + "_" + currentFrame + "_left_eye" + ".PNG";
        String rightEyeImage = _imageDirectory + "_" + currentFrame + "_right_eye" + ".PNG";
        ScreenCapture.CaptureScreenshot(leftEyeImage, ScreenCapture.StereoScreenCaptureMode.LeftEye);
        ScreenCapture.CaptureScreenshot(rightEyeImage, ScreenCapture.StereoScreenCaptureMode.RightEye);
    }
}