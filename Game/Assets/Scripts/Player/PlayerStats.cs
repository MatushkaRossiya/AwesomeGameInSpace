using UnityEngine;
using System.Collections;

public class PlayerStats : MonoSingleton<PlayerStats>
{
    public float maxHealth;
    private float _health;

    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            //GameObject.FindObjectOfType<HUD>().GetComponent<HUD>().updateHealth(_health/maxHealth);   //powiadamia hud o zmianie zycia
            if (_health == 0)
            {
                Application.LoadLevel(1);
            }
        }
    }

    public int maxSyringes;
    private int _syringes;

    public int syringes
    {
        get
        {
            return _syringes;
        }
        set
        {
            _syringes = value;
            //Debug.Log("You now have " + _syringes + " syringes");
        }
    }

    private int _syf;

    public int syf
    {
        get
        {
            return _syf;
        }
        set
        {
            _syf = value;
			GameObject.FindObjectOfType<HUD> ().GetComponent<HUD> ().updateSyf (value);
            //Debug.Log("You now have " + _syf + " syf");
        }
    }

    public float syringeHealAmount;

    void Start()
    {
        _health = maxHealth;
        _syringes = 0;
        _syf = 100; // TODO: don't give freebies
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Gamepad.instance.justPressedDPadUp())
        {
            if (healthPercentage < 1.0f)
            {
                UseSyringe();
            }
        }
    }

    private bool UseSyringe()
    {
        if (syringes > 0)
        {
            syringes--;
            health += syringeHealAmount;
            return true;
        }
        return false;
    }

    public float healthPercentage
    {
        get
        {
            return health / maxHealth;
        }
    }

    public bool canBuySyringe
    {
        get { return syringes < maxSyringes; }
    }
}
