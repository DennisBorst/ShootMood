using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject[] heartIcons;
    private void Start()
    {
        scoreText.text = "Score: " + 0;
    }

    public void UpdateHealth(int health)
    {
        //healthText.text = "Health: " + health;

        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(false);
        }

        for (int i = 0; i < health; i++)
        {
            heartIcons[i].SetActive(true);
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    #region Singleton
    private static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }
    #endregion
}
