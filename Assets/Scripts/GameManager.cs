using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");

        
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("You win!");
        Time.timeScale = 0f;
    }

    public bool IsGameActive()
    {
        return !isGameOver;
    }
}