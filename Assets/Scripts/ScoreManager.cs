using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSound;
    public AudioSource missSound;
    public TMPro.TextMeshProUGUI scoreText;
    private static int comboScore;
    public GameObject scoreBoard;
    private bool songFinished = false;
    public TMPro.TextMeshProUGUI scoreBoardText;

    private void Start()
    {
        Instance = this;
        
        comboScore = 0;

        scoreBoard.SetActive(false);
    }

    public static void Hit()
    {
        Instance.hitSound.Play();
        comboScore++;
    }

    public static void Miss()
    {
        Instance.missSound.Play();
        comboScore = 0;
    }

    private void Update()
    {
        scoreText.text = comboScore.ToString();

        if (SongManager.GetAudioSourceTime() >= SongManager.Instance.audioSource.clip.length)
        {
            scoreBoard.SetActive(true);
            scoreBoardText.text = ($"Score: {scoreText.ToString()}");
        }
    }
}
