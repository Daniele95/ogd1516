using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthPickupBehaviour : NetworkBehaviour {
	public int healthPickup = 50;

	private Text text;
    private float speedRotation = 10f;

	[SyncVar]
	public bool getPickup = false;

    public float RADIUS_PICKUP = 3f;

    public GameObject soundPickup;

    // Use this for initialization
    void Start () {
		text = this.GetComponentInChildren<Text> ();

		text.text = "Health";
	}

	void OnCollisionEnter(Collision col){
		/*if (isServer) {
            //GameObject healthSound = (GameObject)Instantiate(soundPickup, transform.position, transform.rotation);

            //NetworkServer.Spawn(soundPickup);

            NetworkServer.Destroy (gameObject);
		}*/
	}

	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, Time.deltaTime * speedRotation, 0f);

		if (isServer) {
			GameObject[] players = GameObject.FindGameObjectsWithTag("VehicleTeam0");

			for (int i = 0; i < players.Length; i++)
			{
				if (Vector3.Distance(players[i].transform.position, transform.position) <= RADIUS_PICKUP)
				{
					getPickup = true;
					NetworkServer.Destroy (gameObject);

					break;
				}
			}

			players = GameObject.FindGameObjectsWithTag("VehicleTeam1");

			for (int i = 0; i < players.Length; i++)
			{
				if (Vector3.Distance(players[i].transform.position, transform.position) <= RADIUS_PICKUP)
				{
					getPickup = true;
					NetworkServer.Destroy (gameObject);

					break;
				}
			}
		}
		//if (isServer) {
			/*if (getPickup) {
				getPickup = false;
				CmdGetPickup ();
			}*/
		//}
	}
}
