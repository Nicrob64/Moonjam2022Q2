using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Person
{
    public string Title;
    public string GivenName;
    public string Surname;
    public string StreetAddress;
    public string City;
    public string State;
    public string Country;
    public string EmailAddress;
    public string TelephoneNumber;
    public string ZipCode;
    public string UPS;
}

[System.Serializable]
public class ListOfPeople
{
    public List<Person> people;
    public Person GetPerson(int person)
    {
        return people[person%people.Count];
    }
}
