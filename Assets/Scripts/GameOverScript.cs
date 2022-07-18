using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public Text ReasonMessageText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // In case the previous scene didn't have a SceneTransitionHelper
        if(SceneTransitionHelper.Instance == null) return;

        switch(SceneTransitionHelper.Instance.TransitionReason)
        {
            case TransitionReason.GameOverFailedQuota:
                ReasonMessageText.text = "(You failed to meet your quota)";
                break;
            case TransitionReason.GameOverPissedYourself:
                ReasonMessageText.text = "(You pissed yourself as an adult! Gross)";
                break;
            case TransitionReason.GameOverFuckedUpTooManyOrders:
                ReasonMessageText.text = "(You messed up too many shipping orders. How will the company survive?)";
                break;
            case TransitionReason.GameOverFailedTheQTE:
                ReasonMessageText.text = "(Fucked up the QTE lole)";
                break;
            default:
                break;
        }
    }

    public void TryAgain()
    {
        SceneTransitionHelper.Instance.TransitionReason = TransitionReason.Retry;
        switch (SceneTransitionHelper.Instance.FromScene)
        {
            case FromScene.Warehouse:
                SceneManager.LoadScene("Warehouse");
                break;
            case FromScene.QA:
                SceneManager.LoadScene("QAScene");
                break;
            case FromScene.Management:
                SceneManager.LoadScene("ManagerOffice");
                break;
        }
        
        
    }
}
