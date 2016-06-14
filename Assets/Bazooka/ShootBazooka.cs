using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShootBazooka : NetworkBehaviour {
	private Rigidbody body;
	public float speed = 25f;
	public int hitPoints = 50;

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

		body = GetComponent<Rigidbody> ();

		body.velocity = transform.forward * speed;

        RpcAssignTagLayer(gameObject.tag, gameObject.layer);

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

			CmdDoExplosionHitPlayer ();

			Destroy (gameObject);
		}else if (!col.gameObject.CompareTag ("VehicleTeam0") && !col.gameObject.CompareTag ("VehicleTeam1")) {// && !col.gameObject.transform.parent.gameObject.CompareTag ("Vehicle")
			// && !col.gameObject.CompareTag ("Vehicle")
			CmdDoExplosion();

			Destroy (gameObject);
		}

		//Destroy (gameObject);
	}
}
