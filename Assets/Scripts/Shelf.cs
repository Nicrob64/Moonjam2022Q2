using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{

    public Texture boxLogo;

    // Start is called before the first frame update
    void Start()
    {
        BoxLogoImage[] logos = GetComponentsInChildren<BoxLogoImage>();
        foreach(BoxLogoImage b in logos)
        {
            b.SetTexture(boxLogo);
        }
    }
}
