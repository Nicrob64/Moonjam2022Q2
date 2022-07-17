using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{

    public PlayerCharacterController PlayerController;

    public RectTransform StaminaMeterTransform;

    public TextMeshProUGUI RoundTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float staminaRatio = PlayerController.Stamina / PlayerController.MaxStamina;
        StaminaMeterTransform.localScale = new Vector3(staminaRatio, 1f, 1f);

        TimeSpan t = TimeSpan.FromSeconds(GameStateManager.Instance.RemainingTimeInRound);
        RoundTimer.text = String.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }

}
