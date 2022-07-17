using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaMeter : MonoBehaviour
{
    public PlayerCharacterController PlayerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float staminaRatio = PlayerController.Stamina / PlayerController.MaxStamina;
        transform.localScale = new Vector3(staminaRatio, 1f, 1f);
    }
}
