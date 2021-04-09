using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Range(0f, 1.0f)] private float smoothSpeed = 0.125f;
    private Vector3 offset;
    
    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        //transform.position = target.position + offset;
        var desiredPosition = target.position + offset;
        transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothSpeed);

        //transform.LookAt(target);
    }
}