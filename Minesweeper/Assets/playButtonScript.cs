using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playButtonScript : MonoBehaviour
{
    // Handles all button click events in Main Menu Scene
    // Also Handles the showing of Highscores in every difficulty level

    public GameObject easyBtn;
    public GameObject mediumBtn;
    public GameObject hardBtn;
    public Text easyHighscoreText;
    public Text mediumHighscoreText;
    public Text hardHighscoreText;


    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs are a storage solution from Unity to easily save primitive data types
        if (PlayerPrefs.HasKey("easyHighscore")) {

            easyHighscoreText.text = "Best time:\n";
            TimeSpan time = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("easyHighscore"));
            easyHighscoreText.text += time.ToString(@"mm\:ss\:fff");
        }
        else 
            easyHighscoreText.text = "";

        if (PlayerPrefs.HasKey("mediumHighscore")) {
            mediumHighscoreText.text = "Best time:\n";
            TimeSpan time = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("mediumHighscore"));
            mediumHighscoreText.text += time.ToString(@"mm\:ss\:fff");
        }
        else
            mediumHighscoreText.text = "";

        if (PlayerPrefs.HasKey("hardHighscore")) {
            hardHighscoreText.text = "Best time:\n";
            TimeSpan time = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("hardHighscore"));
            hardHighscoreText.text += time.ToString(@"mm\:ss\:fff");
        }
        else
            hardHighscoreText.text = "";

;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playButtonClick() {
        easyBtn.SetActive(true);
        mediumBtn.SetActive(true);
        hardBtn.SetActive(true);
    }
    public void changeScene() {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void easyButtonClick() {
        GameManagerScript.instance.gameMode = "easy";
        
        changeScene();
    }
    public void mediumButtonClick() {
        GameManagerScript.instance.gameMode = "medium";
        changeScene();
    }
    public void hardButtonClick() {
        GameManagerScript.instance.gameMode = "hard";
        changeScene();
    }
    public void quitBtnClick() {
        Application.Quit();
    }
    public void clearHighscoresBtnClick() {
        PlayerPrefs.DeleteAll();
    }

    

}
