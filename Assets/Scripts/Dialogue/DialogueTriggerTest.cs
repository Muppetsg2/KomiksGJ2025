using UnityEngine;

public class DialogueTriggerTest : MonoBehaviour
{
    public TextAsset dialogueContent;

    void Update()
    {
        if (DialogueManager.Instance.dialogueIsPlaying)
        {
            return;
        }
        
        if (InputManager.Instance.GetNextPressed())
        {
            DialogueManager.Instance.EnterDialogueMode(dialogueContent);
        }
    }
}
