using UnityEngine;

[CreateAssetMenu(fileName = "PointStorage", menuName = "Gameplay/Point Storage")]
public class PointStorageSO : DescriptionBaseSO
{
    [Space]
    [ReadOnly] public PointSO lastPointTaken;
}