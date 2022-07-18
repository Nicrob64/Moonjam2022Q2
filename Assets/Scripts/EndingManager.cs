using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    public Sprite unionImg;
    public Sprite corpoImg;
    public AudioClip corpo;
    public AudioClip union;
    public AudioSource asource;
  
    private void Awake()
    {
        if(SceneTransitionHelper.Instance.TransitionReason == TransitionReason.EndingCorpo)
        {
            image.sprite = corpoImg;
            asource.clip = corpo;
        }
        else
        {
            image.sprite = unionImg;
            asource.clip = union;
        }
        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
