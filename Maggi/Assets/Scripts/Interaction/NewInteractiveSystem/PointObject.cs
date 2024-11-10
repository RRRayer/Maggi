using Pudding.StateMachine.ScriptableObjects;
using UnityEngine;

public class PointObject : InteractiveObject
{
    public  void OnStateEnter(Player player)
    {
        Debug.Log("��ȣ�ۿ� ������Ʈ�� ������ State Enter ����");
    }

    public  void OnUpdate(Player player)
    {
        Debug.Log("��ȣ�ۿ� ������Ʈ�� ������ State Update ����");
    }

    public  void OnStateExit(Player player)
    {
        Debug.Log("��ȣ�ۿ� ������Ʈ�� ������ State Exit ����");
    }
}
