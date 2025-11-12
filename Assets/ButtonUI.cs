using UnityEngine;

public class ButtonUI : MonoBehaviour

{
    public GameObject playButton;
    public GameObject songSelectUI;
    public GameObject Songs;

    public void playGame()
    {
        playButton.SetActive(false);
        songSelectUI.SetActive(true);
    }

    public void playSongs()
    {
        songSelectUI.SetActive(false);
        Songs.SetActive(true);
    }
}


