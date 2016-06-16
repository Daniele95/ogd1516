using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthPickupBehaviour : NetworkBehaviour {
	public int healthPickup = 50;

	private Text text;
    private float speedRotation = 10f;

	public bool getPickup;

    public float RADIUS_PICKUP = 3f;

    // Use this for initialization
    void Start () {
		text = this.GetComponentInChildren<Text> ();

		text.text = "Health";
	}

	[Command]
	void CmdGetPickup(){
		NetworkServer.Destroy (gameObject);

		//RpcGetPickup ();
	}

	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, Time.deltaTime * speedRotation, 0f);

		//if (isServer) {
			if (getPickup) {
				getPickup = false;
				CmdGetPickup ();
			}
		//}
	}
}
