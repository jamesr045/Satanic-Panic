using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Components")]
    public static ScoreManager Instance;
    public AudioSource hitSound;
    public AudioSource missSound;
    public TMPro.TextMeshPro comboText;
    public TMPro.TextMeshPro scoreText;
    public TMPro.TextMeshPro hitText;

    [Header("Settings")] 
    public float perfectHitScore;
    public float goodHitScore;
    public float okayHitScore;
    
    public static int Combo;
    public static int MaxCombo;
    private static float _comboMultiplier;
    public static int Score;

    private void Start()
    {
        Instance = this;
        
        Combo = 0;

        hitText.enabled = false;
    }
    private void Update()
    {
        comboText.text = ($"COMBO: {Combo.ToString()}");
        scoreText.text = ($"{Score.ToString()}");

        //Combo Multiplier
        _comboMultiplier = Combo switch
        {
            >= 100 => 2,
            >= 75 => 1.75f,
            >= 50 => 1.5f,
            >= 25 => 1.25f,
            >= 10 => 1.1f,
            <= 0 => 1f,
            _ => _comboMultiplier
        };
        
        if (Combo > MaxCombo) MaxCombo = Combo;
    }
    
    public void PerfectHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(PerfectHitText());
        Combo++;

        Debug.Log(MaxCombo);

        Score += (int)(perfectHitScore * _comboMultiplier);
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }
    public void GoodHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(GoodHitText());
        Combo++;
        
        Debug.Log(MaxCombo);
        
        Score += (int)(goodHitScore * _comboMultiplier);
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }

    public void OkayHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(OkayHitText());
        Combo++;
        
        Debug.Log(MaxCombo);
        
        Score += (int)(okayHitScore * _comboMultiplier);
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }

    public void Miss()
    {
        Instance.missSound.Play();
        Combo = 0;
        
        SongManager.Instance.characterPlayingAudioSource.mute = true;
        SongManager.Instance.characterMissingAudioSource.mute = false;
    }

    
    private IEnumerator PerfectHitText()
    {
        hitText.text = "PERFECT!";
        hitText.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitText.enabled = false;

        if (SongManager.GetAudioSourceTime() >= SongManager.Instance.backgroundAudioSource.clip.length)
        {
            //End of song
        }
    }
    
    private IEnumerator GoodHitText()
    {
        hitText.text = "Good!";
        hitText.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitText.enabled = false;

        if (SongManager.GetAudioSourceTime() >= SongManager.Instance.backgroundAudioSource.clip.length)
        {
            //End of song
        }
    }

    private IEnumerator OkayHitText()
    {
        hitText.text = "Okay";
        hitText.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitText.enabled = false;

        if (SongManager.GetAudioSourceTime() >= SongManager.Instance.backgroundAudioSource.clip.length)
        {
            //End of song
        }
    }
}
