using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animator))]
public class CameraPhone : MonoBehaviour
{
    private const float PhotoAspect = 1.0f; // width / height
    private const float PhotoHeightToScreen = 0.8f;
    private const int PhotoWidth = 256;
    
    [SerializeField] private Transform hand;
    [SerializeField] private Transform handZoomed;

    public float zoomAlpha = 0;

    private Animator _animator;
    private int _zoomDirParam;
    private int _zoomDir = 1;

    private bool _screenshotQueued;


    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    
    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _zoomDirParam = Animator.StringToHash("ZoomDir");
    }

    public void UpdateTransform()
    {
        transform.position = Vector3.Lerp(hand.position, handZoomed.position, zoomAlpha);
        transform.rotation = Quaternion.Lerp(hand.rotation, handZoomed.rotation, zoomAlpha);
    }

    public void ToggleZoomAnim()
    {
        _animator.SetFloat(_zoomDirParam, _zoomDir);
        _animator.Play("PhoneZoom", 0, zoomAlpha);
        _zoomDir = -1 * _zoomDir;
    }

    public void TakePicture()
    {
        if (zoomAlpha >= 1)
        {
            _screenshotQueued = true;
        }
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext ctx, Camera cam)
    {
        if (cam != Camera.main || !_screenshotQueued)
        {
            return;
        }

        _screenshotQueued = false;
        int width = (int)(PhotoHeightToScreen * PhotoAspect * Screen.height);
        int height = (int)(PhotoHeightToScreen * Screen.height);
        
        int x = (int)((Screen.width - width) / 2);
        int y = (int)((Screen.height - height) / 2);
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect rect = new Rect(x, y, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        //screenshotTexture.Compress(false);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
    }
}
