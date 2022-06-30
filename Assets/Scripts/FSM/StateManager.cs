using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateID {
    NULL,
    Idling,
    Searching,
    Wandering,
    Fleeing,
    Chasing
}

/// <summary>
/// Add all states to the statemachine
/// </summary>
/// <remarks></remarks>

public class StateManager : MonoBehaviour
{
    private StateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = GetComponent<StateMachine>();

        _stateMachine.AddState(StateID.Wandering, GetComponent<WanderingState>());
        _stateMachine.AddState(StateID.Idling, GetComponent<IdlingState>());
        _stateMachine.AddState(StateID.Searching, GetComponent<SearchingState>());
        _stateMachine.AddState(StateID.Fleeing, GetComponent<FleeingState>());
        _stateMachine.AddState(StateID.Chasing, GetComponent<ChasingState>());

        _stateMachine.SetState(StateID.Wandering);
    }
}
