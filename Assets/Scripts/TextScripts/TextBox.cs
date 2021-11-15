using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    private GameManager manager = null;
    [Tooltip("How many seconds until the next char appears"), SerializeField]
    private Timer textSpeed = new Timer(.01f);

    [Space(10)]
    [SerializeField]
    private TMP_Text text = null;
    [SerializeField]
    private GameObject buttonPompt = null;
    [SerializeField]
    private RectTransform boxTransform = null;

    [Space(10)]
    [SerializeField, ReadOnly, TextArea]
    private string[] textToShow = new string[0];

    private bool textFinished = false;
    private int charCounter = 0;
    private int stringCounter = 0;
    private ICaller caller;

    private bool isActive = false;

    private void Update()
    {
        if (!textFinished)
        {
            // If the text is not finished and the player presses E, reveal all of the text
            if (Input.GetKeyDown(KeyCode.E))
            {
                FinishText();
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
                    FinishText();
                    return;
                }

                text.maxVisibleCharacters = visibleCount;
            }

            return;
        }

        // If the text is finished revealing and the player presses E, cycle to the next string
        if (Input.GetKeyDown(KeyCode.E))
        {
            buttonPompt.SetActive(false);

            if (++stringCounter >= textToShow.Length)
            {
                // No more text to show
                caller.Callback();
                caller = null;

                HandleVariableSetting(new string[0]);
                return;
            }

            text.maxVisibleCharacters = 0;
            text.text = textToShow[stringCounter];

            textFinished = false;
        }
    }

    private void FinishText()
    {
        text.maxVisibleCharacters = text.textInfo.characterCount;
        charCounter = 0;
        textSpeed.Reset();

        textFinished = true;

        buttonPompt.SetActive(true);
    }


    private void ToggleObject()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void UpdateText(string[] texts, Vector2 boxSize, Vector2 position, ICaller textCaller)
    {
        boxTransform.sizeDelta = boxSize;
        boxTransform.anchoredPosition = position;

        caller = textCaller;

        HandleVariableSetting(texts);
    }

    private void HandleVariableSetting(string[] stringArr)
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