using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreAddText sat;
    public static int playerScore = 0; 
    private int previousScore = 0;

    private TMP_Text scoreText;

    public TMP_Text highscoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        sat = FindFirstObjectByType<ScoreAddText>();
        if(PlayerPrefs.HasKey("highscore")) highscoreText.SetText(PlayerPrefs.GetInt("highscore").ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScore > previousScore){
            sat.ScoreAddEffect(playerScore-previousScore);
        }
        previousScore = playerScore;
        scoreText.SetText(playerScore.ToString());
    }
}
