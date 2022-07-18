using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    public Sprite unionImg;
    public Sprite corpoImg;
    private void Awake()
    {
        if(SceneTransitionHelper.Instance.TransitionReason == TransitionReason.EndingCorpo)
        {
            image.sprite = corpoImg;
        }
        else
        {
            image.sprite = unionImg;
        }
        
    }
}
