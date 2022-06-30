using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all entity states.
/// </summary>
/// <remarks></remarks>
public abstract class State : MonoBehaviour
{
    protected List<WorldNode> path = new List<WorldNode>();
    protected int pathIndex = 0;


    /// <summary>
    /// Walking function used in multiple states. It rotates the entity towards the next position in the path
    /// and then walks towards it.
    /// </summary>
    /// <param name="speed">The speed the entity walks and rotates with</param>
    /// <remarks></remarks>
    protected void WalkTowards(float speed)
    {
        Vector3 newPosition = new Vector3(path[pathIndex].Position.x, transform.position.y, path[pathIndex].Position.z);

        transform.forward = Vector3.RotateTowards(transform.forward, newPosition - transform.position, speed * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
    }

    /// <summary>
    /// Check whether the entity is at the current paths destination. If
    /// this is the case move to the next point of the path
    /// </summary>
    /// <remarks></remarks>
    protected void CheckPosition()
    {
        Vector3 currentPosition = new Vector3(path[pathIndex].Position.x, transform.position.y, path[pathIndex].Position.z);
        if (transform.position == currentPosition)
        {
            pathIndex++;
        }
    }


    /// <summary>
    /// Whenever the state machine switches to a state this function is called
    /// </summary>
    /// <remarks></remarks>
    public virtual void Enter()
    {

    }

    /// <summary>
    /// Same as the Enter function but it is called whenever the state machine leaves 
    /// this state.
    /// </summary>
    /// <remarks></remarks>
    public virtual void Leave()
    {

    }

    /// <summary>
    /// Abstract function to force children to own this function. This function will be called to 
    /// act out a certain state.
    /// </summary>
    /// <remarks></remarks>
    public abstract void Act();
    /// <summary>
    /// Same as the Act function, but the functionality checks whether it should leave a certain state.
    /// </summary>
    /// <remarks></remarks>
    public abstract void Reason();
}
