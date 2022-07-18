using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    int currentStation;
    public List<AudioClip> stations;
    public AudioSource main;
    public AudioSource tune;
    bool isSwitching = false;


    public void ChannelUp()
    {
        if (isSwitching) return;
        currentStation = (currentStation + 1) % stations.Count;
        StartCoroutine(SwitchStation());
    }

    public void ChannelDown()
    {
        if (isSwitching) return;
        currentStation--;
        if(currentStation < 0)
        {
            currentStation = stations.Count - 1;
        }
        StartCoroutine(SwitchStation());
    }

    IEnumerator SwitchStation()
    {
        isSwitching = true;
        tune.PlayOneShot(tune.clip);
        yield return new WaitForSeconds(0.4f);
        main.Pause();
        yield return new WaitForSeconds(1.0f);
        main.clip = stations[currentStation];
        main.Play();
        yield return new WaitForSeconds(0.4f);
        isSwitching = false;
    }
}
