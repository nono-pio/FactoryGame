using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    private Transform playerTransform;

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        transform.position = newPos;
    }
}
