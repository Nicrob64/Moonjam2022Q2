using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderValidator : MonoBehaviour
{
    public Inventory Inventory;
    public AudioSource AudioSource;
    public AudioClip CorrectAudio;
    public AudioClip IncorrectAudio;


    public void ValidateOrder()
    {
        bool valid = Inventory.ValidateAndClearInventory();
        if(valid)
        {
            AudioSource.PlayOneShot(CorrectAudio);
        }
        else
        {
            AudioSource.PlayOneShot(IncorrectAudio);
        }
    }

}
