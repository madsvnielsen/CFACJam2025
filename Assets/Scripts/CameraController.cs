using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothSpeed = 5f; // Speed of camera movement
    private Transform targetStage; // The current active stage

    private GameObject player;

    void Start(){
        player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        if (targetStage != null)
        {

            // Move the camera towards the target stage smoothly
            Vector3 targetPosition = new Vector3(player.transform.position.x, targetStage.position.y, transform.position.z);
            transform.position = targetPosition;//Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }

    public void SetTargetStage(Transform newStage)
    {
        targetStage = newStage;
    }
}
