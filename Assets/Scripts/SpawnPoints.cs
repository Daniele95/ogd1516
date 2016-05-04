using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoints : MonoBehaviour {
	public List<GameObject> spawnpoints;
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void respawn(){
		GameObject[] spawns = spawnpoints.ToArray ();

		int randomRange = Random.Range (0, spawns.Length - 1);

		transform.position = spawns [randomRange].transform.position;
		transform.rotation = Quaternion.identity;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
	}
}
