using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;
    [SerializeField]
    private float damping = 0.0f;

    private Vector3 offset;
    private Vector3 velocity;
    private void Start()
    {
        offset = transform.position - followTarget.position; // what is this??
        offset = new Vector3(0, 0, transform.position.z);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = followTarget.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
    }
}
