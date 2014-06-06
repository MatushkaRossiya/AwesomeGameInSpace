using UnityEngine;
using System.Collections;

public class Tutorial : MonoSingleton<Tutorial>
{
    public Texture2D look;
    public Texture2D move;
    public Texture2D sprint;
    public Texture2D jump;
    public Texture2D crouch;
    public Texture2D flashlight;
    public Texture2D syringe;
    public Texture2D weapon;
    public Texture2D grenade;
    public Texture2D aim;
    public Texture2D mine;
    public Texture2D interact;

    public Texture2D lookPC;
    public Texture2D movePC;
    public Texture2D sprintPC;
    public Texture2D jumpPC;
    public Texture2D crouchPC;
    public Texture2D flashlightPC;
    public Texture2D syringePC;
    public Texture2D weaponPC;
    public Texture2D grenadePC;
    public Texture2D aimPC;
    public Texture2D minePC;
    public Texture2D interactPC;

    private float time = 0.0f;

    private bool tut01 = false;
    private bool tut02 = false;
    private bool tut03 = false;
    private bool tut04 = false;
    private bool tut05 = false;
    private bool tut06 = false;
    private bool tut07 = false;
    private bool tut08 = false;
    private bool tut09 = false;
    private bool tut10 = false;
    private bool tut11 = false;
    private bool tut12 = false;
    private bool tut13 = false;
    private bool tut14 = false;
    private bool tut15 = false;

    // Use this for initialization
    public override void Init()
    {
        if (PlayerPrefs.HasKey("tutorial")) {
            int tutorial = PlayerPrefs.GetInt("tutorial");
            if (tutorial == 0)
                Destroy(this);
        }
        else {
            PlayerPrefs.SetInt("tutorial", 1);
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        if (time > 12.0f && !tut05)
        {
            tut05 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT05"), crouch, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT05"), crouchPC, 3.0f);
            }
            return;
        }
        if (time > 9.0f && !tut04)
        {
            tut04 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT04"), jump, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT04"), jumpPC, 3.0f);
            }
            return;
        }
        if (time > 6.0f && !tut03)
        {
            tut03 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT03"), sprint, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT03"), sprintPC, 3.0f);
            }
            return;
        }
        if (time > 3.0f && !tut02)
        {
            tut02 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT02"), move, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT02"), movePC, 3.0f);
            }
            return;
        }
        if (time > 0.0f && !tut01)
        {
            tut01 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT01"), look, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT01"), lookPC, 3.0f);
            }
            return;
        }

        //PlayerPrefs.SetInt("tutorial", 0);
    }
}
