using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractionEvent : UnityEvent {}

public class Interactable : MonoBehaviour
{

    public InteractionEvent OnInteracted;

    public void Interact()
    {
        OnInteracted?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
