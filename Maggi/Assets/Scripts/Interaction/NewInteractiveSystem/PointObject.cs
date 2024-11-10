using Pudding.StateMachine.ScriptableObjects;
using UnityEngine;

public class PointObject : InteractiveObject
{
    public  void OnStateEnter(Player player)
    {
        Debug.Log("상호작용 오브젝트에 구현된 State Enter 동작");
    }

    public  void OnUpdate(Player player)
    {
        Debug.Log("상호작용 오브젝트에 구현된 State Update 동작");
    }

    public  void OnStateExit(Player player)
    {
        Debug.Log("상호작용 오브젝트에 구현된 State Exit 동작");
    }
}
