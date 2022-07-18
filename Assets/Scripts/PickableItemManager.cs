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
            Debug.Log(string.Format("Set {0} item value to {1} {2}", item, info.ItemName, info.TexturePath));
            item.ItemInfo = info;
        }
    }

    public PickableItemInfo GetRandomSingleItem()
    { 
        return ItemList.Items[UnityEngine.Random.Range(0, ItemList.Items.Count)];
    }
    public PickableItemInfo GetRandomSingleItem(PickableItemInfo butNotThis)
    {
        int index = UnityEngine.Random.Range(0, ItemList.Items.Count);
        PickableItemInfo info = ItemList.Items[index];
        if(info.ItemName == butNotThis.ItemName)
        {
            info = ItemList.Items[(index + 1) % ItemList.Items.Count];
        }
        return info;
    }

    public ListOfPickableItems GenerateNonUniqueItemList(ushort listSize)
    {
        if(listSize < 1)
        {
            throw new ArgumentException("Cannot create a list of size less than one");
        }

        ListOfPickableItems items = new ListOfPickableItems();
        List<PickableItemInfo> myCoolItemList = new List<PickableItemInfo>();
        for(int i=0; i<listSize; i++)
        {
            myCoolItemList.Add(ItemList.Items[UnityEngine.Random.Range(0, ItemList.Items.Count)]);
        }
        items.Items = myCoolItemList;
        return items;
    }
    public ListOfPickableItems GenerateShoppingList(ushort listSize)
    {
        if(listSize > ItemList.Items.Count)
        {
            throw new ArgumentException(String.Format(
                "Cannot create a list of {0} items as there are only {1} unique items",
                listSize,
                ItemList.Items.Count));
        }

        var rand = new System.Random();
        var randomizedItems = ItemList.Items.OrderBy(_ => rand.Next()).ToList();

        if(listSize < ItemList.Items.Count)
        {
            randomizedItems.RemoveRange(listSize, ItemList.Items.Count - listSize);
        }

        var shoppingList = new ListOfPickableItems();
        shoppingList.Items = randomizedItems;

        return shoppingList;
    }
}
