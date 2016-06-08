using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour 
{
	//public Transform target;
	//public float distance = 10.0f;
	//public float smoothingValue = 5.0f;
	//public bool direct = true;

    // The target we are following
	public SimpleController target;
    // The distance in the x-z plane to the target
    public float distance = 7.0f;
    // the height we want the camera to be above the target
    public float height = -3.0f;
    // How much we 
	public float followDamping = 0.01f;
   // public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

   // public bool debug = false;

	//private bool first = true;
    //private SimpleController scriptTarget;

    void Start()
	{
        //if(target)
           // scriptTarget = target.GetComponent<SimpleController>();
    }

	void FixedUpdate()
	{
		if(target)
		{
            /*Vector3 targetPosition = target.position - transform.forward * distance + transform.up * 2;
			if (direct)
				transform.position = targetPosition;
			else //smoothing
				transform.position -= (transform.position - targetPosition) * smoothingValue * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(30, Vector3.right);*/


			/*if (first) {
				transform.position = target.transform.position;
				first = false;
			}
            // Calculate the current rotation angles
            Vector3 wantedRotationAngle = target.transform.eulerAngles;
			float wantedHeight = target.transform.position.y + height;
            Vector3 currentRotationAngle = transform.eulerAngles;
            float currentHeight = transform.position.y;
            // Damp the rotation around the y-axis
            currentRotationAngle = Vector3.Lerp(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            // Convert the angle into a rotation
            Quaternion currentRotation = Quaternion.Euler(currentRotationAngle);
            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.body.position;

			Vector3 myForward = target.myForward;

            transform.position -= currentRotation * myForward * distance;// 

            currentRotation = Quaternion.Euler(target.transform.rotation.eulerAngles);
            transform.rotation = currentRotation;

			//transform.LookAt (target.transform);

            //if (debug)
			//	print(target.forward + " " + target.transform.rotation.eulerAngles);
            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
            // Always look at the target
            //transform.LookAt(target);*/

			Vector3 velocity = Vector3.zero;//target.body.velocity;
			Vector3 forward = target.transform.forward * distance + target.transform.up * height;
			Vector3 needPos = target.transform.position - forward;
			transform.position = Vector3.SmoothDamp(transform.position, needPos, ref velocity, followDamping);
			//transform.LookAt (target.transform);
			transform.rotation = Quaternion.Lerp (transform.rotation, target.transform.rotation, Time.deltaTime * rotationDamping);
        }
	}
}
