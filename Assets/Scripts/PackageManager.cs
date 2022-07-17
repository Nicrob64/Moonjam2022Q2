using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    ListOfPeople listOfPeople;
    public TextAsset peopleFile;

    void Awake()
    {
        listOfPeople = JsonUtility.FromJson<ListOfPeople>(peopleFile.text);
    }

    public Person GetRandomPerson()
    {
        int rando = Random.Range(0, listOfPeople.people.Count);
        return listOfPeople.GetPerson(rando);
    }
}
