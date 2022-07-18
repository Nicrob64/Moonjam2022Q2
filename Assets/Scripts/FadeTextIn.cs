using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTextIn : MonoBehaviour
{
    float fadeinDuration = 1.0f;
    Text img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Text>();
        img.CrossFadeAlpha(0, 0, true);
    }

    public void FadeIn()
    {
        if(img)
        img.CrossFadeAlpha(1.0f, fadeinDuration, false);
    }

    public void FadeOut()
    {
        if(img)
        img.CrossFadeAlpha(0.0f, fadeinDuration, false);
    }
}
