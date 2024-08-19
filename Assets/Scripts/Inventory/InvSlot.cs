using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvSlot : MonoBehaviour
{
    public InvSO currentObject;
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text objectNameText;
    InvObjectHandler objectHandler;

    private void Start()
    {
        objectHandler = FindObjectOfType<InvObjectHandler>();
    }

    public void UpdateSlotData()
    {
        slotImage.sprite = currentObject.slotImage;
        objectNameText.text = currentObject.objectName;
    }

    /*public void SpawnObject()
    {
        print("2");
        if (currentObject != null && !objectHandler.isSpawning)
        {
            print("2.1");
            StartCoroutine(objectHandler.SpawnObject(currentObject, false));
        }
    }*/
}
