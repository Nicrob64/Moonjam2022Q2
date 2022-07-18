using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSkipHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetKey(KeyCode.Alpha1))
            {
                SceneManager.LoadScene("Warehouse");
            }
            else if(Input.GetKey(KeyCode.Alpha2))
            {
                SceneTransitionHelper.Instance.TransitionReason = TransitionReason.PromotionToQA;
                SceneManager.LoadScene("Promotion");
            }
            else if(Input.GetKey(KeyCode.Alpha3))
            {
                SceneTransitionHelper.Instance.TransitionReason = TransitionReason.PromotionToManager;
                SceneManager.LoadScene("Promotion");
            }
        }
    }
}
