using UnityEngine;
using System.Collections;

public class PlayerStats : MonoSingleton<PlayerStats>
{
    public float maxHealth;
    private float _health;
    public AudioClip syringeSound;
    public GameObject minePrefab;

    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            if (_health == 0)
            {
                HeavyAlienFSM[] heavyAliens = FindObjectsOfType<HeavyAlienFSM>();
                foreach (HeavyAlienFSM heavyAlien in heavyAliens) heavyAlien.PlayerDied();
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
        }
    }

    public float syringeHealAmount;

    public int maxMines = 3;
    private int _mines;
    public int mines
    {
        get
        {
            return _mines;
        }
        set
        {
            _mines = value;
        }
    }

    void Start()
    {
        _health = maxHealth;
        _syringes = 0;
        _syf = 100;
        _mines = 3;
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
        if (Input.GetKeyDown(KeyCode.V) || Gamepad.instance.justPressedDPadLeft())
        {
            if (_mines > 0)
            {
                RaycastHit hitInfo;
                Vector3 start = transform.position;
                Vector3 direction = -transform.up;
                
                bool hit = Physics.Raycast(start, direction, out hitInfo, 100.0f, Layers.environment);
                
                if (hit)
                {
                    Instantiate(minePrefab, hitInfo.point, Quaternion.identity);
                    _mines--;
                }
            }
        }
    }

    private bool UseSyringe()
    {
        if (syringes > 0)
        {
            audio.PlayOneShot(syringeSound, 1.0f);
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

    public bool canBuyMine
    {
        get { return mines < maxMines; }
    }
}
