using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Dictionary<PickableItemInfo, int> ItemsHeld = new();

    public void RefreshShoppingList()
    {
        ItemsHeld.Clear();

        ListOfPickableItems list = PickableItemManager.Instance.GenerateShoppingList(4);

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
