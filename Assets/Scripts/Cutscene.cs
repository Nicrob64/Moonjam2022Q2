using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CutsceneEvent
{
    public float time = 0;
    public UnityEvent ev;
    public bool done = false;
}

public class Cutscene : MonoBehaviour
{
    public bool shouldRun = true;
    float currentTime = 0;
    //public List<KeyValuePair<float, UnityEvent>> events;
    public List<CutsceneEvent> events;
    
    public void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        if (shouldRun)
        {
            currentTime += Time.deltaTime;
            bool isEverythingdone = true;
            foreach (CutsceneEvent e in events)
            {
                isEverythingdone = isEverythingdone && e.done;
                if (currentTime > e.time && !e.done)
                {
                    e.ev.Invoke();
                    e.done = true;
                }
            }
            if (isEverythingdone)
            {
                Stop();
            }
        }
       
    }


    public void Reset()
    {
        foreach(CutsceneEvent e in events)
        {
            e.done = false;
        }
        currentTime = 0;   
    }

    public void Play() 
    {
        currentTime = 0;
        shouldRun = true;
    }

    public void Stop()
    {
        currentTime = 0;
        shouldRun = false;
        Reset();
    }
}
