using UnityEngine;
using System.Collections;

public class Tunnel : MonoBehaviour {
	public bool reverse;

	public GameObject listPoints;

	void Start () {
	
	}
	
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("VehicleTeam0") || other.gameObject.CompareTag ("VehicleTeam1")) {
			SimpleController scriptController = other.gameObject.GetComponent<SimpleController> ();

			if (scriptController.inTunnel == 0) {
				scriptController.inTunnel = 1;
				scriptController.targetWayPoint = null;
				scriptController.reverseTunnel = reverse;
				scriptController.wayPointList = listPoints;
			}
		}
	}

	void OnTriggerExit(Collider other){
		/*print (other.gameObject.name + " uscito");

		if (other.gameObject.CompareTag ("VehicleTeam0") || other.gameObject.CompareTag ("VehicleTeam1")) {
			Tunnel tunnelScript = endTunnel.GetComponent<Tunnel>();
			if(tunnelScript.
			SimpleController scriptController = other.gameObject.GetComponent<SimpleController> ();

			scriptController.inTunnel = 0;
		}*/
	}
}
