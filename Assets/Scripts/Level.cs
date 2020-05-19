using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 3.5f;

   public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());

    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("GameOver"); 
    }

    public void LoadStartMenu()
    {
        FindObjectOfType<GameSession>().ResetScore();
        SceneManager.LoadScene("StartMenu");

    }

    public void QuitGame()
    {
        Application.Quit();

    }
}
