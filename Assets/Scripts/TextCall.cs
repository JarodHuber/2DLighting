using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextCall : MonoBehaviour, ITextBoxCaller
{
    [SerializeField] TextBox textBox = null;

    [Space(10)]
    [SerializeField, TextArea]
    string[] textToShow = new string[0];
    [SerializeField]
    Vector2 boxSize = new Vector2(), boxPosition = new Vector2();

    [Space(10)]
    [SerializeField]
    private UnityEvent broadcastTextBoxComplete;

    public void Callback()
    {
        broadcastTextBoxComplete.Invoke();
    }

    public void CallText()
    {
        textBox.UpdateText(textToShow, boxSize, boxPosition, this);
    }
}
