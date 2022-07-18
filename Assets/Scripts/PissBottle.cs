using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PissBottle : MonoBehaviour
{
    public void PissInBottle()
    {
        EventManager.Instance.StartPissingInBottle();
        
        Destroy(gameObject);
    }
}
