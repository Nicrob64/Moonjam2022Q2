using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public int OrdersRemainingForDailyQuota = 5;

    public const float TimePerRoundInSeconds = 300f; // 5 minutes

    public float RemainingTimeInRound = TimePerRoundInSeconds;


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
