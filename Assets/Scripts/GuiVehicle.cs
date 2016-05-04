using UnityEngine;
using System.Collections;

public class GuiVehicle : MonoBehaviour {
	public bool Player = true;
	public int life = 70;
	public int maxLife = 100;

	private int whichTeam = -1;
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		whichTeam = gameObject.CompareTag ("VehicleTeam0") ? 0 : 1;

		body = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag ("HealthPickup")) {
			HealthPickupBehaviour pickup = (HealthPickupBehaviour)col.gameObject.GetComponent<HealthPickupBehaviour> ();
			life += pickup.healthPickup;

			if (life > maxLife)
				life = maxLife;

			Destroy (col.gameObject);
		}
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w / 2f, h);

		if (!Player)
			rect.Set (w / 2f, 0, w / 2f, h);

		style.alignment = TextAnchor.LowerRight;
		style.fontSize = h * 5 / 100;
		style.normal.textColor = Color.red;

		string text = "Life: " + life.ToString();
		GUI.Label(rect, text, style);
	}

	// Update is called once per frame
	void Update () {
		if (life <= 0) {
			life = 100;

			SpawnPoints respawn = GetComponent<SpawnPoints> ();
			respawn.respawn ();

			ControllerGame controller = GameObject.Find ("ControllerGame").GetComponent<ControllerGame> ();
			controller.addScoreTeam (whichTeam);
		}
	}
}
