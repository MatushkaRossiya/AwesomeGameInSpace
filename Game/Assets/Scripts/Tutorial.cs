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

    private bool _showAlienTutorial = false;
    private bool _alienTutorialTimerReset = false;

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

        if (_showAlienTutorial)
        {
            if (!_alienTutorialTimerReset)
            {
                time = 0.0f;
                _alienTutorialTimerReset = true;
            }

            if (time > 8.0f && !tut15)
            {
                tut15 = true;
                PlayerPrefs.SetInt("tutorial", 0);
                if (Gamepad.instance.isConnected())
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT15"), 3.0f);
                }
                else
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT15"), 3.0f);
                }
                return;
            }
            if (time > 4.0f && !tut14)
            {
                tut14 = true;
                if (Gamepad.instance.isConnected())
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT14"), 3.0f);
                }
                else
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT14"), 3.0f);
                }
                return;
            }
            if (time > 0.0f && !tut13)
            {
                tut13 = true;
                if (Gamepad.instance.isConnected())
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT13"), 3.0f);
                }
                else
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT13"), 3.0f);
                }
                return;
            }
        }

        if (time > 44.0f && !tut12)
        {
            tut12 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT12"), interact, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT12"), interactPC, 3.0f);
            }
            return;
        }
        if (time > 40.0f && !tut11)
        {
            tut11 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT11"), mine, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT11"), minePC, 3.0f);
            }
            return;
        }
        if (time > 36.0f && !tut10)
        {
            tut10 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT10"), aim, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT10"), aimPC, 3.0f);
            }
            return;
        }
        if (time > 32.0f && !tut09)
        {
            tut09 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT09"), grenade, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT09"), grenadePC, 3.0f);
            }
            return;
        }
        if (time > 28.0f && !tut08)
        {
            tut08 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT08"), weapon, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT08"), weaponPC, 3.0f);
            }
            return;
        }
        if (time > 24.0f && !tut07)
        {
            tut07 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT07"), syringe, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT07"), syringePC, 3.0f);
            }
            return;
        }
        if (time > 20.0f && !tut06)
        {
            tut06 = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT06"), flashlight, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT06"), flashlightPC, 3.0f);
            }
            return;
        }
        if (time > 16.0f && !tut05)
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
        if (time > 12.0f && !tut04)
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
        if (time > 8.0f && !tut03)
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
        if (time > 4.0f && !tut02)
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
    }

    public void showAlienTutorial()
    {
        _showAlienTutorial = true;
    }
}
