using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 10f;        // Force applied for movement
    public float maxSpeed = 5f;          // Maximum movement speed
    public float stopThreshold = 0.1f;   // Speed at which player stops
    public float minMoveDistance = 1f;   // Minimum distance before changing direction
    public float bounceRotationFactor = 180f; // Rotation adjustment when bouncing

    public float dragFactor = 2f;        // Slows down movement naturally

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    private Transform directionIndicator; // Reference to the direction indicator
    private SpriteRenderer indicatorSprite; // Sprite Renderer for visibility control
    private Vector2 lastDirection = Vector2.right; // Stores last valid movement direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Get Animator component
        rb.freezeRotation = true; // Prevents Rigidbody2D from rotating the sprite
        rb.linearDamping = dragFactor; // Applies natural deceleration
        directionIndicator = transform.GetChild(0);
        indicatorSprite = directionIndicator.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - rb.position);

        // Update direction only if the mouse is far enough
        if (direction.magnitude >= minMoveDistance)
        {
            lastDirection = direction.normalized;
            directionIndicator.gameObject.SetActive(true); // Hide indicator

        } else{
            directionIndicator.gameObject.SetActive(false); // Hide indicator

        }

        // Apply force in the last known direction if the mouse is far enough
        if (direction.magnitude >= minMoveDistance)
        {
            rb.AddForce(lastDirection * moveSpeed);
        }

        // Clamp speed to maxSpeed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Stop movement if speed is below threshold
        if (rb.linearVelocity.magnitude < stopThreshold)
        {
            rb.linearVelocity = Vector2.zero; // Stop completely
            animator.SetBool("IsMoving", false); // Switch to idle animation
        }
        else
        {
            animator.SetBool("IsMoving", true); // Play movement animation
        }

         if (directionIndicator.gameObject.activeSelf) // Only rotate if it's visible
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            directionIndicator.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Flip sprite based on mouse position
        sprite.flipX = mousePosition.x < rb.position.x;
    }

    // Handle bouncing off walls
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        // Reflect movement direction
        lastDirection = Vector2.Reflect(lastDirection, normal).normalized;

        // Add slight random variation in bounce direction for a more dynamic feel
        float bounceAngle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        float rotatedBounceAngle = bounceAngle + Random.Range(-bounceRotationFactor / 2, bounceRotationFactor / 2);
        lastDirection = new Vector2(Mathf.Cos(rotatedBounceAngle * Mathf.Deg2Rad), Mathf.Sin(rotatedBounceAngle * Mathf.Deg2Rad)).normalized;
    }
}
