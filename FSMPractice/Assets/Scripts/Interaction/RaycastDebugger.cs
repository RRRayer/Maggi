using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaycastDebugger : MonoBehaviour
{
    void Update()
    {
        // 카메라에서 마우스 위치로 Ray 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ray의 방향 확인을 위해 DrawRay를 사용
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

        // 실제 Raycast 실행
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray가 충돌한 오브젝트: " + hit.collider.name);
        }
    }
}

