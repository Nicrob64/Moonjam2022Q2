using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TextAsset))]
public class PickableItemManager : MonoBehaviour
{

    public TextAsset ItemInfoFile;
    ListOfPickableItems ItemList;

    void Awake()
    {
        ItemList = JsonUtility.FromJson<ListOfPickableItems>(ItemInfoFile.text);

        // Randomize the order of the list so that pickable items are distributed randomly
        var rand = new System.Random();
        ItemList.Items = ItemList.Items.OrderBy(_ => rand.Next()).ToList();

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
