using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject startGameTooltip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveStartGameToolTip(){
        startGameTooltip.SetActive(false);
    }
}
