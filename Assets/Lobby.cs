using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Lobby : NetworkBehaviour {
	[SyncVar]
	public int activePlayers = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
