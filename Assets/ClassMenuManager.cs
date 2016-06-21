using UnityEngine;
using System.Collections;

public class ClassMenuManager : MonoBehaviour {

    public GameObject drifterMesh;
    public GameObject minerMesh;
    public GameObject camperMesh;

    private GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas = gameObject;
        drifterMesh.SetActive(false);
        minerMesh.SetActive(false);
        camperMesh.SetActive(false);
    }
}
