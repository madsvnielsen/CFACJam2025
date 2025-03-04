using TMPro;
using UnityEngine;

public class ScoreAddText : MonoBehaviour
{
    private Animator animator;
    private TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        text = GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreAddEffect(int scoreDelta){
        text.SetText("+" + scoreDelta.ToString());
        animator.Play("score_add_text");

    }
}
