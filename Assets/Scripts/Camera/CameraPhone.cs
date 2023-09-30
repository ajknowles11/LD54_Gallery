using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class CameraPhone : MonoBehaviour
{
    private const float PhotoAspect = 1.0f; // width / height
    private const float PhotoHeightToScreen = 0.8f;
    private const int PhotoWidth = 256;
    
    [SerializeField] private Transform hand;
    [SerializeField] private Transform handZoomed;

    [SerializeField] private GameObject buttonIcon;
    [SerializeField] private Animator apertureAnimator;
    [SerializeField] private Image lastPic;

    public float zoomAlpha = 0;

    private Animator _animator;
    private int _zoomDirParam;
    private int _zoomDir = 1;

    private bool _screenshotQueued;

    private Camera _camera;
    [SerializeField] private LayerMask layerMask;

    private Rect _screenshotRect;
    
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
        _camera = Camera.main;
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
            apertureAnimator.Play("ApertureFlash", 0, 0);
            
            int width = (int)(PhotoHeightToScreen * PhotoAspect * Screen.height);
            int height = (int)(PhotoHeightToScreen * Screen.height);
            int x = (int)((Screen.width - width) / 2);
            int y = (int)((Screen.height - height) / 2);
            _screenshotRect = new Rect(x, y, width, height);
            _screenshotQueued = true;

            foreach (var obj in Capturable.Rendered)
            {
                var objectScreenPoint = _camera.WorldToScreenPoint(obj.transform.position);
                RaycastHit hit;
                bool captured = _screenshotRect.Contains(objectScreenPoint) && (!Physics.Linecast(_camera.transform.position, obj.transform.position, out hit, layerMask) || hit.collider == obj.Collider);
                if (captured) Debug.Log(obj.name);
            }
        }
    }

    public void ToggleButtonIcon(bool pressed)
    {
        buttonIcon.SetActive(pressed);
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext ctx, Camera cam)
    {
        if (cam != _camera || !_screenshotQueued)
        {
            return;
        }

        _screenshotQueued = false;
        Texture2D screenshotTexture = new Texture2D((int)_screenshotRect.width, (int)_screenshotRect.height, TextureFormat.RGB24, false, true, true);
        screenshotTexture.ReadPixels(_screenshotRect, 0, 0);
        //screenshotTexture.Compress(false);
        // Resize image to save ram
        screenshotTexture.Apply();
        
        lastPic.sprite = Sprite.Create(screenshotTexture, new Rect(0, 0, screenshotTexture.width, screenshotTexture.height),
            new Vector2((float)screenshotTexture.width / 2, (float)screenshotTexture.height / 2));
    }
}
