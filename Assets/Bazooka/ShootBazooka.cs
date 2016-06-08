using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootBazooka : NetworkBehaviour {
	private Rigidbody body;
	public float speed = 25f;
	public int hitPoints = 50;

	// Use this for initialization
	void Start () {
		if (!isServer)
			return;

		body = GetComponent<Rigidbody> ();

		body.velocity = transform.forward * speed;

		//Destroy (this.gameObject, 1f);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision col){
		if (!isServer)
			return;

		print (col.gameObject.tag + " " + gameObject.tag);
		if (col.gameObject.CompareTag ("VehicleTeam0") && gameObject.CompareTag("BulletTeam1") || col.gameObject.CompareTag ("VehicleTeam1") && gameObject.CompareTag("BulletTeam0")) {
			GuiVehicle gui = col.gameObject.GetComponent<GuiVehicle> ();

			gui.TakeDamage (hitPoints);

			Destroy (gameObject);
		}else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {// && !col.gameObject.transform.parent.gameObject.CompareTag ("Vehicle")
			// && !col.gameObject.CompareTag ("Vehicle")
			Destroy (gameObject);
		}

		//Destroy (gameObject);
	}
}
