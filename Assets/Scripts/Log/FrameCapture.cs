using System;
using System.IO;
using UnityEngine;

public class FrameCapture : MonoBehaviour
{
    [SerializeField] private int frameRate = 60;
    private string screenShotsFolder = "ScreenShots";
    private string subFolder;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        // Time.captureDeltaTime = 1.0f / frameRate;
        CreateFolder();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        String fileName = "Frame-" + Time.frameCount + "-.png";
        ScreenCapture.CaptureScreenshot(subFolder + "/" + fileName, ScreenCapture.StereoScreenCaptureMode.LeftEye);
    }

    private void CreateFolder()
    {
        DirectoryInfo directoryInfo;
        string currentSubDir = System.DateTime.Now.ToString("dd-MMM-yyyy-(hh-mm-ss)");

        if (Directory.Exists(screenShotsFolder))
        {
            Debug.Log("Screen Shot Directory already exists!");
            directoryInfo = new DirectoryInfo(screenShotsFolder);
        }
        else
        {
            Debug.Log("Screen Shot Directory created");
            directoryInfo = Directory.CreateDirectory(screenShotsFolder);
        }

        subFolder = directoryInfo.CreateSubdirectory(currentSubDir).FullName;
    }
}