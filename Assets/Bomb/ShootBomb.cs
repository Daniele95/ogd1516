﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootBomb : NetworkBehaviour {
	private Rigidbody body;
	public float speed = 50f;
	public float speedVehicle = 1f;
	public int hitPoints = 50;
	public float MAX_SPEED_VEHICLE = 20f;
	public float MIN_SPEED_VEHICLE = 5f;
	public int FACTOR_HIT_PLAYER = 2;
	public float radius = 10f;


	public GameObject explosion;
	public GameObject explosionGround;
	public GameObject explosionHitPlayer;

	[Command]
	void CmdDoExplosionGround (){
		GameObject shotExplosion = (GameObject)Instantiate (explosionGround, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosion);
	}

	[Command]
	void CmdDoExplosion(){
		GameObject shotExplosion = (GameObject)Instantiate (explosion, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosion);
	}

	[Command]
	void CmdDoExplosionHitPlayer(){
		GameObject shotExplosionHitPlayer = (GameObject)Instantiate (explosionHitPlayer, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosionHitPlayer);
	}

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

			gui.TakeDamage (hitPoints * FACTOR_HIT_PLAYER);

			CmdDoExplosionHitPlayer ();

			Destroy (gameObject);
		}else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {// && !col.gameObject.CompareTag ("Vehicle")
			if (gameObject.CompareTag ("BulletTeam1")) {
				GameObject[] playersEnemy = GameObject.FindGameObjectsWithTag ("VehicleTeam0");

				for (int i = 0; i < playersEnemy.Length; i++) {
					if (Vector3.Distance (playersEnemy[i].transform.position, transform.position) <= radius) {
						GuiVehicle gui = playersEnemy[i].GetComponent<GuiVehicle> ();
						if (gui != null) {
							gui.TakeDamage (hitPoints);

							CmdDoExplosion ();

							//Destroy (gameObject);
						}

						//break;
					}
				}
			}else if (gameObject.CompareTag ("BulletTeam0")) {
				GameObject[] playersEnemy = GameObject.FindGameObjectsWithTag ("VehicleTeam1");

				for (int i = 0; i < playersEnemy.Length; i++) {
					if (Vector3.Distance (playersEnemy[i].transform.position, transform.position) <= radius) {
						GuiVehicle gui = playersEnemy[i].GetComponent<GuiVehicle> ();
						if (gui != null) {
							gui.TakeDamage (hitPoints);


							CmdDoExplosion ();
							//Destroy (gameObject);
						}

						//break;
					}
				}
			}

			CmdDoExplosionGround ();

			Destroy (gameObject);

			//print ("onground");
		}
	}
}
