using System.Linq;
using UnityEngine;

public class StageController : MonoBehaviour
{

    public GameObject activeStage;
    public GameObject[] stages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStage(){
        Destroy(activeStage);
        activeStage = Instantiate(stages[Random.Range(0,stages.Length)]);
        activeStage.transform.position = new Vector3(0,0,0);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0,0,0);
        GameObject.FindFirstObjectByType<CameraController>().targetStage = activeStage.transform;

    }
}
