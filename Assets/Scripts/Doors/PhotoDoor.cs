using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoDoor : MonoBehaviour
{
    public List<string> requiredCapturedNames;
    
    public PhotoDoor nextDoor;
    
    public void OpenDoor()
    {
        Debug.Log("door opened");
    }
}
