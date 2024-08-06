using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;
    public GameObject startingText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI distanceUI;
    public TextMeshProUGUI scoreGOV;
    public TextMeshProUGUI coinGOV;

    public TextMeshProUGUI highScoreTextGOV;
    public TextMeshProUGUI highScoreText;

    public static float highScore;

    public static bool isGameStarted;

    public float lastActivationDistance;
    private const float distanceThreshold = 500f;

    public static int coin;
    public static float score;

    public Transform player;

    private Vector3 lasPosition;
    private static float distanceS;


    private void Awake()
    {
        gameOver = false;
        gameOverPanel.SetActive(false);
        coin = 0;
        score = 0;
        lastActivationDistance = 0f;
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }
    void Start()
    {
        lasPosition = player.position;
        isGameStarted = false;
        lastActivationDistance = 0f;

        highScoreText.text = $"High Score: {highScore}";
    }

    // Update is called once per frame
    void Update()
    {

        distanceS = Vector3.Distance(player.transform.position, lasPosition);
        ScoreRun(distanceS);
        lasPosition = player.transform.position;

        if (score >= highScore)
        {
            highScore = score;
            highScoreText.text = $"High Score: {highScore}";
        }
        if (gameOver)
        {
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
            gameOverPanel.SetActive(true);
            GameOverText();
        }

        if (score >= lastActivationDistance + distanceThreshold)
        {
            lastActivationDistance += distanceThreshold;
            distanceUI.text = $"{ lastActivationDistance} m ";
            
            StartCoroutine(ActivateDistanceUI());
            
        }

        scoreText.text = $"Score: {score}";
        coinText.text = $"Coin: {coin}";

       
        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }

    }

    public static int TakeCoin(int x)
    {
        return coin += x;
    }
    public static float ScoreRun(float x)
    {
        return score += x;
    }
    private IEnumerator ActivateDistanceUI()
    {
        distanceUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        distanceUI.gameObject.SetActive(false);
    }
    private void GameOverText()
    {
        distanceUI.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        coinText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        scoreGOV.text = scoreText.text;
        coinGOV.text = coinText.text;
        highScoreTextGOV.text = $"HighScore: {highScore}";
    }
    public static void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
