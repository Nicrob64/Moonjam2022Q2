using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public TextMeshProUGUI ReasonMessageText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // In case the previous scene didn't have a SceneTransitionHelper
        if(SceneTransitionHelper.Instance == null) return;

        switch(SceneTransitionHelper.Instance.TransitionReason)
        {
            case TransitionReason.GameOverFailedQuota:
                ReasonMessageText.text = "You failed to meet your quota!";
                break;
            case TransitionReason.GameOverPissedYourself:
                ReasonMessageText.text = "You pissed yourself as an adult! Gross!";
                break;
            default:
                break;
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Warehouse");
    }
}
