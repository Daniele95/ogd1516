using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoints : MonoBehaviour {
	//public List<GameObject> spawnpoints;
	//private Rigidbody body;
	private SimpleController scriptMovement;
	public float radiusSpawn = 10f;

	// Use this for initialization
	void Start () {
		//body = GetComponent<Rigidbody> ();
		scriptMovement = GetComponent<SimpleController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void respawn(){
		string whichTagTeam = "";
		int tagTeam = -1;

		if (gameObject.CompareTag("VehicleTeam0")){
			whichTagTeam = "SpawnTeam0";
			tagTeam = 0;
		} else if (gameObject.CompareTag("VehicleTeam1")){
			whichTagTeam = "SpawnTeam1";
			tagTeam = 1;
		}

		GameObject[] spawns = GameObject.FindGameObjectsWithTag (whichTagTeam);

		bool canSpawn = false;
		int randomRange = -1;

		//while (!canSpawn) {
			randomRange = Random.Range (0, spawns.Length);

			//if (spawns [randomRange].GetComponent<SpawnPoint> ().isEmpty (tagTeam))
				canSpawn = true;
		//}

		Vector2 circle = Random.insideUnitCircle;
		Vector3 vector3radius = new Vector3 (circle.x, 0f, circle.y) * radiusSpawn;

		transform.position = spawns [randomRange].transform.position;
		transform.position += vector3radius;
		transform.rotation = spawns [randomRange].transform.rotation;

		scriptMovement.body.velocity = Vector3.zero;
		scriptMovement.velocity = Vector3.zero;

		//body.angularVelocity = Vector3.zero;
		//body.velocity = Vector3.zero;
	}
}