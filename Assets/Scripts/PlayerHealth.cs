using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;             // Maximum HP
    private int currentHealth;            // Current HP

    public float invincibilityTime = 0.5f; // Time before taking damage again
    private bool isInvincible = false;     // Invincibility flag

    private SpriteRenderer spriteRenderer; // Player sprite
    private Color originalColor;           // Store original color

    private AudioSource audioSource;
    public AudioClip damageTakenClip;

    public GameObject[] hearts;

     public Animator diePanelAnimator;

    void Start()
    {
        currentHealth = maxHealth; // Start at full health
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        originalColor = spriteRenderer.color; // Store the default color
        audioSource = GetComponent<AudioSource>();
        UpdateHealthUI();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Spike")) && !isInvincible)
        {
            if(other.gameObject.CompareTag("Enemy")){
                if(other.gameObject.GetComponent<EnemyAI>().isDead) return;
            }
            TakeDamage(1);
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive((i < currentHealth));  // Enable/disable hearts based on HP
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        audioSource.clip = damageTakenClip;
        audioSource.Play();
        UpdateHealthUI();
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityEffect());
    }

    IEnumerator InvincibilityEffect()
    {
        isInvincible = true;
        
        float elapsedTime = 0f;
        while (elapsedTime < invincibilityTime)
        {
            float t = Mathf.PingPong(elapsedTime * 2, 1); // Smooth transition
            spriteRenderer.color = Color.Lerp(originalColor, Color.red, t);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor; // Restore original color
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player Died!");
        PlayerPrefs.SetInt("prevScore", ScoreManager.playerScore);
        if(!PlayerPrefs.HasKey("highscore")){
            PlayerPrefs.SetInt("highscore", ScoreManager.playerScore);
        }else{
            if(PlayerPrefs.GetInt("highscore") < ScoreManager.playerScore){
                PlayerPrefs.SetInt("highscore", ScoreManager.playerScore);
                PlayerPrefs.SetInt("newHighscore", 1);
            }
        }
        FindFirstObjectByType<TopDownCharacterController>().isDead = true;
        diePanelAnimator.Play("DiePanel");
        StartCoroutine(WaitForGO());
    }

    IEnumerator WaitForGO(){
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("GameOver");
    }
}
