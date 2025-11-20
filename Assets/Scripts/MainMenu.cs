using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    
     public void PlaySunsetSupernova()
     {
        SongManager.Instance.StartRhythmGame();
        SongManager.Instance.currentSong = SongManager.Songs.SunsetSupernova;
        
        mainMenuCanvas.SetActive(false);
     }
     
     public void PlaySubways()
     {
         SongManager.Instance.StartRhythmGame();
         SongManager.Instance.currentSong = SongManager.Songs.Subways;
         
         mainMenuCanvas.SetActive(false);
     }
     
     public void PlayNextStopPurgatory()
     {
         SongManager.Instance.StartRhythmGame();
         SongManager.Instance.currentSong = SongManager.Songs.NextStopPurgatory;
         
         mainMenuCanvas.SetActive(false);
     }

     public void QuitGame()
     {
        Application.Quit();
     }
}
