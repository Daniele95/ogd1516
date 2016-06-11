using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoaderClass : NetworkBehaviour {
	public GameObject mineShoot;
	public GameObject machineGunShoot;
	public GameObject laserShot;
	public GameObject bazookaShot;
	public GameObject bombShot;

	//public string tagTeam = "";

	//public GameObject drifterMesh;
	//public GameObject camperMesh;
	//public GameObject minerMesh;

	[SyncVar]
	public int vehicleTypeClass = 0;

	void setSkinModel(int typeClass){
		GameObject drifterMesh = transform.Find ("drifter").gameObject;
		GameObject minerMesh = transform.Find ("miner").gameObject;
		GameObject camperMesh = transform.Find ("camper").gameObject;

		if (typeClass == 0) {//DRIFTER
			drifterMesh.SetActive (true);
			minerMesh.SetActive (false);
			camperMesh.SetActive (false);
		}else if(typeClass == 1){//MINER
			minerMesh.SetActive (true);
			drifterMesh.SetActive (false);
			camperMesh.SetActive (false);
		}else if(typeClass == 2){//CAMPER
			camperMesh.SetActive (true);
			drifterMesh.SetActive (false);
			minerMesh.SetActive (false);
		}
	}

	void setClass(int typeClass){
		SimpleController scriptMovement = GetComponent<SimpleController> ();
		GuiVehicle scriptGUI = GetComponent<GuiVehicle> ();
		Shooting scriptShooting = GetComponent<Shooting> ();
		//GameObject renderer = transform.Find ("positionRenderMesh").gameObject;


		//MeshFilter mesh = GetComponent<MeshFilter> ();

		if (typeClass == 0) {//DRIFTER
			scriptMovement.acceleration = 20000f;
			scriptMovement.FRICTION = 5f;
			scriptMovement.specialPower = 1;

			scriptGUI.maxLife = 100;
			scriptGUI.life = 100;

			scriptShooting.numBulletsFirstWeapon = 720;
			scriptShooting.maxNumBulletsFirstWeapon = 720;
			scriptShooting.startTimerShootFirstWeapon = 0.125f;
			scriptShooting.shoot = machineGunShoot;

			scriptShooting.numBulletsSecondWeapon = 0;
			scriptShooting.maxNumBulletsSecondWeapon = 0;
			scriptShooting.startTimerShootSecondWeapon = 0.125f;
			scriptMovement.damageDrift = 5;
			scriptShooting.shootSecond = null;
		}else if(typeClass == 1){//MINER
			scriptMovement.acceleration = 15000f;
			scriptMovement.FRICTION = 5f;

			scriptGUI.maxLife = 70;
			scriptGUI.life = 70;

			scriptShooting.numBulletsFirstWeapon = 10;
			scriptShooting.maxNumBulletsFirstWeapon = 10;
			scriptShooting.startTimerShootFirstWeapon = 3.34f;
			scriptShooting.shoot = mineShoot;

			scriptShooting.numBulletsSecondWeapon = 15;
			scriptShooting.maxNumBulletsSecondWeapon = 15;
			scriptShooting.startTimerShootSecondWeapon = 4f;
			scriptShooting.shootSecond = bombShot;
		}else if(typeClass == 2){//CAMPER
			scriptMovement.acceleration = 20000f;
			scriptMovement.FRICTION = 5f;
			scriptMovement.specialPower = 2;

			scriptGUI.maxLife = 130;
			scriptGUI.life = 130;

			scriptShooting.numBulletsFirstWeapon = 15;
			scriptShooting.maxNumBulletsFirstWeapon = 15;
			scriptShooting.startTimerShootFirstWeapon = 4f;
			scriptShooting.shoot = bazookaShot;

			scriptShooting.numBulletsSecondWeapon = 20;
			scriptShooting.maxNumBulletsSecondWeapon = 20;
			scriptShooting.startTimerShootSecondWeapon = 1f;
			scriptShooting.shootSecond = laserShot;
		}
	}

	void setTeam(int whichTeam){
		if (whichTeam == 0) {
			gameObject.tag = "VehicleTeam0";
			gameObject.layer = 8;

			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.layer = 8;
			}
		} else if (whichTeam == 1)  {
			gameObject.tag = "VehicleTeam1";
			gameObject.layer = 9;

			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.layer = 9;
			}
		}
	}

	[Command]
	void CmdSetClass(int typeClass)//float rx, float ry, float rz
	{
		vehicleTypeClass = typeClass;
		setSkinModel(vehicleTypeClass);
		setClass (typeClass);
	}

	[Command]
	void CmdSetTeam(int whichTeam)//float rx, float ry, float rz
	{
		setTeam (whichTeam);
		SimpleController scriptMovement = GetComponent<SimpleController> ();
		scriptMovement.team = whichTeam;
	}

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			NetworkManagerHUD netScript = GameObject.Find ("ControllerNet").gameObject.GetComponent<NetworkManagerHUD> ();

			vehicleTypeClass = netScript.classType;

			setClass (vehicleTypeClass);

			CmdSetClass (vehicleTypeClass);

			setTeam (netScript.team);

			CmdSetTeam (netScript.team);
		}
	}

	private bool haveToSetClass = true;

	// Update is called once per frame
	void Update () {
		if (haveToSetClass) {
			//setClass (typeClass);
			setSkinModel(vehicleTypeClass);

			haveToSetClass = false;
		}
	}
}
