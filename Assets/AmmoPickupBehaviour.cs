using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AmmoPickupBehaviour : NetworkBehaviour {
	public int numBulletsPickup = 5;

	private Text text;
    private float speedRotation = 10f;

	public bool getPickup;

    public float RADIUS_PICKUP = 3f;

    // Use this for initialization
    void Start () {
		text = this.GetComponentInChildren<Text> ();

		text.text = "Ammo";

		getPickup = false;
	}

	[Command]
	void CmdGetPickup(){
		Destroy (gameObject);

		//RpcGetPickup ();
	}

	//[ClientRpc]
	//void RpcGetPickup(){
		//Destroy (gameObject);
	//}
	
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
