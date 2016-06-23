using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Lobby : NetworkBehaviour {
	[SyncVar]
	public int activePlayers = 0;

	private int numBlueTeamPlayers = 0;
	private int numRedTeamPlayers = 0;

	void Awake()
	{
		//this avoid the destruction of network manager
		//DontDestroyOnLoad(transform.gameObject); 
	}

	public void matchmaker(GameObject player){
		if (!isServer)
			return;

		int maxPlayers = GameObject.Find("ControllerNet").GetComponent<ControllerNet> ().maxPlayers;

		if (numBlueTeamPlayers == maxPlayers / 2) {
			//player.GetComponent<LoaderClass> ().setTeamMatchMaking (1);

			numRedTeamPlayers++;
		} else if (numRedTeamPlayers == maxPlayers / 2) {
			//player.GetComponent<LoaderClass> ().setTeamMatchMaking (0);

			numBlueTeamPlayers++;
		} else {
			Random.seed = (int)System.DateTime.Now.Ticks;

			float random = Random.Range(0,2);

			print (random);

			if (random == 0) {
				if (numBlueTeamPlayers < maxPlayers / 2) {
					//player.GetComponent<LoaderClass> ().setTeamMatchMaking (0);

					numBlueTeamPlayers++;
				}
			} else if (random == 1) {
				if (numRedTeamPlayers < maxPlayers / 2) {
					//player.GetComponent<LoaderClass> ().setTeamMatchMaking (1);

					numRedTeamPlayers++;
				}
			}
		}

		print ("B:" + numBlueTeamPlayers + " R:" + numRedTeamPlayers);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
