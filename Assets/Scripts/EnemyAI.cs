using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;         // Speed when chasing
    public float lungeSpeed = 15f;       // Speed when lunging
    public float stopThreshold = 0.2f;   // Speed at which the enemy stops after lunging

    public float prepareAttackTime = 0.5f; // Time spent preparing attack
    public float chargeUpTime = 0.5f;      // Time spent charging before lunging
    public float attackCooldown = 1.0f;    // Cooldown before attacking again

    public float dragFactor = 2f;        // Slows down enemy after lunging

    public Collider2D rangeChase;        // Reference to the Chase trigger collider
    public Collider2D rangeAttack;       // Reference to the Attack trigger collider

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    private Vector2 storedLungeDirection; // Stores lunge direction before lunging
    private bool isAttackOnCooldown = false; // Prevents immediate re-attacking

    private enum EnemyState { Idle, Chasing, PreparingAttack, Charging, Attacking, Recovering, Cooldown }
    private EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.linearDamping = dragFactor; // Adds drag for deceleration after lunging
    }

    void Update()
    {
        if (player == null) return;

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;

            case EnemyState.Chasing:
                HandleChasingState();
                break;

            case EnemyState.PreparingAttack:
                // Do nothing, animation is playing
                break;

            case EnemyState.Charging:
                // Do nothing, animation is playing
                break;

            case EnemyState.Attacking:
                // Do nothing, attack is happening
                break;

            case EnemyState.Recovering:
                HandleRecoveringState();
                break;

            case EnemyState.Cooldown:
                // Do nothing, waiting for cooldown to end
                break;
        }
    }

    void HandleIdleState()
    {
        if (rangeChase.bounds.Contains(player.position) && !isAttackOnCooldown)
        {
            StartChasing();
        }
    }

    void HandleChasingState()
    {
        if (rangeAttack.bounds.Contains(player.position) && !isAttackOnCooldown)
        {
            StartPreparingAttack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    void HandleRecoveringState()
    {
        if (rb.linearVelocity.magnitude < stopThreshold)
        {
            animator.SetTrigger("Idle");
            StartCoroutine(AttackCooldown()); // Start cooldown before attacking again
        }
    }

    void StartChasing()
    {
        currentState = EnemyState.Chasing;
        animator.SetBool("IsChasing", true);
    }

    void StartPreparingAttack()
    {
        currentState = EnemyState.PreparingAttack;
        animator.SetTrigger("PrepareAttack");
        rb.linearVelocity = Vector2.zero; // Stop moving

        // Store lunge direction early before charging up
        

        StartCoroutine(PrepareAttack());
    }

    IEnumerator PrepareAttack()
    {
        yield return new WaitForSeconds(prepareAttackTime);

        currentState = EnemyState.Charging;
        animator.SetTrigger("ChargeAttack");

        StartCoroutine(ChargeAttack());
    }

    IEnumerator ChargeAttack()
    {
        storedLungeDirection = (player.position - transform.position).normalized;
        yield return new WaitForSeconds(chargeUpTime);

        currentState = EnemyState.Attacking;
        animator.SetTrigger("Attack");

        LungeAtStoredDirection();
    }

    void LungeAtStoredDirection()
    {
        rb.linearVelocity = storedLungeDirection * lungeSpeed;
        StartCoroutine(CheckRecovery());
    }

    IEnumerator CheckRecovery()
    {
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < stopThreshold);
        currentState = EnemyState.Recovering;
    }

    IEnumerator AttackCooldown()
    {
        currentState = EnemyState.Cooldown;
        isAttackOnCooldown = true;

        yield return new WaitForSeconds(attackCooldown);

        isAttackOnCooldown = false;
        currentState = EnemyState.Idle;
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }
}
