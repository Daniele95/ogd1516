using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmmoPickupBehaviour : MonoBehaviour {
	public int numBulletsPickup = 5;

	private Text text;
    private float speedRotation = 10f;

    // Use this for initialization
    void Start () {
		text = this.GetComponentInChildren<Text> ();

		text.text = "Ammo x " + numBulletsPickup.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, Time.deltaTime * speedRotation, 0f);
    }
}
