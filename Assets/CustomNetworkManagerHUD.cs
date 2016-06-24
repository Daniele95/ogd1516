#if ENABLE_UNET
using UnityEngine.UI;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class CustomNetworkManagerHUD : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField] public bool showGUI = false;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;

		public GameObject lobby;
		public int classType = 0;
		public int team = 0;
		public string player = "Player";
		public string ipAddress = "127.0.0.1";
		public bool host = true;

		// Runtime variable
		bool showServer = false;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
            //this avoid the destruction of network manager
           // DontDestroyOnLoad(transform.gameObject); 
		}

		void Start(){
            GameObject net = GameObject.Find("NetVehicleContainer");

            if (net != null)
            {
                host = net.GetComponent<NetVehicleContainer>().host;
                classType = net.GetComponent<NetVehicleContainer>().classType;
                team = net.GetComponent<NetVehicleContainer>().team;
                player = net.GetComponent<NetVehicleContainer>().player;
                ipAddress = net.GetComponent<NetVehicleContainer>().ipAddress;
            }

            if (host) {
				startHost ();
			} else {
				startClient (ipAddress);
			}
		}

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			int numPlayers = lobby.GetComponent<Lobby> ().activePlayers;
			int maxPlayers = GetComponent<ControllerNet> ().maxPlayers;
			GameObject waitingPlayers = GameObject.Find ("WaitingPlayers");

			//Client ready but not the scene
			if (numPlayers < maxPlayers) {
				if (NetworkClient.active && !ClientScene.ready || NetworkServer.active) {
					string strNumPlayers = "";

					if(numPlayers > 0)
						strNumPlayers = "Currently " + numPlayers + " of " + maxPlayers;

					waitingPlayers.GetComponent<Text> ().text = "Waiting for other players\n" + strNumPlayers;
				}
			} else {
				if(waitingPlayers != null)
					GameObject.Find ("BGWaitingPlayers").SetActive (false);
			}

			if (!NetworkClient.active && !host) {
				startClient (ipAddress);
				print ("START CLIENT");
			}

			if (!showGUI)
				return;

			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			int spacing = 24;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Drifter"))
				{
					classType = 0;
				}

				if (GUI.Button(new Rect(xpos+100, ypos, 100, 20), "Miner"))
				{
					classType = 1;
				}

				if (GUI.Button(new Rect(xpos+200, ypos, 100, 20), "Camper"))
				{
					classType = 2;
				}

				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Blue Team"))
				{
					team = 0;
				}

				if (GUI.Button(new Rect(xpos+100, ypos, 100, 20), "Red Team"))
				{
					team = 1;
				}

				ypos += spacing;

				player = GUI.TextField(new Rect(xpos, ypos, 200, 20), player);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
				{
					manager.StartHost();

					//LoaderClass loaderScript = manager.playerPrefab.gameObject.GetComponent<LoaderClass> ();
					//loaderScript.typeClass = classType;
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
				{
					manager.StartClient();

					//LoaderClass loaderScript = manager.playerPrefab.gameObject.GetComponent<LoaderClass> ();
					//loaderScript.typeClass = classType;
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
				{
					manager.StartServer();
				}
				ypos += spacing;
			}
			else
			{
				if (NetworkServer.active)
				{
					//GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
					//ypos += spacing;
				}
				if (NetworkClient.active)
				{
					//GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
					//ypos += spacing;
				}
			}

			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
				{
					ClientScene.Ready(manager.client.connection);
				
					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += spacing;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				//if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
				//{
				//	manager.StopHost();
				//}
				//ypos += spacing;
			}
			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null)
				{
					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, 100, 20), manager.matchName);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
							{
								manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
							}
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
								{
									manager.matchName = match.name;
									manager.matchSize = (uint)match.currentSize;
									manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
								}
								ypos += spacing;
							}
						}
					}

					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
					{
						showServer = !showServer;
					}
					if (showServer)
					{
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
						{
							manager.SetMatchHost("localhost", 1337, false);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
					}

					ypos += spacing;

					GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
					ypos += spacing;

					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
					{
						manager.StopMatchMaker();
					}
					ypos += spacing;
				}
			}
		}

        public void startClient(string ipAddress)
        {
            manager.networkAddress = ipAddress;
            manager.StartClient();
        }

        public void startHost()
        {
			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				manager.StartHost();
			}
        }

        public void setClass(int classType)
        {
            this.classType = classType;
        }
    }
};
#endif //ENABLE_UNET
