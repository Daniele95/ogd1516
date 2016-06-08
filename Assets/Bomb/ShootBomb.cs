using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootBomb : NetworkBehaviour {
	private Rigidbody body;
	public float speed = 50f;
	public float speedVehicle = 1f;
	public int hitPoints = 10;
	public float MAX_SPEED_VEHICLE = 20f;
	public float MIN_SPEED_VEHICLE = 5f;

	// Use this for initialization
	void Start () {
		if (!isServer)
			return;

		body = GetComponent<Rigidbody> ();

		if (speedVehicle > MAX_SPEED_VEHICLE)
			speedVehicle = MAX_SPEED_VEHICLE;

		if (speedVehicle < MIN_SPEED_VEHICLE)
			speedVehicle = MIN_SPEED_VEHICLE;

		body.velocity = transform.forward * speed * speedVehicle + transform.up * speed * speedVehicle / 2f;

		//Destroy (this.gameObject, 1f);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision col){
		if (!isServer)
			return;

		if (col.gameObject.CompareTag ("VehicleTeam0") && gameObject.CompareTag("BulletTeam1") || col.gameObject.CompareTag ("VehicleTeam1") && gameObject.CompareTag("BulletTeam0")) {
			GuiVehicle gui = col.gameObject.GetComponent<GuiVehicle> ();
			//gui.life -= hitPoints;

			gui.TakeDamage (hitPoints);
		}else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {
			Destroy (gameObject);
		}
	}
}
