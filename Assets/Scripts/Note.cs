using UnityEngine;

public class Note : MonoBehaviour
{
    private double timeCreated;

    public float assignedTime;
    
    private SpriteRenderer spriteRenderer;
    
     void Start()
    {
        timeCreated = assignedTime - SongManager.Instance.noteTimeUntilHit;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        float spawnDelay = SongManager.Instance.songDelayInSeconds - SongManager.Instance.noteTimeUntilHit;


        double timeSinceCreation = spawnDelay > 0 && timeCreated < 0 ? (Time.timeSinceLevelLoad - spawnDelay) + timeCreated : SongManager.GetAudioSourceTime() - timeCreated;
        float t = (float)(timeSinceCreation / (SongManager.Instance.noteTimeUntilHit * 2));
        
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
             transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, t);
             spriteRenderer.enabled = true;
        }
    }
}
