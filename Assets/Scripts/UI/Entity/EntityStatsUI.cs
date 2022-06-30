using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityStatsUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _text;
    [SerializeField]private Image _waterImage;
    [SerializeField]private Image _hungerImage;
    private StateMachine _stateMachine;
    private EntityData _entityData;

    private void Start() {
        _stateMachine = GetComponent<StateMachine>();
        _entityData = GetComponent<EntityData>();
    }

    private void Update() {
        switch (_stateMachine.CurrentID) {
            case StateID.Idling:
                _text.text = "Idling";
                break;
            case StateID.Wandering:
                _text.text = "Wandering";
                break;
            case StateID.Searching:
                _text.text = "Searching";
                break;
            case StateID.Fleeing:
                _text.text = "Fleeing";
                break;
            case StateID.Chasing:
                _text.text = "Chasing";
                break;
            default:
                break;
        }

        _waterImage.fillAmount = (1f/100f) * (100f / _entityData.ThirstThreshold * _entityData.ThirstMeter);
        _hungerImage.fillAmount = (1f/100f) * (100f / _entityData.HungerThreshold * _entityData.HungerMeter);
    }
}
