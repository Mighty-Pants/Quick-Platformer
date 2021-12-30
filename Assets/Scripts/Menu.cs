using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] Text highScoreText;
    void Start()
    {
        int highscore = PlayerPrefs.GetInt("highscore");
        string temp = "";
        if(highscore > 0)
        {
            temp = "HighScore: " + highscore + "\nBy " + PlayerPrefs.GetString("highscorename");
        }
        highScoreText.text = temp;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("life", 4);
    }

    public void ResumeGame()
    {
        int level = PlayerPrefs.GetInt("level");
        if(level > 0)
        {
            SceneManager.LoadScene(level);
        } 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
