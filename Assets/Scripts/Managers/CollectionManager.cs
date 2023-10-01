using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    private List<Capturable> _allCapturables;

    public PhotoDoor activeDoor;
    
    public CameraPhone cameraPhone;

    private void Start()
    {
        // Find phone camera in scene and give a reference to this object
        if (!cameraPhone) cameraPhone = FindObjectOfType<CameraPhone>();
        if (!cameraPhone)
        {
            Debug.LogError("No phone found in scene");
        }
        
        // get all capturables in level
        _allCapturables = FindObjectsOfType<Capturable>().ToList();
        
        cameraPhone.SetStorageSize(activeDoor.requiredCapturables.Count);

        cameraPhone.CollectionManager = this;
    }

    public void AddPhoto(Capturable capturable, int index)
    {
        capturable.PhotoIndices.Add(index);
        if (activeDoor.IsPlayerAt)
        {
            activeDoor.TryOpenDoor();
        }
    }

    public void DeletePhoto(int index)
    {
        // Loop through all dictionary elements, removing index and decrementing any index greater by 1
        foreach (var obj in _allCapturables)
        {
            for (int i = obj.PhotoIndices.Count - 1; i >= 0; i--)
            {
                if (obj.PhotoIndices[i] == index)
                {
                    obj.PhotoIndices.RemoveAt(i);
                }
                else if (obj.PhotoIndices[i] > index)
                {
                    obj.PhotoIndices[i] -= 1;
                }
            }
        }
    }
}
