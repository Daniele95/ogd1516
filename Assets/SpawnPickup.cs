using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnPickup : NetworkBehaviour {
    public float startTimer = 5f;
    public GameObject pickupSpawned;

	private GameObject pickup = null;

	[SyncVar]
    public float timer;

	// Use this for initialization
	void Start () {
        timer = 0f;
	}
	
	[Command]
	void CmdSpawnPickup(){
		if (timer <= 0) {
			pickup = (GameObject)Instantiate (pickupSpawned, transform.position + Vector3.up * 3f, transform.rotation);

			NetworkServer.Spawn (pickup);
		}

		timer = startTimer;
	}

	[ClientRpc]
	void RpcSpawnPickup(){
		if(!isServer)
			pickup = (GameObject)Instantiate(pickupSpawned, transform.position + Vector3.up * 3f, transform.rotation);

		timer = startTimer;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (pickup != null)
			return;

        timer -= Time.fixedDeltaTime;

        if (timer < 0f)
            timer = 0f;
    }

    void Update()
    {
		if (!isServer)
			return;

        if(timer <= 0f)
        {
            if(pickup == null)
            {
				//timer = startTimer;

				CmdSpawnPickup ();

				//RpcSpawnPickup ();
            }
        }
    }
}
