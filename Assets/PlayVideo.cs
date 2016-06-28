using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayVideo : MonoBehaviour {
    public MovieTexture movTexture;

	// Use this for initialization
	void Start () {
	    GetComponent<Renderer>().material.mainTexture = movTexture;
        movTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (!movTexture.isPlaying)
        {
            SceneManager.LoadScene("Main menu");
        }
	}
}