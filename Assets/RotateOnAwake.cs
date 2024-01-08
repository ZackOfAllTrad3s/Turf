using UnityEngine;

public class RotateOnAwake : MonoBehaviour
{
    [SerializeField]
    private bool useWorldRotation = false;
    [SerializeField]
    private Vector3 eulerAngles = new Vector3(-45, 0, 0);

    private void Awake()
    {
        if (useWorldRotation)
            transform.rotation = Quaternion.Euler(eulerAngles);
        else
            transform.localRotation = Quaternion.Euler(eulerAngles);
    }
}
