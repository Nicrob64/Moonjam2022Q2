using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    ListOfPeople listOfPeople;
    public TextAsset peopleFile;
    public BoxLabel boxLabel;

    void Awake()
    {
        listOfPeople = JsonUtility.FromJson<ListOfPeople>(peopleFile.text);
    }

    public Person GetRandomPerson()
    {
        int rando = Random.Range(0, listOfPeople.people.Count);
        return listOfPeople.GetPerson(rando);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            UpdateBoxLabel();
        }
    }

    void UpdateBoxLabel()
    {
        boxLabel.SetDetails(GetRandomPerson());
    }
    
}
