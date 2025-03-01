using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    private CameraController cameraController;

    void Start()
    {
        cameraController = FindFirstObjectByType<CameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            cameraController.SetTargetStage(transform);
        }
    }
}
