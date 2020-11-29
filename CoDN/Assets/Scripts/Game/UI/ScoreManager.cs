using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text codeSizeText;
    [SerializeField] private Text totalTasksText;
    public Animator animator;

    private int currentLevel;
    private GameHandler gamehandler;
    public void ShowScore(int codeSize, int totalTasks, GameHandler g)
    {
        animator.SetBool("isOpen", true);
        currentLevel = PlayerPrefs.GetInt(GameUtility.selectedLevelKey);
        codeSizeText.text = codeSize.ToString();
        totalTasksText.text = totalTasks.ToString();
        gamehandler = g;
        gamehandler.ClearGameState();
    }

    public void NextLevel()
    {         
        PlayerPrefs.SetInt(GameUtility.selectedLevelKey, currentLevel+1);
        gamehandler.initGameState();
        animator.SetBool("isOpen", false);
    }

    public void ReturnMenu()
    {
        animator.SetBool("isOpen", false);
        SceneManager.LoadScene("MenuScene");
    }
}
