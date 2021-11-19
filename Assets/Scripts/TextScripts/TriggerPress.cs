using NaughtyAttributes;
using UnityEngine;

public class TriggerPress : MonoBehaviour
{
    [SerializeField]
    private SystemCaller caller = null;
    [SerializeField]
    private GameObject activateButton = null;

    [Space(10)]
    [SerializeField, ReadOnly]
    private bool isTriggered = false;

    private void Update()
    {
        if (!isTriggered)
            return;

        if (Input.GetKeyDown(PlayerInputs.GetKey("Interact")))
        {
            caller.Broadcast();
            SetTrigger(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (caller is ConditionalCaller cond && cond.ConditionMet()))
            SetTrigger(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SetTrigger(false);
    }

    private void SetTrigger(bool triggerActive)
    {
        isTriggered = triggerActive;
        activateButton.SetActive(triggerActive);
    }
}
