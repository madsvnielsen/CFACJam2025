using UnityEngine;


public enum MusicState {
    Menu,
    Game
}

public class MusicController : MonoBehaviour
{

    public MusicState currentState;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private AudioSource musicSource;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(FindObjectsByType<MusicController>(FindObjectsSortMode.None).Length > 1) Destroy(gameObject);
        currentState = MusicState.Menu;
        musicSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicState(MusicState newState){
        if(newState == currentState) return;
        
        if(newState == MusicState.Menu){
            musicSource.clip = menuMusic;
        } else{
            musicSource.clip = gameMusic;
        }
        musicSource.Play();
        currentState = newState;
    }
}
