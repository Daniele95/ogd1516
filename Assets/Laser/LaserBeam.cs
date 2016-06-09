using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LaserBeam : NetworkBehaviour {
	//private Rigidbody body;
	public float growSpeed = 5f;
	public int hitPoints = 60;

	private float scaleZ = 10f;

	private List<GameObject> hitEnemies;

	//private float alpha = 1f;

	//private Renderer mat;

	//private GameObject pfx;

	// Use this for initialization
	void Start () {
		//mat = GetComponent<Renderer> ();

		if (!isServer)
			return;

		hitEnemies = new List<GameObject> ();

		//body = GetComponent<Rigidbody> ();

		//body.velocity = transform.forward * speed;

		Destroy (this.gameObject, 1f);

		//pfx = transform.GetChild (0);
	}

	// Update is called once per frame
	void Update () {
		//if (!isServer)
		//	return;

		scaleZ += growSpeed * Time.deltaTime;
		transform.localScale = new Vector3 (50, 10, scaleZ);
		transform.position += transform.forward * Time.deltaTime * growSpeed / 2f;

		//alpha -= Time.deltaTime * 2f;
		//mat.material.SetColor ("_Albedo", new Color (0f, 0f, 0f, 0f));
	}

	void OnCollisionEnter(Collision col){
		if (!isServer)
			return;

		print (col.gameObject);

		if (col.gameObject.CompareTag ("VehicleTeam0") && gameObject.CompareTag ("BulletTeam1") || col.gameObject.CompareTag ("VehicleTeam1") && gameObject.CompareTag ("BulletTeam0")) {
			Physics.IgnoreCollision(col.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

			bool found = false;

			//for (int i = 0; i < hitEnemies.Count; i++) {
			if (hitEnemies.Contains (col.gameObject)) {
				found = true;
			} else {
				hitEnemies.Add (col.gameObject);
			}
			//}

			if (!found) {
				GuiVehicle gui = col.gameObject.GetComponent<GuiVehicle> ();
				//gui.life -= hitPoints;

				gui.TakeDamage (hitPoints);

				//print ("hiT:" + hitPoints);
			}

			//Destroy (gameObject);
		}// else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {
			//print ("collision " + col.gameObject.name);
			//Destroy (gameObject);
		//}
	}
}
