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
    public List<Tuple<double, double, bool>> timeStamps = new List<Tuple<double, double, bool>>();
    
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

    [Header("Character Animations")]
    public Animator characterAnimator;
    public AnimationClip hitAnimationClip;


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
                var tempoMap = SongManager.MidiFile.GetTempoMap();
                var noteOnMetricTime = TimeConverter.ConvertTo<MetricTimeSpan>(note.GetTimedNoteOnEvent().Time, tempoMap);
                var noteOffMetricTime = TimeConverter.ConvertTo<MetricTimeSpan>(note.GetTimedNoteOffEvent().Time, tempoMap);

                
                timeStamps.Add(new Tuple<double, double, bool>(
                    (double)noteOnMetricTime.Minutes * 60f + (noteOnMetricTime.Seconds + SongManager.Instance.inputDelayInSeconds) + (double)noteOnMetricTime.Milliseconds / 1000f,
                    (double)noteOffMetricTime.Minutes * 60f + (noteOffMetricTime.Seconds + SongManager.Instance.inputDelayInSeconds) + (double)noteOffMetricTime.Milliseconds / 1000f, note.Length > 128));
            }
        }
    }
    
    private void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex].Item1 - SongManager.Instance.noteTimeUntilHit + SongManager.Instance.inputDelayInSeconds)
            {
                var note = Instantiate(notePrefab, transform.position,  notePrefab.transform.rotation);
                var noteScript = note.GetComponent<Note>();
                notes.Add(noteScript);

                //Normal notes
                noteScript.assignedOnTime = (float)timeStamps[spawnIndex].Item1;
                noteScript.spawnPos = transform.position;
                noteScript.despawnPos = laneDespawn; 
                
                
                if (timeStamps[spawnIndex].Item3) // Check if it is a hold note
                {
                    noteScript.isHoldNote = true;
                    
                    Debug.Log("Hold note");
                    noteScript.assignedOffTime = (float)timeStamps[spawnIndex].Item2;
                    
                    note.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                
                spawnIndex++;
                
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            if (!timeStamps[inputIndex].Item3)
            {
                //Normal notes
                double onTimeStamp = timeStamps[inputIndex].Item1;
                double okayMarginOfError = SongManager.Instance.okayMarginOfError;
                double goodMarginOfError = SongManager.Instance.goodMarginOfError;
                double perfectMarginOfError = SongManager.Instance.perfectMarginOfError;
                double audioTime = SongManager.GetAudioSourceTime() -
                                   (SongManager.Instance.inputDelayInSeconds / 1000.0);

                if (input.WasPressedThisFrame())
                {
                    if (Math.Abs(audioTime - onTimeStamp) < perfectMarginOfError)
                    {
                        PerfectHit();
                        print($"Perfect hit on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else if (Math.Abs(audioTime - onTimeStamp) < goodMarginOfError)
                    {
                        GoodHit();
                        print($"Good hit on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else if (Math.Abs(audioTime - onTimeStamp) < okayMarginOfError)
                    {
                        OkayHit();
                        print($"Okay hit on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else
                    {
                        print($"Missed {inputIndex} note with {Math.Abs(audioTime - onTimeStamp)} delay");
                        Miss();
                    }
                }

                if (onTimeStamp + okayMarginOfError <= audioTime)
                {
                    Miss();
                    print($"Missed {inputIndex} note");
                    inputIndex++;
                }
            }
            else
            {
                //Hold notes
                double onTimeStamp = timeStamps[inputIndex].Item1;
                double offTimeStamp = timeStamps[inputIndex].Item2;
                double okayMarginOfError = SongManager.Instance.okayMarginOfError;
                double goodMarginOfError = SongManager.Instance.goodMarginOfError;
                double perfectMarginOfError = SongManager.Instance.perfectMarginOfError;
                double audioTime = SongManager.GetAudioSourceTime() -
                                   (SongManager.Instance.inputDelayInSeconds / 1000.0);


                if (input.WasPressedThisFrame())
                {
                    if (Math.Abs(audioTime - onTimeStamp) < perfectMarginOfError)
                    {
                        //Start hold note
                        PerfectHit();
                        print($"Perfect hold hit on {inputIndex} note");
                        notes[inputIndex].GetComponent<SpriteRenderer>().enabled = false;
                        notes[inputIndex].pauseNote = true;
                    }
                    else if (Math.Abs(audioTime - onTimeStamp) < goodMarginOfError)
                    {
                        //Start hold note
                        GoodHit();
                        print($"Good hold hit on {inputIndex} note");
                        notes[inputIndex].GetComponent<SpriteRenderer>().enabled = false;
                        notes[inputIndex].pauseNote = true;
                    }
                    else if (Math.Abs(audioTime - onTimeStamp) < okayMarginOfError)
                    {
                        //Start hold note
                        OkayHit();
                        print($"Okay hold hit on {inputIndex} note");
                        notes[inputIndex].GetComponent<SpriteRenderer>().enabled = false;
                        notes[inputIndex].pauseNote = true;
                    }
                    else
                    {
                        print($"Missed {inputIndex} note with {Math.Abs(audioTime - onTimeStamp)} delay");
                        Miss();
                    }
                }

                
                if (input.WasReleasedThisFrame())
                {
                    if (Math.Abs(audioTime - onTimeStamp) < perfectMarginOfError)
                    {
                        //Start hold note
                        PerfectHit();
                        print($"Perfect hold finish on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        Destroy(notes[inputIndex].releaseNotePoint.gameObject);
                        inputIndex++;
                    }
                    else if (Math.Abs(audioTime - onTimeStamp) < goodMarginOfError)
                    {
                        //Start hold note
                        GoodHit();
                        print($"Good hold finish on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        Destroy(notes[inputIndex].releaseNotePoint.gameObject);
                        inputIndex++;
                    }
                    else if (Math.Abs(audioTime - offTimeStamp) < okayMarginOfError)
                    {
                        OkayHit();
                        print($"Okay hold finish on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        Destroy(notes[inputIndex].releaseNotePoint.gameObject);
                        inputIndex++;
                    }
                    else
                    {
                        print($"Missed {inputIndex} note with {Math.Abs(audioTime - onTimeStamp)} delay");
                        notes[inputIndex].pauseNote = false;
                        notes[inputIndex].GetComponent<SpriteRenderer>().enabled = true;
                        Miss();
                    }
                }
                
                if (offTimeStamp + okayMarginOfError <= audioTime)
                {
                    Miss();
                    print($"Missed {inputIndex} hold note");
                    inputIndex++;
                }
            }
        }

        if (input.triggered)
        {
            //Change colour of hit line for the lane
            StartCoroutine(HitColour());
        }
        
        if (input.WasPressedThisFrame()) characterAnimator.Play(hitAnimationClip.name);
    }

    private void OkayHit()
    {
        //Play hit sound/effects
        ScoreManager.Instance.OkayHit();
    }
    
    private void GoodHit()
    {
        //Play hit sound/effects
        ScoreManager.Instance.GoodHit();
    }
    
    private void PerfectHit()
    {
        //Play hit sound/effects
        ScoreManager.Instance.PerfectHit();
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
