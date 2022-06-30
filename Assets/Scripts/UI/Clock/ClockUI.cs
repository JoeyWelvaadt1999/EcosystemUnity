using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField]private RectTransform _hourHand;
    [SerializeField]private RectTransform _minuteHand;
    private DayTime _time;

    private void Start() {
        _time = FindObjectOfType<DayTime>();
    }

    private void Update() {
        _minuteHand.eulerAngles = new Vector3(0,0, -((360f/60f) * _time.Seconds));
        _hourHand.eulerAngles = new Vector3(0,0, -((360f/12f) * _time.HoursPast));
    }
}
