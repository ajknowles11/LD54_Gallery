using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCapturable : MonoBehaviour
{
    [SerializeField] private Capturable _capturable;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Respawning");
        _capturable.gameObject.SetActive(true);
    }
}
