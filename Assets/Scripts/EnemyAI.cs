using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;         // Speed when chasing
    public float lungeSpeed = 15f;       // Speed when lunging
    public float stopThreshold = 0.2f;   // Speed at which the enemy stops after lunging

    public AudioClip enemyHitClip;
    public AudioClip enemyDeadClip;
     public AudioClip enemyAttackClip;
    private AudioSource audioSource;

    public int health = 2;

    public GameObject[] hearts;

    public float prepareAttackTime = 0.5f; // Time spent preparing attack
    public float chargeUpTime = 0.5f;      // Time spent charging before lunging
    public float attackCooldown = 1.0f;    // Cooldown before attacking again

    public float dragFactor = 2f;        // Slows down enemy after lunging

    public Collider2D rangeChase;        // Reference to the Chase trigger collider
    public Collider2D rangeAttack;       // Reference to the Attack trigger collider

    public Rigidbody2D rb;
    private Animator animator;

    private bool isInvincible = false;
    public Animator explosionAnimator;
    private Transform player;

    private Vector2 storedLungeDirection; // Stores lunge direction before lunging
    private bool isAttackOnCooldown = false; // Prevents immediate re-attacking

    private enum EnemyState { Idle, Chasing, PreparingAttack, Charging, Attacking, Recovering, Cooldown }
    private EnemyState currentState = EnemyState.Idle;

    public bool isDead = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if(StageController.currentStage <= 1) {
            Destroy(gameObject);
            return;
        }
        rb.linearDamping = dragFactor; // Adds drag for deceleration after lunging
        health = Random.Range(1, Mathf.Min(Mathf.Max(1,StageController.currentStage-1), 7));
        audioSource = GetComponent<AudioSource>();
        UpdateHealthbar();
    }

    void Update()
    {
        if (player == null || isDead) return;

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;

            case EnemyState.Chasing:
                HandleChasingState();
                break;

            case EnemyState.PreparingAttack:
      
                break;

            case EnemyState.Charging:
            
                break;

            case EnemyState.Attacking:
                break;

            case EnemyState.Recovering:
                HandleRecoveringState();
                break;

            case EnemyState.Cooldown:
                break;
        }
    }
    void UpdateHealthbar(){
        for(int i = 0; i < hearts.Length; i++){
            if(i < health) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
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
        if(isDead) yield break;
        currentState = EnemyState.Charging;
        animator.SetTrigger("ChargeAttack");

        StartCoroutine(ChargeAttack());
    }

    IEnumerator ChargeAttack()
    {
        storedLungeDirection = (player.position - transform.position).normalized;
        yield return new WaitForSeconds(chargeUpTime);

        currentState = EnemyState.Attacking;
        if(isDead) yield break;
        animator.SetTrigger("Attack");
        LungeAtStoredDirection();
    }

    void LungeAtStoredDirection()
    {
        audioSource.clip = enemyAttackClip;
        audioSource.Play();
        if(!isInvincible){
            rb.linearVelocity = storedLungeDirection * lungeSpeed;
        }
        
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

    public void TakeDamage(){      
        if(isInvincible) return; 
        audioSource.clip = enemyHitClip;
        audioSource.Play();
        health--;
        UpdateHealthbar();
        if(health <= 0){
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(WaitDeadAnimationFinish());
        } else {
        StartCoroutine(InvincibilityFrames());
        }
        
    }

    IEnumerator InvincibilityFrames(){
        isInvincible = true;
        yield return new WaitForSeconds(0.5f);
        isInvincible = false;
    }

    IEnumerator WaitDeadAnimationFinish(){
        animator.Play("mole_dead");
        explosionAnimator.Play("death_explosion");
        audioSource.clip = enemyDeadClip;
        audioSource.Play();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        ScoreManager.playerScore += 100;
        Destroy(this.gameObject);
    }
}
