using UnityEngine;
using System.Collections;

public abstract class Interactive : MonoBehaviour
{
    public abstract string message
    {
        get;
    }

    public virtual void MomentaryAction(){}

	public virtual void HoldAction(){}
}
