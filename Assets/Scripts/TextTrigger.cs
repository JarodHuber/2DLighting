using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] TextBox textBox = null;

    [Space(10)]
    [SerializeField, TextArea] 
    string[] textToShow = new string[0];
    [SerializeField] 
    Vector2 boxSize = new Vector2(), boxPosition = new Vector2();

    [Space(10)]
    [SerializeField, ReadOnly]
    bool isTriggered = false;

    private void Update()
    {
        if (!isTriggered)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            textBox.UpdateText(textToShow, boxSize, boxPosition);
            isTriggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isTriggered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isTriggered = false;
    }
}
