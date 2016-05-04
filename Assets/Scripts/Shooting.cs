using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {
	public bool Player = true;
	public GameObject shoot;
	public GameObject shooter;
	public float startTimerShoot = 0.25f;
	public int numBullets = 1;
	public int maxNumBullets = 10;

	private float timerShoot = 0f;
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}

	void FixedUpdate(){
		timerShoot -= Time.fixedDeltaTime;

		if (timerShoot < 0f)
			timerShoot = 0f;
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w / 2f, h);

		if (!Player)
			rect.Set (w / 2f, 0, w / 2f, h);

		style.alignment = TextAnchor.LowerLeft;
		style.fontSize = h * 5 / 100;
		style.normal.textColor = Color.red;

		string text = "Bullets: " + numBullets.ToString() + "/" + maxNumBullets.ToString();
		GUI.Label(rect, text, style);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag ("AmmoPickup")) {
			AmmoPickupBehaviour pickup = (AmmoPickupBehaviour)col.gameObject.GetComponent<AmmoPickupBehaviour> ();
			numBullets += pickup.numBulletsPickup;

			if (numBullets > maxNumBullets)
				numBullets = maxNumBullets;

			Destroy (col.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if ((Player && Input.GetKey (KeyCode.E)) || !Player && Input.GetKey(KeyCode.RightControl)) {
			if (timerShoot <= 0f) {
				if (numBullets > 0) {
					numBullets--;

					GameObject shot = (GameObject)Instantiate (shoot, shooter.transform.position, shooter.transform.rotation);
					if (Player)
						shot.tag = "BulletTeam0";
					else
						shot.tag = "BulletTeam1";

					timerShoot = startTimerShoot;
				}
			}
		}
	}
}
