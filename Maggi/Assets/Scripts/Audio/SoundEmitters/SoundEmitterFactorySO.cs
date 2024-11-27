using UnityEngine;
using Maggi.Factory;

[CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "Factory/SoundEmitter Factory")]
public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
{
    public SoundEmitter prefab = default;

    public override SoundEmitter Create()
    {
        return Instantiate(prefab);
    }
}
