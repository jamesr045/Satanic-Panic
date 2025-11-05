using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    private InputAction input;
    
    public GameObject notePrefab;

    private List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    
    private int spawnIndex = 0;
    private int inputIndex = 0;

    public Transform laneHitPos;
    public Transform laneDespawnPos;
    
    public Vector3 laneHit;
    public Vector3 laneDespawn;


    private void Awake()
    {
        if (this.CompareTag("Lane 1")) input = InputSystem.actions.FindAction("Lane 1 Press");
        if (this.CompareTag("Lane 2")) input = InputSystem.actions.FindAction("Lane 2 Press");
        if (this.CompareTag("Lane 3")) input = InputSystem.actions.FindAction("Lane 3 Press");
        if (this.CompareTag("Lane 4")) input = InputSystem.actions.FindAction("Lane 4 Press");
        
        laneHit = laneHitPos.position;
        laneDespawn = laneDespawnPos.position;
    }
    
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
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
                var note = Instantiate(notePrefab, transform);
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
                }
            }

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }

    private void Hit()
    {
        //Play hit sound/effects
        ScoreManager.Hit();
    }

    private void Miss()
    {
        //Play miss sound/effects
        ScoreManager.Miss();
    }
}
