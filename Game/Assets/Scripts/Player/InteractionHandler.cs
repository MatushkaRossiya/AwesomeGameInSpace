using UnityEngine;
using System.Collections;

public class InteractionHandler : MonoBehaviour {
    public float touchRange = 0.7f;
    public float touchRadius = 0.2f;
    private string message;
    Vector2 screenPosition;
    void Update() {
        RaycastHit hitInfo;
        message = null;
        if (Physics.SphereCast(transform.position, touchRadius, transform.forward, out hitInfo, touchRange)) {
            Interactive interactiveObject = hitInfo.collider.GetComponent<Interactive>();
            if (interactiveObject != null) {
                message = interactiveObject.message;
                screenPosition = Camera.main.WorldToScreenPoint(interactiveObject.transform.position);
                if (Input.GetKeyDown(KeyCode.F)) {
                    interactiveObject.Action();
                }
            }
        }
        
    }

    void OnGUI() {
        if (!string.IsNullOrEmpty(message))
            GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 100, 100), message);
    }
}
