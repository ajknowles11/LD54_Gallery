using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPhone : MonoBehaviour
{
    [SerializeField] private GameObject phoneNormal;
    [SerializeField] private GameObject phoneHollow;
    
    [SerializeField] private Transform hand;
    [SerializeField] private Transform handZoomed;

    public float zoomAlpha = 0;
    
    private void Start()
    {
        phoneNormal.SetActive(true);
        phoneHollow.SetActive(false);
    }

    public void UpdateTransform()
    {
        transform.position = Vector3.Lerp(hand.position, handZoomed.position, zoomAlpha);
        transform.rotation = Quaternion.Lerp(hand.rotation, handZoomed.rotation, zoomAlpha);
    }

    private void SetHollow(bool hollow)
    {
        phoneNormal.SetActive(!hollow);
        phoneHollow.SetActive(hollow);
    }
}
