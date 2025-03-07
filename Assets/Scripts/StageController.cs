using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class StageController : MonoBehaviour
{

    public GameObject activeStage;
    public GameObject[] stages;

    public GameObject tutorialStage;
    public AudioClip nextStageSound;

    public TMP_Text stageGreetText;

    public static int currentStage = 1;

    public Animator panelAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        currentStage = 1;
            activeStage = Instantiate(tutorialStage);
        
        activeStage.transform.position = new Vector3(0,0,0);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0,0,0);
        GameObject.FindFirstObjectByType<TopDownCharacterController>().GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GameObject.FindFirstObjectByType<CameraController>().targetStage = activeStage.transform;
        PlayerPrefs.SetInt("newHighscore", 0);
        ScoreManager.playerScore = 0;
        
        FindFirstObjectByType<MusicController>().SetMusicState(MusicState.Tutorial);
        GreetStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStage(){
      panelAnimator.Play("panelFade");
      FindFirstObjectByType<MusicController>().SetMusicState(MusicState.Game);
      StartCoroutine(GoToNextStage());
    }

    void GreetStage(){
        
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(0,0,0);
        GameObject.FindFirstObjectByType<CameraController>().targetStage = activeStage.transform;
        GameObject.FindFirstObjectByType<TopDownCharacterController>().GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        AudioSource playerAudio = player.GetComponent<AudioSource>();
        playerAudio.clip = nextStageSound;
        playerAudio.Play();
        ScoreManager.playerScore += 200;
        currentStage++;
        GreetStage();
    }
}
