using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedBoxSpawnChance
{
    [Tooltip("Box prefab to spawn. May be null to indicate the possibility of an empty slot.")]
    public GameObject Prefab;
    [Tooltip(
        "Chance of this box prefab spawning. For instance, if there are 3 prefabs with weights 25, 25 and 50, the first two have a 25% chance and the third has a 50% chance.")]
    public double Weight;
}

public class Shelf : MonoBehaviour
{

    // public Texture boxLogo;

    [Tooltip("List of box prefabs which can be spawned in each spot.")]
    public List<WeightedBoxSpawnChance> BoxPrefabs;


    private double SumOfWeights;
    private System.Random Rand;

    // Returns a random prefab from the list of box prefabs based on the weighted chance of spawning each box.
    // May return null, indicating an empty slot.
    GameObject GetRandomWeightedPrefab()
    {
        double value = Rand.NextDouble() * SumOfWeights;

        foreach(var prefabInfo in BoxPrefabs)
        {
            value -= prefabInfo.Weight;
            if(value > 0) continue;

            return prefabInfo.Prefab;
        }

        throw new Exception("Should never get here");
    }

    void Awake()
    {
        Rand = new System.Random();
        foreach(var prefabInfo in BoxPrefabs)
        {
            SumOfWeights += prefabInfo.Weight;
        }

        foreach(var transform in gameObject.GetComponentsInChildren<Transform>())
        {
            if(transform.gameObject.CompareTag("BoxSpawn"))
            {
                GameObject prefab = GetRandomWeightedPrefab();
                if(prefab != null)
                {
                    GameObject newBox = Instantiate(prefab, transform.position, transform.rotation);
                    newBox.transform.SetParent(transform);
                }
            }
        }
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     BoxLogoImage[] logos = GetComponentsInChildren<BoxLogoImage>();
    //     foreach(BoxLogoImage b in logos)
    //     {
    //         b.SetTexture(boxLogo);
    //     }
    // }
}
