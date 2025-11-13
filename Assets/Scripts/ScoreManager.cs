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

    public void Hit()
    {
        Instance.hitSound.Play();
        StartCoroutine(HitText());
        _comboScore++;
    }

    public void Miss()
    {
        Instance.missSound.Play();
        _comboScore = 0;
    }

    private void Update()
    {
        scoreText.text = ($"COMBO: {_comboScore.ToString()}");
    }

    private IEnumerator HitText()
    {
        hitText.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitText.enabled = false;

        if (SongManager.GetAudioSourceTime() >= SongManager.Instance.audioSource.clip.length)
        {
            //End of song
        }
    }
}
