using UnityEngine;
using System.Collections;

public class RemoteTrigger : Interactive
{
    public string _message;
    public RemoteActor remoteActor;

    public override string message
    {
        get { return _message; }
    }

    public override void Action()
    {
        remoteActor.Action();
    }
}
