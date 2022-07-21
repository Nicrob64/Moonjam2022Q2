using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class InfractionWriter : MonoBehaviour
{
    public AudioClip typeWriterClick;
    public AudioClip typeWriterBell;
    AudioSource asource;

    public float minTimeBetweenCharacters = 0.1f;
    public float maxTimeBetweenCharacters = 0.2f;

    public float slideDuration = 0.5f;
    float currentTime = 0;
    float targetY = 300;
    float originalPos = 0;
    bool animate = false;

    public TextMeshProUGUI text;
    public RectTransform infractionHolder;

    string targetString;

    void Awake()
    {
        asource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (animate)
        {
            currentTime += Time.deltaTime;
            float pc = currentTime / slideDuration;
            pc *= pc;
            Vector2 r = infractionHolder.anchoredPosition; 
            if(currentTime > slideDuration)
            {
                r.y = targetY;
                animate = false;
                currentTime = 0;
            }
            else
            {
                r.y = Mathf.Lerp(originalPos, targetY, pc);
            }
            infractionHolder.anchoredPosition = r; 
        }
    }

    public IEnumerator Infraction(string s)
    {
        targetY = 0;
        originalPos = infractionHolder.rect.height;
        currentTime = 0;
        animate = true;
        text.text = "";
        yield return new WaitForSeconds(slideDuration);
        PlayString(s);
    }

    public void PlayString(string s)
    {
        targetString = s;
        StartCoroutine(AutoType(targetString));
    }


    private IEnumerator AutoType(string s)
    {
        yield return null;
        text.text = s;
        Canvas.ForceUpdateCanvases();
        text.text = string.Empty;

        s = GetFormattedText(s);
        for (int i = 0; i < s.Length; i++)
        {
            text.text = s.Substring(0, i + 1);
            asource.PlayOneShot(typeWriterClick);
            yield return new WaitForSeconds(Random.Range(minTimeBetweenCharacters, maxTimeBetweenCharacters));
        }
        asource.PlayOneShot(typeWriterBell);
        targetY = infractionHolder.rect.height;
        originalPos = 0;
        currentTime = 0;
        animate = true;
    }

    private string GetFormattedText(string s)
    {
        string[] words = s.Split(' ');

        int width = Mathf.FloorToInt(text.rectTransform.rect.width);
        float space = TextWidthApproximation(" ", text.font, text.fontSize, text.fontStyle);

        string newText = string.Empty;
        float count = 0;
        for (int i = 0; i < words.Length; i++)
        {
            float size = TextWidthApproximation(words[i], text.font, text.fontSize, text.fontStyle);
            if (i == 0)
            {
                newText += words[i];
                count += size;
            }
            else if (count + space > width || count + space + size > width)
            {
                newText += "\n";
                newText += words[i];
                count = size;
            }
            else if (count + space + size <= width)
            {
                newText += " " + words[i];
                count += space + size;
            }
        }
        return newText;
    }

    public float TextWidthApproximation(string text, TMP_FontAsset fontAsset, float fontSize, FontStyles style)
    {
        // Compute scale of the target point size relative to the sampling point size of the font asset.
        float pointSizeScale = fontSize / (fontAsset.faceInfo.pointSize * fontAsset.faceInfo.scale);
        float emScale = fontSize * 0.01f;

        float styleSpacingAdjustment = (style & FontStyles.Bold) == FontStyles.Bold ? fontAsset.boldSpacing : 0;
        float normalSpacingAdjustment = fontAsset.normalSpacingOffset;

        float width = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char unicode = text[i];
            TMP_Character character;
            // Make sure the given unicode exists in the font asset.
            if (fontAsset.characterLookupTable.TryGetValue(unicode, out character))
                width += character.glyph.metrics.horizontalAdvance * pointSizeScale + (styleSpacingAdjustment + normalSpacingAdjustment) * emScale;
        }

        return width;
    }

}
