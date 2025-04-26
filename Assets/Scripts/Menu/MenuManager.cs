using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private string lastLevel = "";
    [SerializeField] private Button continueBtn;
    [SerializeField] private GameObject levelsCanvas;
    [SerializeField] private GameObject menuCanvas;

    private void Start()
    {
        Menu();
        lastLevel = PlayerPrefs.GetString("LastLevel", "");
        continueBtn.interactable = lastLevel != "";
    }

    public void Continue()
    {
        SceneManager.LoadScene(lastLevel);
    }

    public void Levels()
    {
        menuCanvas.SetActive(false);
        levelsCanvas.SetActive(true);
    }

    public void Menu()
    {
        menuCanvas.SetActive(true);
        levelsCanvas.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
