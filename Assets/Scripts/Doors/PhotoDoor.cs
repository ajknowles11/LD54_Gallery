using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PhotoDoor : MonoBehaviour
{
    public List<Capturable> requiredCapturables;
    
    public PhotoDoor nextDoor;

    public CollectionManager collectionManager;

    [NonSerialized] public bool IsPlayerAt = false;

    [SerializeField] private GameObject doorObject; //this is the physical door that blocks you
    
    public void TryOpenDoor()
    {
        foreach (var obj in requiredCapturables)
        {
            if (obj.PhotoIndices.Count == 0)
            {
                return;
            }
        }

        collectionManager.activeDoor = nextDoor;
        collectionManager.cameraPhone.SetStorageSize(nextDoor.requiredCapturables.Count);
        doorObject.SetActive(false);
        Debug.Log("door opened");
    }

    private void OnTriggerEnter(Collider other)
    {
        IsPlayerAt = true;
        TryOpenDoor();
        Debug.Log("entered");
    }

    private void OnTriggerExit(Collider other)
    {
        IsPlayerAt = false;
        Debug.Log("exited");
    }
}
