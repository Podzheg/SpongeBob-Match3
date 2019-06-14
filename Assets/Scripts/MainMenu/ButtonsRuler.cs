using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsRuler : MonoBehaviour
{
    [SerializeField]
    Button playButton, exitButton, aboutButton, highscoreButton, leaveButton, stayButton;

    [SerializeField]
    GameObject leaveStayMenu;

    void Start()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        exitButton.onClick.AddListener(() => Exit());
        aboutButton.onClick.AddListener(() => SceneManager.LoadScene("About"));
        highscoreButton.onClick.AddListener(() => SceneManager.LoadScene("Highscore"));
        leaveButton.onClick.AddListener(() => Application.Quit());
        stayButton.onClick.AddListener(() => Stay());
    }

    void Exit()
    {
        exitButton.gameObject.SetActive(false);
        leaveStayMenu.SetActive(true);
    }

    void Stay()
    {
        exitButton.gameObject.SetActive(true);
        leaveStayMenu.SetActive(false);
    }

    void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
