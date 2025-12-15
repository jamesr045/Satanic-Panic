using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    
    [Header("AudioSources")]
    public AudioSource backgroundAudioSource;
    public AudioSource characterPlayingAudioSource;
    public AudioSource characterMissingAudioSource;
    
    
    public enum Songs
    {
        SunsetSupernova,
        Subways,
        NextStopPurgatory
    }

    [Header("Song Selection")]
    public Songs currentSong;
    
    [Header("Song Files")]
    public AudioClip[] backgroundSongs;
    public AudioClip[] characterPlayingSongs;
    public AudioClip[] characterMissingSongs;
    public string[] midiFilePaths;
    public string songFileLocation;
    public static MidiFile MidiFile;
    
    [Header("Selected Song Files")]
    public AudioClip selectedSongBg;
    public AudioClip selectedSongHit;
    public AudioClip selectedSongMiss;
    
    [Header("Lanes")]
    public Lane[] lanes;
    
    public Transform rhythmTrackPos;
    public Countdown countdown;
    
    [Header("Settings")]
    public float songDelayInSeconds;
    public double okayMarginOfError;
    public double goodMarginOfError;
    public double perfectMarginOfError;
    public float inputDelayInSeconds;
    public float noteTimeUntilHit;

    public bool songStarted;
    private bool _songOver;
    

    
    
    private void Start()
    {
        Instance = this;
    }

    public void StartRhythmGame()
    {
        switch (currentSong)
        {
            case Songs.SunsetSupernova:
                print("Selected Song: Sunset Supernova");
                selectedSongBg = backgroundSongs[0];
                selectedSongHit = characterPlayingSongs[0];
                selectedSongMiss = characterMissingSongs[0];
                songFileLocation = midiFilePaths[0];
                break;
            case Songs.Subways:
                print("Selected Song: Subways");
                selectedSongBg = backgroundSongs[1];
                selectedSongHit = characterPlayingSongs[1];
                selectedSongMiss = characterMissingSongs[1];
                songFileLocation = midiFilePaths[1];
                break;
            case Songs.NextStopPurgatory:
                print("Selected Song: Next Stop: Purgatory");
                selectedSongBg = backgroundSongs[2];
                selectedSongHit = characterPlayingSongs[2];
                selectedSongMiss = characterMissingSongs[2];
                songFileLocation = midiFilePaths[2];
                break;
        }
        
        ScoreManager.Instance.ResetScores();
        
        countdown.StartCountdown();
        
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }
    
    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + songFileLocation))
        {
            yield return www.SendWebRequest();

            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    MidiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }
    
    private void ReadFromFile()
    {
        MidiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + songFileLocation);
        GetDataFromMidi();
    }
    
    public void GetDataFromMidi()
    {
        var notes = MidiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
        {
            lane.SetTimeStamps(array);
        }
        
        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        backgroundAudioSource.clip = selectedSongBg;
        backgroundAudioSource.Play();
        StartCoroutine(WaitForSongEnd(backgroundAudioSource.clip.length));
        
        characterPlayingAudioSource.clip = selectedSongHit;
        characterPlayingAudioSource.Play();
        
        characterMissingAudioSource.clip =  selectedSongMiss;
        characterMissingAudioSource.Play();

        characterPlayingAudioSource.mute = false;
        characterMissingAudioSource.mute = true;
        
        songStarted = true;
    }

    public static double GetAudioSourceTime()
    {
        if (Instance.backgroundAudioSource.clip != null)
        {
            return (double)Instance.backgroundAudioSource.timeSamples / Instance.backgroundAudioSource.clip.frequency;
        }
        else return 0;
    }

    // private void Update()
    // {
    //     if (!backgroundAudioSource.isPlaying && !_songOver && songStarted)
    //     {
    //         _songOver = true;
    //         songStarted = false;
    //         Debug.Log("Song Over");
    //         StartCoroutine(EndOfSong());
    //     }
    // }

    IEnumerator WaitForSongEnd(float songLength)
    {
        yield return new WaitForSeconds(songLength);
        
        Debug.Log("Song Over");
        StartCoroutine(EndOfSong());
    }

    IEnumerator EndOfSong()
    {
        yield return new WaitForSeconds(1f);
        
        MainMenu.Instance.OpenScoreScreen();
    }
}
