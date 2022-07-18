using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Dictionary<PickableItemInfo, int> ItemsHeld = new Dictionary<PickableItemInfo, int>();

    public void RefreshShoppingList()
    {
        // Can only get a new list when the current one has been completed or failed
        if(ItemsHeld.Count > 0)
        {
            return;
        }

        ListOfPickableItems list = PickableItemManager.Instance.GenerateShoppingList(GameStateManager.Instance.GetOrderSize());

        foreach(var pickableItem in list.Items)
        {
            ItemsHeld.Add(pickableItem, 0);
        }

        Debug.Log(ItemsHeld);

        EventManager.Instance.ShoppingListChanged(ItemsHeld);
    }

    public void PickupItem(PickableItemInfo item)
    {
        if(!ItemsHeld.ContainsKey(item))
        {
            // Can't pick up an item that isn't on the current list
            return;
        }

        ItemsHeld[item] += 1;

        EventManager.Instance.ShoppingListChanged(ItemsHeld);
    }

    public bool ValidateAndClearInventory()
    {
        if(ItemsHeld.Count == 0) return false;

        bool result = true;

        foreach(var item in ItemsHeld)
        {
            if(item.Value < 1)
            {
                result = false;
            }
        }

        ItemsHeld.Clear();
        EventManager.Instance.ShoppingListChanged(ItemsHeld);

        if(result)
        {
            GameStateManager.Instance.CompleteOrder();
        }

        return result;
    }

    void Awake()
    {
        EventManager.Instance.OnItemPicked += PickupItem;
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
