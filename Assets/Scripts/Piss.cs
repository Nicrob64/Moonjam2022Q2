using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Piss : MonoBehaviour
{
    public PlayerCharacterController PlayerController;

    public float MaxPiss = 100f;
    public float PissGainPerSecond = 1f;
    public float PissGraceTimer = 5f;
    public float PissDuration = 10f;

    public float CurrentPiss { get; private set; }
    public float PissOverloadTimer { get; private set; }

    private bool _pissing;
    public bool Pissing
    {
        get { return _pissing; }
        set
        {
            _pissing = value;
            if(_pissing) PissOverloadTimer = 0;

            PlayerController.MovementFrozen = _pissing;
        }
    }

    void Awake()
    {
        CurrentPiss = 0;
        PissOverloadTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_pissing)
        {
            CurrentPiss -= (MaxPiss / PissDuration) * Time.deltaTime;
            if(CurrentPiss <= 0)
            {
                Pissing = false;
            }

            return;
        }

        CurrentPiss += PissGainPerSecond * Time.deltaTime;

        if(CurrentPiss >= MaxPiss)
        {
            CurrentPiss = MaxPiss;
            PissOverloadTimer += Time.deltaTime;
        }

        if(PissOverloadTimer > PissGraceTimer)
        {
            // You pissed yourself!
            Debug.LogWarning("You pissed yourself!");
            PlayerController.gameObject.SetActive(false);

            SceneManager.LoadScene("GameOver");
        }
    }
}
