using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : InteractiveObject
{
    public override void OnStateEnter()
    {
        Debug.Log("��ȣ�ۿ� ������Ʈ�� ������ State Enter ����");
    }

    public override void OnStateExit()
    {
        Debug.Log("��ȣ�ۿ� ������Ʈ�� ������ State Exit ����");
    }

    public override void OnUpdate()
    {
        Debug.Log("��ȣ�ۿ� ������Ʈ�� ������ State Update ����");
    }
}
