using UnityEngine;
using TMPro; // 注意這行
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public float loseHeight = 1f; // 堆疊到這個高度算輸
    public TMP_Text gameOverText; // 連結 TMP Text
    public TMP_Text scoreText; // 連結分數顯示文本
    //重開按鈕
    public GameObject restartButton;
    public GameObject exitButton;
    public static int score = 0; // 分數

    public static bool isGameOver = false;
    public static List<Ball> activeBalls = new List<Ball>();

    void Start()
    {
        if(gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false); // 一開始隱藏
            exitButton.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true);
            MoveRestartToBottomLeft();
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            CheckLoseCondition();
        }
        scoreText.text = "Score: " + score;
        //esc退出遊戲
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainScene");
    }
    public static void RegisterBall(Ball ball)
    {
        activeBalls.Add(ball);
    }

    public static void UnregisterBall(Ball ball)
    {
        activeBalls.Remove(ball);
    }
    public static void AddScore(int points)
    {
        score += points;
    }

    void CheckLoseCondition()
    {   
        float highestY = float.MinValue;
        // 找所有球
        foreach (Ball b in activeBalls)
        {
            if (b == null) continue;

            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();

            if (!b.isDroped || b.isMerging || !rb.IsSleeping())
                continue;

            float topY = b.GetComponent<CircleCollider2D>().bounds.max.y;

            if (topY > highestY)
                highestY = topY;
        }
        if (highestY >= loseHeight)
        {
            Debug.Log("Lose Condition Met: HighestY = " + highestY);
            GameOver();
        }
    }
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // 暫停遊戲
        //將按鈕移到畫面中央
        if (restartButton != null)
        {
            restartButton.SetActive(true);
            MoveRestartToCenter();
        }
        if (exitButton != null)
        {
            exitButton.SetActive(true);
        }
        if(gameOverText != null)
            gameOverText.gameObject.SetActive(true);

    }
    void MoveRestartToBottomLeft()
    {
        RectTransform rect = restartButton.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.anchoredPosition = new Vector2(120, 60);
    }

    void MoveRestartToCenter()
    {
        RectTransform rect = restartButton.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.anchoredPosition = new Vector2(0, -120);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 重新載入當前場景
    }
    void Awake()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        score = 0;
    }
}