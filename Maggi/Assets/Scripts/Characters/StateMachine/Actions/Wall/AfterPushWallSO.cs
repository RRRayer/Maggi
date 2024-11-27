﻿using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AfterPushWall", menuName = "State Machines/Actions/After Push Wall")]
public class AfterPushWallSO : StateActionSO<AfterPushWall>
{
	public float speed = 4.0f;
    public LayerMask floorLayerMask;
}

public class AfterPushWall : StateAction
{
	protected new AfterPushWallSO _originSO => (AfterPushWallSO)base.OriginSO;
    private Player _player;
    private Transform _transform;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
    }

    private Vector3 OriginVec; 

	public override void OnUpdate()
	{
		_player.movementVector.x = OriginVec.x;
		_player.movementVector.z = OriginVec.z;
		_player.movementVector.x += _player.movementInput.x * _originSO.speed;
		_player.movementVector.z += _player.movementInput.z * _originSO.speed;
    }
	
	public override void OnStateEnter()
	{
		OriginVec = _player.movementVector;
	}
	
	public override void OnStateExit()
	{
	}
}
