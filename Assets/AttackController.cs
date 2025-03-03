using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float attackCooldown = 1f; // Cooldown between attacks

    public float attackDuration = 1f; // Cooldown between attacks
    private bool canAttack = true;
    private bool isAttacking = false;

    private Animator animator;

    public Animator hitEffectAnimator;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Rotate towards the mouse
        RotateTowardsMouse();

        // Detect right-click for attack
        if (Input.GetMouseButtonDown(1) && canAttack) // Right-click
        {
            StartCoroutine(Attack());
        }
    }

    void RotateTowardsMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;
        animator.SetTrigger("Attack");
       

        yield return new WaitForSeconds(attackDuration); // Attack duration before hiding sprite

        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown); // Wait for cooldown
        canAttack = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && isAttacking)
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                hitEffectAnimator.Play("hit_effect");
                enemy.TakeDamage();
                Debug.Log("Hit enemy!");
            }
        }
    }
}
