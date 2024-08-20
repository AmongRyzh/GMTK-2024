using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MetalBlock"))
        {
            GetComponentInParent<HydraulicPress>().MetalBlockCollision();
        }
    }
}
