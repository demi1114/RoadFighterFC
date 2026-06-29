using UnityEngine;

public class MarkerSpawner : MonoBehaviour
{
    public GameObject markerPrefab;

    public int markerCount = 20;
    public float spacing = 100f;

    void Start()
    {
        for (int i = 0; i < markerCount; i++)
        {
            Vector3 pos = new Vector3(
                0f,
                1f,
                100 +  i * spacing
            );

            Instantiate(markerPrefab, pos, Quaternion.identity);
        }
    }
}