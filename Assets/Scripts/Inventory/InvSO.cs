using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Scriptable Objects/Inventory Object")]
public class InvSO : ScriptableObject
{
    public string objectName;
    public Sprite slotImage;
    public InvObject objectPrefab;
}
