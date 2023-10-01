using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LoadCapturableData
{
    public string name;
    public Sprite sprite;
}

public class RuntimeCapturableData
{
    public RuntimeCapturableData(List<int> idxs, CollectionImage image)
    {
        PhotoIndices = idxs;
        SidebarImage = image;
    }
    public List<int> PhotoIndices;
    public CollectionImage SidebarImage;

}

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    private Dictionary<string, RuntimeCapturableData> _capturableDictionary = new(); // holds name of obj and 

    [SerializeField] private GameObject sidebar;
    [SerializeField] private CollectionImage imagePrefab;

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

        _cameraPhone.CollectionManager = this;
        
        // Populate dictionary and sidebar
        foreach (var data in levelData.targets)
        {
            CollectionImage newImage = Instantiate(imagePrefab, sidebar.transform, false);
            newImage.SetSprite(data.sprite);
            _capturableDictionary.Add(data.name, new RuntimeCapturableData(new List<int>(), newImage));
        }
    }

    public void AddPhoto(string objName, int index)
    {
        var data = _capturableDictionary[objName];
        data.PhotoIndices.Add(index);
        // we know we have captured obj
        data.SidebarImage.SetCollected(true);

        // now check if all door conditions satisfied
        if (activeDoor)
        {
            foreach (var name in activeDoor.requiredCapturedNames)
            {
                if (_capturableDictionary[name].PhotoIndices.Count == 0)
                {
                    return;
                }
            }
            activeDoor.OpenDoor();
            activeDoor = activeDoor.nextDoor;
        }
    }

    public void DeletePhoto(int index)
    {
        // Loop through all dictionary elements, removing index and decrementing any index greater by 1
        foreach (var kvp in _capturableDictionary)
        {
            var indices = kvp.Value.PhotoIndices;
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                if (indices[i] == index)
                {
                    indices.RemoveAt(i);
                }
                else if (indices[i] > index)
                {
                    indices[i] -= 1;
                }
            }
            // check if no indices
            if (indices.Count == 0)
            {
                // not captured
                kvp.Value.SidebarImage.SetCollected(false);
            }
        }
    }
}
