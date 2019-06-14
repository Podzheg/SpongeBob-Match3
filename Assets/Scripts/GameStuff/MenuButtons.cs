using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField]
    Button menuButton, leaveButton, stayButton, sadToMenu, sadReset;

    [SerializeField]
    GameObject pauseScreen;

    [SerializeField]
    GameStats stats;

    void Start()
    {
        menuButton.onClick.AddListener(() => Pause());
        leaveButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        stayButton.onClick.AddListener(() => Stay());
        sadReset.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        sadToMenu.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }

    void Pause()
    {
        Time.timeScale = 0;
        stats.isEating = true;
        pauseScreen.SetActive(true);
    }

    void Stay()
    {
        stats.isEating = false;
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }
}
