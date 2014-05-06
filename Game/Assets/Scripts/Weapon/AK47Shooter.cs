using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class AK47Shooter : MonoBehaviour {
	public GameObject bulletSource;
	public GameObject granadeLauncher;
	public GameObject granadePrefab;
	public float granadeSpeed;
    public AudioClip pewSound;
    public AudioClip outOfAmmoSound;

    [Range(0.1f, 0.5f)]
    public float shotPeriod;
    public int clipSize;
	public float recoil;
	public float handling;


    ShootingPhase shootingPhase;

    private float nextShot;
    private int currentClip;
	private float spread;
	private float knockBack;
	private float granadeCooldown = 1.0f;
	private float nextGranade = 0.0f;

	bool playerIndexSet = false;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

    void Start() {
        shootingPhase = ShootingPhase.NotShooting;
        nextShot = Time.fixedTime;
        currentClip = clipSize;
    }

    void Update() {
		if(nextGranade > 0.0f) nextGranade -= Time.deltaTime;

		if (Input.GetMouseButton(0) || state.Triggers.Right > 0.75f) {
            if (shootingPhase == ShootingPhase.NotShooting)
                shootingPhase = ShootingPhase.ShootingStart;
        } else {
            if (shootingPhase == ShootingPhase.Shooting)
                shootingPhase = ShootingPhase.NotShooting;
        }
        
		if (Input.GetKeyDown(KeyCode.R) || (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released)) {
            currentClip = clipSize;
        }

		if (nextGranade <= 0.0f && (Input.GetMouseButtonDown(2) || (state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder == ButtonState.Released))) {
			GameObject granade = Instantiate(granadePrefab, granadeLauncher.transform.position, granadeLauncher.transform.localRotation) as GameObject;
			granade.rigidbody.velocity = transform.root.rigidbody.velocity + granadeLauncher.transform.forward * granadeSpeed;
			nextGranade = granadeCooldown;
		}
    }

    void FixedUpdate() {
		if (!playerIndexSet || !prevState.IsConnected)
		{
			for (int i = 0; i < 4; ++i)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected)
				{
					Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}
		
		prevState = state;
		state = GamePad.GetState(playerIndex);

		float effectiveHandling = handling;
		if (PlayerController.instance.isCrouching) {
			effectiveHandling *= 4.0f;
		}
		spread += PlayerController.instance.speed * 0.001f;
		knockBack *= Mathf.Exp(-Time.fixedDeltaTime * effectiveHandling);
		spread *= Mathf.Exp(-Time.fixedDeltaTime * effectiveHandling);
        
        if (Time.fixedTime >= nextShot && shootingPhase != ShootingPhase.NotShooting) {
			RaycastHit hitInfo;
			Vector3 start = bulletSource.transform.position;
			Vector3 dir = bulletSource.transform.forward;
			float angle = Random.Range(-Mathf.PI, Mathf.PI);
			float dist = Random.Range(0, spread);
			dir += transform.up * Mathf.Sin(angle) * dist;
			dir += transform.right * Mathf.Cos(angle) * dist;
			dir.Normalize();

			bool hit = Physics.Raycast(start, dir, out hitInfo, 10000.0f);
            shootingPhase = ShootingPhase.Shooting;
            nextShot = Time.fixedTime + shotPeriod;


            if (currentClip <= 0) {
                audio.PlayOneShot(outOfAmmoSound);
            } else {
                --currentClip;
                audio.PlayOneShot(pewSound);
				if (hit) {
					Debug.DrawLine(start, hitInfo.point, Color.red, 0.2f, true);
					Damageable damagable = hitInfo.collider.GetComponent<Damageable>();
					if (damagable != null) {
						damagable.DealDamage(dir * 11.0f);
					}
					MaterialHolder mat = hitInfo.collider.GetComponent<MaterialHolder>();
					if (mat != null) {
						GameObject hole = Instantiate(mat.materialProperties.bulletHolePrefab, hitInfo.point + 0.01f * hitInfo.normal, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
						hole.transform.parent = hitInfo.transform;
						BulletHoleManager.instance.AddBulletHole(hole);
					}
				}
				else {
					Debug.DrawRay(start, dir * 1000.0f, Color.red, 0.2f, true);
				}
				spread += recoil;
				knockBack += recoil;
				FirstPersonCameraController.instance.verticalAngle += knockBack * 10.0f;
				
            }
        }
		transform.localRotation = Quaternion.Euler(new Vector3(-100.0f * knockBack, 0, 0));
    }

    void OnGUI() {
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.Label(new Rect(sw * 0.8f, sh * 0.8f, sw * 0.2f, sh * 0.2f), "AMMO: " + currentClip);
    }



	enum ShootingPhase {
		NotShooting,
		ShootingStart,
		Shooting
	}
}
