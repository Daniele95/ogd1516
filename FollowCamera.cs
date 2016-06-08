using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour 
{
	public Transform target;
	public float distance = 10.0f;
	public float smoothingValue = 5.0f;
	public bool direct = true;

	void Start()
	{

	}

	void FixedUpdate()
	{
		if(target)
		{
			Vector3 targetPosition = target.position - transform.forward * distance;
			if (direct)
				transform.position = targetPosition;
			else //smoothing
				transform.position -= (transform.position - targetPosition) * smoothingValue * Time.deltaTime;
		}
	}
}
