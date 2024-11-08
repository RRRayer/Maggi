using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public InteractionType Type = InteractionType.General;

    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void OnUpdate();
}
