using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back(){
        SceneManager.LoadScene("Menu");
    }
}
