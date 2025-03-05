using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public TMP_Text newscoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newscoreText.SetText("");
        if(PlayerPrefs.HasKey("newHighscore")){
            if(PlayerPrefs.GetInt("newHighscore") == 1) newscoreText.SetText("New highscore!");
        }
        if(PlayerPrefs.HasKey("prevScore")) scoreText.SetText(PlayerPrefs.GetInt("prevScore").ToString());
        if(PlayerPrefs.HasKey("highscore")) highscoreText.SetText(PlayerPrefs.GetInt("highscore").ToString());

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Menu(){
        SceneManager.LoadScene("Menu");
    }
    public void Retry(){
        SceneManager.LoadScene("MainScene");
    }
}
