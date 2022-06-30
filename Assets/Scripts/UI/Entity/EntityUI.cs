using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityUI : MonoBehaviour
{
    [SerializeField]private RectTransform _scrollRect;
    [SerializeField]private RectTransform _buttonRect;
    [SerializeField]private Button _entityButton;
    private StateManager[] _stateManagers;
    private List<GameObject> _gameObjects;
    private FollowTarget _followTarget;

    private void Start() {
        _followTarget = FindObjectOfType<FollowTarget>();
        _stateManagers = FindObjectsOfType<StateManager>();
        _scrollRect.sizeDelta = new Vector2(_scrollRect.sizeDelta.x, _buttonRect.sizeDelta.y * _stateManagers.Length);
        for(int i = 0; i < _stateManagers.Length; i++) {
            GameObject obj = Instantiate(_entityButton.gameObject, transform.position, Quaternion.identity, transform);
            
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -_buttonRect.sizeDelta.y * i);
            obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = _stateManagers[i].gameObject.name;
            int param = i;
            obj.GetComponent<Button>().onClick.AddListener(() => { _followTarget.SetTarget(_stateManagers[param].gameObject); });
            // _gameObjects.Add(obj);
        }
    }

    // private void Update() {
    //     List<StateManager> managers = new List<StateManager>(FindObjectsOfType<StateManager>());
    //     Debug.Log(managers.Count);
    //     if(managers.Count != _stateManagers.Length) {
    //         for(int i = 0; i < _stateManagers.Length; i++) {
    //             if(!managers.Contains(_stateManagers[i])) {
    //                 Destroy(_gameObjects[_gameObjects.IndexOf(_stateManagers[i].gameObject)]);
    //                 _gameObjects.Remove(_stateManagers[i].gameObject);
    //             }
    //         }
    //     }
    // }
}
