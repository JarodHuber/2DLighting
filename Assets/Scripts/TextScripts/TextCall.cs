using UnityEngine;
using UnityEngine.Events;

public class TextCall : SystemCaller
{
    [SerializeField]
    private TextManager textBox = null;

    [Space(10)]
    [SerializeField, TextArea]
    private string[] textToShow = new string[0];
    [SerializeField]
    private Vector2 boxSize = new Vector2(), boxPosition = new Vector2();

    [Space(10)]
    public UnityEvent broadcastTextBoxComplete;

    public override void Callback()
    {
        broadcastTextBoxComplete.Invoke();
    }

    public override void Broadcast()
    {
        textBox.UpdateText(textToShow, boxSize, boxPosition, this);
    }
}
