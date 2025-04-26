using UnityEditor.UIElements;
using UnityEngine;

public delegate void SignFlippedEventHandler(bool flipped);
public delegate void SignRuinedEventHandler();

[RequireComponent(typeof(Collider2D))]
public class SignManager : MonoBehaviour
{
    [TagField]
    public string playerTag;
    public GameObject interactionObj;

    public SpriteRenderer toFlip;
    public bool flipX = false;
    public bool flipY = false;
    public bool ruinable = false;

    public bool flipped = false;
    public bool showInteraction = false;

    public event SignFlippedEventHandler OnFlip;
    public event SignRuinedEventHandler OnRuined;

    private void Start()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("Nie ma input managera");
        }
        else
        {
            InputManager.Instance.OnInteractPressed += Flip;
        }
        interactionObj.SetActive(showInteraction);
        toFlip.GetComponent<Rigidbody2D>().simulated = false;
        foreach (var col in toFlip.GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && !showInteraction)
        {
            showInteraction = !showInteraction;
            interactionObj.SetActive(showInteraction);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && showInteraction)
        {
            showInteraction = !showInteraction;
            interactionObj.SetActive(showInteraction);
        }
    }

    public bool Ruin()
    {
        if (!ruinable) return false;

        toFlip.GetComponent<Rigidbody2D>().simulated = true;
        foreach (var col in toFlip.GetComponents<Collider2D>())
        {
            col.enabled = true;
        }
        OnRuined?.Invoke();
        return true;
    }

    public void Flip()
    {
        if (showInteraction)
        {
            if (flipX)
            {
                toFlip.flipX = !toFlip.flipX;
            }
            if (flipY)
            {
                toFlip.flipY = !toFlip.flipY;
            }
            flipped = !flipped;
            OnFlip?.Invoke(flipped);
        }
    }
}
