using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SimpleController : NetworkBehaviour
{
	[SyncVar]
	public int team = 0;

	//public bool drifting = false;

    public float acceleration = 20000f;
	//public float MAX_SPEED = 10000f;
    public float accelerationSteer = 40f;
	public float driftFrictionSteer = 0.5f;
	public float driftFriction = 0.05f;

    public float wheelBase = 0f;
    public float GRAVITY = 9.81f;
    public float FRICTION = 5f;
    public float STEER_FRICTION = 12f;
    public float deltaGround = 2f;
    public float lerpSpeed = 10f;
	public float lerpSpeedQuaternion = 20f;

	public int specialPower = 0;

	//[SyncVar(hook = "OnChangeDrift")]
	public bool isDriftingDamage = false;

	public bool isCamping = false;

	public int damageDrift = 0;

	public GameObject explosionDrift;

	//public float hoverUpForce = 0.1f;
	//public float hoverDownForce = 0.1f;

	//public bool debug = false;
	
    private Vector3 myForward;
	private Quaternion targetRot;

	private Shooting shooting;

    private Vector3 velocity = Vector3.zero;
    private Vector3 input;

    private float carHeading;
    private float steerAngle;

    private Vector3 myNormal;
    private Vector3 surfaceNormal;

	public Rigidbody body;

    private int isGrounded = 3;

    //private Vector3 frontWheel;
    //private Vector3 backWheel;
    private Ray ray;

	private Vector3 dirDrift;

	public int inTunnel = 0;

	public GameObject wayPointList;
	public int currentWayPoint = 0; 
	public GameObject targetWayPoint;
	public float speed = 4f;
	public float accSpeed = 0f;
	public bool reverseTunnel = false;

	private GuiVehicle gui;

	[Command]
	void CmdDoExplosionHitDrift(){
		GameObject driftHitExplosion = (GameObject)Instantiate (explosionDrift, transform.position, transform.rotation);

		NetworkServer.Spawn (driftHitExplosion);
	}

	void OnCollisionEnter(Collision col){
		//if (!isLocalPlayer)
		//	return;

		if (col.gameObject.CompareTag ("VehicleTeam0") || col.gameObject.CompareTag ("VehicleTeam1")) {
			SimpleController script = col.gameObject.GetComponent<SimpleController> ();
			//print ("dr" + script.isDriftingDamage);
			if (script.isDriftingDamage) {
				if (col.gameObject.CompareTag ("VehicleTeam0") && gameObject.CompareTag ("VehicleTeam1") || col.gameObject.CompareTag ("VehicleTeam1") && gameObject.CompareTag ("VehicleTeam0")) {
					//print("damage drifting");

					gui.TakeDamageDrift (damageDrift);

					CmdDoExplosionHitDrift ();
				}
			}
		}
	}

    void Start()
    {
        body = GetComponent<Rigidbody>();

        myNormal = transform.up;

		gui = gameObject.GetComponent<GuiVehicle> ();

        //frontWheel = new Vector3();
        //backWheel = new Vector3();

        ray = new Ray();

		shooting = GetComponent<Shooting> ();

		if (isLocalPlayer) {
			GameObject camera = GameObject.Find ("MainCamera");
			FollowCamera script = camera.GetComponent<FollowCamera> ();
			script.target = this;
		}

		/*if (team == 0) {
			GetComponent<MeshRenderer> ().material.color = Color.blue;
			gameObject.tag = "VehicleTeam0";
		} else if(team == 1){
			GetComponent<MeshRenderer> ().material.color = Color.red;
			gameObject.tag = "VehicleTeam1";
		}*/
    }

    //private float cost = 0f;
   // private bool needHover = true;

   	void FixedUpdate()
    {
		if (!isLocalPlayer)
			return;
    }

	void standardUpdate(){
		CmdIsDoingCamping (false);
		CmdIsDoingDrift (false);

		steerAngle += input.x * accelerationSteer * Mathf.Rad2Deg * Time.deltaTime;// *input.z
		steerAngle -= steerAngle * STEER_FRICTION * Time.deltaTime;

		transform.Rotate(0, steerAngle * Time.deltaTime, 0);

		dirDrift = myForward;

		//if (inTunnel == 0) {
		if (accSpeed > 0f) {
			velocity = accSpeed * 10f * myForward * Time.deltaTime;
			body.AddForceAtPosition (velocity, body.position + transform.up * 0.5f + transform.forward * 0f, ForceMode.Impulse);
			accSpeed = 0f;
		} else {
			velocity = input.z * myForward * acceleration * Time.deltaTime;
		}

		velocity = Vector3.Lerp (velocity, Vector3.zero, Time.deltaTime * FRICTION * isGrounded);

		body.velocity = Vector3.Lerp (body.velocity, Vector3.zero, Time.deltaTime * FRICTION * isGrounded);
		//}

		body.AddForceAtPosition (velocity * isGrounded, body.position + transform.up * 0.5f + transform.forward * 0f, ForceMode.Force);
	}

	void walk(){
		// rotate towards the target
		myForward = Vector3.RotateTowards(transform.forward, targetWayPoint.transform.position - body.position, speed*Time.deltaTime, 0.0f);

		accSpeed += speed * Time.deltaTime;
		//myForward = transform.forward;

		// move towards the target
		transform.position = Vector3.MoveTowards(body.position, targetWayPoint.transform.position,   speed*Time.deltaTime);

		if(transform.position == targetWayPoint.transform.position)
		{
			if (reverseTunnel) {
				currentWayPoint--;

				if(currentWayPoint >= 0)
					targetWayPoint = wayPointList.transform.GetChild(currentWayPoint).gameObject;
			} else {
				currentWayPoint++;

				if(currentWayPoint < this.wayPointList.transform.childCount)
					targetWayPoint = wayPointList.transform.GetChild(currentWayPoint).gameObject;
			}
		}
	}

	void Update()
    {
		RaycastHit hit;

		ray.origin = body.position;
		ray.direction = -myNormal;
		if (Physics.Raycast(ray, out hit, deltaGround))
		{
			isGrounded = hit.distance <= deltaGround ? 1 : 0;
			surfaceNormal = hit.normal;

			//print (hit.collider.gameObject.name);
		}
		else {
			isGrounded = 0;
			surfaceNormal = transform.up;
			//print ("0");
		}

		Vector3 velocityRef = Vector3.zero;
		myNormal = Vector3.SmoothDamp(myNormal, surfaceNormal, ref velocityRef, 0.005f);

		if(isGrounded == 0)
			myNormal = Vector3.Lerp(myNormal, Vector3.up, lerpSpeed * 0.5f * Time.deltaTime);

		if (inTunnel == 0) {
			myForward = Vector3.Cross (transform.right, myNormal);
			Vector3 realGravity = -GRAVITY * myNormal;

			if (isGrounded == 0) {
				body.AddForce(realGravity * Time.deltaTime, ForceMode.Acceleration);
			}
		}

		targetRot = Quaternion.LookRotation(myForward, myNormal);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeedQuaternion * Time.deltaTime);

		if (!isLocalPlayer)
			return;

		if (gui.life > 0) {
			GetInput ();
		}

		if (inTunnel == 1) {
			steerAngle = 0f;

			if (reverseTunnel) {
				if (targetWayPoint == null) {
					targetWayPoint = wayPointList.transform.GetChild (wayPointList.transform.childCount - 3).gameObject;
					currentWayPoint = wayPointList.transform.childCount - 3;
				}

				if (currentWayPoint >= 0) {
					walk ();
				}else
					inTunnel = 0;
			} else {
				if (targetWayPoint == null) {
					targetWayPoint = wayPointList.transform.GetChild (2).gameObject;
					currentWayPoint = 2;
				}
		
				if (currentWayPoint < wayPointList.transform.childCount) {
					walk ();
				} else
					inTunnel = 0;
			}
		} else {
			if (specialPower == 2) {
				//if (input.y > 0f) {// && body.velocity.magnitude > 1f
				if (isGrounded == 1) {
					if (isCamping) {
						CmdIsDoingCamping (true);

						shooting.currentWeapon = 1;

						steerAngle += input.x * accelerationSteer * driftFrictionSteer * Mathf.Rad2Deg * Time.deltaTime;// *input.z
						steerAngle -= steerAngle * STEER_FRICTION * Time.deltaTime;

						transform.Rotate (0, steerAngle * Time.deltaTime, 0);

						velocity = Vector3.zero;

						body.velocity = Vector3.zero;

						//print ("CAMPING");
					} else {
						standardUpdate ();	

						shooting.currentWeapon = 0;

						CmdIsDoingCamping (false);

						//print ("ENDCAMPING");
					}
				} else {
					standardUpdate ();
				}
				//}
			} else if (specialPower == 1) {
				if (input.y > 0f && body.velocity.magnitude > 1f) {
					if (shooting.currentWeapon == 1)
						CmdIsDoingDrift (true);

					steerAngle += input.x * accelerationSteer * driftFrictionSteer * Mathf.Rad2Deg * Time.deltaTime;// *input.z
					steerAngle -= steerAngle * STEER_FRICTION * Time.deltaTime;

					transform.Rotate (0, steerAngle * Time.deltaTime, 0);

					velocity = input.z * dirDrift * acceleration * Time.deltaTime;

					velocity = Vector3.Lerp (velocity, Vector3.zero, Time.deltaTime * driftFriction * FRICTION * isGrounded);

					body.velocity = Vector3.Lerp (body.velocity, Vector3.zero, Time.deltaTime * driftFriction * FRICTION * isGrounded);
				} else {
					standardUpdate ();
				}
			} else {
				standardUpdate ();
			}
		}

		body.angularVelocity = Vector3.zero;

		//print(accSpeed);

		//print (input.y);
        //Debug.DrawRay(body.position, -myNormal * 10f, Color.red);
	}

	[Command]
	void CmdIsDoingDrift(bool drift)//float rx, float ry, float rz
	{
		isDriftingDamage = drift;
	}

	[Command]
	void CmdIsDoingCamping(bool camping)//float rx, float ry, float rz
	{
		isCamping = camping;
	}

    void GetInput()
    {
        input = Vector3.zero;
		if (Input.GetKey (KeyCode.RightArrow))
			input.x += 1.0f;
		if (Input.GetKey (KeyCode.LeftArrow))
			input.x -= 1.0f;
		if (Input.GetKey (KeyCode.UpArrow))
			input.z += 1.0f;
		if (Input.GetKey (KeyCode.DownArrow))
			input.z -= 1.0f;

		if (inTunnel == 0) {
			if (specialPower == 2) {
				if (Input.GetKeyDown (KeyCode.Z))
				if (isCamping)
					isCamping = false;
				else
					isCamping = true;
			} else {
				if (Input.GetKey (KeyCode.Z))
					input.y += 1.0f;
			}
		}

        if (input != Vector3.zero)
            input.Normalize();
    }
}
