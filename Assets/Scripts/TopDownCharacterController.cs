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

    public float bounceDelay = 0.2f; // Time before bouncing off

    public float jumpDuration = 0.5f; // How long the jump lasts
    public float jumpScaleFactor = 1.3f; // How much to enlarge during jump
    public bool isJumping = false;

    public bool startGame = true;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    public Animator glowAnimator;
    private Transform directionIndicator;
    private SpriteRenderer indicatorSprite;
    private Vector2 lastDirection = Vector2.right;
    private bool isSwitchingDirection = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        rb.linearDamping = dragFactor;

        directionIndicator = transform.GetChild(0);
        indicatorSprite = directionIndicator.GetComponent<SpriteRenderer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        // Check for Space keypress in Update() instead of FixedUpdate()
        if (Input.GetMouseButtonDown(0) && (!isSwitchingDirection || startGame))
        {
            startGame = false;
            FindFirstObjectByType<MenuController>().RemoveStartGameToolTip();
            StartCoroutine(SwitchDirectionTowardsMouse());
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(Jump());
        }
    }

    
 IEnumerator Jump()
    {
        isJumping = true;
        rb.gravityScale = 0; // Disable gravity to simulate jumping
        rb.linearVelocity = Vector2.zero; // Stop movement

        // Enlarge sprite
        Vector3 originalScale = transform.localScale;
        Vector3 jumpScale = originalScale * jumpScaleFactor;

        float elapsedTime = 0f;
        while (elapsedTime < jumpDuration / 2)
        {
            transform.localScale = Vector3.Lerp(originalScale, jumpScale, elapsedTime / (jumpDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(jumpDuration / 2);

        elapsedTime = 0f;
        while (elapsedTime < jumpDuration / 2)
        {
            transform.localScale = Vector3.Lerp(jumpScale, originalScale, elapsedTime / (jumpDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        rb.gravityScale = 1; // Re-enable gravity
        isJumping = false;
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
       if (collision.contactCount == 0) return;

        Vector2 averageNormal = Vector2.zero;
        Vector2 averageContactPoint = Vector2.zero;



        // Sum up all contact normals
        for (int i = 0; i < collision.contactCount; i++)
        {
            averageNormal += collision.GetContact(i).normal;
            averageContactPoint += collision.GetContact(i).point;

        }

        averageNormal.Normalize(); // Get average normal
        averageContactPoint /= collision.contactCount;


        // Reflect movement direction using the averaged normal
        Vector2 newDirection = Vector2.Reflect(lastDirection, averageNormal).normalized;

        // Stop movement temporarily
        StartCoroutine(BounceWithDelay(newDirection, averageContactPoint));
    }

    IEnumerator BounceWithDelay(Vector2 newDirection, Vector2 contactPoint)
    {
        isSwitchingDirection = true; // Prevent movement during pause
        rb.linearVelocity = Vector2.zero;  // Stop movement

         // Rotate sprite to face the contact point
        Vector2 directionToContact = contactPoint - (Vector2)transform.position;
        float contactAngle = Mathf.Atan2(directionToContact.y, directionToContact.x) * Mathf.Rad2Deg;
        
        // Rotate the sprite so the bottom faces the contact point
        transform.rotation = Quaternion.Euler(0, 0, contactAngle + 90f); 
        directionIndicator.gameObject.SetActive(false);

        // Optional: Play impact animation or effect
        animator.SetTrigger("Impact");

        yield return new WaitForSeconds(bounceDelay); // Wait before bouncing

        transform.rotation = Quaternion.Euler(0, 0, 0);
        directionIndicator.gameObject.SetActive(true);

        lastDirection = newDirection; // Update direction after delay
        rb.linearVelocity = lastDirection * maxSpeed; // Resume movement

        isSwitchingDirection = false; // Allow movement again

        // Flip sprite based on direction
        sprite.flipX = lastDirection.x < 0;
    }



}
