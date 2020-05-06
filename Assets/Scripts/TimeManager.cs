using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    [HideInInspector] public bool timeStopped = false;

    private float currentTime;
    private bool slowmotion = false;

    private float oldFixedDeltaTime;

    private void Start()
    {
        oldFixedDeltaTime = Time.fixedDeltaTime;
        Debug.Log(oldFixedDeltaTime);
        ResetTime();
    }

    private void Update()
    {
        if (slowmotion)
        {
            currentTime = Timer(currentTime);

            if(currentTime <= 0)
            {
                ResetTime();
            }
        }
    }

    public void DoSlowMotionPlayerHit()
    {
        if (GameManager.Instance.gameOver) { return; }

        slowmotion = true;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.deltaTime * 0.2f;
        currentTime = slowdownLength;
    }

    public void DoSlowMotionEnemyHit()
    {
        if (GameManager.Instance.gameOver) { return; }

        slowmotion = true;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.deltaTime * 0.2f;
        currentTime = 0.04f;
    }

    public void ChangeTime(bool stopTime)
    {
        if (stopTime)
        {
            timeStopped = true;
        }
        else
        {
            timeStopped = false;
            ResetTime();
        }
    }

    private void ResetTime()
    {
        slowmotion = false;
        currentTime = slowdownLength;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = oldFixedDeltaTime;
    }

    private float Timer(float timer)
    {
        timer -= Time.deltaTime;
        return timer;
    }

    #region Singleton
    private static TimeManager instance;
    private void Awake()
    {
        instance = this;
    }
    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimeManager();
            }
            return instance;
        }
    }
    #endregion
}
