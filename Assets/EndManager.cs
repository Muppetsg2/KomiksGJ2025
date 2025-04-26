using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EndManager : MonoBehaviour
{
    private EndManager instance;

    public EndManager Instance {  get { return instance; } }

    [SerializeField] private GameObject endFinishedCanvas;
    [SerializeField] private GameObject endDeathCanvas;

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