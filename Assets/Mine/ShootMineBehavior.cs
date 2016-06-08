using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootMineBehavior : NetworkBehaviour {
	private Rigidbody body;
	public int hitPoints = 70;

	// Use this for initialization
	void Start () {
		if (!isServer)
			return;

		body = GetComponent<Rigidbody> ();

		//body.velocity = transform.forward * speed;

		//Destroy (this.gameObject, 1f);
	}

	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;

		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;

		if (gameObject.CompareTag ("BulletTeam1")) {
			GameObject[] playersEnemy = GameObject.FindGameObjectsWithTag ("VehicleTeam0");

			for (int i = 0; i < playersEnemy.Length; i++) {
				if (Vector3.Distance (playersEnemy[i].transform.position, transform.position) <= 5f) {
					GuiVehicle gui = playersEnemy[i].GetComponent<GuiVehicle> ();
					gui.TakeDamage (hitPoints);

					Destroy (gameObject);

					break;
				}
			}
		}else if (gameObject.CompareTag ("BulletTeam0")) {
			GameObject[] playersEnemy = GameObject.FindGameObjectsWithTag ("VehicleTeam1");

			for (int i = 0; i < playersEnemy.Length; i++) {
				if (Vector3.Distance (playersEnemy[i].transform.position, transform.position) <= 5f) {
					GuiVehicle gui = playersEnemy[i].GetComponent<GuiVehicle> ();
					gui.TakeDamage (hitPoints);

					Destroy (gameObject);

					break;
				}
			}
		}
	}

	/*void OnCollisionEnter(Collision col){
		if (!isServer)
			return;

		if (col.gameObject.CompareTag ("VehicleTeam0") && gameObject.CompareTag("BulletTeam1") || col.gameObject.CompareTag ("VehicleTeam1") && gameObject.CompareTag("BulletTeam0")) {
			GuiVehicle gui = col.gameObject.GetComponent<GuiVehicle> ();
			//gui.life -= hitPoints;

			gui.TakeDamage (hitPoints);
		}

		Destroy (gameObject);
	}*/
}
