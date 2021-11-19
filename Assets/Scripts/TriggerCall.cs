using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCall : MonoBehaviour
{
    [System.Flags]
    private enum TriggerType
    {
        OnTriggerEnter = 1,
        OnTriggerExit = 2, 
    }

    [SerializeField] 
    private TriggerType type = 0;
    [SerializeField]
    private string targetTag = "Player";

    [Space(10)]
    public UnityEvent broadcastTrigger = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((type & TriggerType.OnTriggerEnter) == 0)
            return;

        if(collision.CompareTag(targetTag))
            broadcastTrigger.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((type & TriggerType.OnTriggerExit) == 0)
            return;

        if (collision.CompareTag(targetTag))
            broadcastTrigger.Invoke();
    }
}
