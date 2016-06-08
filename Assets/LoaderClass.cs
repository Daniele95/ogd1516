using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoaderClass : NetworkBehaviour {
	public GameObject mineShoot;
	public GameObject machineGunShoot;
	public GameObject laserShot;
	public GameObject bazookaShot;
	public GameObject bombShot;

	private int typeClass = 1;

	// Use this for initialization
	void Start () {
		SimpleController scriptMovement = GetComponent<SimpleController> ();
		GuiVehicle scriptGUI = GetComponent<GuiVehicle> ();
		Shooting scriptShooting = GetComponent<Shooting> ();

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
	
	// Update is called once per frame
	void Update () {
	
	}
}
