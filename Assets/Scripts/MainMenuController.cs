using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string mainSceneName = "MainScene"; // Name of your main game scene
    public string creditsSceneName = "Credits"; // Name of your credits scene


    void Start(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindFirstObjectByType<MusicController>().SetMusicState(MusicState.Menu);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
