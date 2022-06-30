using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WanderingState : State
{
    [SerializeField]private float _speed;
    private AStar _aStar;
    private WorldGrid _grid;
    private StateMachine _stateMachine;
    private FieldOfView _fov;
    // private List<WorldNode> _path = new List<WorldNode>();
    private GameObject _target;
    private Vector3 _targetPosition;
    private EntityData _data;
    // private int _pathIndex = 0;

    public override void Enter() {
        _aStar = FindObjectOfType<AStar>();
        _stateMachine = GetComponent<StateMachine>();
        _fov = GetComponent<FieldOfView>();
        _grid = FindObjectOfType<WorldGrid>();
        _data = GetComponent<EntityData>();
        PickTarget();
        path = _aStar.FindPath(transform.position, _targetPosition);
    }

    public override void Act()
    {
        CheckPosition();
        WalkTowards(_speed);
        
        FindWaterSources();
        FindFoodSources();
    }

    private void PickTarget() {
        List<Collider> objs = _fov.GetObjectsInFOV(transform.position, transform.forward, LayerMask.GetMask("Terrain"));
        System.Random rand = new System.Random(); 

        _target = objs[rand.Next(0, objs.Count - 1)].gameObject;
        _targetPosition = _target.transform.position;
    }

    private void FindFoodSources() {
        List<Collider> objs = _fov.GetObjectsInFOV(transform.position, transform.forward, LayerMask.GetMask("Entity"));
        foreach (Collider c in objs) {
            Vector3 pos = c.transform.position;
            if(_data.IsPrey(c.gameObject)) {
                _data.AddSource(SourceType.Food, pos);
            }
        }
    }

    private void FindWaterSources() {
        List<Collider> cols = _fov.GetObjectsInFOV(transform.position, transform.forward, LayerMask.GetMask("Water"));
        foreach (Collider c in cols) {
            Vector3 pos = c.transform.position;
            if (_grid.CheckNeighbours(_grid.GetNeighboursFromNode(_grid.GetNodeFromPosition(pos))) > 1) {
                _data.AddSource(SourceType.Water, pos);
            }
        }
    }

    public override void Reason()
    {
        if(pathIndex >= path.Count - 1) {
            // Debug.Log("Switch State");
            _stateMachine.SetState(StateID.Idling);
        }

        List<Collider> cols = _fov.GetObjectsInFOV(transform.position,transform.forward, LayerMask.GetMask("Entity"));
        
    
        if(cols.Count > 0 && _data.IsPredator(cols[0].gameObject)) {
            Debug.Log(gameObject.name + " " + cols.Count);
            _data.CurrentPredator = cols[0].gameObject;
            _stateMachine.SetState(StateID.Fleeing);
        }
    
        if(cols.Count > 0 && _data.IsPrey(cols[0].gameObject) && _data.CanConsume(cols[0].gameObject)) {
            float followChance = 100f - (100f / _data.HungerThreshold * _data.HungerMeter);
            int rand = (int)Random.Range(0, 100);
            if(followChance < rand) {
                _data.CurrentPrey = cols[0].gameObject;
                _stateMachine.SetState(StateID.Chasing);
            }
        }
    }

    public override void Leave() {
        path = new List<WorldNode>();
        pathIndex = 0;
    }

    private void OnDrawGizmos() {
        if(_data.WaterSources.Count > 0) {
            Gizmos.color = new Color(1f,0f,1f, 0.4f);
            for(int i = 0; i < _data.WaterSources.Count; i++) {
                Vector3 pos = new Vector3(_data.WaterSources[i].x, 8, _data.WaterSources[i].z);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }
    
}
