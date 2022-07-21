using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused
}

public struct GameRound
{
    // How long the round lasts in seconds
    public ushort RoundTime;
    // Number of orders the player needs to fulfill to complete this round
    public ushort Quota;
    // Number of items per order
    public ushort ItemsPerOrder;
    // Random modifier for the number of items per order. Orders will randomly have +- this many items.
    public ushort OrderSizeModifier;
}

public class GameStateManager : MonoBehaviour
{
    private static readonly GameRound[] _rounds = new[]
    {
        new GameRound { RoundTime = 180, Quota = 3, ItemsPerOrder = 2, OrderSizeModifier = 0 },
        new GameRound { RoundTime = 210, Quota = 4, ItemsPerOrder = 3, OrderSizeModifier = 1 },
        new GameRound { RoundTime = 270, Quota = 5, ItemsPerOrder = 5, OrderSizeModifier = 2 }
        // new GameRound { RoundTime = 300, Quota = 7, ItemsPerOrder = 5, OrderSizeModifier = 2 },
        // new GameRound { RoundTime = 360, Quota = 10, ItemsPerOrder = 6, OrderSizeModifier = 2 }
    };
    private ushort _currentRoundIndex = 0;
    private readonly System.Random _rand = new System.Random();
    private static GameStateManager _instance;

    public static GameStateManager Instance
    {
        get { return _instance; }
    }

    public GameRound CurrentRound
    {
        get { return _rounds[_currentRoundIndex]; }
    }

    public GameState CurrentState { get; private set; }

    public ushort OrdersRemainingForDailyQuota { get; private set; }

    public float RemainingTimeInRound { get; private set; }

    public float RoundTransitionDuration = 5.0f;

    public Cutscene narrator;
    public Announcement announcement;

    private void NextRound()
    {
        if(_currentRoundIndex >= _rounds.Length - 1)
        {
            Debug.LogWarning("Reached final round; can't advance to a new round.");
            SceneTransitionHelper.Instance.TransitionReason = TransitionReason.PromotionToQA;
            SceneTransitionHelper.Instance.FromScene = FromScene.Warehouse;
            SceneManager.LoadScene("Promotion");
            return;
        }

        CurrentState = GameState.Paused;

        _currentRoundIndex++;
        OrdersRemainingForDailyQuota = CurrentRound.Quota;
        RemainingTimeInRound = CurrentRound.RoundTime;

        EventManager.Instance.RoundComplete();
    }

    public void OnRoundTransitionComplete()
    {
        Debug.Log("GameStateManager.OnRoundTransitionComplete");
        CurrentState = GameState.Playing;
    }

    public void CompleteOrder()
    {
        OrdersRemainingForDailyQuota--;
        if(OrdersRemainingForDailyQuota == 0)
        {
            NextRound();
        }


        EventManager.Instance.PackageCompleted();
    }

    public ushort GetOrderSize()
    {
        int orderSize = CurrentRound.ItemsPerOrder;
        orderSize += _rand.Next(CurrentRound.OrderSizeModifier * -1, CurrentRound.OrderSizeModifier);

        if(orderSize <= 0)
        {
            Debug.LogWarning(String.Format("Generated invalid order size {0}. Round parameters are set incorrectly.", orderSize));
            orderSize = 1;
        }

        return (ushort) orderSize;
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        CurrentState = GameState.Playing;
        OrdersRemainingForDailyQuota = CurrentRound.Quota;
        RemainingTimeInRound = CurrentRound.RoundTime;

        EventManager.Instance.OnRoundTransitionComplete += OnRoundTransitionComplete;
    }

    private void Start()
    {
        if (SceneTransitionHelper.Instance.TransitionReason == TransitionReason.Retry)
        {
            narrator.shouldRun = false;
            announcement.StartBGM();
            announcement.StartAnnouncements();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentState != GameState.Playing)
        {
            return;
        }

        RemainingTimeInRound -= Time.deltaTime;

        if(RemainingTimeInRound <= 0 && OrdersRemainingForDailyQuota > 0)
        {
            Debug.Log("You didn't meet your daily quota!");
            SceneTransitionHelper.Instance.TransitionReason = TransitionReason.GameOverFailedQuota;
            SceneTransitionHelper.Instance.FromScene = FromScene.Warehouse;

            SceneManager.LoadScene("GameOver");
        }
    }
}
