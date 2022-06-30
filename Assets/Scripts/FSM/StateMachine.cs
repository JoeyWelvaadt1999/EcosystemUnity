using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the situation of all the states.
/// </summary>
/// <remarks></remarks>

public class StateMachine : MonoBehaviour
{
    private Dictionary<StateID, State> _states = new Dictionary<StateID, State>();
    private State _currentState;
    private StateID _currentID;
    public StateID CurrentID
    {
        get { return _currentID; }
    }


    /// <summary>
    /// If there is a current state act it out and let it reason.
    /// </summary>
    /// <remarks></remarks>
    private void Update()
    {
        if (_currentState == null)
            return;
        // Debug.Log(_currentState);
        _currentState.Reason();
        _currentState.Act();
    }

    public void SetState(StateID id)
    {
        if (!_states.ContainsKey(id))
            return;

        if (_currentState != null)
            _currentState.Leave();

        _currentState = _states[id];
        _currentID = id;
        _currentState.Enter();
    }

    public void AddState(StateID id, State state)
    {
        _states.Add(id, state);
    }
}
