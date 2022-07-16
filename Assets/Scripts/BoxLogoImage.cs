using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLogoImage : MonoBehaviour
{

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    public void SetTexture(Texture tex)
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetTexture("_MainTex", tex);
        _renderer.SetPropertyBlock(_propBlock);
    }

}
