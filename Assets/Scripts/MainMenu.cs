using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public Image mainMenuTransform;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    [Header("Screens")]
    public GameObject mainMenu;
    public GameObject songSelection;
    public GameObject settingsMenu;
    public GameObject volumeMenu;
    public GameObject scoreScreen;

    private void Start()
    {
        Instance = this;

        mainMenu.SetActive(true);
        songSelection.SetActive(false);
        settingsMenu.SetActive(false);
        volumeMenu.SetActive(false);
        scoreScreen.SetActive(false);
    }

    public void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
    }

    public void PlaySunsetSupernova()
     {
        SongManager.Instance.currentSong = SongManager.Songs.SunsetSupernova;
        SongManager.Instance.StartRhythmGame();
        
        CloseMenu();
     }
     
     public void PlaySubways()
     {
         SongManager.Instance.currentSong = SongManager.Songs.Subways;
         SongManager.Instance.StartRhythmGame();
         
         CloseMenu();
     }
     
     public void PlayNextStopPurgatory()
     {
         SongManager.Instance.currentSong = SongManager.Songs.NextStopPurgatory;
         SongManager.Instance.StartRhythmGame();
         
         CloseMenu();
     }

     public void OpenMenu()
     {
         Debug.Log("Opening Menu");
         
         mainMenuTransform.transform.DOMoveY(0,1);
     }
     
     public void CloseMenu()
     {
         Debug.Log("Closing Menu");

         mainMenuTransform.transform.DOMoveY(500,1);
         //mainMenuTransform.DOFade(0, 1);
     }

     public void OpenScoreScreen()
     {
         mainMenu.SetActive(false);
         songSelection.SetActive(false);
         settingsMenu.SetActive(false);
         volumeMenu.SetActive(false);
         scoreScreen.SetActive(true);

         scoreText.text = ($"Final Score: {ScoreManager.Score}");
         comboText.text = ($"Highest Combo: {ScoreManager.MaxCombo}");
         
         Debug.Log("Opening Score Screen");
         mainMenuTransform.transform.DOMoveY(0,1);
     }

     public void QuitGame()
     {  
        Application.Quit();
     }
}
