using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class ShoppingListTextGenerator : MonoBehaviour
{

    private TextMeshProUGUI _shoppingListText;

    void UpdateShoppingList(Dictionary<PickableItemInfo, int> list)
    {
        Debug.Log("UpdateShoppingList called");

        string text = "";

        foreach(var item in list)
        {
            // If the item has already been collected, mark it with a strikethrough
            if(item.Value > 0)
            {
                text += String.Format("<s>{0}</s>\n", item.Key.ItemName);
            }
            else
            {
                text += item.Key.ItemName + "\n";
            }
        }

        Debug.Log(text);
        
        _shoppingListText.text = text;
    }

    void Awake()
    {
        _shoppingListText = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(_shoppingListText);
    }

    void Start()
    {
        EventManager.Instance.OnShoppingListChanged += UpdateShoppingList;
    }
}
