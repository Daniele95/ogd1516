using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Lobby : NetworkBehaviour {
	[SyncVar]
	public int activePlayers = 0;

	private int numBlueTeamPlayers = 0;
	private int numRedTeamPlayers = 0;

	public void matchmaker(GameObject player){
		if (!isServer)
			return;

		int maxPlayers = GetComponent<ControllerNet> ().maxPlayers;

		if (numBlueTeamPlayers == 2) {
			player.GetComponent<LoaderClass> ().teamPlayer = 1;

			numRedTeamPlayers++;
		} else if (numRedTeamPlayers == 2) {
			player.GetComponent<LoaderClass> ().teamPlayer = 0;

			numBlueTeamPlayers++;
		} else {
			Random.seed = (int)System.DateTime.Now.Ticks;

			float random = Random.Range(0,2);

			if (random == 0) {
				if (numBlueTeamPlayers < 2) {
					player.GetComponent<LoaderClass> ().teamPlayer = 0;

					numBlueTeamPlayers++;
				}
			} else if (random == 1) {
				if (numRedTeamPlayers < 2) {
					player.GetComponent<LoaderClass> ().teamPlayer = 1;

					numRedTeamPlayers++;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
