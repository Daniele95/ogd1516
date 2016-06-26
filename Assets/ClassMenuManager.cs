using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ClassMenuManager : MonoBehaviour {

    public GameObject drifterMesh;
    public GameObject minerMesh;
    public GameObject camperMesh;
    public GameObject fakeMesh;
    public Image arrowRight;
    public Image arrowLeft;
    public Sprite arrowLeft_glow;
    public Sprite arrowRight_glow;
    public RawImage hex_firstWeapon;       //hexes contains the logos and ammo information
    public RawImage hex_secondWeapon;
    public RawImage hex_specialAbility;
    public Text healthText;
    public Text speedText;
    public Text classText;
    public Text shortDescription;
    public Text LblSpecialAbility;

    public string ipAddress = "127.0.0.1";

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

    public Image healthContainer;
    public Image speedContainer;
    public Sprite healthAverage;
    public Sprite speedAverage;
    public Sprite healthLow;
    public Sprite healthHigh;
    public Sprite speedLow;
    public Sprite speedHigh;

    public bool host;

    private GameObject canvas;
    private GameObject activeMesh;
    private string[] descriptions = new string[7];
    private string[] classNames = new string[7];
    private bool axisInUse;
    private int currentConfiguration;

    private Vector2 AVERAGE_PARAMETER_SIZE = new Vector2(277f, 69f);
    private Vector2 HIGH_PARAMETER_SIZE = new Vector2(369f, 69f);
    private Vector2 LOW_PARAMETER_SIZE = new Vector2(199f, 69f);

    private Vector3 AVERAGE_HEALTH_POSITION = new Vector3(151f,22.7f,0f);
    private Vector3 LOW_HEALTH_POSITION = new Vector3(112f, 22.7f,0f);
    private Vector3 HIGH_HEALTH_POSITION = new Vector3(194f,22.7f,0f);
    private Vector3 AVERAGE_SPEED_POSITION = new Vector3(151,-10.4f,0f);
    private Vector3 LOW_SPEED_POSITION = new Vector3(112f,-10.4f,0f);
    private Vector3 HIGH_SPEED_POSITION = new Vector3(193f,-10.4f,0f);

    private const float AXIS_THRESHOLD = 0.8f;
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

        descriptions[0] = "This hovercar drives fast and is able to quickly change direction drifting. Open holes wide in your enemies with the two machine guns, or drift your way through them (press <color=#ff0000ff>B</color> to drift).";
        descriptions[1] = "This hovercar lets you approach the battle in a more tactical way: mining the battlefield or dropping bombs from a distance it'll always be sure to deliver a great deal of damage taking few risks.";
        descriptions[2] = "This hovercar sure likes fireworks. Slow but sturdy, it is equipped with the Laser Cannon: equipping it with <color=#ffff00ff>Y</color> makes the vehicle go in Camper Mode, where it cannot move but delivers an incredible deal of damage.";
        descriptions[3] = "This hovercar has a great tactical value. It can cure nearby allies or sting the enemies, making them constantly lose some health over time. <color=#ff0000ff>NOT AVAILABLE IN THIS DEMO</color>";
        descriptions[4] = "This hovercar likes some contact. It is very fast and effective when thrown right in the middle of the action. It delivers damage either by crashing its enemies or burning them with the flamethrower. <color=#ff0000ff>NOT AVAILABLE IN THIS DEMO</color>";
        descriptions[5] = "This hovercar doesn't really like all this movement around it. It is able to chain enemies and bring them close, just to sting them with a thousand needles. <color=#ff0000ff>NOT AVAILABLE IN THIS DEMO</color>";
        descriptions[6] = "This hovercar helps stopping the firepower coming at its team with the shield, and it is also able to deliver some damage from behind it through bombs. <color=#ff0000ff>NOT AVAILABLE IN THIS DEMO</color>";

        ShowConfiguration(currentConfiguration);
    }

    void FixedUpdate()
    {
        activeMesh.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }

    void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > AXIS_THRESHOLD || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!axisInUse)
            {
                if (Input.GetAxisRaw("Horizontal") < -AXIS_THRESHOLD || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    previousConfiguration();
                    //TODO: make leftArrow blink
                }
                else if (Input.GetAxisRaw("Horizontal") > AXIS_THRESHOLD || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    nextConfiguration();
                    //TODO: make rightArrow blink
                }
                axisInUse = true;
                GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
            }
        }
        else
            axisInUse = false;

        if (Input.GetButtonDown("XboxB"))
        {
            if(host)
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().ReturnToHostMenu();
            else
                GameObject.Find("Cnvs_join").GetComponent<JoinManager>().WakeUp();
            GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
        }
        if (Input.GetButtonDown("XboxA"))
        {
            if (host && currentConfiguration < 3)
            {
                GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().classType = currentConfiguration;
                GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().host = true;
                SceneManager.LoadScene("demo");

            }
            else if (!host && currentConfiguration < 3)
            {
                GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().classType = currentConfiguration;
                GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().ipAddress = ipAddress;
                GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().host = false;
                SceneManager.LoadScene("demo");
            }
            GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
        }
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
                setParametersBars("average","high");
                hex_firstWeapon.GetComponentInChildren<Text>().text = "130";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "-";
                hex_specialAbility.gameObject.SetActive(true);
                LblSpecialAbility.gameObject.SetActive(true);
                break;
            case 1:         //Miner
                activateMiner();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = mineSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = bombSprite;
                setParametersBars("low", "average");
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
                setParametersBars("high", "low");
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
                setParametersBars("high", "high");
                hex_firstWeapon.GetComponentInChildren<Text>().text = "90";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "10";
                hex_specialAbility.gameObject.SetActive(true);
                LblSpecialAbility.gameObject.SetActive(true);
                break;
            case 4:         //Rammer
                activateFake();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = rotatingBladesSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = flameThrowerSprite;
                setParametersBars("average", "low");
                hex_firstWeapon.GetComponentInChildren<Text>().text = "-";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "90";
                hex_specialAbility.gameObject.SetActive(false);
                LblSpecialAbility.gameObject.SetActive(false);
                break;
            case 5:         //Chainer
                activateFake();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = chainSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = needlesSprite;
                setParametersBars("average", "average");
                hex_firstWeapon.GetComponentInChildren<Text>().text = "-";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "-";
                hex_specialAbility.gameObject.SetActive(false);
                LblSpecialAbility.gameObject.SetActive(false);
                break;
            case 6:         //Defender
                activateFake();
                hex_firstWeapon.GetComponentInChildren<Image>().sprite = shieldSprite;
                hex_secondWeapon.GetComponentInChildren<Image>().sprite = bombSprite;
                setParametersBars("low", "high");
                hex_firstWeapon.GetComponentInChildren<Text>().text = "-";
                hex_secondWeapon.GetComponentInChildren<Text>().text = "12";
                hex_specialAbility.gameObject.SetActive(false);
                LblSpecialAbility.gameObject.SetActive(false);
                break;
        }
        shortDescription.text = descriptions[configuration];
        classText.text = classNames[configuration];
    }

    private void setParametersBars(string healthValue, string speedValue)
    {
        switch(healthValue)
        {
            case "low":
                healthContainer.sprite = healthLow;
                healthContainer.rectTransform.sizeDelta = LOW_PARAMETER_SIZE;
                healthContainer.rectTransform.localPosition = LOW_HEALTH_POSITION;
                break;
            case "average":
                healthContainer.sprite = healthAverage;
                healthContainer.rectTransform.sizeDelta = AVERAGE_PARAMETER_SIZE;
                healthContainer.rectTransform.localPosition = AVERAGE_HEALTH_POSITION;
                break;
            case "high":
                healthContainer.sprite = healthHigh;
                healthContainer.rectTransform.sizeDelta = HIGH_PARAMETER_SIZE;
                healthContainer.rectTransform.localPosition = HIGH_HEALTH_POSITION;
                break;
        }

        switch (speedValue)
        {
            case "low":
                speedContainer.sprite = speedLow;
                speedContainer.rectTransform.sizeDelta = LOW_PARAMETER_SIZE;
                speedContainer.rectTransform.localPosition = LOW_SPEED_POSITION;
                break;
            case "average":
                speedContainer.sprite = speedAverage;
                speedContainer.rectTransform.sizeDelta = AVERAGE_PARAMETER_SIZE;
                speedContainer.rectTransform.localPosition = AVERAGE_SPEED_POSITION;
                break;
            case "high":
                speedContainer.sprite = speedHigh;
                speedContainer.rectTransform.sizeDelta = HIGH_PARAMETER_SIZE;
                speedContainer.rectTransform.localPosition = HIGH_SPEED_POSITION;
                break;
        }
    }

    private void activateDrifter()
    {
        drifterMesh.SetActive(true);
        minerMesh.SetActive(false);
        camperMesh.SetActive(false);
        fakeMesh.SetActive(false);

        activeMesh = drifterMesh;
    }

    private void activateMiner()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(true);
        camperMesh.SetActive(false);
        fakeMesh.SetActive(false);

        activeMesh = minerMesh;
    }

    private void activateCamper()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(false);
        camperMesh.SetActive(true);
        fakeMesh.SetActive(false);

        activeMesh = camperMesh;
    }

    private void activateFake()
    {
        drifterMesh.SetActive(false);
        minerMesh.SetActive(false);
        camperMesh.SetActive(false);
        fakeMesh.SetActive(true);

        activeMesh = fakeMesh;
    }
}
