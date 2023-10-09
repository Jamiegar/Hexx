using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
public class ScreenshotHandeler : MonoBehaviour
{
    [SerializeField] private int imagePixelWidth = 1920;
    [SerializeField] private int imagePixelHeight = 1080;

    private Camera mainCamera;
    private bool takeScreenshotOnFrame;


    private void EndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        Result();
    }


    public void TakeScreenShot()
    {
        mainCamera = gameObject.GetComponent<Camera>();

        mainCamera.targetTexture = RenderTexture.GetTemporary(imagePixelWidth, imagePixelHeight, 16);
        takeScreenshotOnFrame = true;
    }


    private void Result()
    {
        if (takeScreenshotOnFrame)
        {
            takeScreenshotOnFrame = false;
            RenderTexture renderTexture = mainCamera.targetTexture;

            Texture2D result = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            result.ReadPixels(rect, 0, 0);

            byte[] byteArray = result.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", byteArray);
            Debug.Log("Screenshot taken!");

            RenderTexture.ReleaseTemporary(renderTexture);
            mainCamera.targetTexture = null;
        }
    }

    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += EndCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= EndCameraRendering;
    }


}



#endif
