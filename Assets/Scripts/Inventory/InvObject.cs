using UnityEngine;
using UnityEngine.Tilemaps;

public class InvObject : MonoBehaviour
{
    public InvSO scriptableObject;

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        print("6");
        if (other.CompareTag("Ground") && FindObjectOfType<InvObjectHandler>().objects.Find(x => x.GetComponent<InvObject>() == this))
        {
            print("6.1");
            FindObjectOfType<InvObjectHandler>().objects.Remove(this);
            Destroy(gameObject);
        }
    }*/
}
