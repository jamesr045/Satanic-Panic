using UnityEngine;

public class Note : MonoBehaviour
{
    private double timeCreated;

    public float assignedTime;
    
    private SpriteRenderer spriteRenderer;
    
    public Vector3 spawnPos;
    public Vector3 despawnPos;
    
     void Start()
    {
        timeCreated = assignedTime - SongManager.Instance.noteTimeUntilHit;
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
        
        double timeSinceCreation = spawnDelay > 0 && timeCreated < 0 ? (Time.timeSinceLevelLoad - spawnDelay) + timeCreated : SongManager.GetAudioSourceTime() - timeCreated;
        float t = (float)(timeSinceCreation / (SongManager.Instance.noteTimeUntilHit * 2));
        
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
             transform.position = Vector3.Lerp(spawnPos, despawnPos, t);
             spriteRenderer.enabled = true;
        }
    }
}
