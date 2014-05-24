using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject player;
    //private Vector3 offset;

    private Light lightComponent;

    private bool flicker = false;
    private int flickerCount = 0;
    private float flickerTimeCurrent = 0.0f;
    private float flickerTimeOff = 0.0f;
    private float flickerTimeOn = 0.0f;
    private int flickerFrequency;
    
    // Use this for initialization
    void Start()
    {
        //offset = new Vector3(0.5f, 0.75f, 0.0f);
        lightComponent = GetComponent<Light>();
        flickerFrequency = Random.Range(1, 5);
    }
    
    void FixedUpdate()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, playerCamera.transform.rotation, 0.70f);
        //this.transform.position = player.transform.position + offset;

        if (flicker)
        {
            if (flickerTimeCurrent < flickerTimeOff)
            {
                //lightComponent.enabled = false;
                lightComponent.intensity = Random.Range(0.05f, 0.2f);
                flickerTimeCurrent += Time.fixedDeltaTime;
            }
            else if (flickerTimeCurrent < flickerTimeOn)
            {
                //lightComponent.enabled = true;
                lightComponent.intensity = Random.Range(0.9f, 1.10f);
                flickerTimeCurrent += Time.fixedDeltaTime;
            }
            else if (flickerCount > 0)
            {
                flickerTimeCurrent = 0.0f;
                flickerTimeOff = Random.Range(0.5f, 0.75f);
                flickerTimeOn = flickerTimeOff + Random.Range(0.5f, 0.75f);
                flickerCount--;
            }
            else 
            {
                //lightComponent.enabled = true;
                lightComponent.intensity = 1.5f;
                flicker = false;
                flickerFrequency = Random.Range(5, 15);
            }
        }

        if (Input.GetKeyDown(KeyCode.F) || Gamepad.instance.justPressedDPadDown())
        {
            if (lightComponent) lightComponent.enabled = !lightComponent.enabled;
        }
    }

    public void Flicker(float strength)
    {
        if (flicker)
            return;
        if (flickerFrequency > 0)
        {
            flickerFrequency--;
            return;
        }

        flicker = true;
        //Debug.Log(strength);
        flickerCount = (int)Random.Range(1, strength / 2.0f);
        if (flickerCount > 3)
            flickerCount = 3;
    }
}
