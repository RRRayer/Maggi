using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // 플레이어
    public float followSpeed = 5f;
    public float fixedX = 0f;        // X 고정값
    public float fixedY = 5f;        // Y 고정값 (카메라 높이)
    public float fixedZOffset = -10f; // Z 오프셋 (뒤에서 얼마나 떨어질지)

    private void LateUpdate()
    {
        if (target == null) return;

        // 플레이어의 z값만 따라가고 x/y는 고정
        Vector3 targetPos = new Vector3(fixedX, fixedY, target.position.z + fixedZOffset);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
    }
}
