﻿using UnityEngine;
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

		healthRect.transform.localScale = new Vector3((health / (float)maxLife), 1f, 1f);

		if (health/(float)maxLife < 0.3f) {
			healthRect.color = Color.red;
		} else {
			healthRect.color = Color.green;
		}

		//print (life + " " + maxLife);
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		life -= amount;

		if (life <= 0) {
			life = maxLife;

			RpcRespawn ();

			int whichTeam;

			if (gameObject.CompareTag ("VehicleTeam0")) {
				whichTeam = 0;
			} else {
				whichTeam = 1;
			}

			ControllerGaming controller = GameObject.Find ("ControllerGame").GetComponent<ControllerGaming> ();
			controller.addScoreTeam (whichTeam);
		}

		RpcDamage(amount);
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

		text = this.GetComponentInChildren<Text> ();
		healthRect = this.GetComponentInChildren<Image> ();
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
		if (!isLocalPlayer)
			return;

		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h);

		//if (!Player)
		//	rect.Set (w / 2f, 0, w / 2f, h);

		style.alignment = TextAnchor.UpperRight;
		style.fontSize = h * 5 / 100;
		style.normal.textColor = Color.white;

		string textGUI = "Life: " + life.ToString();
		GUI.Label(rect, textGUI, style);

		text.text = "Player"; 
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
	}
}