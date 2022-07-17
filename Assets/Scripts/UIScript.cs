using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class UIScript : MonoBehaviour
{

    public GameObject Player;

    public RectTransform StaminaMeterTransform;
    public RectTransform PissMeterTransform;

    public TextMeshProUGUI RoundTimer;

    public TextMeshProUGUI ShoppingList;
    public TextMeshProUGUI RemainingQuota;

    private PlayerCharacterController _playerController;
    private Piss _playerPiss;

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

        Debug.Log(text);
        
        ShoppingList.text = text;
    }

    void UpdateRemainingQuota()
    {
        RemainingQuota.text = String.Format("Remaining quota: {0}", GameStateManager.Instance.OrdersRemainingForDailyQuota);
    }
    
    void Awake()
    {
        _playerController = Player.GetComponent<PlayerCharacterController>();
        Assert.IsNotNull(_playerController);

        _playerPiss = Player.GetComponent<Piss>();
        Assert.IsNotNull(_playerPiss);

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

        TimeSpan t = TimeSpan.FromSeconds(GameStateManager.Instance.RemainingTimeInRound);
        RoundTimer.text = String.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }

}
