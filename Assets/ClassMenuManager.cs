using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClassMenuManager : MonoBehaviour {

    public GameObject drifterMesh;
    public GameObject minerMesh;
    public GameObject camperMesh;
    public Image arrowRight;
    public Image arrowLeft;
    public Sprite arrowLeft_glow;
    public Sprite arrowRight_glow;
    public Image health;
    public Image speed;
    public RawImage hex_firstWeapon;       //hexes contains the logos and ammo information
    public RawImage hex_secondWeapon;
    public RawImage hex_specialAbility;
    public Text healthText;
    public Text speedText;
    public Text classText;
    public Text shortDescription;
    public Text LblSpecialAbility;

    public Sprite shotSprite;
    public Sprite mineSprite;
    public Sprite laserSprite;
    public Sprite driftSprite;
    public Sprite bombSprite;
    public Sprite bazookaSprite;
    public Sprite chainSprite;
    public Sprite shieldSprite;
    public Sprite needlesSprite;
    public Sprite rotatingBladesSprite;
    public Sprite poisonSprite;
    public Sprite flameThrowerSprite;
    public Sprite cureSprite;
    public Sprite driftingSprite;
    public Sprite camperSprite;
    public Sprite medicalSprite;

    public bool host;

    private GameObject canvas;
    private GameObject activeMesh;
    private string[] descriptions = new string[7];
    private string[] classNames = new string[7];
    private bool axisInUse;
    private int currentConfiguration;
    private const float AXIS_THRESHOLD = 0.3f;
    private const int NUMBER_OF_CONFIGURATIONS = 7; 

    // Use this for initialization
    void Start () {
        canvas = gameObject;
        axisInUse = false;
        currentConfiguration = 0;

        classNames[0] = "Drifter";
        classNames[1] = "Miner";
        classNames[2] = "Camper";
        classNames[3] = "Medical";
        classNames[4] = "Rammer";
        classNames[5] = "Chainer";
        classNames[6] = "Defender";

        descriptions[0] = "Hit the enemies drifting, with the machine gun or sideblades if the enemy comes up beside.";
        descriptions[1] = "Hide mines everywhere to trap the opponent or throw the bombs with a catapult.";
        descriptions[2] = "Low but strong, knock down the enemy with a bazooka or a laser cannon, choosing the camping modality.";
        descriptions[3] = "Cure your mates or poison your opponents, following and supporting the team.";
        descriptions[4] = "Fast and strong, you could break down the enemy only colliding, or burn through a flame thrower.";
        descriptions[5] = "Enchain the enemy to attack him directly with the needles on your car body.";
        descriptions[6] = "Protect you and your mate with the shield, throwing bombs around the arena.";

        ShowConfiguration(currentConfiguration);
    }

    void FixedUpdate()
    {
        activeMesh.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!axisInUse)
            {
                if (Input.GetAxisRaw("Horizontal") < -AXIS_THRESHOLD)
                {
                    previousConfiguration();
                    //TODO: make leftArrow blink
                }
                else if (Input.GetAxisRaw("Horizontal") > AXIS_THRESHOLD)
                {
                    nextConfiguration();
                    //TODO: make rightArrow blink
                }
                axisInUse = true;
            }
        }
        else
            axisInUse = false;

        if (Input.GetButtonDown("XboxB"))
        {
            if(host)
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().ReturnToPrivateGameMenu();
            else
                GameObject.Find("Cnvs_join").GetComponent<JoinManager>().WakeUp();
        }
        if (Input.GetButtonDown("XboxA"))
            print("Let's play");

    }

    private void nextConfiguration()
    {
        currentConfiguration = (++currentConfiguration) % NUMBER_OF_CONFIGURATIONS;
        ShowConfiguration(currentConfiguration);
    }

    private void previousConfiguration()
    {
        currentConfiguration--;
        if (currentConfiguration < 0)
            currentConfiguration = NUMBER_OF_CONFIGURATIONS-1;
        ShowConfiguration(currentConfiguration);
    }

    private void ShowConfiguration(int configuration)
    {
        switch(configuration)
        {
            case 0:         //Drifter
                activateDrifter();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = shotSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = driftSprite;
                hex_specialAbility.GetComponentInChildren<Image>().sprite = driftingSprite;
                hex_firstWeapon.GetComponentInChildren<Text>().text = "130";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "-";
                hex_specialAbility.gameObject.SetActive(true);
                LblSpecialAbility.gameObject.SetActive(true);
                break;
            case 1:         //Miner
                activateMiner();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = mineSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = bombSprite;
                hex_firstWeapon.GetComponentInChildren<Text>().text = "10";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "12";
                hex_specialAbility.gameObject.SetActive(false);
                LblSpecialAbility.gameObject.SetActive(false);
                break;
            case 2:         //Camper
                activateCamper();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = bazookaSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = laserSprite;
                hex_specialAbility.GetComponentInChildren<Image>().sprite = camperSprite;
                hex_firstWeapon.GetComponentInChildren<Text>().text = "8";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "10";
                hex_specialAbility.gameObject.SetActive(true);
                LblSpecialAbility.gameObject.SetActive(true);
                break;
            case 3:         //Medical
                activateFake();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = cureSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = poisonSprite;
                hex_specialAbility.GetComponentInChildren<Image>().sprite = medicalSprite;
                hex_firstWeapon.GetComponentInChildren<Text>().text = "90";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "10";
                hex_specialAbility.gameObject.SetActive(true);
                LblSpecialAbility.gameObject.SetActive(true);
                break;
            case 4:         //Rammer
                activateFake();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = rotatingBladesSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = flameThrowerSprite;
                hex_firstWeapon.GetComponentInChildren<Text>().text = "-";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "90";
                hex_specialAbility.gameObject.SetActive(false);
                LblSpecialAbility.gameObject.SetActive(false);
                break;
            case 5:         //Chainer
                activateFake();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = chainSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = needlesSprite;
                hex_firstWeapon.GetComponentInChildren<Text>().text = "-";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "-";
                hex_specialAbility.gameObject.SetActive(false);
                LblSpecialAbility.gameObject.SetActive(false);
                break;
            case 6:         //Defender
                activateFake();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = shieldSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = bombSprite;
                hex_firstWeapon.GetComponentInChildren<Text>().text = "-";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "12";
                hex_specialAbility.gameObject.SetActive(false);
                LblSpecialAbility.gameObject.SetActive(false);
                break;
        }
        shortDescription.text = descriptions[configuration];
        classText.text = classNames[configuration];
    }

    private void activateDrifter()
    {
        drifterMesh.SetActive(true);
        minerMesh.SetActive(false);
        camperMesh.SetActive(false);
        //fakeMesh.SetActive(false);

        activeMesh = drifterMesh;
    }

    private void activateMiner()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(true);
        camperMesh.SetActive(false);
        //fakeMesh.SetActive(false);

        activeMesh = minerMesh;
    }

    private void activateCamper()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(false);
        camperMesh.SetActive(true);
        //fakeMesh.SetActive(false);

        activeMesh = camperMesh;
    }

    private void activateFake()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(false);
        camperMesh.SetActive(false);

        //fakeMesh.SetActive(true);
    }
}
