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
    float currentTime = 0;
    //public List<KeyValuePair<float, UnityEvent>> events;
    public List<CutsceneEvent> events;
    
    public void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        foreach(CutsceneEvent e in events)
        {
            if(currentTime > e.time && !e.done)
            {
                e.ev.Invoke();
                e.done = true;
            }
        }
       
    }

}
