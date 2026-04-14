using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float airMultiplier = 0.5f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public int maxJumps = 2;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;
    private int jumpCount;

    float horizontal;
    float vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevents tipping over
    }

    void Update()
    {
        // Input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset Y
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }

        // Limit speed
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}