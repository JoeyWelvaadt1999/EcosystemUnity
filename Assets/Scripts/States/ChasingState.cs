using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : State
{
    [SerializeField]private float _chaseTime;
    private float _currentTime;
    [SerializeField]private float _speed;
    [SerializeField]private float _consumeRadius;
    private EntityData _entityData;
    private AStar _astar;
    private StateMachine _machine;
    private bool _caughtPrey;

    public override void Enter() {
        _entityData = GetComponent<EntityData>();
        _astar = FindObjectOfType<AStar>();
        _machine = GetComponent<StateMachine>();
        path = _astar.FindPath(transform.position, _entityData.CurrentPrey.transform.position);
    }

    public override void Act() {
        _currentTime += Time.deltaTime;
        if(pathIndex >= path.Count - 1) {
            path = _astar.FindPath(transform.position, _entityData.CurrentPrey.transform.position);
            pathIndex = 0;
        }
        
        float dist = Vector3.Distance(transform.position, _entityData.CurrentPrey.transform.position);

        if(dist < _consumeRadius) {
            ConsumePrey();
        }

        CheckPosition();
        WalkTowards(_speed);
    }

    private void ConsumePrey() {
        _entityData.AddNutrtion(_entityData.CurrentPrey);
        Destroy(_entityData.CurrentPrey);
        _entityData.CurrentPrey = null;
        _caughtPrey = true;
    }

    public override void Reason() {
        if(!_entityData.HasHunger() && _currentTime >= _chaseTime) {
            _machine.SetState(StateID.Wandering);
        } else if (_caughtPrey) {
            _machine.SetState(StateID.Idling);
        }
    }

    public override void Leave() {
        path = new List<WorldNode>();
        pathIndex = 0;
        _caughtPrey = false;
        _currentTime = 0;
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _consumeRadius);
    }
}
