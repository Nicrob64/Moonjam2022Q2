using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Dictionary<PickableItemInfo, int> ItemsHeld = new Dictionary<PickableItemInfo, int>();

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
