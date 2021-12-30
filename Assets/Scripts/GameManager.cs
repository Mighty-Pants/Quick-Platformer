using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Image[] lives;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Text TittleText;
    [SerializeField] Text DescriptionText;
    [SerializeField] Button PauseButton;
    [SerializeField] InputField inputName;
    [SerializeField] GameObject victoryMenu;
    [SerializeField] Animator transition;

    int livesRemaining = 4;
    int maxlives;
    int score = 0;
    float transitionTime = 1f;

    bool gameOver = false;
    bool isPause = false;

    private void Start()
    {
        maxlives = livesRemaining;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
       
        PlayerPrefs.SetInt("level", sceneIndex);
        score = PlayerPrefs.GetInt("score");
        scoreText.text = "Score: " + score;
        livesRemaining = PlayerPrefs.GetInt("life");
        for(int i = 0; i < 4; i++)
        {
            if (i < livesRemaining)
                lives[i].enabled = true;
            else lives[i].enabled = false;
        }
        AudioManager.instance.playBack(sceneIndex);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton.GetComponentInChildren<Text>().text = "Resume";
            if(isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void LoseLive()
    {
        if (livesRemaining == 0)
            return;

        livesRemaining--;

        //An image live co so thu tu bang voi liveremaining hien tai
        lives[livesRemaining].enabled = false;

        if (livesRemaining == 0)
            endGame();
    }

    public void addLive()
    {
        if(livesRemaining < maxlives)
        {         
            //Hien image live co so thu tu bang voi liveremaining hien tai
            lives[livesRemaining].enabled = true;
            livesRemaining++;
            
        }
    }
    public void updateScore(int point)
    {
        score += point;
        scoreText.text = "Score: " + score;
        Debug.Log(score);
    }

    public void endGame()
    {
        AudioManager.instance.playMusic("lose");
        gameOver = true;
        PlayerPrefs.SetInt("level", 0);
        TittleText.text = "Game Over";
        PauseButton.GetComponentInChildren<Text>().text = "Restart";
        //Kiem tra highscore
        int highscore = PlayerPrefs.GetInt("highscore");
        if (score > highscore)
        {
            DescriptionText.text = "You got new HighScore: " + score + "\nEnter your name";
            inputName.gameObject.SetActive(true);
        }
        else
        {
            DescriptionText.text = "Score: " + score;
        }
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void newHighScore()
    {
        string highScoreName = inputName.text;
        PlayerPrefs.SetString("highscorename", highScoreName);
        PlayerPrefs.SetInt("highscore", score);
        inputName.gameObject.SetActive(false);
        DescriptionText.text = "Congratulation " + highScoreName + "\nYour new HighScore: " + score;
    }
    public bool isOver()
    {
        return gameOver;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        TittleText.text = "Pause";
        DescriptionText.text = "Score: " + score;
        Time.timeScale = 0f;
        isPause = true;
    }

    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel()
    {
        if (!gameOver)
            Resume();
        else
        {
            Resume();
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.SetInt("score", 0);
            PlayerPrefs.SetInt("life", 4);
        }
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("life", livesRemaining);
        PlayerPrefs.SetInt("score", score);
        int lv = PlayerPrefs.GetInt("level");
        StartCoroutine(LoadNextLevel(lv));
        
    }

    IEnumerator LoadNextLevel(int lv)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(lv + 1);
    }
    public void victory()
    {
        AudioManager.instance.playMusic("victory");
        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
    }

    public void winGame()
    {
        victoryMenu.SetActive(false);
        gameOver = true;
        PlayerPrefs.SetInt("level", 0);
        TittleText.text = "Game Over";
        PauseButton.GetComponentInChildren<Text>().text = "Restart";
        TittleText.text = "Victory";
        //Kiem tra highscore
        int highscore = PlayerPrefs.GetInt("highscore");
        if (score > highscore)
        {
            DescriptionText.text = "You got new HighScore: " + score + "\nEnter your name";
            inputName.gameObject.SetActive(true);
        }
        else
        {
            DescriptionText.text = "Score: " + score;
        }
        pauseMenu.SetActive(true);
    }

}
