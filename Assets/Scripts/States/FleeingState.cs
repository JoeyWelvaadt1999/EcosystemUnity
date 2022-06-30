using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingState : State
{
    [SerializeField]private float _fleeTime;
    [SerializeField]private float _speed;
    private float _currentTime;
    private StateMachine _machine;
    private FieldOfView _fov;
    private EntityData _entityData;
    private AStar _astar;

    public override void Enter()
    {
        _fov = GetComponent<FieldOfView>();
        _entityData = GetComponent<EntityData>();
        _astar = FindObjectOfType<AStar>();
        _machine = GetComponent<StateMachine>();
    }

    private Vector3 FindFurthestTarget() {
        List<Collider> colliders = _fov.GetObjectsInFOV(transform.position, transform.forward, LayerMask.GetMask("Terrain"));
        float highestDist = float.MinValue;
        Vector3 target = Vector3.zero;
        foreach (Collider col in colliders) {
            float dist = Vector3.Distance(_entityData.CurrentPredator.transform.position, col.transform.position);
            if(dist > highestDist) {
                highestDist = dist;
                target = col.transform.position;
            }
        }
        return target;
    }

    public override void Act()
    {
        _currentTime += Time.deltaTime;
        if(pathIndex >= path.Count - 1) {
            path = _astar.FindPath(transform.position, FindFurthestTarget());
            pathIndex = 0;
        }

        CheckPosition();
        WalkTowards(_speed);

    }

    public override void Reason()
    {
        if(_currentTime >= _fleeTime) {
            _machine.SetState(StateID.Wandering);
        }
    }

    public override void Leave() {
        path = new List<WorldNode>();
        pathIndex = 0;
        _currentTime = 0;
    }
}
