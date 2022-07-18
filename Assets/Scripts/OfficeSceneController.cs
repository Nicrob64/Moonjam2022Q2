using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSceneController : MonoBehaviour
{

    public QTEScript QTE;

    public void DoQTESession(int session)
    {
        QTE.Reset();

        if(session == 0)
        {
            QTE.AddButtonPrompt(KeyCode.B, 1f);
            QTE.AddButtonPrompt(KeyCode.A, 1f);
            QTE.AddButtonPrompt(KeyCode.L, 1f);
            QTE.AddButtonPrompt(KeyCode.D, 1f);
            QTE.AddButtonPrompt(KeyCode.At, 1f);
            QTE.Activate(3);
        }

        if(session == 1)
        {
            QTE.Reset();

            QTE.AddButtonPrompt(KeyCode.LeftBracket, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Alpha1, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Alpha5, 0.5f);
            QTE.AddButtonPrompt(KeyCode.RightBracket, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Ampersand, 1f);

            QTE.Activate(3);
        }

        if(session == 2)
        {
            QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.DownArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Hash, 1.0f);
            QTE.AddButtonPrompt(KeyCode.RightArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.RightArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.LeftArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Equals, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Return, 0.5f);
        }
    }

}
