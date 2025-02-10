using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -20f;
    public float jumpHeight = 2f;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ���������, ����� �� �������� �� ����� � ������� OverlapSphere
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ��������� ��������, ����� �������� �� "������" �� �����
        }

        // �������� �����-������
        float move = Input.GetAxis("Horizontal");
        Vector3 moveDirection = transform.right * move;
        controller.Move(moveDirection * speed * Time.deltaTime);

        // ������
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // ��������� ����������
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}