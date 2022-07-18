using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionReason
{
    Undefined,
    GameOverFailedQuota,
    GameOverPissedYourself
}

// Helper class for passing info between scenes
public class SceneTransitionHelper : MonoBehaviour
{
    private static SceneTransitionHelper _instance;

    public static SceneTransitionHelper Instance
    {
        get { return _instance; }
    }

    public TransitionReason TransitionReason = TransitionReason.Undefined;

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

        DontDestroyOnLoad(gameObject);
    }
}
