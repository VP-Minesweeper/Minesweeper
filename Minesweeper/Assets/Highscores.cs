using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscores : MonoBehaviour
{
    // Handles HighScores and handles 2 button click events
    public StopwatchScript stopWatch;
    public Text highScoreText;
    public Text checkWinText;
    void Start() // HighScores starts functioning when gameOverPannel pannel is enabled
    {
        if (checkWinText.text.Equals("You WIN!")) { // scores are counted  only if player wins
            updateHighScore();
        } else {
            highScoreText.text = "";
        }

    }
    public void updateHighScore() { // Checks if current score is a HighScore and gives feedback to the player with text HIGHSCORE!
        highScoreText.text = "";
        if (GameManagerScript.instance.gameMode.Equals("easy")) {
            if (!PlayerPrefs.HasKey("easyHighscore")) {
                PlayerPrefs.SetFloat("easyHighscore", stopWatch.currentTime);
                highScoreText.text = "HIGHSCORE!";
            }
            else if (PlayerPrefs.GetFloat("easyHighscore") >= stopWatch.currentTime) {
                PlayerPrefs.SetFloat("easyHighscore", stopWatch.currentTime);
                highScoreText.text = "HIGHSCORE!";
            } else {
                highScoreText.text = "";
            }
        }

        if (GameManagerScript.instance.gameMode.Equals("medium")) {
            if (!PlayerPrefs.HasKey("mediumHighscore")) {
                PlayerPrefs.SetFloat("mediumHighscore", stopWatch.currentTime);
                highScoreText.text = "HIGHSCORE!";
            }
            else if (PlayerPrefs.GetFloat("mediumHighscore") >= stopWatch.currentTime) {
                PlayerPrefs.SetFloat("mediumHighscore", stopWatch.currentTime);
                highScoreText.text = "HIGHSCORE!";
            }
            else {
                highScoreText.text = "";
            }
        }

        if (GameManagerScript.instance.gameMode.Equals("hard")) {
            if (!PlayerPrefs.HasKey("hardHighscore")) {
                PlayerPrefs.SetFloat("hardHighscore", stopWatch.currentTime);
                highScoreText.text = "HIGHSCORE!";
            }
            else if (PlayerPrefs.GetFloat("hardHighscore") >= stopWatch.currentTime) {
                PlayerPrefs.SetFloat("hardHighscore", stopWatch.currentTime);
                highScoreText.text = "HIGHSCORE!";
            }
            else {
                highScoreText.text = "";
            }
        }
    }

    public void menuClick() { 
        SceneManager.LoadScene("Main Menu");
    }
    public void retryClick() {
        SceneManager.LoadScene("SampleScene");
    }
}
