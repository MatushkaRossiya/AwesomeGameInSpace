using UnityEngine;
using System.Collections;

public class InteractionHandler : MonoBehaviour
{
    public float touchRange = 0.7f;
    public float touchRadius = 0.2f;
    private string message;
    private Vector2 screenPosition;
    private Interactive interactiveObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Gamepad.instance.justPressedX())
        {
            if (interactiveObject != null)
            {
                interactiveObject.Action();
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
            GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 100, 100), message);
    }
}
