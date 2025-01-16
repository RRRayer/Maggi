using UnityEngine;
using UnityEngine.Timeline;

public class Breakable : MonoBehaviour
{
    [SerializeField] private int count = 3;

    [Space]

    [Header("If it is Needed, Broadcasting on")]
    [SerializeField] private TimelineAssetEventChannelSO _onStartTimeline = default;
    [SerializeField] private TimelineAsset _timeline = default;

    private bool isBroken = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;

        if (collision.gameObject.TryGetComponent(out LightObject e))
        {
            count -= 1;
            if (count <= 0)
            {
                isBroken = true;
                ReplaceToBrokenObject();
            }
        }
    }

    private void ReplaceToBrokenObject()
    {
        // Play Timeline On Broken
        if (_onStartTimeline != null && _timeline != null)
            _onStartTimeline.RaiseEvent(_timeline);

        // Destroy Gameobject and Replace to deactivate automatically
        if (TryGetComponent(out ReplaceObject _replaceObject))
        {
            _replaceObject.ChangeObject();
        }
    }
}
