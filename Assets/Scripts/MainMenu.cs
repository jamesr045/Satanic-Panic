using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image mainMenuTransform;
    
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

     public void QuitGame()
     {
        Application.Quit();
     }
}
