using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlingState : State
{
    [SerializeField]private float _rotationSpeed;
    [Header("Time in seconds")]
    [SerializeField]private float _idleTime;
    [SerializeField]private int _fovThreshold;
    private float _timeRemaining;
    private StateMachine _stateMachine;
    private FieldOfView _fov;
    private EntityData _entityData;

    public override void Enter()
    {
        _stateMachine = GetComponent<StateMachine>();
        _fov = GetComponent<FieldOfView>();
        _entityData = GetComponent<EntityData>();
        _timeRemaining = _idleTime;
    }

    public override void Act()
    {
        // 
        _timeRemaining -= Time.deltaTime;
        if(_fov.GetObjectsInFOV(transform.position, transform.forward, LayerMask.GetMask("Terrain")).Count <= _fovThreshold) {
            transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime));
            Debug.Log("Rotating");
        }
    }

    public override void Reason()
    {   
        // Debug.Log(transform.rotation + " - " + _newRotation + " - " + transform.eulerAngles);
        if(_entityData.HasThirst() || (_entityData.HasHunger() && _entityData.FoodSources.Count > 0)) {
            _stateMachine.SetState(StateID.Searching);
        }

        if(_timeRemaining <= 0 && _fov.GetObjectsInFOV(transform.position, transform.forward, LayerMask.GetMask("Terrain")).Count > _fovThreshold) {
            _stateMachine.SetState(StateID.Wandering);
        }

        List<Collider> cols = _fov.GetObjectsInFOV(transform.position,transform.forward, LayerMask.GetMask("Entity"));

        if(cols.Count > 0 && _entityData.IsPredator(cols[0].gameObject)) {
            _entityData.CurrentPredator = cols[0].gameObject;
            _stateMachine.SetState(StateID.Fleeing);
        }

        
    }

    public override void Leave()
    {
        
    }
}
