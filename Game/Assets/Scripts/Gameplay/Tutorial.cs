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
	public Texture2D melee;
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
	public Texture2D meleePC;
    public Texture2D interactPC;

    private static float time = 0.0f;

    private static bool[] tut = new bool[20];

    private static bool _showAlienTutorial = false;
    private static bool _alienTutorialTimerReset = false;
    private static bool _enabled;

    // Use this for initialization
    public override void Init()
    {
		//PlayerPrefs.SetInt("tutorial", 1);
        checkIfEnabled();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        checkIfEnabled();

        time += Time.fixedDeltaTime;

        if (_showAlienTutorial)
        {
            if (!_alienTutorialTimerReset)
            {
                time = 0.0f;
                _alienTutorialTimerReset = true;
            }

            if (time > 8.0f && !tut[16])
            {
                tut[16] = true;
                PlayerPrefs.SetInt("tutorial", 0);
                if (Gamepad.instance.isConnected())
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT16"), 3.0f);
                }
                else
                {
                    HUD.instance.setTutorialVisible(Loc.instance.getText("TUT16"), 3.0f);
                }
                Destroy(this);
                return;
            }
            if (time > 4.0f && !tut[15])
            {
                tut[15] = true;
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
            if (time > 0.0f && !tut[14])
            {
                tut[14] = true;
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
        }

		if (time > 48.0f && !tut[13])
		{
			tut[13] = true;
			if (Gamepad.instance.isConnected())
			{
				HUD.instance.setTutorialVisible(Loc.instance.getText("TUT13"), interact, 3.0f);
			}
			else
			{
				HUD.instance.setTutorialVisible(Loc.instance.getText("TUT13"), interactPC, 3.0f);
			}
			return;
		}
        if (time > 44.0f && !tut[12])
        {
            tut[12] = true;
            if (Gamepad.instance.isConnected())
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT12"), melee, 3.0f);
            }
            else
            {
                HUD.instance.setTutorialVisible(Loc.instance.getText("TUT12"), meleePC, 3.0f);
            }
            return;
        }
        if (time > 40.0f && !tut[11])
        {
            tut[11] = true;
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
        if (time > 36.0f && !tut[10])
        {
            tut[10] = true;
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
        if (time > 32.0f && !tut[9])
        {
            tut[9] = true;
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
        if (time > 28.0f && !tut[8])
        {
            tut[8] = true;
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
        if (time > 24.0f && !tut[7])
        {
            tut[7] = true;
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
        if (time > 20.0f && !tut[6])
        {
            tut[6] = true;
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
        if (time > 16.0f && !tut[5])
        {
            tut[5] = true;
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
        if (time > 12.0f && !tut[4])
        {
            tut[4] = true;
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
        if (time > 8.0f && !tut[3])
        {
            tut[3] = true;
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
        if (time > 4.0f && !tut[2])
        {
            tut[2] = true;
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
        if (time > 0.0f && !tut[1])
        {
            tut[1] = true;
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

    private void checkIfEnabled()
    {
        if (PlayerPrefs.HasKey("tutorial")) {
            int tutorial = PlayerPrefs.GetInt("tutorial");
            if (tutorial == 0)
            {
                _enabled = false;
                Destroy(this);
            }
            else
            {
                _enabled = true;
            }
        }
        else {
            PlayerPrefs.SetInt("tutorial", 1);
            _enabled = true;
        }
    }

    public static bool isEnabled()
    {
        return _enabled;
    }
}
