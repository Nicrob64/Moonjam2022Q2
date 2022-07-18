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
        OnShoppingListChanged?.Invoke(list);
    }

    public event Action<PickableItemInfo> OnItemPicked;
    public void ItemPicked(PickableItemInfo item)
    {
        OnItemPicked?.Invoke(item);
    }

    // Event for when the player has completed a round (fulfilled their daily quota)
    public event Action OnRoundComplete;
    public void RoundComplete()
    {
        OnRoundComplete?.Invoke();
    }

    // Event for when the visual transition between rounds has finished
    public event Action OnRoundTransitionComplete;
    public void RoundTransitionComplete()
    {
        OnRoundTransitionComplete?.Invoke();
    }

}
