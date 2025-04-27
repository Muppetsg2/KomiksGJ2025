using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EndManager : MonoBehaviour
{
    private EndManager instance;

    public EndManager Instance {  get { return instance; } }

    [SerializeField] private GameObject endFinishedCanvas;
    [SerializeField] private GameObject endDeathCanvas;
    [SerializeField] private GameObject pauseCanvas;

    public bool isEnd = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one EndManager in the Scene");
        }

        instance = this;
    }

    private void Start()
    {
        endFinishedCanvas.SetActive(false);
        endDeathCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        InputManager.Instance.OnPausePressed += OpenPauseScreen;
    }

    public void OpenPauseScreen()
    {
        if (!isEnd)
        {
            if (!pauseCanvas.activeSelf)
            {
                bool isDialog = DialogueManager.Instance != null && DialogueManager.Instance.dialogueIsPlaying;

                if (isDialog)
                {
                    InputManager.Instance.DisableDialogMap();
                }
                else
                {
                    InputManager.Instance.DisablePlayerMap();
                }

                InputManager.Instance.EnableUIMap();
                pauseCanvas.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                ClosePauseScreen();
            }
        }
    }

    public void ClosePauseScreen()
    {
        bool isDialog = DialogueManager.Instance != null && DialogueManager.Instance.dialogueIsPlaying;

        pauseCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        InputManager.Instance.DisableUIMap();

        if (isDialog)
        {
            InputManager.Instance.EnableDialogMap();
        }
        else
        {
            InputManager.Instance.EnablePlayerMap();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isEnd = true;
        endFinishedCanvas.SetActive(true);
    }

    public void OpenDeathScreen()
    {
        isEnd = true;
        endDeathCanvas.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(string level)
    {
        PlayerPrefs.SetString("LastLevel", level);
        SceneManager.LoadScene(level);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}