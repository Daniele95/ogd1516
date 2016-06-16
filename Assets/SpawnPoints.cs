using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoints : MonoBehaviour {
	public List<GameObject> spawnpoints;
	//private Rigidbody body;

	// Use this for initialization
	void Start () {
		//body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void respawn(){
		string whichTagTeam;

		if (gameObject.CompareTag("VehicleTeam0")){
			whichTagTeam = "SpawnTeam0";
		} else {
			whichTagTeam = "SpawnTeam1";
		}

		GameObject[] spawns = GameObject.FindGameObjectsWithTag (whichTagTeam);

		int randomRange = Random.Range (0, spawns.Length);

		transform.position = spawns [randomRange].transform.position;
		transform.rotation = spawns [randomRange].transform.rotation;
		//body.angularVelocity = Vector3.zero;
		//body.velocity = Vector3.zero;
	}
}
