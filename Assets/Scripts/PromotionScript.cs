using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PromotionScript : MonoBehaviour
{
    public Text promotionText;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        switch (SceneTransitionHelper.Instance.TransitionReason)
        {
            case TransitionReason.PromotionToQA:
                promotionText.text = "You have been promoted to\nQuality Assurance Specialist";
                break;
            case TransitionReason.PromotionToManager:
                promotionText.text = "You have been promoted to\nLocal Branch Manager";
                break;
        }
    }

    public void MoveToNextScene()
    {
        switch (SceneTransitionHelper.Instance.TransitionReason)
        {
            case TransitionReason.PromotionToQA:
                SceneManager.LoadScene("QAScene");
                break;
            case TransitionReason.PromotionToManager:
                
                break;
        }
    }
}
