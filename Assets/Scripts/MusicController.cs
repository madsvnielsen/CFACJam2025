using UnityEngine;


public enum MusicState {
    Menu,
    Game,
    Tutorial
}

public class MusicController : MonoBehaviour
{

    public MusicState currentState;

    public AudioClip menuMusic;
    public AudioClip gameMusic;
       public AudioClip tutorialMusic;

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
        } else if(newState == MusicState.Tutorial){
            musicSource.clip = tutorialMusic;
        } else{
            musicSource.clip = gameMusic;
        }
        musicSource.Play();
        currentState = newState;
    }
}
