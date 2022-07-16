using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum QTEStatus
{
    Unhandled,
    Failed,
    Succeeded
}

public struct QTEButton
{
    // Character that the player has to press
    public KeyCode keyCode;

    // Delay before the character is pressable
    public float delay;

    // Status for the current QTE
    public QTEStatus status;
}

public class QTEScript : MonoBehaviour
{

    // Mapping from Unity KeyCodes to the corresponding symbols in the 212 Keyboard font
    // BUG: Characters that require a key combo like ? (shift + /) are not accepted by the input logic
    public static Dictionary<KeyCode, char> KeyCodeMap = new Dictionary<KeyCode, char>()
    {
        {KeyCode.Backspace, 'h'},
        {KeyCode.Delete, 'k'},
        {KeyCode.Tab, 'e'},
        {KeyCode.Return, 'a'},
        {KeyCode.Escape, 'm'},
        {KeyCode.Space, 'w'},
        {KeyCode.UpArrow, 'q'},
        {KeyCode.DownArrow, 'r'},
        {KeyCode.RightArrow, 's'},
        {KeyCode.LeftArrow, 't'},
        {KeyCode.Alpha0, '0'},
        {KeyCode.Alpha1, '1'},
        {KeyCode.Alpha2, '2'},
        {KeyCode.Alpha3, '3'},
        {KeyCode.Alpha4, '4'},
        {KeyCode.Alpha5, '5'},
        {KeyCode.Alpha6, '6'},
        {KeyCode.Alpha7, '7'},
        {KeyCode.Alpha8, '8'},
        {KeyCode.Alpha9, '9'},
        {KeyCode.Exclaim, '!'},
        {KeyCode.DoubleQuote, '"'},
        {KeyCode.Hash, '#'},
        {KeyCode.Dollar, '$'},
        {KeyCode.Percent, '%'},
        {KeyCode.Ampersand, '&'},
        {KeyCode.Quote, '\''},
        {KeyCode.LeftParen, '('},
        {KeyCode.RightParen, ')'},
        {KeyCode.Asterisk, '*'},
        {KeyCode.Plus, '+'},
        {KeyCode.Comma, ','},
        {KeyCode.Minus, '-'},
        {KeyCode.Period, '.'},
        {KeyCode.Slash, '/'},
        {KeyCode.Colon, ':'},
        {KeyCode.Semicolon, ';'},
        {KeyCode.Less, '<'},
        {KeyCode.Equals, '='},
        {KeyCode.Greater, '>'},
        {KeyCode.Question, '?'},
        {KeyCode.At, '@'},
        {KeyCode.LeftBracket, '['},
        {KeyCode.Backslash, '\\'},
        {KeyCode.RightBracket, ']'},
        {KeyCode.Caret, '^'},
        {KeyCode.Underscore, '_'},
        {KeyCode.BackQuote, '`'},
        {KeyCode.A, 'A'},
        {KeyCode.B, 'B'},
        {KeyCode.C, 'C'},
        {KeyCode.D, 'D'},
        {KeyCode.E, 'E'},
        {KeyCode.F, 'F'},
        {KeyCode.G, 'G'},
        {KeyCode.H, 'H'},
        {KeyCode.I, 'I'},
        {KeyCode.J, 'J'},
        {KeyCode.K, 'K'},
        {KeyCode.L, 'L'},
        {KeyCode.M, 'M'},
        {KeyCode.N, 'N'},
        {KeyCode.O, 'O'},
        {KeyCode.P, 'P'},
        {KeyCode.Q, 'Q'},
        {KeyCode.R, 'R'},
        {KeyCode.S, 'S'},
        {KeyCode.T, 'T'},
        {KeyCode.U, 'U'},
        {KeyCode.V, 'V'},
        {KeyCode.W, 'W'},
        {KeyCode.X, 'X'},
        {KeyCode.Y, 'Y'},
        {KeyCode.Z, 'Z'}
    };

