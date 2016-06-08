using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ControllerNet : NetworkManager {
	

	private bool firstTeam = true;

	/*public override void OnServerConnect(NetworkConnection conn){
		
	}*/

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{	
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		//SpawnPoints scriptSpawnPoints = player.GetComponent<SpawnPoints> ();
		string whichTagTeam = "SpawnTeam0";

		if (firstTeam){
			firstTeam = false;

			player.GetComponent<SimpleController> ().team = 0;
			player.gameObject.GetComponent<MeshRenderer> ().material.color = Color.blue;
			player.gameObject.tag = "VehicleTeam0";
			whichTagTeam = "SpawnTeam0";
		} else {
			player.GetComponent<SimpleController> ().team = 1;
			player.gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;
			player.gameObject.tag = "VehicleTeam1";
			whichTagTeam = "SpawnTeam1";
		}

		GameObject[] spawns = GameObject.FindGameObjectsWithTag (whichTagTeam);

		int randomRange = Random.Range (0, spawns.Length);

		player.transform.position = spawns [randomRange].transform.position;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}
}
