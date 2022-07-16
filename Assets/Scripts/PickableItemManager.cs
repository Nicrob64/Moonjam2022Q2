using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextAsset))]
public class PickableItemManager : MonoBehaviour
{

    public TextAsset ItemInfoFile;
    ListOfPickableItems ItemList;

    void Awake()
    {
        ItemList = JsonUtility.FromJson<ListOfPickableItems>(ItemInfoFile.text);

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PickableItem");

        foreach(var gameObject in gameObjects)
        {
            var item = gameObject.GetComponent<PickableItem>();
            if(item == null)
            {
                Debug.LogWarning("GameObject tagged as PickableItem has no PickableItem component", gameObject);
                continue;
            }

            PickableItemInfo info = GetRandomItem();
            Debug.Log(string.Format("Set {0} item value to {1} {2}", item, info.ItemName, info.TexturePath));
            item.ItemInfo = info;
        }
    }

    public PickableItemInfo GetRandomItem()
    {
        int rando = Random.Range(0, ItemList.Items.Count);
        return ItemList.Items[rando];
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
