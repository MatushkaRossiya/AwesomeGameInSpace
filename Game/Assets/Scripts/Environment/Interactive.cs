using UnityEngine;
using System.Collections;

public abstract class Interactive : MonoBehaviour
{
    public abstract string message
    {
        get;
    }

    public abstract void Action();
}
