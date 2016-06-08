using UnityEngine;
using System.Collections;

public class SpawnPickup : MonoBehaviour {
    public float startTimer = 5f;
    public GameObject pickupSpawned;

    private GameObject pickup = null;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        timer -= Time.fixedDeltaTime;

        if (timer < 0f)
            timer = 0f;
    }

    void Update()
    {
        if(timer <= 0f)
        {
            if(pickup == null)
            {
                pickup = (GameObject)Instantiate(pickupSpawned, transform.position, transform.rotation);

                timer = startTimer;
            }
        }
    }
}
