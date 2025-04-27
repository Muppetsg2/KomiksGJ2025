using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogueTriggerCollider : MonoBehaviour
{
    public TextAsset dialogueContent;

    public void StartClicked()
    {
        if (DialogueManager.Instance.dialogueIsPlaying)
        {
            return;
        }

        DialogueManager.Instance.EnterDialogueMode(dialogueContent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartClicked();
        }
    }
}
