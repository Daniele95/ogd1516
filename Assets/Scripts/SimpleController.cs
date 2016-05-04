using UnityEngine;
using System.Collections;

public class SimpleController : MonoBehaviour
{
	public bool player = true;
    public float acceleration = 40;
    public float accelerationSteer = 40;
    public float wheelBase;
    public float GRAVITY = 9.81f;
    public float FRICTION = 4f;
    public float STEER_FRICTION = 4f;
    public float deltaGround = 0.2f;
    public float lerpSpeed = 2f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 input;

    private float carHeading;
    private float carSpeed;
    private float steerAngle;

    private Vector3 myNormal;
    private Vector3 surfaceNormal;

    private Rigidbody body;
    private Collider carCollider;

    //private float distGround;
    private bool isGrounded;

    private Vector3 frontWheel;
    private Vector3 backWheel;
    private Ray ray;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();
        carCollider = GetComponent<Collider>();

        myNormal = transform.up;

        frontWheel = new Vector3();
        backWheel = new Vector3();

        ray = new Ray();
    }

    void FixedUpdate()
    {
        body.AddForce(-GRAVITY * myNormal, ForceMode.Acceleration);
        body.MovePosition(body.position + velocity * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        GetInput();

		velocity -= velocity * FRICTION * Time.deltaTime;
        velocity += input.z * transform.forward * acceleration * Time.deltaTime;

        steerAngle += accelerationSteer * Time.deltaTime;
        steerAngle -= steerAngle * STEER_FRICTION * Time.deltaTime;
        
        frontWheel.Set((wheelBase / 2 * Mathf.Cos(carHeading) + Mathf.Cos(carHeading + steerAngle)) * carSpeed, 0f, (wheelBase / 2 * Mathf.Sin(carHeading) + Mathf.Sin(carHeading + steerAngle)) * carSpeed);
        backWheel.Set((-wheelBase / 2 * Mathf.Cos(carHeading) + Mathf.Cos(carHeading)) * carSpeed, 0f, (wheelBase / 2 * Mathf.Sin(carHeading * carSpeed) + Mathf.Cos(carHeading)) * carSpeed);

        carHeading = Mathf.Atan2(frontWheel.z - backWheel.z, frontWheel.x - backWheel.x);

        // movement code - turn left/right with Horizontal axis:
        //transform.position += velocity * Time.deltaTime;
        transform.Rotate(0, input.x * steerAngle * Mathf.Rad2Deg, 0);

        // update surface normal and isGrounded:

        RaycastHit hit;

        ray.origin = transform.position;
        ray.direction = -myNormal; // cast ray downwards
        if (Physics.Raycast(ray, out hit))
        { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= deltaGround; //distGround + 
            //print(hit.distance + " " + (deltaGround) + " " + isGrounded);
            surfaceNormal = hit.normal;
        }
        else {
            isGrounded = false;
            // assume usual ground normal to avoid "falling forever"
            surfaceNormal = Vector3.up;
        }

        //print(isGrounded);

        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        var myForward = Vector3.Cross(transform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:

        var targetRot = Quaternion.LookRotation(myForward, myNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);

        body.angularVelocity = Vector3.zero;

        Debug.DrawRay(transform.position, transform.TransformDirection(myForward) * 10f, Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(transform.right) * 10f, Color.green);
        Debug.DrawRay(transform.position, transform.TransformDirection(myNormal) * 10f, Color.blue);
    }

    void GetInput()
    {
        //get the input
        input = Vector3.zero;
		if (!player) {
			if (Input.GetKey (KeyCode.RightArrow))
				input.x += 1.0f;
			if (Input.GetKey (KeyCode.LeftArrow))
				input.x -= 1.0f;
			if (Input.GetKey (KeyCode.UpArrow))
				input.z += 1.0f;
			if (Input.GetKey (KeyCode.DownArrow))
				input.z -= 1.0f;
		} else {
			if (Input.GetKey (KeyCode.D))
				input.x += 1.0f;
			if (Input.GetKey (KeyCode.A))
				input.x -= 1.0f;
			if (Input.GetKey (KeyCode.W))
				input.z += 1.0f;
			if (Input.GetKey (KeyCode.S))
				input.z -= 1.0f;
		}
        //make sure the input doesn't exceed 1 if we go diagonally
        if (input != Vector3.zero)
            input.Normalize();
    }
}
