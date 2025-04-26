using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject dialogueChoicesParent;
    [SerializeField] private GameObject choicePrefab;
    private GameObject[] choices = {};
    private TextMeshProUGUI[] choicesText = { };
    private int maxChoiceNum = 4;
    private bool nextBlocked = false;

    private Story currentStory;

    private static DialogueManager instance;

    public static DialogueManager Instance { get { return instance; } }

    public bool dialogueIsPlaying {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one InputManager in the Scene");
        }

        instance = this;
    }

    private void Start()
    {
        ExitDialogueModeRaw();
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (InputManager.Instance.GetNextPressed() && !nextBlocked)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        ExitDialogueModeRaw();
    }

    private void ExitDialogueModeRaw()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoice = currentStory.currentChoices;

        if (currentChoice.Count == 0) return;

        nextBlocked = true;

        if (currentChoice.Count > maxChoiceNum)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoice.Count);
        }

        int choicesNum = Mathf.Min(currentChoice.Count, maxChoiceNum);
        choices = new GameObject[choicesNum];
        choicesText = new TextMeshProUGUI[choicesNum];

        int index = 0;
        foreach (Choice choice in currentChoice)
        {
            int capturedIndex = index;
            GameObject obj = Instantiate(choicePrefab, dialogueChoicesParent.transform, false);
            obj.GetComponent<Button>().onClick.AddListener(() => { MakeChoice(capturedIndex); });
            choices[index] = obj;
            choices[index].gameObject.SetActive(true);
            choicesText[index] = obj.GetComponentInChildren<TextMeshProUGUI>();
            choicesText[index].text = choice.text;
            index++;
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);

        ClearChoices();

        dialogueText.text = currentStory.Continue();

        ContinueStory();

        nextBlocked = false;
    }

    private void ClearChoices()
    {
        if (choicesText.Length != 0)
        {
            System.Array.Clear(choicesText, 0, choicesText.Length);
        }
        foreach (var ch in choices)
        {
            DestroyImmediate(ch.gameObject);
        }
        if (choices.Length != 0)
        {
            System.Array.Clear(choices, 0, choices.Length);
        }
    }
}