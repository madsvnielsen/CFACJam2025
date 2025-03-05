using UnityEngine;

public class ArtificialCursorLock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector2 screenSize;
    private Vector2 cursorPosition;

    public Vector2 cursorWorldPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        screenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        cursorPosition = new Vector2(screenSize.x/2, screenSize.y/2);
        cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorWorldPosition);
        
    }

    // Update is called once per frame
    void Update()
    {
        cursorPosition += (Vector2)Input.mousePositionDelta * 2f;
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, screenSize.x);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, screenSize.y);
        cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorPosition) ;
    }
}
