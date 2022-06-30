using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayTime : MonoBehaviour
{
    [SerializeField]private Light _light;
    [SerializeField]private Slider _slider;
    [SerializeField]private TextMeshProUGUI _sliderText;
    [Range(1,100)][SerializeField]private int _timeScale;
    private int _daysPast;
    public int DaysPast { 
        get { return _daysPast; }
        set { _daysPast = value; }
    }

    private int _hoursPast;
    public int HoursPast {
        get { return _hoursPast; }
    }
    private int _seconds;
    public int Seconds {
        get { return _seconds; }
    }
    private float _currentTimer;

    private void Start() {
        
    }

    private void CaclulateTime() {
        _currentTimer += Time.deltaTime;
        _seconds = (int)_currentTimer % 60;
        if(_seconds + 1 >= 60) {
            
            _hoursPast += 1;
            _currentTimer = 0;
        }
        if(_hoursPast >= 24) {
            _daysPast += 1;
            _hoursPast = 0;
        }
    }

    private void Update() {
        CaclulateTime();
        RotateLight();
        if(!SettingsUI.SettingsEnabled)
            Time.timeScale = _slider.value;
    }

    private void RotateLight() {
        float hourStep = 360f/24f;
        float minStep = hourStep/60f;

        float newX = ((hourStep * _hoursPast)-90) + (minStep * _seconds);
        
        Quaternion target = Quaternion.Euler(newX, _light.transform.rotation.y, _light.transform.rotation.z);
        _light.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f);
    }
    
    public void SetSliderLabel() {
        _sliderText.text = _slider.value.ToString();
        
    }
    
}

