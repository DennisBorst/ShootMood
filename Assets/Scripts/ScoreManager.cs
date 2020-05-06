using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float scoreTimer;
    [SerializeField] private int scoreIncrease;

    [HideInInspector] public int score = 0;
    [HideInInspector] public int kills = 0;
    private float currentFrame = 1;

    private void Start()
    {
        currentFrame = scoreTimer;
    }

    private void Update()
    {
        currentFrame = Timer(currentFrame);
        if(currentFrame <= 0)
        {
            currentFrame = scoreTimer;
            score += scoreIncrease;
            UIManager.Instance.UpdateScore(score);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        kills++;
        UIManager.Instance.UpdateScore(score);
    }

    private float Timer(float timer)
    {
        timer -= Time.deltaTime;
        return timer;
    }

    #region Singleton
    private static ScoreManager instance;
    private void Awake()
    {
        instance = this;
    }
    public static ScoreManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ScoreManager();
            }
            return instance;
        }
    }
    #endregion
}
