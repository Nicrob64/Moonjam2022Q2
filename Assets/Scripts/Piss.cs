using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piss : MonoBehaviour
{
    public float MaxPiss = 100f;
    public float PissGainPerSecond = 1f;
    public float PissGraceTimer = 5f;

    public float CurrentPiss { get; private set; }
    public float PissOverloadTimer { get; private set; }

    void Urinate()
    {
        CurrentPiss = 0;
        PissOverloadTimer = 0;
    }

    void Awake()
    {
        Urinate();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentPiss += PissGainPerSecond * Time.deltaTime;

        if(CurrentPiss >= MaxPiss)
        {
            PissOverloadTimer += Time.deltaTime;
        }

        if(PissOverloadTimer > PissGraceTimer)
        {
            // You pissed yourself!
            Debug.LogWarning("You pissed yourself!");
        }
    }
}
