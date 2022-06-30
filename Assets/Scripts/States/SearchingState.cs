using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : State
{
    [SerializeField]private float _speed;
    private StateMachine _machine;
    private WorldGrid _grid;
    private AStar _astar;
    private EntityData _entityData;
    private FieldOfView _fov;

    public override void Enter() {
        _machine = GetComponent<StateMachine>();
        _grid = FindObjectOfType<WorldGrid>();
        _astar = FindObjectOfType<AStar>();
        _entityData = GetComponent<EntityData>();
        _fov = GetComponent<FieldOfView>();
        pathIndex = 0;
    }

    public override void Act() {
        //Prioritize water over food
        if(_entityData.HasThirst()) {    
            path = _astar.FindPath(transform.position, _entityData.GetClosestSource(SourceType.Water, transform.position));
            Debug.Log("Water searching: " + path.Count);
            if(pathIndex >= path.Count - 1) {
                _entityData.ResetThirst();
            }
        }else if (_entityData.HasHunger() && !_entityData.HasThirst()) {
            Vector3 pos = _entityData.GetClosestSource(SourceType.Food, transform.position);
            path = _astar.FindPath(transform.position, pos);
            if(pathIndex >= path.Count - 1) {
                _entityData.RemoveSource(SourceType.Food, pos);
            }
        }

        WalkTowards(_speed);
        CheckPosition();
    }   

    public override void Reason() {
        if(((_entityData.WaterSources.Count == 0 && _entityData.HasThirst() && !_entityData.HasHunger()) || (_entityData.FoodSources.Count == 0 && _entityData.HasHunger() && !_entityData.HasThirst())) 
            || (!_entityData.HasThirst() && !_entityData.HasHunger())) {
            _machine.SetState(StateID.Wandering);
        }
        List<Collider> cols = _fov.GetObjectsInFOV(transform.position,transform.forward, LayerMask.GetMask("Entity"));

        if(cols.Count > 0 && _entityData.IsPredator(cols[0].gameObject)) {
            _entityData.CurrentPredator = cols[0].gameObject;
            _machine.SetState(StateID.Fleeing);
        } else if (cols.Count > 0 && _entityData.IsPrey(cols[0].gameObject)) {
            _machine.SetState(StateID.Chasing);
        }
    }
}
