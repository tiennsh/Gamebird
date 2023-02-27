using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameGUIManager : Singleton<GameGUIManager>
{
    public GameObject homeGui;
    public GameObject gameGui;
    public GameObject pauseButton;

    public Dialog gameDialog;
    public Dialog pauseDialog;

    public Image fireRateFilled;
    public Text timerText;
    public Text killedCountingText;

    Dialog m_curDialog;

    public Dialog CurDialog { get => m_curDialog; set => m_curDialog = value; }

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowGameGui(bool isShow)
    {
        if(gameGui)
            gameGui.SetActive(isShow);
        if(homeGui)
            homeGui.SetActive(!isShow);
    }

    public void UpdateTimer(string time)
    {
        if (timerText)
            timerText.text = time;
    }

    public void UpdateKilledCounting(int killed)
    {
        if(killedCountingText)
            killedCountingText.text = "x" + killed.ToString();
    }

    public void UpdateFireRate(float rate)
    {
        if(fireRateFilled)
            fireRateFilled.fillAmount = rate;
    }

    public void ContinueGame()
    {
        Time.timeScale = 0f;

        pauseButton.SetActive(false);

        if (pauseDialog)
        {
            pauseDialog.Show(true);
            pauseDialog.UpdateDialog("GAME PAUSE", "BEST KILLED : x" + Prefs.bestScore);
            m_curDialog = pauseDialog;
        }
            
    }

    public void ResumeGame()
    {
        if (pauseDialog)
            m_curDialog.Show(false);

        Time.timeScale = 1f;

    }

    public void BackToHome()
    {
        ResumeGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Replay()
    {
        if (m_curDialog)
            m_curDialog.Show(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ExitGame()
    {
        ResumeGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Application.Quit();
    }
}
