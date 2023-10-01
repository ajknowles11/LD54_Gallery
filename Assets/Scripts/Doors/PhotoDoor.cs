using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoDoor : MonoBehaviour
{
    public List<Capturable> requiredCapturables;
    
    public PhotoDoor nextDoor;
    
    public void OpenDoor()
    {
        Debug.Log("door opened");
    }

    public void SetImageCaptured(Capturable obj, bool captured)
    {
        
    }
}
