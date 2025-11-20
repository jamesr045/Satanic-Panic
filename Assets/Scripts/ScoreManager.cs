using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSound;
    public AudioSource missSound;
    public TMPro.TextMeshPro scoreText;
    public TMPro.TextMeshPro hitText;
    
    private static int _comboScore;

    private void Start()
    {
        Instance = this;
        
        _comboScore = 0;

        hitText.enabled = false;
    }
    
    public void PerfectHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(PerfectHitText());
        _comboScore++;
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }
    public void GoodHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(GoodHitText());
        _comboScore++;
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }

    public void OkayHit()
    {
        Instance.hitSound.Play();
        StartCoroutine(OkayHitText());
        _comboScore++;
        
        SongManager.Instance.characterPlayingAudioSource.mute = false;
        SongManager.Instance.characterMissingAudioSource.mute = true;
    }

    public void Miss()
    {
        Instance.missSound.Play();
        _comboScore = 0;
        
        SongManager.Instance.characterPlayingAudioSource.mute = true;
        SongManager.Instance.characterMissingAudioSource.mute = false;
    }

    private void Update()
    {
        scoreText.text = ($"COMBO: {_comboScore.ToString()}");
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
