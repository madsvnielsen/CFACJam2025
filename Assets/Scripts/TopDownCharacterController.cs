using System.Collections;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float maxSpeed = 5f;
    public float stopThreshold = 0.1f;
    public float minMoveDistance = 1f;
    public float bounceRotationFactor = 180f;
    public float dragFactor = 2f;
    public float switchDirectionDelay = 0.5f;
    public float rotationSpeed = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    public Animator glowAnimator;
    private Transform directionIndicator;
    private SpriteRenderer indicatorSprite;
    private Vector2 lastDirection = Vector2.right;
    private bool isSwitchingDirection = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        rb.linearDamping = dragFactor;

        directionIndicator = transform.GetChild(0);
        indicatorSprite = directionIndicator.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check for Space keypress in Update() instead of FixedUpdate()
        if (Input.GetKeyDown(KeyCode.Space) && !isSwitchingDirection)
        {
            Debug.Log("Keypress detected!");
            StartCoroutine(SwitchDirectionTowardsMouse());
        }
    }

    void FixedUpdate()
    {
        if (!isSwitchingDirection)
        {
            rb.AddForce(lastDirection * moveSpeed);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        if (rb.linearVelocity.magnitude < stopThreshold)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
        else
        {
            animator.SetBool("IsMoving", true);
        }

        sprite.flipX = lastDirection.x < 0;

       if (directionIndicator.gameObject.activeSelf)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mousePosition - (Vector2)directionIndicator.position).normalized;

            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            directionIndicator.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    IEnumerator SwitchDirectionTowardsMouse()
    {
        isSwitchingDirection = true;
        glowAnimator.SetTrigger("SwitchDirection");
        rb.linearVelocity = Vector2.zero;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newDirection = (mousePosition - rb.position).normalized;
        if (newDirection.magnitude > 0.1f)
        {
            lastDirection = newDirection;
        }

        yield return new WaitForSeconds(switchDirectionDelay);


      

        isSwitchingDirection = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contactCount == 0) return; // Ensure there are contacts

        Vector2 averageNormal = Vector2.zero;

        // Sum up all contact normals
        for (int i = 0; i < collision.contactCount; i++)
        {
            averageNormal += collision.GetContact(i).normal;
        }

        // Get the average normal
        averageNormal.Normalize();

        // Reflect movement direction using the averaged normal
        lastDirection = Vector2.Reflect(lastDirection, averageNormal).normalized;

        // Apply a force in the new direction to maintain momentum
        rb.linearVelocity = lastDirection * maxSpeed;

        // Flip sprite direction appropriately
        sprite.flipX = lastDirection.x < 0;
    }

}
