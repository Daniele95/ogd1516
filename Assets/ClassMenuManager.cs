using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClassMenuManager : MonoBehaviour {

    public GameObject drifterMesh;
    public GameObject minerMesh;
    public GameObject camperMesh;
    public Image arrowRight;
    public Image arrowLeft;
    public Image health;
    public Image speed;
    public Image hex_firstWeapon;       //hexes contains the logos
    public Image hex_secondWeapon;
    public Image hex_specialAbility;
    public Text healthText;
    public Text speedText;
    public Text classText;
    public Text shortDescription;

    private GameObject canvas;
    public GameObject activeMesh;

    // Use this for initialization
    void Start () {
        canvas = gameObject;
        drifterMesh.SetActive(true);
        minerMesh.SetActive(false);
        camperMesh.SetActive(false);

        activeMesh = drifterMesh;
    }

    void FixedUpdate()
    {
        activeMesh.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }

    private void activateDrifter()
    {
        drifterMesh.SetActive(true);
        minerMesh.SetActive(false);
        camperMesh.SetActive(false);

        activeMesh = drifterMesh;
    }

    private void activateMiner()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(true);
        camperMesh.SetActive(false);

        activeMesh = minerMesh;
    }

    private void activateCamper()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(false);
        camperMesh.SetActive(true);

        activeMesh = camperMesh;
    }
}
