using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class ClickButton : MonoBehaviour
{
    public UnityEvent clickAction;
    float amountToDepress = 0.011f;
    public GameObject buttonBit;
    AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        aud.PlayOneShot(aud.clip);
        clickAction.Invoke();
        buttonBit.transform.localPosition = new Vector3(0, -amountToDepress, 0);
    }

    private void OnMouseUp()
    {
        buttonBit.transform.localPosition = new Vector3(0, 0, 0);
    }
}
