using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : InteractiveObject
{
    public override void OnStateEnter()
    {
        Debug.Log("상호작용 오브젝트에 구현된 State Enter 동작");
    }

    public override void OnStateExit()
    {
        Debug.Log("상호작용 오브젝트에 구현된 State Exit 동작");
    }

    public override void OnUpdate()
    {
        Debug.Log("상호작용 오브젝트에 구현된 State Update 동작");
    }
}
