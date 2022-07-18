using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class UIScript : MonoBehaviour
{

    public GameObject Player;

    public RectTransform StaminaMeterTransform;
    public RectTransform PissMeterTransform;
    public Image PissOverlay;

    public TextMeshProUGUI RoundTimer;

    public TextMeshProUGUI ShoppingList;
    public TextMeshProUGUI RemainingQuota;

    [Tooltip("Rate at which the piss overlay flashes when the piss limit is exceeded, seconds per flash.")]
    public float PissOverlayPulseRate = 1.0f;

    private PlayerCharacterController _playerController;
    private Piss _playerPiss;
    private bool _overlayShown = false;

    void UpdateShoppingList(Dictionary<PickableItemInfo, int> list)
    {
        Debug.Log("UpdateShoppingList called");

        string text = "";

        foreach(var item in list)
        {
            // If the item has already been collected, mark it with a strikethrough
            if(item.Value > 0)
            {
                text += String.Format("<s>{0}</s>\n", item.Key.ItemName);
            }
            else
            {
                text += item.Key.ItemName + "\n";
            }
        }
        
        ShoppingList.text = text;
    }

    void UpdateRemainingQuota()
    {
        RemainingQuota.text = String.Format("Remaining quota: {0}", GameStateManager.Instance.OrdersRemainingForDailyQuota);
    }

    IEnumerator PulsePissOverlay()
    {
        _overlayShown = true;
        float startTime = Time.time;

        while(_playerPiss.PissOverloadTimer > 0)
        {
            float deltaTime = Time.time - startTime;
            float overlayOpacity = (float) Math.Abs(Math.Sin(deltaTime * Math.PI));
            PissOverlay.color = new Color(1f, 1f, 1f, overlayOpacity);
            yield return null;
        }

        PissOverlay.color = new Color(1f, 1f, 1f, 0);
        _overlayShown = false;
    }
    
    void Awake()
    {
        _playerController = Player.GetComponent<PlayerCharacterController>();
        Assert.IsNotNull(_playerController);

        _playerPiss = Player.GetComponent<Piss>();
        Assert.IsNotNull(_playerPiss);

        UpdateRemainingQuota();

        EventManager.Instance.OnShoppingListChanged += UpdateShoppingList;
        EventManager.Instance.OnPackageCompleted += UpdateRemainingQuota;
    }

    // Update is called once per frame
    void Update()
    {
        float staminaRatio = Math.Min(_playerController.Stamina / _playerController.MaxStamina, 1f);
        StaminaMeterTransform.localScale = new Vector3(staminaRatio, 1f, 1f);

        float pissRatio = Math.Min(_playerPiss.CurrentPiss / _playerPiss.MaxPiss, 1f);
        PissMeterTransform.localScale = new Vector3(pissRatio, 1f, 1f);

        if(_playerPiss.PissOverloadTimer > 0 && !_overlayShown)
        {
            StartCoroutine(PulsePissOverlay());
        }

        TimeSpan t = TimeSpan.FromSeconds(GameStateManager.Instance.RemainingTimeInRound);
        RoundTimer.text = String.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }

}
