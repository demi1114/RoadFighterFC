using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    public float moveSpeed = 40f;

    [Header("‰½•bŒã‚Éíœ‚·‚é‚©")]
    public float destroyTime = 10f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
