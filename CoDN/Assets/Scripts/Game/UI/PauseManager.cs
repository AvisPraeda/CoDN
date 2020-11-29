using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private float timeScale;
    [SerializeField] private GameObject pausePanel;
    public void pauseButton()
    {
        pausePanel.SetActive(true);
        timeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void continueButton()
    {        
        Time.timeScale = timeScale;
        pausePanel.SetActive(false);
    }

    public void menuButton()
    {
        Time.timeScale = timeScale;
        SceneManager.LoadScene("MenuScene");
    }
}
