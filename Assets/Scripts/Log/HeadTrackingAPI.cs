using System;
using System.IO;
using System.Text;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Log
{
    public class HeadTrackingAPI : ITracker
    {
        public StringBuilder InitiateTracker(StringBuilder csvLogger)
        {
            csvLogger = new StringBuilder();
            string[] header =
            {
                "Time", "#Frame", "HeadQRotationX", "HeadQRotationY", "HeadQRotationZ", "HeadQRotationW",
                "HeadEulX", "HeadEulY", "HeadEulZ", "AspectRatio", "CameraDepth", "FOV", "EyeConvergence", "Velocity"
            };
            header.ForEach(item => csvLogger.Append(item + ","));
            csvLogger.AppendLine();

            return csvLogger;
        }

        public StringBuilder TrackHead(  String time, int frameCount, Camera activeCamera, StringBuilder csvLogger)
        {
            if (activeCamera != null)
            {
                var rotation = activeCamera.transform.rotation;
                Vector3 velocity = activeCamera.velocity;

                csvLogger.AppendFormat(
                    "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}",
                    time, frameCount, rotation.x, rotation.y, rotation.z, rotation.w,
                    rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z,
                    activeCamera.aspect, activeCamera.depth, activeCamera.fieldOfView,
                    activeCamera.stereoConvergence, velocity.magnitude);
                csvLogger.AppendLine();
            }

            return csvLogger;
        }

        public void SaveToFile(String path, StringBuilder csvLogger)
        {
            File.WriteAllText(
                path + "head_tracking_"+DateTime.Now.ToString("dd-MMM-yyyy-(hh-mm-ss)") + ".csv",
                csvLogger.ToString());
            Debug.Log("Head Tracking data saved at: " + path);
            Logger.Log(LogLevel.INFO, "Head Tracking data saved at: " + path);
        }
    }
}