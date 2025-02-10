using UnityEngine;

public class SideCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 7f;
    public Vector3 offset;
    public float rotationAngle = 11f; // Угол наклона камеры
    public float rotationSpeed = 5f;  // Скорость поворота камеры

    private void LateUpdate()
    {
        if (target == null) return;

        // Двигаем камеру
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Проверяем направление движения персонажа
        float moveInput = Input.GetAxis("Horizontal");
        float targetRotationY = 0f;

        if (moveInput > 0) // Движение вправо
        {
            targetRotationY = rotationAngle;
        }
        else if (moveInput < 0) // Движение влево
        {
            targetRotationY = -rotationAngle;
        }

        // Плавный поворот камеры
        Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}