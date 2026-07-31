using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject tutorialPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        creditsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowMenu()
    {
        creditsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void ShowTutorial()
    {
        menuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void ShowPlayScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PlayerRevCami");
    }
}
