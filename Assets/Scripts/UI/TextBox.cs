using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    GameManager manager = null;
    [Tooltip("How many seconds until the next char appears"), SerializeField]
    Timer textSpeed = new Timer(.01f);
    [SerializeField] 
    TMP_Text text = null;
    [SerializeField] 
    RectTransform boxTransform = null;

    [SerializeField, ReadOnly, TextArea]
    string[] textToShow = new string[0];
    bool textFinished = false;
    int charCounter = 0;
    int stringCounter = 0;

    bool isActive = false;

    private void Update()
    {
        if (!textFinished)
        {
            // If the text is not finished and the player presses E, reveal all of the text
            if (Input.GetKeyDown(KeyCode.E))
            {
                text.maxVisibleCharacters = text.textInfo.characterCount;
                charCounter = 0;
                textSpeed.Reset();

                textFinished = true;
                return;
            }

            // Reveal next char
            if (textSpeed.Check())
            {
                // is only 0 when charCounter == count
                int visibleCount = ++charCounter % text.textInfo.characterCount; // Changed this from postIncrement % count + 1 to preIncrement % count

                // All characters have been revealed
                if (visibleCount == 0)
                {
                    text.maxVisibleCharacters = text.textInfo.characterCount;
                    charCounter = 0;

                    textFinished = true;

                    return;
                }

                text.maxVisibleCharacters = visibleCount;
            }

            return;
        }

        // If the text is finished revealing and the player presses E, cycle to the next string
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (++stringCounter >= textToShow.Length)
            {
                // No more text to show
                HandleVariableSetting(new string[0]);

                return;
            }

            text.maxVisibleCharacters = 0;
            text.text = textToShow[stringCounter];

            textFinished = false;
        }
    }


    void ToggleObject()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void UpdateText(string[] texts, Vector2 boxSize, Vector2 position)
    {
        boxTransform.sizeDelta = boxSize;
        boxTransform.anchoredPosition = position;

        HandleVariableSetting(texts);
    }

    void HandleVariableSetting(string[] stringArr)
    {
        textToShow = stringArr;

        charCounter = 0;
        stringCounter = 0;
        textFinished = false;

        text.maxVisibleCharacters = 0;
        text.text = (textToShow.Length != 0) ? textToShow[0] : "";

        ToggleObject();
        manager.TogglePlayerPause();
    }
}
