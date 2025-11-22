using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Assingables
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;

    //Movement
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpHeight = 3f;

    //Camera Movement
    [SerializeField] private float smoothTurnTime = 0.2f;

    private float smoothVelocity;

    //Ground Check & Gravity
    [SerializeField] private float gravity = -10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Grounded Check
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Movement Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            //Turning and Tracking Camera Movement
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }
}
