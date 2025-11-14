using UnityEngine;

public class Note : MonoBehaviour
{
    private double _timeCreated;
    private double _releaseNoteTimeCreated;

    private bool _releaseNoteSpawned = false;

    public bool isHoldNote = false;
    public GameObject releaseNotePointPrefab;
    public GameObject releaseNotePoint;
    public bool pauseNote = false;

    public float assignedOnTime;
    public float assignedOffTime;
    
    private SpriteRenderer _spriteRenderer;
    
    public Vector3 spawnPos;
    public Vector3 despawnPos;

    private LineRenderer _lineRenderer;
    
    
     void Start()
    {
        _timeCreated = assignedOnTime - SongManager.Instance.noteTimeUntilHit;
        _releaseNoteTimeCreated = assignedOffTime - SongManager.Instance.noteTimeUntilHit;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = true;
        
        transform.rotation = SongManager.Instance.rhythmTrackPos.rotation;
    }

    
    void Update()
    {       
        float spawnDelay = SongManager.Instance.songDelayInSeconds - SongManager.Instance.noteTimeUntilHit;
        
        double timeSinceCreation = spawnDelay > 0 && _timeCreated < 0 ? (Time.timeSinceLevelLoad - spawnDelay) + _timeCreated : SongManager.GetAudioSourceTime() - _timeCreated;
        double releaseNoteTimeSinceCreation = spawnDelay > 0 && _releaseNoteTimeCreated < 0 ? (Time.timeSinceLevelLoad - spawnDelay) + _releaseNoteTimeCreated : SongManager.GetAudioSourceTime() - _releaseNoteTimeCreated;
        
        float t = (float)(timeSinceCreation / (SongManager.Instance.noteTimeUntilHit * 2));
        float releaseT = (float)(releaseNoteTimeSinceCreation / (SongManager.Instance.noteTimeUntilHit * 2));
        
        if (isHoldNote && _releaseNoteSpawned == false)
        {
            _releaseNoteSpawned = true;
            releaseNotePoint = Instantiate(releaseNotePointPrefab, spawnPos, transform.rotation);
            
            // Add a LineRenderer component
            _lineRenderer = gameObject.AddComponent<LineRenderer>();

            // Set the material
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

            // Set the color
            _lineRenderer.startColor = Color.cyan;
            _lineRenderer.endColor = Color.cyan;

            // Set the width
            _lineRenderer.startWidth = 0.2f;
            _lineRenderer.endWidth = 0.2f;

            // Set the number of vertices
            _lineRenderer.positionCount = 2;

            //Set order in layer
            _lineRenderer.sortingOrder = 3;
        }
        
        if (!isHoldNote && t > 1)
        {
            Destroy(gameObject);
        }
        else if (isHoldNote && releaseT > 1)
        {
            Destroy(releaseNotePoint);
            Destroy(gameObject);
        }
        else
        {
            if (!pauseNote) transform.position = Vector3.Lerp(spawnPos, despawnPos, t);

             if (isHoldNote)
             {
                 releaseNotePoint.GetComponent<SpriteRenderer>().enabled = true;
                 releaseNotePoint.transform.position = Vector3.Lerp(spawnPos, despawnPos, releaseT);
                 
                 // Set the positions of the vertices
                 _lineRenderer.SetPosition(0, transform.position);
                 _lineRenderer.SetPosition(1, releaseNotePoint.transform.position);
             }
        }
    }
}
