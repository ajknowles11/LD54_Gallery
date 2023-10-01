using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    private List<Capturable> _allCapturables;

    [SerializeField] private PhotoDoor activeDoor;
    
    private CameraPhone _cameraPhone;

    private void Start()
    {
        // Find phone camera in scene and give a reference to this object
        _cameraPhone = FindObjectOfType<CameraPhone>();
        if (!_cameraPhone)
        {
            Debug.LogError("No phone found in scene");
        }
        
        // get all capturables in level
        _allCapturables = FindObjectsOfType<Capturable>().ToList();

        _cameraPhone.CollectionManager = this;
    }

    public void AddPhoto(Capturable capturable, int index)
    {
        capturable.PhotoIndices.Add(index);

        // now check if all door conditions satisfied
        if (activeDoor)
        {
            bool canOpen = true;
            foreach (var obj in activeDoor.requiredCapturables)
            {
                if (obj.PhotoIndices.Count == 0)
                {
                    canOpen = false;
                }
                else
                {
                    activeDoor.SetImageCaptured(obj, true);
                }
            }

            if (canOpen)
            {
                activeDoor.OpenDoor();
                activeDoor = activeDoor.nextDoor;
            }
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
            // check if no indices
            if (obj.PhotoIndices.Count == 0)
            {
                // not captured
                activeDoor.SetImageCaptured(obj, false);
            }
        }
    }
}
