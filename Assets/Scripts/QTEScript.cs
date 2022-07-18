using System;
using System.Collections;
using System.Collections.Generic;
// using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    // static readonly KeyCode[] _allKeyCodes =
    //     System.Enum.GetValues(typeof(KeyCode))
    //     .Cast<KeyCode>()
    //     .Where(k => ((int)k < (int)KeyCode.Mouse0))
    //     .ToArray();

    // Mapping from Unity KeyCodes to the corresponding symbols in the 212 Keyboard font
    // Characters that require a key combination, like ~ (i.e. Shift + `) are not supported because
    // Input.GetKeyDown always returns false for those key codes, and I can't be bothered to figure it out
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
        {KeyCode.RightArrow, 't'},
        {KeyCode.LeftArrow, 's'},
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
        {KeyCode.Comma, ','},
        {KeyCode.Minus, '-'},
        {KeyCode.Period, '.'},
        {KeyCode.Slash, '/'},
        {KeyCode.Semicolon, ';'},
        {KeyCode.Equals, '='},
        {KeyCode.LeftBracket, '['},
        {KeyCode.Backslash, '\\'},
        {KeyCode.RightBracket, ']'},
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
        {KeyCode.Z, 'Z'},

        {KeyCode.Ampersand, '&'},
        {KeyCode.Hash, '#'},
        {KeyCode.Percent, '%'},
        {KeyCode.RightParen, ')'},
        {KeyCode.At, '@'},
        {KeyCode.LeftCurlyBracket, '{'}
    };

    Dictionary<KeyCode, KeyCode> substitutions = new Dictionary<KeyCode, KeyCode>()
    {
        {KeyCode.Ampersand, KeyCode.Alpha7 },
        {KeyCode.Hash, KeyCode.Alpha3 },
        {KeyCode.Percent, KeyCode.Alpha5 },
        {KeyCode.RightParen, KeyCode.Alpha0 },
        {KeyCode.At, KeyCode.Alpha2 },
        {KeyCode.LeftCurlyBracket, KeyCode.LeftBracket },
    };
    
    public TextMeshProUGUI TextMesh;
    public AudioSource AudioSource;
    public AudioClip GoodInputSound;
    public AudioClip BadInputSound;
    public Color ReadyColor = Color.cyan;
    public Color CorrectColor = Color.green;
    public Color IncorrectColor = Color.red;
    

    // Grace period during which the player can successfully press the button after the initial delay
    public float GracePeriod = 1.0f;
    // Time it takes for a button to fade out after it's handled or the time limit is exceeded
    public float FadeOut = 0.25f;
    // Number of failures before failing the QTE
    public ushort FailureLimit { get; private set; }
    // How many times the player has failed the current QTE
    ushort _failureCount = 0;

    // true if the QTE is currently running
    bool _active = false;

    // Elapsed time for the current character
    float _elapsedTime = 0.0f;

    // List of button prompts for the QTE sequence
    readonly List<QTEButton> _buttonPrompts = new();

    public void AddButtonPrompt(KeyCode keyCode, float delay)
    {
        if(!KeyCodeMap.ContainsKey(keyCode))
        {
            throw new ArgumentException("Invalid key code");
        }

        _buttonPrompts.Add(new QTEButton()
        {
            keyCode = keyCode,
            delay = delay,
            status = QTEStatus.Unhandled
        });
    }

    public void Reset()
    {
        _buttonPrompts.Clear();
    }

    void UpdateText()
    {
        QTEButton button = _buttonPrompts[0];
        char c = KeyCodeMap[button.keyCode];

        TextMesh.rectTransform.anchoredPosition = new Vector3(UnityEngine.Random.Range(0.0f, 800.0f) - 400, UnityEngine.Random.Range(0.0f, 500.0f) - 250, 0);
        TextMesh.text = new string(c, 1);
    }

    // Start displaying the QTE
    public void Activate(ushort failureLimit)
    {
        FailureLimit = failureLimit;

        _failureCount = 0;
        _elapsedTime = 0;

        _active = true;
        TextMesh.gameObject.SetActive(_active);

        UpdateText();
    }

    // End the QTE. Happens automatically when all of the button prompts play out.

    public void Deactivate()
    {
        _active = false;
        TextMesh.gameObject.SetActive(_active);
    }

    /*
    public static IEnumerable<KeyCode> GetCurrentKeys()
    {
        if (Input.anyKey)
            for (int i = 0; i < _allKeyCodes.Length; i++)
                if (Input.GetKey(_allKeyCodes[i]))
                    yield return _allKeyCodes[i];
    }

    void DebugInput()
    {
        string keys = "";
        foreach(var keyCode in GetCurrentKeys())
        {
            keys += keyCode.ToString() + "";
        }

        Debug.Log(keys);
    }
    */

    // Call when the player fails an input (either by hitting it too early, hitting the wrong key, or missing it entirely)
    void ButtonFailed(ref QTEButton button)
    {
        button.status = QTEStatus.Failed;
        _failureCount++;

        AudioSource?.PlayOneShot(BadInputSound);

        if(_failureCount >= FailureLimit)
        {
            Debug.Log("QTE failed!");
            SceneTransitionHelper.Instance.TransitionReason = TransitionReason.GameOverFailedTheQTE;
            SceneTransitionHelper.Instance.FromScene = FromScene.Management;
            SceneManager.LoadScene("GameOver");
            Deactivate();
        }
    }

    void HandleInput()
    {
        QTEButton button = _buttonPrompts[0];

        if(button.status != QTEStatus.Unhandled)
        {
            return;
        }

        // Debug.Log(String.Format("Is key code {0} held down? {1}", button.keyCode, Input.GetKey(button.keyCode)));

        bool modifierDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool needsMod = substitutions.ContainsKey(button.keyCode);
        KeyCode toPress = button.keyCode;
        if (needsMod)
        {
            toPress = substitutions[toPress];
        }

        if(Input.GetKey(toPress) && _elapsedTime >= button.delay && (needsMod == modifierDown))
        {
            AudioSource?.PlayOneShot(GoodInputSound);
            button.status = QTEStatus.Succeeded;
        }
        else
        {
            ButtonFailed(ref button);
        }

        _buttonPrompts[0] = button;
    }

    void NextButton()
    {
        _buttonPrompts.RemoveAt(0);
        _elapsedTime = 0;

        if(_buttonPrompts.Count == 0)
        {
            return;
        }

        UpdateText();
    }

    // Start is called before the first frame update
    /*
    void Start()
    {        
        AddButtonPrompt(KeyCode.H, 1.0f);
        AddButtonPrompt(KeyCode.E, 1.0f);
        AddButtonPrompt(KeyCode.L, 1.0f);
        AddButtonPrompt(KeyCode.L, 1.25f);
        AddButtonPrompt(KeyCode.O, 0.75f);
        AddButtonPrompt(KeyCode.LeftArrow, 0.5f);
        AddButtonPrompt(KeyCode.RightArrow, 0.5f);

        Activate(3);
    }
    */

    void Awake()
    {
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        // DebugInput();

        if(!_active)
        {
            return;
        }

        if(_buttonPrompts.Count == 0)
        {
            Deactivate();
            return;
        }

        if(Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)
                || Input.GetKeyDown(KeyCode.RightShift)
                || Input.GetKeyDown(KeyCode.Mouse0)
                || Input.GetKeyDown(KeyCode.Mouse1)
                || Input.GetKeyDown(KeyCode.Mouse2)
                || Input.GetKeyDown(KeyCode.Mouse3)
                || Input.GetKeyDown(KeyCode.Mouse4)
                || Input.GetKeyDown(KeyCode.Mouse5))
            {
                //modifier or mouse
            }
            else
            {
                HandleInput();
            }
        }

        QTEButton button = _buttonPrompts[0];

        _elapsedTime += Time.deltaTime;

        float scale = _elapsedTime / button.delay;
        if(scale > 1.0f) scale = 1.0f;

        TextMesh.transform.localScale = new Vector3(scale, scale, 1.0f);

        if(_elapsedTime > button.delay)
        {
            TextMesh.color = ReadyColor;
        }
        else
        {
            TextMesh.color = Color.white;
            TextMesh.alpha = 1.0f;
        }

        if(button.status == QTEStatus.Succeeded)
        {
            TextMesh.color = CorrectColor;
        }
        else if(button.status == QTEStatus.Failed)
        {
            TextMesh.color = IncorrectColor;
        }

        if(_elapsedTime > button.delay + GracePeriod)
        {
            if(button.status == QTEStatus.Unhandled)
            {
                // GIGA HACK
                // As far as I can tell there's no way to get a reference to a member of a list in C#
                // So I have to reassign the 0th list element to make sure we get the updated values next frame
                ButtonFailed(ref button);
                _buttonPrompts[0] = button;
            }

            float deltaTime = _elapsedTime - (button.delay + GracePeriod);
            TextMesh.alpha = 1.0f - (deltaTime / FadeOut);

            if(deltaTime >= GracePeriod)
            {
                NextButton();
            }
        }
    }
}
