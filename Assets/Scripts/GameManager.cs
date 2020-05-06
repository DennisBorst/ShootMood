using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI kills;
    [SerializeField] private TextMeshProUGUI score;

    [HideInInspector] public bool gameOver;

    private float timer;

    private void Update()
    {
        timer = Timer(timer);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }
    }

    public void GameOver()
    {
        gameOver = true;
        Time.timeScale = 0f;

        time.text = "Time survived: " + (int)timer + " sec.";
        kills.text = "Kills: " + ScoreManager.Instance.kills;
        score.text = "Score: " + ScoreManager.Instance.score;
        endPanel.SetActive(true);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private float Timer(float timer)
    {
        timer += Time.deltaTime;
        return timer;
    }

    #region Singleton
    private static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    #endregion
}
