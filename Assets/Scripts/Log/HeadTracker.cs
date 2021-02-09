using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

namespace Log
{
    public class HeadTracker : MonoBehaviour
    {
        private Camera _activeCamera;
        private StringBuilder csvLogger;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    
        void Start()
        {
            CreateCsvHeader();
        }
        void FixedUpdate()
        {
            HeadTracking();
        }

        void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
            Debug.Log("New Scene has Loaded");
            if (_activeCamera == null)
            {
                Debug.Log("Current Cameras: " + Camera.allCameras.Length);
                _activeCamera = Camera.allCameras[0];
            }
        }
        private void HeadTracking()
        {
            String time = DateTime.Now.ToString("hh-mm-ss-fff");

            if (_activeCamera != null)
            {
                var rotation = _activeCamera.transform.rotation;
                Vector3 velocity = _activeCamera.velocity;
            
                csvLogger.AppendFormat(
                    "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}",
                    time, Time.frameCount, rotation.x, rotation.y, rotation.z, rotation.w,
                    rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z,
                    _activeCamera.aspect, _activeCamera.depth, _activeCamera.fieldOfView,
                    _activeCamera.stereoConvergence, velocity.x, velocity.y, velocity.z);
                csvLogger.AppendLine();
            }
        }

        private void OnDestroy()
        {
            System.IO.File.WriteAllText(
                "Logs/head_tracking_data-" + DateTime.Now.ToString("dd-MMM-yyyy-(hh-mm-ss)") + ".csv",
                csvLogger.ToString());
        }

        private void CreateCsvHeader()
        {
            csvLogger = new StringBuilder();
            string[] header =
            {
                "Time", "#Frame", "HeadQRotationX", "HeadQRotationY", "HeadQRotationZ", "HeadQRotationW",
                "HeadEulX", "HeadEulY", "HeadEulZ", "AspectRatio", "CameraDepth", "FOV", "CamEyeConvergence",
                "VelocityX", "VelocityY", "VelocityZ"
            };
            header.ForEach(item => csvLogger.Append(item + ","));
            csvLogger.AppendLine();
        }
    }
    
    
} 