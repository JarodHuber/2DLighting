using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SystemCaller : MonoBehaviour
{
    public abstract void Callback();
    public abstract void Broadcast();
}

public abstract class ConditionalCaller : SystemCaller
{
    public abstract bool ConditionMet();
}
