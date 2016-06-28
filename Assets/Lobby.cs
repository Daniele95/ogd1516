using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Lobby : NetworkBehaviour {
	[SyncVar]
	public int activePlayers = 0;

	[SyncVar]
	public int numBlueTeamPlayers = 0;

	[SyncVar]
	public int numRedTeamPlayers = 0;

	[ClientRpc]
	void RpcAddPlayer(int players){
		//if (isLocalPlayer) {
		activePlayers = players;
		//}
	}

	public void addPlayer(){
		activePlayers++;

		RpcAddPlayer (activePlayers);
	}

	void Awake()
	{
		//this avoid the destruction of network manager
		//DontDestroyOnLoad(transform.gameObject); 
	}

	[Command]
	void CmdSetTeam(int team)//float rx, float ry, float rz
	{
		if (team == 0)
			numBlueTeamPlayers++;
		if (team == 1)
			numRedTeamPlayers++;
	}

	public int matchmaker(){
		if (!isServer)
			return -1;

		int team = -1;

		int maxPlayers = GameObject.Find("ControllerNet").GetComponent<ControllerNet> ().maxPlayers;

		if (numBlueTeamPlayers == maxPlayers / 2) {
			team = 1;

			CmdSetTeam (1);
		} else if (numRedTeamPlayers == maxPlayers / 2) {
			team = 0;

			CmdSetTeam (0);
		} else {
			Random.seed = (int)System.DateTime.Now.Ticks;

			float random = Random.Range(0,2);

			//print (random);

			if (random == 0) {
				if (numBlueTeamPlayers < maxPlayers / 2) {
					team = 0;

					CmdSetTeam (0);
				}
			} else if (random == 1) {
				if (numRedTeamPlayers < maxPlayers / 2) {
					team = 1;

					CmdSetTeam (1);
				}
			}
		}

		return team;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if (!GameObject.Find ("ControllerNet").GetComponent<ControllerNet> ().canPlay(false) && GameObject.Find("ControllerGame").GetComponent<ControllerGaming>().timer <= 0f)
		//	activePlayers = 0;
	}
}
