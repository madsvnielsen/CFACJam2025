using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class StageController : MonoBehaviour
{

    public GameObject activeStage;
    public GameObject[] stages;

    public TMP_Text stageGreetText;

    public static int currentStage = 0;

    public Animator panelAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeStage = Instantiate(stages[Random.Range(0,stages.Length)]);
        activeStage.transform.position = new Vector3(0,0,0);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0,0,0);
        GameObject.FindFirstObjectByType<TopDownCharacterController>().rb.linearVelocity = Vector2.zero;
        GameObject.FindFirstObjectByType<CameraController>().targetStage = activeStage.transform;
        PlayerPrefs.SetInt("newHighscore", 0);
        ScoreManager.playerScore = 0;
        GreetStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStage(){
      panelAnimator.Play("panelFade");
      StartCoroutine(GoToNextStage());
    }

    void GreetStage(){
        currentStage++;
        stageGreetText.SetText("Stage " + currentStage.ToString());
        StartCoroutine(removeStageGreeting());


    }

    IEnumerator removeStageGreeting(){
        yield return new WaitForSeconds(1f);
        stageGreetText.SetText("");
    }

    IEnumerator GoToNextStage(){
        yield return new WaitForSeconds(0.5f);
          Destroy(activeStage);
        activeStage = Instantiate(stages[Random.Range(0,stages.Length)]);
        activeStage.transform.position = new Vector3(0,0,0);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0,0,0);
        GameObject.FindFirstObjectByType<CameraController>().targetStage = activeStage.transform;
        ScoreManager.playerScore += 200;
        GreetStage();
    }
}
