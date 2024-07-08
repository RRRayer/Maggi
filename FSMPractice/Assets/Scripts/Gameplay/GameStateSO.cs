using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Gameplay, // regular state: player moves, attacks, can perform actions
    Pause,    // pause menu is opened, the whole game world is frozen
    Cutscene,
    LocationTransition
}

public class GameStateSO : DescriptionBaseSO
{
    public GameState CurrentGameState => _currentGameState;

    [Header("Game states")]
    [SerializeField] [ReadOnly] private GameState _currentGameState = default;
    [SerializeField] [ReadOnly] private GameState _previousGameState = default;

    public void UpdateGameState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;

        _previousGameState = _currentGameState;
        _currentGameState = newGameState;
    }
}
