using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -20f;
    public float jumpHeight = 2f;
    public float wallJumpForce = 5f;
    public float wallJumpDecay = 0.5f; // Скорость уменьшения силы отталкивания
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform wallCheck2;
    public float groundDistance = 0.2f;
    public float wallDistance = 0.3f;
    public LayerMask groundMask;
    public LayerMask wallMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWall2;
    private bool facingRight = true;
    private bool isWallJumping = false; // Флаг для проверки состояния отталкивания от стены
    private float wallJumpVelocityX = 0f; // Храним компонент силы отталкивания по оси X

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isTouchingWall = Physics.CheckSphere(wallCheck.position, wallDistance, wallMask);
        isTouchingWall2 = Physics.CheckSphere(wallCheck2.position, wallDistance, wallMask);

        // Если на земле и падаем, сбрасываем силу отталкивания
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isWallJumping = false; // Сбросить эффект отталкивания от стены
            wallJumpVelocityX = 0f; // Сбросить силу отталкивания по оси X при касании земли
        }

        // Движение
        float move = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(move, 0, 0); // Двигаем только по X
        controller.Move(moveDirection * speed * Time.deltaTime);

        // Разворот персонажа
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Отталкивание от стены
        if (Input.GetButtonDown("Jump") && !isGrounded && (isTouchingWall || isTouchingWall2))
        {
            velocity.y = Mathf.Sqrt((jumpHeight*2) * -2f * gravity);  // Прыжок вверх

            // Определяем направление отталкивания в зависимости от текущего движения
            if (facingRight)
            {
                // Если игрок двигается вправо, отталкиваем его влево
                wallJumpVelocityX = -wallJumpForce;
            }
            else
            {
                // Если игрок двигается влево, отталкиваем его вправо
                wallJumpVelocityX = wallJumpForce;
            }

            isWallJumping = true; // Устанавливаем флаг Wall Jump
        }

        // Если игрок не касается стены, сбрасываем флаг wall jump
        if (!isTouchingWall && !isTouchingWall2)
        {
            isWallJumping = false;
        }

        // Постепенное уменьшение силы отталкивания от стены
        if (isWallJumping)
        {
            wallJumpVelocityX = Mathf.Lerp(wallJumpVelocityX, 0f, wallJumpDecay * Time.deltaTime); // Постепенно угасает
        }

        // Применяем силу отталкивания по оси X
        velocity.x = wallJumpVelocityX;

        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Метод для поворота персонажа
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f); // Поворот только для визуала, движение остаётся нормальным
    }
}