using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    TopDownCharacterController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start(){
        player = GetComponentInParent<TopDownCharacterController>();
    }
    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Hole")) // Make sure your hole tiles are tagged as "Hole"
    {
        if (!player.isJumping) 
        {
            GetComponentInParent<PlayerHealth>().TakeDamage(1);
            Debug.Log("Player fell into a hole and took damage!");
        }
        else
        {
            Debug.Log("Player jumped over the hole!");
        }
    }
}
}
