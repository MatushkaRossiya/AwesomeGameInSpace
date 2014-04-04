using UnityEngine;
using System.Collections;

public class PushButton : Interactive {
    public Door door;

    public override string message {
        get { return "OPEN DOOR";  }
    }

    public override void Action() {
        door.Toggle();
    }
}