    public TextMeshProUGUI textMesh;

    // Grace period during which the player can successfully press the button after the initial delay
    public float gracePeriod = 1.0f;

    // true if the QTE is currently running
    bool active = false;

    // Elapsed time for the current character
    float elapsedTime = 0.0f;

    // List of button prompts for the QTE sequence
    List<QTEButton> buttonPrompts = new List<QTEButton>();

    public void AddButtonPrompt(KeyCode keyCode, float delay)
    {
        if(!KeyCodeMap.ContainsKey(keyCode))
        {
            throw new ArgumentException("Invalid key code");
        }

        buttonPrompts.Add(new QTEButton()
        { 
            keyCode = keyCode,
            delay = delay,
            status = QTEStatus.Unhandled
        });
    }

    public void Reset()
    {
        buttonPrompts.Clear();
    }

    void UpdateText()
    {
        QTEButton button = buttonPrompts[0];
        char c = KeyCodeMap[button.keyCode];

        textMesh.text = new string(c, 1);
    }

    // Start displaying the QTE
    public void Activate()
    {
        active = true;
        textMesh.renderMode = TextRenderFlags.Render;
        textMesh.alpha = 1.0f;

        UpdateText();        
    }

    // End the QTE. Happens automatically when all of the button prompts play out.

    public void Deactivate()
    {
        textMesh.alpha = 1.0f;
        textMesh.renderMode = TextRenderFlags.DontRender;
        active = false;
    }

    void HandleInput()
    {
        QTEButton button = buttonPrompts[0];

        if(button.status != QTEStatus.Unhandled)
        {
            return;
        }

        if(elapsedTime < button.delay)
        {
            button.status = QTEStatus.Failed;
        }
        else if(Input.GetKeyDown(button.keyCode))
        {
            button.status = QTEStatus.Succeeded;
        }
        else
        {
            button.status = QTEStatus.Failed;
        }

        buttonPrompts[0] = button;
    }

    void NextButton()
    {
        buttonPrompts.RemoveAt(0);
        elapsedTime = 0;

        if(buttonPrompts.Count == 0)
        {
            return;
        }

        UpdateText();
    }

    // Start is called before the first frame update
    void Start()
    {        
        AddButtonPrompt(KeyCode.H, 1.0f);
        AddButtonPrompt(KeyCode.E, 1.0f);
        AddButtonPrompt(KeyCode.L, 1.0f);
        AddButtonPrompt(KeyCode.L, 1.25f);
        AddButtonPrompt(KeyCode.O, 0.75f);
        AddButtonPrompt(KeyCode.Question, 0.5f);

        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
        {
            return;
        }

        if(buttonPrompts.Count == 0)
        {
            Deactivate();
            return;
        }

        if(Input.anyKeyDown)
        {
            HandleInput();
        }

        QTEButton button = buttonPrompts[0];

        elapsedTime += Time.deltaTime;

        float scale = elapsedTime / button.delay;
        if(scale > 1.0f) scale = 1.0f;

        textMesh.transform.localScale = new Vector3(scale, scale, 1.0f);

        if(elapsedTime > button.delay)
        {
            textMesh.color = Color.blue;
            textMesh.alpha = 1.0f - ((elapsedTime - button.delay) / gracePeriod);
        }
        else
        {
            textMesh.color = Color.white;
            textMesh.alpha = 1.0f;
        }

        if(button.status == QTEStatus.Succeeded)
        {
            textMesh.color = new Color(0.0f, 1.0f, 0.0f, textMesh.alpha);
        }
        else if(button.status == QTEStatus.Failed)
        {
            textMesh.color = new Color(1.0f, 0.0f, 0.0f, textMesh.alpha);
        }

        if(elapsedTime > button.delay + gracePeriod)
        {
            Debug.Log(String.Format("Elapsed time {0} delay {1} grace period {2}", elapsedTime, button.delay, gracePeriod));
            NextButton();
        }
    }
}
