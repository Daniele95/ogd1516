using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootBehaviour : NetworkBehaviour {
	private Rigidbody body;
	public float speed = 50f;
	public int hitPoints = 10;

	private float timer = 0f;

	//private Detonator detonator;

	public GameObject explosion;
	public GameObject explosionHitPlayer;

	[Command]
	void CmdDoExplosion(){
		//GameObject shotExplosion = (GameObject)Instantiate (explosion, transform.position, transform.rotation);

		//NetworkServer.Spawn (shotExplosion);
		NetworkServer.Destroy (gameObject);
		//Destroy (gameObject);
	}

	[Command]
	void CmdDoExplosionHitPlayer(){
		GameObject shotExplosionHitPlayer = (GameObject)Instantiate (explosionHitPlayer, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosionHitPlayer);
		NetworkServer.Destroy (gameObject);
		//Destroy (gameObject);
	}

    [ClientRpc]
    void RpcAssignTagLayer(string tag, int layer)
    {
		gameObject.tag = tag;
        gameObject.layer = layer;
    }

    // Use this for initialization
    void Start () {
		if (!isServer)
			return;

		timer = Time.time;

		body = GetComponent<Rigidbody> ();

		body.velocity = transform.forward * speed;

        //Destroy (this.gameObject, 5f);

        //detonator = GetComponent<Detonator> ();

        RpcAssignTagLayer(gameObject.tag, gameObject.layer);
    }
	
	// Update is called once per frame
	void Update () {
		if(isServer)
			if (Time.time - timer > 5)
				NetworkServer.Destroy (gameObject);
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
		}else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {
			//detonator.Explode ();

			CmdDoExplosion ();
		}
	}
}
