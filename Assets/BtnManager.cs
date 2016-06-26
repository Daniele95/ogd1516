using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;


public class BtnManager : MonoBehaviour {

    public Canvas classMeshManager;
    public Canvas classUIManager;
    public UsernameChanger usernameChanger;

    private GameObject canvas;
    private Button[] buttons;
    private string firstButton = "Btn_NewGame";
    private List<string> interactibleButtonsNames = new List<string>();
    private Color activeColor;
    private Color inactiveColor;

	// Use this for initialization
	void Start () {
        classMeshManager.gameObject.SetActive(false);
        classUIManager.gameObject.SetActive(false);

        canvas = this.gameObject;
        usernameChanger = GameObject.Find("TextUsername").GetComponent<UsernameChanger>();

        activeColor = new Color(1f, 1f, 1f, 1f);
        inactiveColor = new Color(148 / 255f, 148 / 255f, 148 / 255f, 1f);

        interactibleButtonsNames.Add("Btn_NewGame");
        interactibleButtonsNames.Add("Btn_Exit");
        interactibleButtonsNames.Add("Btn2_PrivateGame");
        interactibleButtonsNames.Add("Btn3_Host");
        interactibleButtonsNames.Add("Btn3_Join");
        interactibleButtonsNames.Add("Btn4_1v1");
        interactibleButtonsNames.Add("Btn4_2v2");
        interactibleButtonsNames.Add("Btn4_3v3");
        interactibleButtonsNames.Add("Btn4_4v4");

        OpenFirstMenu();
	}

    public void OpenFirstMenu()
    {
        buttons = canvas.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (interactibleButtonsNames.Contains(buttons[i].name))
                setButtonInteractible(buttons[i], true);
            else
                setButtonInteractible(buttons[i], false);

            if (buttons[i].name.Contains("Btn2") || buttons[i].name.Contains("Btn3") || buttons[i].name.Contains("Btn4"))
                buttons[i].gameObject.SetActive(false);
            else
                buttons[i].gameObject.SetActive(true);

            if (buttons[i].name.Contains(firstButton))
                buttons[i].Select();
        }
        usernameChanger.currentMenu = UsernameChanger.FIRST;
    }

    public void ReturnToPrivateGameMenu()
    {
        classUIManager.gameObject.SetActive(false);
        classMeshManager.gameObject.SetActive(false);
        for(int i = 0; i<buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            setButtonInteractible(buttons[i], buttons[i].name.Contains("Btn3") && interactibleButtonsNames.Contains(buttons[i].name));
            if (buttons[i].name.Equals("Btn3_Host"))
                buttons[i].Select();
        }
        usernameChanger.currentMenu = UsernameChanger.THIRD;
    }

    public void ReturnToHostMenu()
    {
        classUIManager.gameObject.SetActive(false);
        classMeshManager.gameObject.SetActive(false);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            setButtonInteractible(buttons[i], buttons[i].name.Contains("Btn4") && interactibleButtonsNames.Contains(buttons[i].name));
            if (buttons[i].name.Equals("Btn4_1v1"))
                buttons[i].Select();
        }
        usernameChanger.currentMenu = UsernameChanger.FOURTH;
    }

    public void OpenNewGameMenu()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn2"))
            {
                buttons[i].gameObject.SetActive(true);
                if (buttons[i].name.Equals("Btn2_PrivateGame"))
                    buttons[i].Select();
            }
            else if (buttons[i].name.Contains("Btn3"))
                buttons[i].gameObject.SetActive(false);
            else
                setButtonInteractible(buttons[i], false);
        }
        usernameChanger.currentMenu = UsernameChanger.SECOND;
    }

    public void OpenPrivateGameMenu()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn3"))
            {
                buttons[i].gameObject.SetActive(true);
                if (buttons[i].name.Equals("Btn3_Host"))
                    buttons[i].Select();
            }
            else
                setButtonInteractible(buttons[i], false);
        }
        usernameChanger.currentMenu = UsernameChanger.THIRD;
    }

    public void OpenGameModeMenu()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn4"))
            {
                buttons[i].gameObject.SetActive(true);
                setButtonInteractible(buttons[i], true);
                if (buttons[i].name.Contains("1v1"))
                    buttons[i].Select();
            }
            else
                setButtonInteractible(buttons[i], false);
        }
        usernameChanger.currentMenu = UsernameChanger.FOURTH;
    }

    public void OpenClassSelection()
    {
        classMeshManager.gameObject.SetActive(true);
        classUIManager.gameObject.SetActive(true);

        classMeshManager.GetComponent<ClassMenuManager>().host = true;

        usernameChanger.currentMenu = UsernameChanger.CLASSES;

        CloseEverything();
    }

    public void setNumberOfPlayers(int numberOfPlayers)
    {
        GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().numberOfPlayers = numberOfPlayers;
    }
  
    public void CloseNewGameMenu()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn2") || buttons[i].name.Contains("Btn3"))
                buttons[i].gameObject.SetActive(false);
            else
            {
                if (interactibleButtonsNames.Contains(buttons[i].name))
                {
                    setButtonInteractible(buttons[i], true);
                    if (buttons[i].name.Contains(firstButton))
                        buttons[i].Select();
                }
            }
        }
    }

    public void ClosePrivateGameMenu()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn3") || buttons[i].name.Contains("Btn4"))
                buttons[i].gameObject.SetActive(false);
            else
                if (interactibleButtonsNames.Contains(buttons[i].name) && buttons[i].name.Contains("Btn2"))
                {
                    setButtonInteractible(buttons[i], true);
                    if (buttons[i].name.Contains("Private"))
                        buttons[i].Select();
                }
            else
                setButtonInteractible(buttons[i], false);
        }
        usernameChanger.currentMenu = UsernameChanger.SECOND;
    }

    public void CloseGameModeMenu()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name.Contains("Btn4"))
                buttons[i].gameObject.SetActive(false);
            else
                if (interactibleButtonsNames.Contains(buttons[i].name) && buttons[i].name.Contains("Btn3"))
                {
                    setButtonInteractible(buttons[i], true);
                    if (buttons[i].name.Contains("Host"))
                        buttons[i].Select();
                }
                else
                    setButtonInteractible(buttons[i], false);
        }
        usernameChanger.currentMenu = UsernameChanger.THIRD;
    }

    public void CloseEverything()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);
    }

    private void setButtonInteractible(Button button, bool interactible)
    {
        button.interactable = interactible;
        Text text = button.gameObject.GetComponentInChildren<Text>();
        if (interactible)
            text.color = activeColor;
        else
            text.color = inactiveColor;
    }

    public void Quit()
    {
        Application.Quit();
        print("Quit");
    }
}
