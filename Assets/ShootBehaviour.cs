﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootBehaviour : NetworkBehaviour {
	private Rigidbody body;
	public float speed = 50f;
	public int hitPoints = 10;

	// Use this for initialization
	void Start () {
		if (!isServer)
			return;

		body = GetComponent<Rigidbody> ();

		body.velocity = transform.forward * speed;

		Destroy (this.gameObject, 5f);
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

			Destroy (gameObject);
		}else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {
			Destroy (gameObject);
		}
	}
}