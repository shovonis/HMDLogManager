using System;
using System.IO;
using System.Text;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Log
{
    public class MotionTrackerAPI : ITracker
    {
        Vector3 lastPosition = Vector3.zero;
        public float speed = 0;

        public StringBuilder InitiateTracker(StringBuilder csvLogger)
        {
            csvLogger = new StringBuilder();
            string[] header =
            {
                "Time", "#Frame", "Speed_X", "Speed_Y", "Speed_Z", "Speed_Mag", "Angular_Speed_X", "Angular_Speed_Y",
                "Angular_Speed_Z", "Angular_Speed_Mag", "Center_of_mass_X", "Center_of_mass_Y", "Center_of_mass_Z"
            };
            header.ForEach(item => csvLogger.Append(item + ","));
            csvLogger.AppendLine();

            return csvLogger;
        }

        public StringBuilder StartTracking(String time, int frameCount, Rigidbody parentRigidbody,
            StringBuilder csvLogger)
        {
            if (parentRigidbody != null)
            {
                
                Vector3 velocity = parentRigidbody.velocity;
                Vector3 angularVelocity = parentRigidbody.angularVelocity;
                Vector3 centerOfMass = parentRigidbody.centerOfMass;

                csvLogger.AppendFormat(
                    "{0}, {1}, {2}, {3}, {4}, {5},{6}, {7}, {8}, {9}, {10}, {11}, {12}",
                    time, frameCount, velocity.x, velocity.y,
                    velocity.z, velocity.magnitude, angularVelocity.x, angularVelocity.y, angularVelocity.z,
                    angularVelocity.magnitude, centerOfMass.x, centerOfMass.y, centerOfMass.z);

                csvLogger.AppendLine();
            }
            else
            {
                Debug.LogError("No Rigidbody Found in Parent Component! Make sure the parent have a rigidbody.");
            }

            return csvLogger;
        }

        public void SaveToFile(string path, StringBuilder csvLogger)
        {
            File.WriteAllText(
                path + "motion_tracking_" + DateTime.Now.ToString("dd-MMM-yyyy-(hh-mm-ss)") + ".csv",
                csvLogger.ToString());
            Debug.Log("Motion tracking data saved at: " + path);
            Logger.Log(LogLevel.INFO, "Motion Tracking data saved at: " + path);
        }
    }
}