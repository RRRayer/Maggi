using UnityEngine.AddressableAssets;

public class GameSceneSO : DescriptionBaseSO
{
    public GameSceneType sceneType;
    public AssetReference sceneReference;
    public AudioCueSO musicTrack;

    public enum GameSceneType
    {
        // Playerable
        Location,
        Menu,

        // Special Scenes
        Initialisation,
        PersistentManager,
        GamePlay
    }
}
