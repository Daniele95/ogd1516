using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootBehaviour : NetworkBehaviour {
	private Rigidbody body;
	public float speed = 50f;
	public int hitPoints = 10;

	//private Detonator detonator;

	public GameObject explosion;
	public GameObject explosionHitPlayer;

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

		body.velocity = transform.forward * speed;

		Destroy (this.gameObject, 5f);

		//detonator = GetComponent<Detonator> ();
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

			CmdDoExplosionHitPlayer ();

			//detonator.Explode ();

			Destroy (gameObject);
		}else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {
			//detonator.Explode ();

			CmdDoExplosion ();

			Destroy (gameObject);
		}
	}
}
