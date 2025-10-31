using UnityEngine;

public class TestPersepctiveScale : MonoBehaviour
{
    private Vector3 screenTop = new Vector3(24,7,0);
    
    void Update()
    {
        float distanceFromScreenTop = Vector3.Distance(screenTop, transform.position);
        float minDistance = 0f;
        float maxDistance = 14f;
        float minSize = 0;
        float maxSize = 1.75f;
        
        float normalizedDistance = Mathf.InverseLerp(minDistance, maxDistance, distanceFromScreenTop);
        float scale = Mathf.Lerp(minSize, maxSize, normalizedDistance);
        
        transform.localScale = new Vector3(scale, scale, scale);    
    }
}
