using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]private GameObject _settingsPanel;
    public static bool SettingsEnabled { get; set; }
    

    private void Awake() {
        _settingsPanel.SetActive(SettingsUI.SettingsEnabled);
    }

    private void Update() {
        if (SettingsUI.SettingsEnabled) {
            Time.timeScale = 0;
        }
    }

    public void SetSettingsEnabled(bool enabled) {
        _settingsPanel.SetActive(enabled);
        SettingsUI.SettingsEnabled = enabled;
    }
}
