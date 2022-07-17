using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    private static GameStateManager _instance;

    public static GameStateManager Instance
    {
        get { return _instance; }
    }

    public int OrdersRemainingForDailyQuota { get; private set; }

    public float RoundTime { get; private set; }

    public float RemainingTimeInRound { get; private set; }

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

        OrdersRemainingForDailyQuota = 5;
        RoundTime = 300f; // 5 minutes
        RemainingTimeInRound = RoundTime;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RemainingTimeInRound -= Time.deltaTime;
    }
}
