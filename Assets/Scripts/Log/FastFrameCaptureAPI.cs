using System;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Log
{
    [RequireComponent(typeof(Camera))]
    public class FastFrameCaptureAPI : MonoBehaviour
    {
        private Camera snapCam;

        private int resWidth = 256;

        private int resHeight = 256;

        private Texture2D _bufferTexture2D;
        // Start is called before the first frame update
        void Start()
        {
            snapCam = GetComponent<Camera>();
            if (snapCam.targetTexture == null)
            {
                snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24); 
            }
            else
            {
                var targetTexture = snapCam.targetTexture;
                resHeight = targetTexture.height;
                resWidth = targetTexture.width;
            }
        }

        // Update is called once per frame
        void Update()
        {
            CaptureFrame();
        }

        private void CaptureFrame()
        {
            _bufferTexture2D = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
           _bufferTexture2D.ReadPixels(new Rect(0, 0, resHeight, resHeight), 0, 0);
           byte[] image = _bufferTexture2D.EncodeToPNG();
           String filename = Application.dataPath + "/" + "test-image-" + Time.frameCount;
           System.IO.File.WriteAllBytes(filename, image);
        }

        private void OnDestroy()
        {
            
        }
    }
}