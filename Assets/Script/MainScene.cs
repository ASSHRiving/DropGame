using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
