using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PhotoDoor : MonoBehaviour
{
    public List<Capturable> requiredCapturables;
    public List<GameObject> capturedIndicators;
    
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

        if (nextDoor) {
            collectionManager.activeDoor = nextDoor;
            collectionManager.cameraPhone.SetStorageSize(nextDoor.requiredCapturables.Count);
        }
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

    public void UpdateImages()
    {
        for (int i = 0; i < requiredCapturables.Count; i++)
        {
            capturedIndicators[i].SetActive(requiredCapturables[i].PhotoIndices.Count > 0);
        }
    }
}
