using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct QTEButton
{
    // Character that the player has to press
    public char character;

    // Delay before the character is pressable
    public float delay;
}

public class QTEScript : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    // Grace period during which the player can successfully press the button after the initial delay
    public float gracePeriod = 0.25f;

    // true if the QTE is currently running
    bool active = false;

    // Elapsed time for the current character
    float elapsedTime = 0.0f;

    // List of button prompts for the QTE sequence
    List<QTEButton> buttonPrompts = new List<QTEButton>();

    public void AddButtonPrompt(char character, float delay)
    {
        buttonPrompts.Add(new QTEButton()
        { 
            character = character,
            delay = delay
        });
    }

    public void Reset()
    {
        buttonPrompts.Clear();
    }

    public void Activate()
    {
        active = true;
        textMesh.renderMode = TextRenderFlags.Render;
        textMesh.alpha = 1.0f;
    }

    public void Deactivate()
    {
        textMesh.alpha = 1.0f;
        textMesh.renderMode = TextRenderFlags.DontRender;
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {        
        AddButtonPrompt('H', 1.0f);
        AddButtonPrompt('E', 0.5f);
        AddButtonPrompt('L', 1.0f);
        AddButtonPrompt('L', 0.25f);
        AddButtonPrompt('O', 0.33f);
        AddButtonPrompt('?', 0.1f);

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

        elapsedTime += Time.deltaTime;

        QTEButton button = buttonPrompts[0];
        textMesh.text = new string(button.character, 1);

        float scale = elapsedTime / button.delay;
        if(scale > 1.0f) scale = 1.0f;

        textMesh.transform.localScale = new Vector3(scale, scale, 1.0f);

        if(elapsedTime > button.delay)
        {
            textMesh.alpha = 1.0f - ((elapsedTime - button.delay) / gracePeriod);
        }
        else
        {
            textMesh.alpha = 1.0f;
        }

        if(elapsedTime > button.delay + gracePeriod)
        {

            buttonPrompts.RemoveAt(0);
            elapsedTime = 0;
        }
    }
}
