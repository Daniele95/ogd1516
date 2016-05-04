using UnityEngine;
using System.Collections;

public class ShootBehaviour : MonoBehaviour {
	private Rigidbody body;
	public float speed = 50f;
	public int hitPoints = 10;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();

		body.velocity = transform.forward * speed;

		Destroy (this.gameObject, 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag ("VehicleTeam0") && gameObject.CompareTag("BulletTeam1") || col.gameObject.CompareTag ("VehicleTeam1") && gameObject.CompareTag("BulletTeam0")) {
			GuiVehicle gui = col.gameObject.GetComponent<GuiVehicle> ();
			gui.life -= hitPoints;

			Destroy (gameObject);
		}
	}
}
