using System.Collections.Generic;
using UnityEngine;

public class InvObjectCounter : MonoBehaviour
{
    public List<InvObject> objects { get; private set; }

    [SerializeField] private int maxObjectCount;

    public int MaxObjectCount()
    {
        return maxObjectCount;
    }
}
