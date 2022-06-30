using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class will check for a certain field of view
/// it can easily be altered by changing the variables.
/// </summary>
/// <remarks></remarks>
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float _viewRadius;
    public float ViewRadius
    {
        get { return _viewRadius; }
    }
    [Range(0, 360)] [SerializeField] private float _viewAngle;
    public float ViewAngle
    {
        get { return _viewAngle; }
    }
    [SerializeField] private bool _showGUI;
    private Vector2Int _testGridSize;

    public Vector3 DirFromAngle(float angle, bool isGlobal)
    {
        if (!isGlobal)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    private void Update()
    {
        GetObjectsInFOV(transform.position, transform.forward, LayerMask.GetMask("Terrain"));
    }

    /// <summary>
    /// Based on a layer mask will get all objects within a certain field of view
    /// using the IsInFOV function.
    /// </summary>
    /// <param name="pos">The position which needs to be determined</param>
    /// <param name="layerMask">Integer used by unity for layers of gameobjects</param>
    /// <returns>List of colliders</returns>
    /// <remarks></remarks>
    public List<Collider> GetObjectsInFOV(Vector3 pos, Vector3 direction, LayerMask layerMask)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, _viewRadius, layerMask);
        List<Collider> inFovColliders = new List<Collider>();
        foreach (Collider col in colliders)
        {
            if (IsInFOV(pos, col.gameObject.transform.position))
            {
                if (col.gameObject != this.gameObject)
                {
                    inFovColliders.Add(col);
                }
            }
        }
        return inFovColliders;
    }

    /// <summary>
    /// Checks whether a positions is within a field of view based
    /// on its position and direction from the target
    /// </summary>
    /// <param name="pos">The position which has to be determined</param>
    /// <param name="target">Target position used as starting point</param>
    /// <returns>True when position is in field of view</returns>
    /// <remarks></remarks>
    private bool IsInFOV(Vector3 pos, Vector3 target)
    {
        float distance = Vector3.Distance(target, pos);
        Vector3 relative = transform.InverseTransformPoint(target);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

        // Debug.Log(transform.eulerAngles.y);
        if (distance < _viewRadius)
        {
            if (angle < (_viewAngle / 2f) && Mathf.Abs(angle) < (_viewAngle / 2f))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        _testGridSize = new Vector2Int((int)Mathf.Ceil(_viewRadius * 2), (int)Mathf.Ceil(_viewRadius * 2));
        if (_showGUI)
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.4f);
            Gizmos.DrawSphere(transform.position, _viewRadius);

            for (int x = 0; x < _testGridSize.x; x++)
            {
                for (int y = 0; y < _testGridSize.y; y++)
                {
                    Vector3 pos = new Vector3((transform.position.x + 0.5f - Mathf.Floor(_testGridSize.x / 2f)) + x, transform.position.y - 1, (transform.position.z + 0.5f - Mathf.Floor(_testGridSize.y / 2f)) + y);
                    Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
                    if (IsInFOV(transform.position, transform.forward, pos))
                    {
                        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
                    }
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
