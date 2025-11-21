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
    
    private static int _combo;
    private static float _comboMultiplier;
    private static int _score;

    private void Start()
    {
        Instance = this;
        
        _combo = 0;

        hitText.enabled = false;
    }
    private void Update()
    {
        comboText.text = ($"COMBO: {_combo.ToString()}");
        scoreText.text = ($"{_score.ToString()}");

        //Combo Multiplier
        _comboMultiplier = _combo switch
        {
            >= 100 => 2,
            >= 75 => 1.75f,
            >= 50 => 1.5f,
            >= 25 => 1.25f,
            >= 10 => 1.1f,
            <= 0 => 1f,
            _ => _comboMultiplier
        };
    }
    
    public void PerfectHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(PerfectHitText());
        _combo++;

        _score += (int)(perfectHitScore * _comboMultiplier);
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }
    public void GoodHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(GoodHitText());
        _combo++;
        
        _score += (int)(goodHitScore * _comboMultiplier);
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }

    public void OkayHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(OkayHitText());
        _combo++;
        
        _score += (int)(okayHitScore * _comboMultiplier);
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }

    public void Miss()
    {
        Instance.missSound.Play();
        _combo = 0;
        
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
