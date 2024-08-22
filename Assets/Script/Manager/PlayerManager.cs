using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public static bool isWinGame;

    public static bool gameOver;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject startingText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI distanceUI;
    public TextMeshProUGUI scoreGOV;
    public TextMeshProUGUI coinGOV;
    public TextMeshProUGUI scoreWin;
    public TextMeshProUGUI coinWin;
    public TextMeshProUGUI levelText;

    public GameObject highScoreGOV;
    public TextMeshProUGUI highScoreText;

    public static float highScore;

    public static bool isGameStarted;

    public TextMeshProUGUI distanceGOV;
    public float lastActivationDistance;
    private const float distanceThreshold = 500f;

    public static int coin;
    public static int score;
    private float poinsitionNow;

    public Transform player;

    private Vector3 lasPosition;
    private static float distanceS;

    private bool isHScore = false;
    public TextMeshProUGUI speedText;
    public float velocitySpeed;

    public Vector3 CoinNextPos { get; internal set; }

    private void Awake()
    {
        isWinGame = false;
        gameOver = false;
        gameOverPanel.SetActive(false);
        coin = 0;
        poinsitionNow = 0;
        score = 0;
        lastActivationDistance = 0f;
        isHScore = false;
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }
    void Start()
    {
        


        lasPosition = player.position;
        isGameStarted = false;
        lastActivationDistance = 0f;
        if (TileManager.currentScene == "Play")
        {
            highScoreText.text = $"{highScore}";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceS = Vector3.Distance(player.transform.position, lasPosition);
        DistanceRun(distanceS);
        lasPosition = player.transform.position;
        if (isGameStarted && !gameOver)
        {
            ScoreRun();
        }
        if (score >= highScore && TileManager.currentScene == "Play")
        {
            isHScore = true;
            highScore = score;
            highScoreText.text = $"{highScore}";
        }


        if (isWinGame)
        {
            WinGameText();
        }

        if (gameOver)
        {
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
            gameOverPanel.SetActive(true);
            GameOverText();
        }

        if (poinsitionNow >= lastActivationDistance + distanceThreshold)
        {
            lastActivationDistance += distanceThreshold;
            distanceUI.text = $"{ lastActivationDistance} m ";
            
            StartCoroutine(ActivateDistanceUI());
            
        }
        if (TileManager.currentScene == "Play")
        {
            velocitySpeed = poinsitionNow / 6;
            speedText.text = $"{velocitySpeed:00.000} km/h";
        }
        

        scoreText.text = $"{score}";
        coinText.text = $"{coin}";

        if (UIManager.isSaveMe)
        {
            gameOver = false;
            gameOverPanel.SetActive(false);
            StartCoroutine(SaveMeCD());
        }

        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }

    }


    private IEnumerator SaveMeCD()
    {
        yield return new WaitForSeconds(2f);
        UIManager.isSaveMe = false;

    }
    public static int TakeCoin(int x)
    {
        return coin += x;
    }
    public int ScoreRun()
    {
        if (!UIManager.isPause && !isWinGame)
        {
            return score += 1;
        }
        else return score;
    }
    public float DistanceRun(float x)
    {
        return poinsitionNow += x;
    }
    private IEnumerator ActivateDistanceUI()
    {
        distanceUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        distanceUI.gameObject.SetActive(false);
    }

    private void WinGameText()
    {
        winPanel.SetActive(true);
        scoreWin.text = scoreText.text;
        coinWin.text = $"Coin: {coin}";
        levelText.text = $"Level{TileManager.indexLevel}";
    }

    private void GameOverText()
    {
        if (isHScore)
        {
            highScoreGOV.SetActive(true);
        }
        distanceUI.gameObject.SetActive(false);

        scoreGOV.text = scoreText.text;
        coinGOV.text = $"Coin: {coin}";
        distanceGOV.text = $"Distance: {(int)poinsitionNow}m";
    }
    public static void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
