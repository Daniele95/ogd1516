using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GuiVehicle : NetworkBehaviour {
	//public bool Player = true;
	[SyncVar(hook = "OnChangeHealth")]
	public int life = 100;

	[SyncVar]
	public int maxLife = 130;
    public GameObject Drifter;
    public GameObject Camper;
    public GameObject Miner;

    //public float timerRespawnHit = 1f;

    //private int whichTeam = -1;
    //private Rigidbody body;

    private Text text;
	private Image healthRect;

	public GameObject explosionSound;
	public GameObject explosion;

	public float startTimerRespawn = 5f;
	public float timerRespawn;

    private GameObject lifeLoader;

	private LoaderClass loaderScript;

	private GameObject cameraObject;

	private SimpleController scriptMovement;

    public GameObject respawnSoundGameObject;


    [Command]
	void CmdDoExplosionRespawn(){
		GameObject shotExplosion = (GameObject)Instantiate (explosion, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosion);

        GameObject respawnSound = (GameObject)Instantiate(respawnSoundGameObject, transform.position, transform.rotation);

        NetworkServer.Spawn(respawnSound);
    }

	[Command]
	void CmdDoExplosionSound(){
		GameObject shotExplosionSound = (GameObject)Instantiate (explosionSound, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosionSound);
	}

	[ClientRpc]
	void RpcDamage(int amount)
	{
		
	}

	[ClientRpc]
	void RpcRespawn(){
		if (isLocalPlayer) {
			SpawnPoints respawn = GetComponent<SpawnPoints> ();
			respawn.respawn ();
		}
	}

	void OnChangeHealth (int health)
	{
		life = health;

		if (healthRect != null) {
			healthRect.transform.localScale = new Vector3 ((life / (float)maxLife), 1f, 1f);

			if (health / (float)maxLife < 0.3f) {
				healthRect.color = Color.red;
			} else {
				healthRect.color = Color.green;
			}
		}

		//print (life + " " + maxLife);
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		if (life > 0) {

			life -= amount;

			if (life <= 0) {
				life = 0;

				CmdDoExplosionSound ();

				CmdDoExplosionRespawn ();

				scriptMovement.inTunnel = 0;

				int whichTeam;

				if (gameObject.CompareTag ("VehicleTeam0")) {
					whichTeam = 0;
				} else {
					whichTeam = 1;
				}

				ControllerGaming controller = GameObject.Find ("ControllerGame").GetComponent<ControllerGaming> ();
				controller.addScoreTeam (whichTeam);   //add score if the player is dead 

				RpcRespawn ();    //do the respawn
			}

			RpcDamage (amount);
		}
	}

	[Command]
	void CmdDoDriftDamage(int amount)//float rx, float ry, float rz
	{
		TakeDamage (amount);
	}

	public void TakeDamageDrift(int amount)
	{
		//life -= amount;
		CmdDoDriftDamage (amount);
	}

	// Use this for initialization
	void Start () {
		
        //whichTeam = gameObject.CompareTag ("VehicleTeam0") ? 0 : 1;

        //body = GetComponent<Rigidbody> ();
        Transform user = transform.FindChild("HUD").FindChild("User");
		text = user.GetComponent<Text> ();
		Transform lifeBar = transform.FindChild ("HUD").FindChild("User").FindChild ("HUDLife");
		healthRect = lifeBar.FindChild("Life").GetComponent<Image> ();
		timerRespawn = startTimerRespawn;

        loaderScript = GetComponent<LoaderClass> ();

		scriptMovement = GetComponent<SimpleController> ();

		cameraObject = GameObject.Find ("MainCamera");

		if (isLocalPlayer) {
			user.gameObject.SetActive (false);
			//lifeBar.gameObject.SetActive (false);
		
			//if (setLifeBar) {
			Drifter = GameObject.Find ("Life31");
			Camper = GameObject.Find ("Life17");
			Miner = GameObject.Find ("Life");
			

			if (loaderScript.vehicleTypeClass == 1) {                    //if it's a miner
					Miner.SetActive (true);
					Drifter.SetActive (false);
					Camper.SetActive (false);
					lifeLoader = GameObject.Find ("LifeLoader");     //takes the object LifeLoader

				} else if (loaderScript.vehicleTypeClass == 0) {                //if it's a drifter
					Drifter.SetActive (true);
					Miner.SetActive (false);
					Camper.SetActive (false);
					lifeLoader = GameObject.Find ("LifeLoader31");    //takes the object LifeLoader31

				} else if (loaderScript.vehicleTypeClass == 2) {                //if it's a camper
					Camper.SetActive (true);
					Drifter.SetActive (false);
					Miner.SetActive (false);
					lifeLoader = GameObject.Find ("LifeLoader17");    //takes the object LifeLoader17

				}

				//setLifeBar = false;
			//}
		}
	}

	void OnCollisionEnter(Collision col){
		/*if (col.gameObject.CompareTag ("HealthPickup")) {
            if (life < maxLife)
            {
                HealthPickupBehaviour pickup = (HealthPickupBehaviour)col.gameObject.GetComponent<HealthPickupBehaviour>();
                life += pickup.healthPickup;

                if (life > maxLife)
                    life = maxLife;

                //Destroy (col.gameObject);

                pickup.getPickup = true;
            }
		}*/
	}

	void OnGUI()
	{
		//if (!isLocalPlayer)
	//		return;

		/*int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h);

		//if (!Player)
		//	rect.Set (w / 2f, 0, w / 2f, h);

		style.alignment = TextAnchor.UpperRight;
		style.fontSize = h * 5 / 100;
		style.normal.textColor = Color.white;

		string textGUI = "Life: " + life.ToString();
		GUI.Label(rect, textGUI, style);*/

		if (isLocalPlayer) { 
			if (lifeLoader != null) {
				lifeLoader.GetComponent<Image> ().fillAmount = life / (float)maxLife;      //fills current life of the player

				lifeLoader.GetComponent<Image> ().color = Color.Lerp (Color.red, Color.green, life / (float)maxLife);
			}
		}

		text.transform.LookAt (cameraObject.transform, scriptMovement.myNormal);
		//healthRect.transform.LookAt (camera.transform, scriptMovement.myNormal);
		//text.transform.rotation = Quaternion.Euler (0f, text.transform.rotation.eulerAngles.y, 0f);

		text.text = loaderScript.userPlayer;
		text.color = loaderScript.teamColor;
	}

	//private bool setLifeBar = true;

	// Update is called once per frame
	void Update () {
		//text.transform.LookAt (Camera.main.transform);
		//healthRect.canvas.transform.LookAt(Camera.main.transform);
		//if (life <= 0) {
		//	life = 100;

			/*SpawnPoints respawn = GetComponent<SpawnPoints> ();
			respawn.respawn ();

			ControllerGame controller = GameObject.Find ("ControllerGame").GetComponent<ControllerGame> ();
			controller.addScoreTeam (whichTeam);*/
		//}



		if (life <= 0) {
			timerRespawn -= Time.deltaTime;

			if (timerRespawn <= 0f) {
				timerRespawn = startTimerRespawn;

				life = maxLife;
			}
		}

        if (life < maxLife)
        {
            GameObject[] pickups = GameObject.FindGameObjectsWithTag("HealthPickup");

            for (int i = 0; i < pickups.Length; i++)
            {
                HealthPickupBehaviour pickup = (HealthPickupBehaviour)pickups[i].GetComponent<HealthPickupBehaviour>();

                if (Vector3.Distance(pickups[i].transform.position, transform.position) <= pickup.RADIUS_PICKUP)
                {
                    life += pickup.healthPickup;

                    if (life > maxLife)
                        life = maxLife;

                    break;
                }
            }
        }
    }
}
