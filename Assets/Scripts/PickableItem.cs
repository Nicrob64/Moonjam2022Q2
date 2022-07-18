using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PickableItemInfo
{
    public string ItemName;
    public string TexturePath;
}

[System.Serializable]
public struct ListOfPickableItems
{
    public List<PickableItemInfo> Items;
}

[RequireComponent(typeof(Renderer))]
public class PickableItem : MonoBehaviour
{

    PickableItemInfo _itemInfo;
    public PickableItemInfo ItemInfo
    {
        get
        {
            return _itemInfo;
        }
        set
        {
            _itemInfo = value;
            Image = Resources.Load<Texture2D>(_itemInfo.TexturePath);
            LabelRenderer.material.SetTexture("_MainTex", Image);
        }
    }
    public Texture2D Image;

    public Renderer LabelRenderer;

    public void AddToInventory()
    {
        EventManager.Instance.ItemPicked(ItemInfo);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
