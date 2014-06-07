using UnityEngine;
using System.Collections;

public class InteractionHandler : MonoBehaviour
{
    public float touchRange = 0.7f;
    public float touchRadius = 0.2f;
	public float holdActionThershold = 0.5f;
    public GUIStyle textStyle;

    private string message;
    private Vector2 screenPosition;
    private Interactive interactiveObject;
	private float holdTime;
	private float holdFrames;
	private bool hold = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Gamepad.instance.justPressedX()) {
			holdTime = holdActionThershold;
			hold = true;
			holdFrames = 0;
        } else if (hold && (Input.GetKeyUp(KeyCode.E) || Gamepad.instance.justReleasedX()) && interactiveObject != null) {
			interactiveObject.MomentaryAction();
			hold = false;
		} else if (hold) {
			++holdFrames;
			holdTime -= Time.deltaTime;
			if(holdTime < 0 && holdFrames > 1 && interactiveObject != null){
				interactiveObject.HoldAction();
				hold = false;
			}
		}
    }

    void FixedUpdate()
    {
        message = null;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, touchRadius, transform.forward, touchRange);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                interactiveObject = hit.collider.GetComponent<Interactive>();
                if (interactiveObject != null)
                {
                    message = interactiveObject.message;
                    screenPosition = Camera.main.WorldToScreenPoint(interactiveObject.transform.position);
                    break;
                }
            }
        }
    }

    void OnGUI()
    {
        if (!string.IsNullOrEmpty(message))
            GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 256, 96), message, textStyle);
    }
}
