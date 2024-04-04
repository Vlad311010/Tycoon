using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void OnStateEnter(AICore core);
    public abstract void ExecuteState(AICore core);
    public abstract State UpdateState(AICore core);
}
