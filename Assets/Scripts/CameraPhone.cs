using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPhone : MonoBehaviour
{
    [SerializeField] private GameObject phoneNormal;
    [SerializeField] private GameObject phoneHollow;

    [SerializeField] private Vector3 zoomPosition;
    [SerializeField] private Quaternion zoomRotation;

    public float zoomAlpha = 0;
    
    private void Start()
    {
        phoneNormal.SetActive(true);
        phoneHollow.SetActive(false);
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(Vector3.zero, zoomPosition, zoomAlpha);
        transform.localRotation = Quaternion.Lerp(Quaternion.identity, zoomRotation, zoomAlpha);
    }

    private void SetHollow(bool hollow)
    {
        phoneNormal.SetActive(!hollow);
        phoneHollow.SetActive(hollow);
    }
}
