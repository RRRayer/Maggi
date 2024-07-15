using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameSceneSO : DescriptionBaseSO
{
    public GameSceneType sceneType;
    public AssetReference sceneReference;

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
