using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CollectionImage : MonoBehaviour
{

    [SerializeField] private GameObject indicator;

    public void SetCollected(bool collected)
    {
        indicator.SetActive(collected);
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }
}
