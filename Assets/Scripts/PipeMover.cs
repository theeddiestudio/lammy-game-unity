using UnityEngine;

public class PipeMover : MonoBehaviour
{
    public float speed = 5f;
    private float leftEdge;

    void Start()
    {
        // calculate the left position wrt camera
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    
    void Update()
    {
        // move left every frame with deltaTime for fps
        transform.position += Vector3.left * speed * Time.deltaTime;

        // delete if gone left
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
