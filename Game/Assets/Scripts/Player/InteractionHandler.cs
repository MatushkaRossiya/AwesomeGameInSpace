using UnityEngine;
using System.Collections;

public class InteractionHandler : MonoSingleton<InteractionHandler>
{
    public float touchRange = 0.7f;
    public float touchRadius = 0.2f;
	public float holdActionThershold = 0.5f;

    private string message;
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
        } else if (hold && (Input.GetKeyUp(KeyCode.E) || Gamepad.instance.justReleasedX())){
			hold = false;
			holdFrames = 0;
			if (interactiveObject != null) {
				interactiveObject.MomentaryAction();
			}
		} else if (hold) {
			++holdFrames;
			holdTime -= Time.deltaTime;
			if(holdTime < 0 && holdFrames > 1 && interactiveObject != null){
				interactiveObject.HoldAction();
				hold = false;
			}
		}

		if(message != null)
			HUD.instance.setHintvisible(message, 0.3f);
    }

    void FixedUpdate()
    {
		float directionFactor = transform.localEulerAngles.x;
		if (directionFactor > 90) directionFactor -= 360;
		directionFactor = 1 + Mathf.Abs(directionFactor / 90 * 1.5f);
		RaycastHit[] hits = Physics.SphereCastAll(
			transform.position, 
			touchRadius * directionFactor, 
			transform.forward, touchRange * directionFactor);

		if (hold) {
			bool containsExisting = false;
			if (hits.Length > 0) {
				Interactive temp;
				foreach (var hit in hits) {
					temp = hit.collider.GetComponent<Interactive>();
					if (temp != null) {
						if (temp == interactiveObject) {
							containsExisting = true;
							break;
						}
					}
				}
			}

			if (!containsExisting) {
				interactiveObject = null;
				message = null;
				hold = false;
				holdFrames = 0;
			}
		}
		else {
			interactiveObject = null;
			if (hits.Length > 0) {
				int topPriority = int.MinValue;
				Interactive temp = null;
				foreach (var hit in hits) {
					temp = hit.collider.GetComponent<Interactive>();
					if (temp != null && temp.priority > topPriority) {
						topPriority = temp.priority;
						interactiveObject = temp;
					}
				}
			}

			if (interactiveObject != null) {
				message = interactiveObject.message;
			}
			else {
				message = null;
			}
		}
    }
}
