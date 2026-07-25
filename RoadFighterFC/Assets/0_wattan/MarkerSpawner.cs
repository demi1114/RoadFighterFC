using UnityEngine;
using static MarkerSpawner;

public class MarkerSpawner : MonoBehaviour
{
    [Header("生成するPrefab")]
    public GameObject markerPrefab;

    /*[Header("生成数")]
    public int markerCount = 50;*/

    [Header("間隔")]
    public float spacing = 100f;

    /*[Header("開始位置")]
    public Vector3 startPosition = new Vector3(0f, 1f, 100f);

    void Start()
    {
        for (int i = 0; i < markerCount; i++)
        {
            Vector3 pos = startPosition + new Vector3(0f, 0f, i * spacing);

            Instantiate(markerPrefab, pos, Quaternion.identity);
        }
    }*/

    [Header("道のチェックポイント（順番にアサイン）")]
    public Transform[] roadPoints; // コーナーごとに置いたTransform

    void Start()
    {
        if (roadPoints.Length < 2) return;

        float distanceCovered = 0f;

        // 各区間（Point 0 -> 1, Point 1 -> 2 ...）を順番に処理
        for (int i = 0; i < roadPoints.Length - 1; i++)
        {
            Transform startPoint = roadPoints[i];
            Transform endPoint = roadPoints[i + 1];

            Vector3 segmentVector = endPoint.position - startPoint.position;
            float segmentLength = segmentVector.magnitude; // 区間の長さ
            Vector3 direction = segmentVector.normalized; // 進む向き
            Quaternion rotation = Quaternion.LookRotation(direction); // 向きの回転

            // 生成を区間内でspacingごとの間隔で繰り返す
            while (distanceCovered < segmentLength)
            {
                Vector3 spawnPos = startPoint.position + (direction * distanceCovered);
                Instantiate(markerPrefab, spawnPos, rotation);

                distanceCovered += spacing;
            }

            // 次の区間に余った距離を繰り越す（余白調整）
            distanceCovered -= segmentLength;
        }
    }

}