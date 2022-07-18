using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TextAsset))]
public class PickableItemManager : MonoBehaviour
{

    private static PickableItemManager _instance;

    public static PickableItemManager Instance
    {
        get { return _instance; }
    }

    public TextAsset ItemInfoFile;
    ListOfPickableItems ItemList;

    void Awake()
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

        ItemList = JsonUtility.FromJson<ListOfPickableItems>(ItemInfoFile.text);

        // Randomize the order of the list so that pickable items are distributed randomly
        var rand = new System.Random();
        ItemList.Items = ItemList.Items.OrderBy(_ => rand.Next()).ToList();
    }

    void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PickableItem");

        for(int i = 0; i < gameObjects.Length; ++i)
        {
            var gameObject = gameObjects[i];
            var item = gameObject.GetComponent<PickableItem>();
            if(item == null)
            {
                Debug.LogWarning("GameObject tagged as PickableItem has no PickableItem component", gameObject);
                continue;
            }

            PickableItemInfo info = ItemList.Items[i % ItemList.Items.Count];
            // Debug.Log(string.Format("Set {0} item value to {1} {2}", item, info.ItemName, info.TexturePath));
            item.ItemInfo = info;
        }
    }

    // public PickableItemInfo GetRandomItem()
    // {
    //     int rando = System.Random.Range(0, ItemList.Items.Count);
    //     return ItemList.Items[rando];
    // }

    public ListOfPickableItems GenerateShoppingList(ushort listSize)
    {
        if(listSize > ItemList.Items.Count)
        {
            //We should be able to duplicate items though, I want to order 80 tails ya dig
            Debug.LogWarning(String.Format(
                "Cannot create a list of {0} items as there are only {1} unique items",
                listSize,
                ItemList.Items.Count));

            listSize = (ushort) ItemList.Items.Count;
        }

        var rand = new System.Random();
        var randomizedItems = ItemList.Items.OrderBy(_ => rand.Next()).ToList();

        if(listSize < ItemList.Items.Count)
        {
            randomizedItems.RemoveRange(listSize, ItemList.Items.Count - listSize);
        }

        var shoppingList = new ListOfPickableItems()
        {
            Items = randomizedItems
        };

        return shoppingList;
    }
}
