using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tobii.XR;
using Valve.VR;
using ViveSR.anipal.Eye;


namespace Log
{
    public class LogManager : MonoBehaviour
    {
        // Serialized Properties 
        [SerializeField] private bool enableHeadTracking;
        [SerializeField] private bool enableEyeTracking;
        [SerializeField] private bool enableMotionTracking;
        [SerializeField] private bool enableImageTracking;
        [SerializeField] private TobiiXR_Settings Settings;


        // Private Properties  
        private String _dataSaveDirectory;
        private String _imageSaveDirectory;
        private String _simulationName;
        private Camera _activeCamera;
        private StringBuilder _headTrackingStringBuilder;
        private StringBuilder _eyeTrackingStringBuilder;
        private StringBuilder _motionTrackerStringBuilder;
        private HeadTrackingAPI _headTrackingAPI;
        private EyeTrackingAPI _eyeTrackingAPI;
        private FrameCaptureAPI _frameCaptureAPI;
        private MotionTrackerAPI _motionTrackerAPI;
        private Rigidbody _parentRigidbody;
        private String _participantName;

        private void Awake()
        {
            // Tobi XR Settings
            TobiiXR.Start(Settings);

            // Get the Participant Name
            _participantName = PlayerPrefs.HasKey("playerName")
                ? PlayerPrefs.GetString("playerName") + DateTime.Now.ToFileTime()
                : "DemoParticipant-" + DateTime.Now.ToFileTime();

            if (_participantName.Equals(""))
            {
                _participantName = "DemoParticipant-" + DateTime.Now.ToFileTime();
            }
            
            
            // Check eye calibration is required.
            bool isEyeCalibrationRequired = false;

            if (SRanipal_Eye_API.IsViveProEye())
            {
                int error = SRanipal_Eye_API.IsUserNeedCalibration(ref isEyeCalibrationRequired);
                
                    Debug.Log("Eye Calibration Required!");
                    Logger.Log(LogLevel.INFO, "Eye Calibration Required!");

                    SRanipal_Eye.LaunchEyeCalibration();
                    Debug.Log("Eye Calibration error: " + error);
                    Logger.Log(LogLevel.DEBUG, "Eye Calibration error: " + error);
               
            }
            else
            {
                Debug.Log("Eye Tracking not supported in this device! Device is not HTC-Vive Pro Eye.");
                Logger.Log(LogLevel.INFO, "Eye Tracking not supported in this device! Device is not HTC-Vive Pro Eye.");
                enableEyeTracking = false;
            }
        }

        void Start()
        {
            _simulationName = SceneManager.GetActiveScene().name;
            _activeCamera = Camera.allCameras[0];

            CreateDataDirectory();
            InitiateTracking();

            Logger.Log(LogLevel.INFO, "Simulation Started : " + _simulationName);
        }

        private void Update()
        {
            StartTracking();
        }

        private void CreateDataDirectory()
        {
            _dataSaveDirectory = Application.dataPath + "/Data/" + _participantName + "/" + _simulationName + "/";
            if (!Directory.Exists(_dataSaveDirectory))
            {
                Directory.CreateDirectory(_dataSaveDirectory);
                Debug.Log("Directory Created : " + _dataSaveDirectory);
                Logger.Log(LogLevel.INFO, "Directory Created : " + _dataSaveDirectory);
            }

            // Creating a subdirectory for images.
            if (enableImageTracking)
            {
                _imageSaveDirectory = _dataSaveDirectory + "/" + "Frames/";
                if (!Directory.Exists(_imageSaveDirectory))
                {
                    Directory.CreateDirectory(_imageSaveDirectory);
                    Debug.Log("Image Save Directory Created : " + _imageSaveDirectory);
                    Logger.Log(LogLevel.INFO, "Image Save Directory Created : " + _imageSaveDirectory);
                }
            }
        }

        private void InitiateTracking()
        {
            if (enableHeadTracking)
            {
                Debug.Log("Head Tracking is turned on.");
                _headTrackingAPI = new HeadTrackingAPI();
                _headTrackingStringBuilder = _headTrackingAPI.InitiateTracker(csvLogger: _headTrackingStringBuilder);
            }

            if (enableEyeTracking)
            {
                Debug.Log("Eye Tracking is turned on.");
                _eyeTrackingAPI = new EyeTrackingAPI();
                _eyeTrackingStringBuilder = _eyeTrackingAPI.InitiateTracker(_eyeTrackingStringBuilder);
            }

            if (enableImageTracking)
            {
                Debug.Log("Frame Image Capture is enabled.");
                _frameCaptureAPI = new FrameCaptureAPI();
            }

            if (enableMotionTracking)
            {
                Debug.Log("Motion Tracking is turned on.");

                _motionTrackerAPI = new MotionTrackerAPI();
                _motionTrackerStringBuilder = _motionTrackerAPI.InitiateTracker(csvLogger: _motionTrackerStringBuilder);
                _parentRigidbody = GetComponentInParent<Rigidbody>();
            }
        }

        private void StartTracking()
        {
            int frameCount = Time.frameCount;
            String time = DateTime.Now.ToString("hh-mm-ss-fff");

            if (enableHeadTracking)
            {
                _headTrackingStringBuilder =
                    _headTrackingAPI.TrackHead(time, frameCount, _activeCamera, _headTrackingStringBuilder);
            }

            if (enableEyeTracking)
            {
                _eyeTrackingStringBuilder = _eyeTrackingAPI.StartTracking(time, frameCount, _eyeTrackingStringBuilder);
            }

            if (enableImageTracking)
            {
                _frameCaptureAPI.SaveImageToFile(_imageSaveDirectory, frameCount, time);
            }

            if (enableMotionTracking)
            {
                _motionTrackerStringBuilder =
                    _motionTrackerAPI.StartTracking(time, frameCount, _parentRigidbody, _motionTrackerStringBuilder);
            }
        }

        private void OnDestroy()
        {
            if (enableHeadTracking)
            {
                _headTrackingAPI.SaveToFile(_dataSaveDirectory, csvLogger: _headTrackingStringBuilder);
            }

            if (enableEyeTracking)
            {
                _eyeTrackingAPI.SaveToFile(_dataSaveDirectory, csvLogger: _eyeTrackingStringBuilder);
            }

            if (enableMotionTracking)
            {
                _motionTrackerAPI.SaveToFile(_dataSaveDirectory, csvLogger: _motionTrackerStringBuilder);
            }

            // Clean Players Name
            PlayerPrefs.SetString("playerName", "");
        }
    }
}