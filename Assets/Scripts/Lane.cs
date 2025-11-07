using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteInput;
    private InputAction input;
    
    public GameObject notePrefab;

    private List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    
    private int spawnIndex = 0;
    private int inputIndex = 0;
    
    [Header("Hit Positions")]
    public Transform laneHitPos;
    public Transform laneDespawnPos;
    
    public Vector3 laneHit;
    public Vector3 laneDespawn;

    [Header("Visual Hit Response")] 
    public GameObject laneHitDisplay;
    private SpriteRenderer _laneHitSprite;

    public Color defaultHitPointColour;
    public Color hitColour;
    


    private void Awake()
    {
        if (this.CompareTag("Lane 1")) input = InputSystem.actions.FindAction("Lane 1 Press");
        if (this.CompareTag("Lane 2")) input = InputSystem.actions.FindAction("Lane 2 Press");
        if (this.CompareTag("Lane 3")) input = InputSystem.actions.FindAction("Lane 3 Press");
        if (this.CompareTag("Lane 4")) input = InputSystem.actions.FindAction("Lane 4 Press");
        
        laneHit = laneHitPos.position;
        laneDespawn = laneDespawnPos.position;
        _laneHitSprite = laneHitDisplay.GetComponent<SpriteRenderer>();
    }
    
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteInput)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    
    private void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTimeUntilHit)
            {
                var note = Instantiate(notePrefab, transform.position,  notePrefab.transform.rotation);
                var noteScript = note.GetComponent<Note>();
                notes.Add(noteScript);
                noteScript.assignedTime = (float)timeStamps[spawnIndex];
                noteScript.spawnPos = transform.position;
                noteScript.despawnPos = laneDespawn;
                
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp =  timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInSeconds / 1000.0);
    
            if (input.triggered)
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Missed {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    Miss();
                }
            }

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }

        if (input.triggered)
        {
            //Change colour of hit line for the lane
            StartCoroutine(HitColour());
        }
    }

    private void Hit()
    {
        //Play hit sound/effects
        ScoreManager.Instance.Hit();
    }

    private void Miss()
    {
        //Play miss sound/effects
        ScoreManager.Instance.Miss();
    }


    private IEnumerator HitColour()
    {
        Debug.Log("button pressed");
        _laneHitSprite.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        _laneHitSprite.color = Color.white;
    }
}
