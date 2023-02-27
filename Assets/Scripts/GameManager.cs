using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public Bird[] birdPrefabs;
    public float sqawnTime;
    public int timeLimit;

    int m_curTimeLimit;
    int m_birdKilled;
    bool m_isGameover;
    
    public global::System.Int32 BirdKilled { get => m_birdKilled; set => m_birdKilled = value; }
    public global::System.Boolean IsGameover { get => m_isGameover; set => m_isGameover = value; }

    public override void Awake()
    {
        MakeSingleton(false);

        m_curTimeLimit = timeLimit;
    }

    public override void Start()
    {
        m_curTimeLimit = timeLimit;

        GameGUIManager.Ins.ShowGameGui(false);
        GameGUIManager.Ins.UpdateKilledCounting(m_birdKilled);
    }

    public void PlayGame()
    {
        StartCoroutine(GameSqawn());

        StartCoroutine(TimeCountDown());

        GameGUIManager.Ins.ShowGameGui(true);
    }

    IEnumerator TimeCountDown()
    {
        while(m_curTimeLimit > 0)
        {
            yield return new WaitForSeconds(1f);

            m_curTimeLimit--;

            if(m_curTimeLimit <= 0)
            {
                m_isGameover = true;

                if(m_birdKilled > Prefs.bestScore)
                {
                    GameGUIManager.Ins.gameDialog.UpdateDialog("NEW BEST", "BEST KILLED : x" + m_birdKilled);
                }
                else
                {
                    GameGUIManager.Ins.gameDialog.UpdateDialog("KILLED : x" + m_birdKilled, "BEST KILLED : x" + Prefs.bestScore);
                }

                GameGUIManager.Ins.gameDialog.Show(true);
                GameGUIManager.Ins.CurDialog = GameGUIManager.Ins.gameDialog;

                Prefs.bestScore = m_birdKilled;
            }

            GameGUIManager.Ins.UpdateTimer(IntToTime(m_curTimeLimit));
        }
    }

    IEnumerator GameSqawn()
    {
        while(true)
        {
            SqawnBird();
            yield return new WaitForSeconds(sqawnTime);
        }
        
    }

    void SqawnBird()
    {
        Vector3 sqawnPos = Vector3.zero;

        float randCheck = Random.Range(0f, 1f);

        if(randCheck >= 0.5f)
        {
            sqawnPos = new Vector3(10, Random.Range(-0.5f, 3.5f), 0);
        }else
        {
            sqawnPos = new Vector3(-10, Random.Range(-0.5f, 3.5f), 0);
        }

        if(birdPrefabs != null && birdPrefabs.Length >0)
        {
            int randIdx = Random.Range(0, birdPrefabs.Length);

            if(birdPrefabs[randIdx] != null)
            {
                Bird birdClone = Instantiate(birdPrefabs[randIdx], sqawnPos, Quaternion.identity);
            }
        }
    }

    string IntToTime(int time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt((time +1) % 60);

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void ShotDie()
    {
        m_curTimeLimit += 2;
        GameGUIManager.Ins.UpdateTimer(IntToTime(m_curTimeLimit));
    }
}
