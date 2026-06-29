using UnityEngine;

public class MarkerSpawner : MonoBehaviour
{
    [Header("ê∂ê¨Ç∑ÇÈPrefab")]
    public GameObject markerPrefab;

    [Header("ê∂ê¨êî")]
    public int markerCount = 50;

    [Header("ä‘äu")]
    public float spacing = 100f;

    [Header("äJénà íu")]
    public Vector3 startPosition = new Vector3(0f, 1f, 100f);

    void Start()
    {
        for (int i = 0; i < markerCount; i++)
        {
            Vector3 pos = startPosition + new Vector3(0f, 0f, i * spacing);

            Instantiate(markerPrefab, pos, Quaternion.identity);
        }
    }
}