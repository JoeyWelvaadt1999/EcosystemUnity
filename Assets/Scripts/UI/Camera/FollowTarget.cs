using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]private float _cameraHeight;
    private GameObject _target;

    private void Update() {
        if(_target == null)
            return;
        Follow();
    }

    private void Follow() {
        transform.position = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
    }

    public void SetTarget(GameObject target) {
        _target = target;
    }
}
