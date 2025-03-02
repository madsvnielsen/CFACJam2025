using UnityEngine;
using UnityEngine.SceneManagement;

public class GOController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;
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
