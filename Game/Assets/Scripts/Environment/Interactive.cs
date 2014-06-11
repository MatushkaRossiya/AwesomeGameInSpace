using UnityEngine;
using System.Collections;

public abstract class Interactive : MonoBehaviour
{
    public abstract string message
    {
        get;
    }

	public int priority = 0;

    public virtual void MomentaryAction(){}

	public virtual void HoldAction(){}
}
