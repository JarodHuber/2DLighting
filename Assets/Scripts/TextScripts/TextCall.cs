using UnityEngine;
using UnityEngine.Events;

public class TextCall : MonoBehaviour, ICaller
{
    [SerializeField]
    private TextBox textBox = null;

    [Space(10)]
    [SerializeField, TextArea]
    private string[] textToShow = new string[0];
    [SerializeField]
    private Vector2 boxSize = new Vector2(), boxPosition = new Vector2();

    [Space(10)]
    public UnityEvent broadcastTextBoxComplete;

    public void Callback()
    {
        broadcastTextBoxComplete.Invoke();
    }

    public void CallText()
    {
        textBox.UpdateText(textToShow, boxSize, boxPosition, this);
    }
}
