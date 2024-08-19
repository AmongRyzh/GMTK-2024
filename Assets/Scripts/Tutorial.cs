using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text case2Text;
    private PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        player.SelectSpeedState(0);
    }

    public IEnumerator TutorialCollider(int colliderID)
    {
        switch (colliderID)
        {
            case 2:
                player.SelectSpeedState(0);
                case2Text.gameObject.SetActive(true);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                player.SelectSpeedState(1);
                player.canChangeSpeed = true;
                break;
            case 3:
                player.SelectSpeedState(0);
                yield return new WaitUntil(() => FindObjectOfType<InvSlot>().currentObject != null);
                player.canChangeSpeed = true;
                //player.SelectSpeedState(1);
                break;
            case 4:
                player.SelectSpeedState(0);
                yield return new WaitUntil(() => FindObjectOfType<InvObjectHandler>().objects.Count != 0);
                player.canChangeSpeed = true;
                break;
            default:
                break;
        }
    }
}
