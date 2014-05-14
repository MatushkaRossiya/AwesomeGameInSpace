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

    void FixedUpdate()
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

        message = null;
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, touchRadius, transform.forward, touchRange);
        if (hits.Length > 0)
        {
			foreach (var hit in hits) {
				Interactive interactiveObject = hit.collider.GetComponent<Interactive>();
				if (interactiveObject != null) {
					message = interactiveObject.message;
					screenPosition = Camera.main.WorldToScreenPoint(interactiveObject.transform.position);
					if (Input.GetKeyDown(KeyCode.E) || state.Buttons.X == ButtonState.Pressed) {
						interactiveObject.Action();
					}
					break;
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
