using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class HorizontalDirectionSign : MonoBehaviour
{
    public SignManager sign;
    public float moveAwayTime = 0.5f;

    private void Start()
    {
        sign.OnFlip += OnFlip;
    }

    void OnFlip(bool flipped)
    {
        transform.localPosition = new Vector3(transform.localPosition.x * -1, transform.localPosition.y, transform.localPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(sign.playerTag))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if ((sign.flipped && rb.linearVelocityX > 0) || (!sign.flipped && rb.linearVelocityX < 0))
            {
                StartCoroutine(MoveBack(collision.gameObject));
            }
        }
    }

    private IEnumerator MoveBack(GameObject player)
    {
        InputManager.Instance.DisablePlayer();

        Rigidbody2D prb = player.GetComponent<Rigidbody2D>();

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        pm.OnMove(new Vector2(prb.linearVelocityX >= 0 ? -1 : 1, 0));

        yield return new WaitForSeconds(moveAwayTime);

        pm.OnMove(new Vector2(0, 0));
        InputManager.Instance.EnablePlayer();
    }
}
