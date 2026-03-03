using UnityEngine;
using TMPro; // 注意這行
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float loseHeight = 1f; // 堆疊到這個高度算輸
    public TMP_Text gameOverText; // 連結 TMP Text
    public TMP_Text scoreText; // 連結分數顯示文本
    //重開按鈕
    public GameObject restartButton;
    public static int score = 0; // 分數

    public static bool isGameOver = false;

    void Start()
    {
        if(gameOverText != null)
            gameOverText.gameObject.SetActive(false); // 一開始隱藏

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
            Application.Quit();
        }
    }
    public static void AddScore(int points)
    {
        score += points;
    }

    void CheckLoseCondition()
    {
        // 找所有球
        Ball[] allBalls = FindObjectsByType<Ball>(FindObjectsSortMode.None);

        float highestY = float.MinValue;

        foreach (Ball b in allBalls)
        {
            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();

            if(b.isDroped && rb != null && rb.linearVelocity.magnitude < 0.1f && !b.isMerging) // 只考慮已落地且靜止的球
            {
                float topY = b.transform.position.y + b.GetComponent<CircleCollider2D>().radius;
                if(topY > highestY)
                    highestY = topY;
            }
        }

        // 如果最高球超過 loseHeight → 遊戲結束
        if(highestY >= loseHeight)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // 暫停遊戲
        //將按鈕移到畫面中央
        if (restartButton != null)
        {
            restartButton.SetActive(true);
            MoveRestartToCenter();
        }
        if(gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        Debug.Log("Game Over!");
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