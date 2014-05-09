using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class InteractionHandler : MonoBehaviour
{
    public float touchRange = 0.7f;
    public float touchRadius = 0.2f;
    private string message;
    Vector2 screenPosition;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    void Update()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);

        RaycastHit hitInfo;
        message = null;
        if (Physics.SphereCast(transform.position, touchRadius, transform.forward, out hitInfo, touchRange))
        {
            Interactive interactiveObject = hitInfo.collider.GetComponent<Interactive>();
            if (interactiveObject != null)
            {
                message = interactiveObject.message;
                screenPosition = Camera.main.WorldToScreenPoint(interactiveObject.transform.position);
                if (Input.GetKeyDown(KeyCode.F) || state.Buttons.X == ButtonState.Pressed)
                {
                    interactiveObject.Action();
                }
            }
        }
    }

    void OnGUI()
    {
        if (!string.IsNullOrEmpty(message))
            GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 100, 100), message);
    }
}
