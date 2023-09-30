using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] [Tooltip("Player mouse look sensitivity")]
    private float lookSensitivity = 0.1f;
    
    [SerializeField] [Tooltip("Movement speed in m/s")]
    private float movementSpeed = 3.0f;

    [SerializeField] [Tooltip("Acceleration due to gravity in m/s2")]
    private float gravity = 9.81f;

    [SerializeField] [Tooltip("Maximum fall speed in m/s")]
    private float maxFallSpeed = 100.0f;

    [SerializeField] [Tooltip("Jump velocity, in m/s")]
    private float jumpVelocity = 3.0f;


    [SerializeField] private CameraPhone phone;
    
    private CharacterController _controller;
    private Camera _camera;

    private Vector2 _lookInput;
    private float _camPitch;
    private Vector2 _movementInput;
    private float _yVelocity;
    private bool _hasJustJumped;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _camPitch = _camera.transform.eulerAngles.x;
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void LateUpdate()
    {
        UpdateLook();
        phone.UpdateTransform();
    }

    private void UpdateLook()
    {
        transform.Rotate(Vector3.up, _lookInput.x * lookSensitivity);
        _camPitch -= _lookInput.y * lookSensitivity;
        _camPitch = Mathf.Clamp(_camPitch, -90.0f, 90.0f);
        Vector3 camEulerAngles = _camera.transform.eulerAngles;
        _camera.transform.eulerAngles = new Vector3(_camPitch, camEulerAngles.y, camEulerAngles.z);
    }
    
    private void UpdateMovement()
    {
        bool jumpedThisFrame = false;
        if (_hasJustJumped)
        {
            jumpedThisFrame = true;
            _hasJustJumped = false;
        }
        
        if (_controller.isGrounded && jumpedThisFrame)
        {
            _yVelocity = jumpVelocity;
        }
        else
        {
            _yVelocity = Mathf.Max(_yVelocity - gravity * Time.deltaTime, -maxFallSpeed);
        }
        Vector3 velocity = Time.deltaTime * movementSpeed * (transform.forward * _movementInput.y + transform.right * _movementInput.x) + Time.deltaTime * _yVelocity * Vector3.up;
        _controller.Move(velocity);
    }

    public void OnInputLook(InputAction.CallbackContext ctx)
    {
        _lookInput = ctx.ReadValue<Vector2>();
    }
    
    public void OnInputMove(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnInputJump(InputAction.CallbackContext ctx)
    {
        _hasJustJumped = ctx.performed;
    }

    public void OnInputZoom(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            phone.ToggleZoomAnim();
        }
    }
    
}
