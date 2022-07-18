using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSceneController : MonoBehaviour
{

    public QTEScript QTE;

    IEnumerator DoQTEs()
    {

        QTE.AddButtonPrompt(KeyCode.B, 1f);
        QTE.AddButtonPrompt(KeyCode.A, 1f);
        QTE.AddButtonPrompt(KeyCode.L, 1f);
        QTE.AddButtonPrompt(KeyCode.D, 1f);

        QTE.Activate(3);

        // Very hacky, should have a way for the QTE to communicate when it's done.
        // Maybe it should expose its own coroutine.
        yield return new WaitForSeconds(2f + (1f + QTE.GracePeriod + QTE.FadeOut) * 4);

        QTE.Reset();

        QTE.AddButtonPrompt(KeyCode.LeftBracket, 0.5f);
        QTE.AddButtonPrompt(KeyCode.Alpha1, 0.5f);
        QTE.AddButtonPrompt(KeyCode.Alpha5, 0.5f);
        QTE.AddButtonPrompt(KeyCode.RightBracket, 0.5f);

        QTE.Activate(3);

        yield return new WaitForSeconds(2f + (0.5f + QTE.GracePeriod + QTE.FadeOut) * 4);

        QTE.Reset();

        QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.DownArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.DownArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.LeftArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.RightArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.LeftArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.RightArrow, 0.5f);
        QTE.AddButtonPrompt(KeyCode.B, 0.5f);
        QTE.AddButtonPrompt(KeyCode.A, 0.5f);
        QTE.AddButtonPrompt(KeyCode.Return, 0.5f);

        QTE.Activate(3);
    }

    void Start()
    {
        StartCoroutine(DoQTEs());
    }
}
