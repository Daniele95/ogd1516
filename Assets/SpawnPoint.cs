using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isEmpty(int spawnTeam){
		bool empty = true;

		if (spawnTeam == 0) {
			GameObject[] team0 = GameObject.FindGameObjectsWithTag ("VehicleTeam0");

			for (int i = 0; i < team0.Length && empty; i++) {
				if (Vector3.Distance (team0 [i].transform.position, transform.position) < 1f && team0 [i].GetComponent<GuiVehicle> ().life == 0)
					empty = false;
			}
		}else if (spawnTeam == 1) {
			GameObject[] team1 = GameObject.FindGameObjectsWithTag ("VehicleTeam1");

			for (int i = 0; i < team1.Length && empty; i++) {
				if (Vector3.Distance (team1 [i].transform.position, transform.position) < 1f && team1 [i].GetComponent<GuiVehicle> ().life == 0)
					empty = false;
			}
		}

		print (empty);

		return empty;
	}
}
