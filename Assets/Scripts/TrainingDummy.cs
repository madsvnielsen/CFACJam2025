using System.Collections;
using UnityEngine;
public class TrainingDummy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float flashDuration = 0.2f;
    private Color originalColor;

    private AudioSource aSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        aSource = GetComponent<AudioSource>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
         aSource.Play();
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
       
    }
}