using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GuiVehicle : NetworkBehaviour {
	//public bool Player = true;
	[SyncVar(hook = "OnChangeHealth")]
	public int life = 100;

	public int maxLife = 100;

	//public float timerRespawnHit = 1f;

	//private int whichTeam = -1;
	//private Rigidbody body;

	private Text text;
	private Image healthRect;

	public GameObject explosion;

	public float startTimerRespawn = 5f;
	public float timerRespawn;

	private GameObject lifeLoader;

	private LoaderClass loaderScript;

	private GameObject camera;

	private SimpleController scriptMovement;

	[Command]
	void CmdDoExplosionRespawn(){
		GameObject shotExplosion = (GameObject)Instantiate (explosion, transform.position, transform.rotation);

		NetworkServer.Spawn (shotExplosion);
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
			healthRect.transform.localScale = new Vector3 ((health / (float)maxLife), 1f, 1f);

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

				CmdDoExplosionRespawn ();

				scriptMovement.inTunnel = 0;

				int whichTeam;

				if (gameObject.CompareTag ("VehicleTeam0")) {
					whichTeam = 0;
				} else {
					whichTeam = 1;
				}

				ControllerGaming controller = GameObject.Find ("ControllerGame").GetComponent<ControllerGaming> ();
				controller.addScoreTeam (whichTeam);

				RpcRespawn ();
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

		text = transform.FindChild("HUD").FindChild("User").GetComponent<Text> ();
		healthRect = this.GetComponentInChildren<Image> ();
		timerRespawn = startTimerRespawn;

		lifeLoader = GameObject.Find ("LifeLoader");

		loaderScript = GetComponent<LoaderClass> ();

		camera = GameObject.Find ("MainCamera");

		scriptMovement = GetComponent<SimpleController> ();
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag ("HealthPickup")) {
			HealthPickupBehaviour pickup = (HealthPickupBehaviour)col.gameObject.GetComponent<HealthPickupBehaviour> ();
			life += pickup.healthPickup;

			if (life > maxLife)
				life = maxLife;

			Destroy (col.gameObject);
		}
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

		lifeLoader.GetComponent<Image> ().fillAmount = life / (float)maxLife;

		text.transform.LookAt (camera.transform, scriptMovement.myNormal);
		//text.transform.rotation = Quaternion.Euler (0f, text.transform.rotation.eulerAngles.y, 0f);

		text.text = loaderScript.userPlayer; 
	}

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
	}
}
