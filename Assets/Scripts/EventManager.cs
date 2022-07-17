using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private static EventManager _instance;

    public static EventManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    public event Action OnPackageCompleted;
    public void PackageCompleted()
    {
        OnPackageCompleted?.Invoke();
    }

    public event Action<Dictionary<PickableItemInfo, int>> OnShoppingListChanged;
    public void ShoppingListChanged(Dictionary<PickableItemInfo, int> list)
    {
        Debug.Log(OnShoppingListChanged);
        OnShoppingListChanged?.Invoke(list);
    }

    public event Action<PickableItemInfo> OnItemPicked;
    public void ItemPicked(PickableItemInfo item)
    {
        OnItemPicked?.Invoke(item);
    }

}
