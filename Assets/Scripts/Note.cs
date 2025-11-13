using UnityEngine;

public class Note : MonoBehaviour
{
    private double _timeCreated;
    private double _releaseNoteTimeCreated;

    private bool _releaseNoteSpawned = false;

    public bool isHoldNote = false;
    public GameObject releaseNotePointPrefab;
    private GameObject _releaseNotePoint;

    public float assignedOnTime;
    public float assignedOffTime;
    
    private SpriteRenderer spriteRenderer;
    
    public Vector3 spawnPos;
    public Vector3 despawnPos;
    
    
     void Start()
    {
        _timeCreated = assignedOnTime - SongManager.Instance.noteTimeUntilHit;
        _releaseNoteTimeCreated = assignedOffTime - SongManager.Instance.noteTimeUntilHit;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        transform.rotation = SongManager.Instance.rhythmTrackPos.rotation;
    }

    
    void Update()
    {
        // //Scaling things in 2D view
        // float distanceFromScreenTop = Vector3.Distance(spawnPos, transform.position);
        // float minDistance = 0f;
        // float maxDistance = 17f;
        // float minSize = 0;
        // float maxSize = 0.35f;
        //
        // float normalizedDistance = Mathf.InverseLerp(minDistance, maxDistance, distanceFromScreenTop);
        // float scale = Mathf.Lerp(minSize, maxSize, normalizedDistance);
        //
        // transform.localScale = new Vector3(scale, scale, scale);

        
        
        
        float spawnDelay = SongManager.Instance.songDelayInSeconds - SongManager.Instance.noteTimeUntilHit;
        
        double timeSinceCreation = spawnDelay > 0 && _timeCreated < 0 ? (Time.timeSinceLevelLoad - spawnDelay) + _timeCreated : SongManager.GetAudioSourceTime() - _timeCreated;
        double releaseNoteTimeSinceCreation = spawnDelay > 0 && _releaseNoteTimeCreated < 0 ? (Time.timeSinceLevelLoad - spawnDelay) + _releaseNoteTimeCreated : SongManager.GetAudioSourceTime() - _releaseNoteTimeCreated;
        
        float t = (float)(timeSinceCreation / (SongManager.Instance.noteTimeUntilHit * 2));
        float releaseT = (float)(releaseNoteTimeSinceCreation / (SongManager.Instance.noteTimeUntilHit * 2));
        
        if (isHoldNote && _releaseNoteSpawned == false)
        {
            _releaseNoteSpawned = true;
            _releaseNotePoint = Instantiate(releaseNotePointPrefab, spawnPos, transform.rotation);
        }
        
        if (!isHoldNote && t > 1)
        {
            Destroy(gameObject);
        }
        else if (isHoldNote && releaseT > 1)
        {
            Destroy(_releaseNotePoint);
            Destroy(gameObject);
        }
        else
        {
             transform.position = Vector3.Lerp(spawnPos, despawnPos, t);
             spriteRenderer.enabled = true;
             
             if (isHoldNote) _releaseNotePoint.GetComponent<SpriteRenderer>().enabled = true;
             if (isHoldNote) _releaseNotePoint.transform.position = Vector3.Lerp(spawnPos, despawnPos, releaseT);
        }
    }
}
