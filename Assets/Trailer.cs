using UnityEngine;
using System.Collections;

public class Trailer : MonoBehaviour {
    public GameObject explosion;
    private GameObject player;
    private bool doExplosion = true;
    public float radius = 15f;
    //public GameObject positionGameObject;

	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update() {
        player = GameObject.FindGameObjectWithTag("VehicleTeam0");

        if (player != null)
        {
            if (doExplosion)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Vector3.Distance(player.transform.position, transform.position) < radius)
                {
                    doExplosion = false;
                    explosion = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
                }
            }
        }
	}
}
