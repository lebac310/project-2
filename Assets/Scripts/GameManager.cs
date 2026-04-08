using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    public void GameOver(string reason)
    {
        Debug.Log("Game Over: " + reason);

        // dừng game
        Time.timeScale = 0f;
    }
}