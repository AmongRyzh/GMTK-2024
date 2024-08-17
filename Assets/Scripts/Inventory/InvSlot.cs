using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvSlot : MonoBehaviour
{
    public InvSO currentObject;
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text objectNameText;

    public void UpdateSlotData()
    {
        slotImage.sprite = currentObject.slotImage;
        objectNameText.text = currentObject.objectName;
    }

    public void SpawnObject()
    {
        print("2");
        if (currentObject != null)
        {
            print("2.1");
            StartCoroutine(FindObjectOfType<InvObjectHandler>().SpawnObject(currentObject));
        }
    }
}
