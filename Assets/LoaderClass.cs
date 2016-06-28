using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoaderClass : NetworkBehaviour {
	public GameObject mineShoot;
	public GameObject machineGunShoot;
	public GameObject laserShot;
	public GameObject bazookaShot;
	public GameObject bombShot;

    //Amaranto #a7197b
    //Lime #d0ea2b

	//public string tagTeam = "";

	//public GameObject drifterMesh;
	//public GameObject camperMesh;
	//public GameObject minerMesh;

	[SyncVar]
	public int vehicleTypeClass = 0;

	[SyncVar]
	public int teamPlayer = 0;

	[SyncVar]
	public string userPlayer;

    public Texture textureTeam0;
    public Texture textureTeam1;

    public Texture textureTeam0Score;
    public Texture textureTeam1Score;

    private GameObject ammoGUI;
    private GameObject weaponGUI;
    private GameObject scoreGUI;
    private GameObject scoreBottomGUI;

	public Color teamColor;

	/*public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		base.OnDeserialize(reader, initialState);
	}*/

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

			scriptShooting.numBulletsFirstWeapon = 125;
			scriptShooting.maxNumBulletsFirstWeapon = 125;
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
			scriptShooting.startTimerShootFirstWeapon = 4f;
			scriptShooting.shoot = mineShoot;

			scriptShooting.numBulletsSecondWeapon = 12;
			scriptShooting.maxNumBulletsSecondWeapon = 12;
			scriptShooting.startTimerShootSecondWeapon = 3f;
			scriptShooting.shootSecond = bombShot;
		}else if(typeClass == 2){//CAMPER
			scriptMovement.acceleration = 20000f;
			scriptMovement.FRICTION = 5f;
			scriptMovement.specialPower = 2;

			scriptGUI.maxLife = 130;
			scriptGUI.life = 130;

			scriptShooting.numBulletsFirstWeapon = 8;
			scriptShooting.maxNumBulletsFirstWeapon = 8;
			scriptShooting.startTimerShootFirstWeapon = 4f;
			scriptShooting.shoot = bazookaShot;

			scriptShooting.numBulletsSecondWeapon = 10;
			scriptShooting.maxNumBulletsSecondWeapon = 10;
			scriptShooting.startTimerShootSecondWeapon = 4f;
			scriptShooting.shootSecond = laserShot;
		}
	}

	void setTeam(int whichTeam){
		Renderer material = null;

		GameObject drifterMesh = transform.Find ("drifter").gameObject;
		GameObject minerMesh = transform.Find ("miner").gameObject;
		GameObject camperMesh = transform.Find ("camper").gameObject;

		if (vehicleTypeClass == 0) {
			material = drifterMesh.transform.FindChild ("BODY").GetComponent<Renderer> ();
		} else if (vehicleTypeClass == 1) {
			material = minerMesh.transform.FindChild ("BODY").GetComponent<Renderer> ();
		} else if (vehicleTypeClass == 2) {
			material = camperMesh.transform.FindChild ("BODY").GetComponent<Renderer> ();
		}

		Material[] materials = material.materials;

		if (whichTeam == 0) {
			gameObject.tag = "VehicleTeam0";
			gameObject.layer = 11;

			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.layer = 8;
			}

			for(int i = 0; i < materials.Length; i++){
				//print (materials [i].name);
				if(materials[i].name.Contains("METALLO")){
					teamColor = new Color(167 / 255f, 25 / 255f, 123 / 255f);
					materials [i].SetColor ("_Color", teamColor);//0.82f, 0.92f, 0.17f

					break;
				}
			}
		} else if (whichTeam == 1)  {
			gameObject.tag = "VehicleTeam1";
			gameObject.layer = 12;

			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.layer = 9;
			}

			for(int i = 0; i < materials.Length; i++){
				//print (materials [i].name);
				if(materials[i].name.Contains("METALLO")){
					teamColor = new Color(208 / 255f, 234 / 255f, 43 / 255f);
					materials [i].SetColor ("_Color", teamColor);//0.82f, 0.92f, 0.17f

					break;
				}
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
		teamPlayer = whichTeam;
		setTeam (whichTeam);
		SimpleController scriptMovement = GetComponent<SimpleController> ();
		scriptMovement.team = whichTeam;
	}

	[Command]
	void CmdSetUser(string user)//float rx, float ry, float rz
	{
		userPlayer = user;
	}

	[ClientRpc]
	void RpcMatchMaking(int team){
		if (isLocalPlayer) {
			teamPlayer = team;
			print ("RPCLOCAL" + teamPlayer);
            assignTeam();
        }
	}

	//private bool setObjectsFind = true;

	// Use this for initialization
	void Start () {
        GameObject net = GameObject.Find("ControllerNet");
        CustomNetworkManagerHUD netScript = net.GetComponent<CustomNetworkManagerHUD>();
        ControllerNet controllerNetScript = net.GetComponent<ControllerNet>();

        if (controllerNetScript.matchmaking) {
			if (isServer) {
				teamPlayer = GameObject.Find ("Lobby").GetComponent<Lobby> ().matchmaker ();
				//CmdSetTeam (teamPlayer);
				RpcMatchMaking(teamPlayer);
				print ("TEAMPLAYER:" + teamPlayer);
			}
		}

        assignTeam();
	}

    public void assignTeam()
    {
        GameObject net = GameObject.Find("ControllerNet");
        CustomNetworkManagerHUD netScript = net.GetComponent<CustomNetworkManagerHUD>();
        ControllerNet controllerNetScript = net.GetComponent<ControllerNet>();

        if (isLocalPlayer)
        {
            //if (setObjectsFind) {
            if (net != null)
            {
                if (!controllerNetScript.matchmaking)
                {
                    teamPlayer = netScript.team;
                }

                print("TEAM:" + teamPlayer);

                vehicleTypeClass = netScript.classType;
                userPlayer = netScript.player;

                setTeam(teamPlayer);
                setClass(vehicleTypeClass);

                CmdSetTeam(teamPlayer);
                CmdSetClass(vehicleTypeClass);

                CmdSetUser(userPlayer);

                ammoGUI = GameObject.Find("Ammo");

                if (teamPlayer == 0)
                {
                    ammoGUI.GetComponent<RawImage>().texture = textureTeam0;
                }
                else if (teamPlayer == 1)
                {
                    ammoGUI.GetComponent<RawImage>().texture = textureTeam1;
                }

                weaponGUI = GameObject.Find("Weapon");

                if (teamPlayer == 0)
                {
                    weaponGUI.GetComponent<RawImage>().texture = textureTeam0;
                }
                else if (teamPlayer == 1)
                {
                    weaponGUI.GetComponent<RawImage>().texture = textureTeam1;
                }

                scoreGUI = GameObject.Find("Score");

                if (teamPlayer == 0)
                {
                    scoreGUI.GetComponent<RawImage>().texture = textureTeam0Score;
                }
                else if (teamPlayer == 1)
                {
                    scoreGUI.GetComponent<RawImage>().texture = textureTeam1Score;
                }

                scoreBottomGUI = GameObject.Find("ScoreBottom");

                if (teamPlayer == 0)
                {
                    scoreBottomGUI.GetComponent<RawImage>().texture = textureTeam0Score;
                }
                else if (teamPlayer == 1)
                {
                    scoreBottomGUI.GetComponent<RawImage>().texture = textureTeam1Score;
                }

                //setObjectsFind = false;
            }
            //}
        }
    }

	//private bool haveToSetClass = true;

	private bool needSpawn = true;

	// Update is called once per frame
	void Update () {
		/*if (haveToSetClass) {
			//setClass (typeClass);
			setSkinModel(vehicleTypeClass);

			haveToSetClass = false;
		}*/


		setSkinModel(vehicleTypeClass);
		setTeam (teamPlayer);

		if (needSpawn && GameObject.Find ("ControllerNet").GetComponent<ControllerNet> ().canPlay (true)) {
			needSpawn = false;

			SpawnPoints respawn = GetComponent<SpawnPoints> ();
			respawn.respawn ();
		}
	}
}
