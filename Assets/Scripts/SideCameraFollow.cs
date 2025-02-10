using UnityEngine;

public class SideCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 7f;
    public Vector3 offset;
    public float rotationAngle = 11f;
    public float rotationSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float moveInput = Input.GetAxis("Horizontal");
        float targetRotationY = 0f;

        if (moveInput > 0)
        {
            targetRotationY = rotationAngle;
        }
        else if (moveInput < 0)
        {
            targetRotationY = -rotationAngle;
        }

        Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}