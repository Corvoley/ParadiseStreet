using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameManager gameManager;



    [SerializeField] private int numOfHearts;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    [SerializeField] private GameObject retryScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        scoreText.text = "0";
        gameManager.OnScoreUpdate += GameManager_OnScoreUpdate;
        gameManager.OnPlayerSpawned += GameManager_OnPlayerSpawned;
        gameManager.OnHealthDepleted += GameManager_OnHealthDepleted;
    }

    private void GameManager_OnHealthDepleted()
    {
        retryScreen.SetActive(true);
        finalScoreText.text = "Score:\n" + scoreText.text;
        scoreText.text = "0";
    }

    public void TooglePauseScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
    }


    private void GameManager_OnPlayerSpawned()
    {
        gameManager.OnHealthChanged += CheckHealth;
        numOfHearts = gameManager.Health;
        CheckHealth();
    }
    
    private void GameManager_OnScoreUpdate()
    {
        scoreText.text = gameManager.Score.ToString();
    }

    private void OnDestroy()
    {
        gameManager.OnScoreUpdate -= GameManager_OnScoreUpdate;
    }


    private void CheckHealth()
    {        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < gameManager.Health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }


}


