using System.Collections;
using UnityEngine;

public class InvObjectHandler : MonoBehaviour
{
    private InvSlot invSlot;
    private SpriteRenderer spriteRenderer;
    private bool canPickup = true;

    [SerializeField] private float checkRadius;

    // Start is called before the first frame update
    void Start()
    {
        invSlot = FindObjectOfType<InvSlot>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        if (Input.GetKeyDown(KeyCode.H) && canPickup)
        {
            print("1");
            var objects = Physics2D.OverlapCircleAll(transform.position, checkRadius);
            print("1.1");

            GameObject invObject = null;
            //bool invObjectExists = false;

            foreach (var obj in objects)
            {
                print("1.2");
                if (obj.TryGetComponent<InvObject>(out InvObject invObject228))
                {
                    print($"1.3 ({invObject228})");
                    invObject = invObject228.gameObject; 
                    invSlot.currentObject = invObject.GetComponent<InvObject>().scriptableObject;
                    invSlot.UpdateSlotData();
                    return;
                }
            }
        }
    }

    public IEnumerator SpawnObject(InvSO invSO)
    {
        print("2.2");
        canPickup = false;
        spriteRenderer.sprite = invSO.objectPrefab.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.color = new Color(255, 255, 255, 0.5f);
        print("2.3");
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Escape)) || Input.GetKeyDown(KeyCode.Y));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("2.4.1");
            spriteRenderer.sprite = null;
            canPickup = true;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            print("2.4.2.1");
            InvObject spawnedObject = Instantiate(invSO.objectPrefab, transform.position, Quaternion.identity);
            print($"2.4.2.2 ({spawnedObject})");
            spriteRenderer.sprite = null;
            canPickup = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
