
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;


[RequireComponent(typeof(PhotonView))]
public class PUNMenu : PunBehaviour
{

    // References to GUI game objects, so they can be enabled/disabled and
    // used to show useful messages to player.

    public static bool IsInRoom = false;
    public static bool IsCreater = false;
    public static bool IsInLobby = false;
    public static bool IsInMaster = false;

    private List<string> randomRooms = new List<string>();

    public static PUNMenu Instant;
    public string InvitedPlayerName = "";
    void Awake()
    {

        if (Instant == null)
        {
            Instant = this;
            DontDestroyOnLoad(Instant);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void Start()
    {

        if (PhotonNetwork.connected)
        {

            if (PhotonNetwork.insideLobby == false)
            {

                if (LoadingView.Instance != null) LoadingView.Instance.Show();
                PhotonNetwork.JoinLobby();

                return;
            }
            if (LoadingView.Instance != null) LoadingView.Instance.Hide();

            return;
        }

        if (LoadingView.Instance != null) LoadingView.Instance.Show();
        PhotonNetwork.ConnectUsingSettings("v1.0");

    }

    // When connected to Photon, enable nickname editing (too)
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected from master");
        IsInMaster = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnectedFromPhoton()
    {
        IsInMaster = false;
        Debug.Log("Disconnected from master");
        Start();
        if (LoadingView.Instance != null) LoadingView.Instance.Show();
    }


    // When connected to Photon Lobby, disable nickname editing and messages text, enables room list
    public override void OnJoinedLobby()
    {
        IsInLobby = true;
        Debug.Log("Connected to lobby");
        PhotonNetwork.player.name = GlobalValue.ProfileName;
        SetCustomPropertiesCreater();
        if (LoadingView.Instance != null) LoadingView.Instance.Hide();

    }

    private void SetCustomPropertiesCreater()
    {
        string sceneName = "Game";
        SetCustomPropertiesPlayer(PhotonNetwork.player, GlobalValue.ProfileName, GlobalValue.AvatarID, GlobalValue.SeatPosition, GlobalValue.Coins);
    }

    public void SetCustomPropertiesPlayer(PhotonPlayer player, string name, int avatarID, int seatposition, int coins)
    {
        Debug.Log("avatar : " + avatarID);
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "Name", name }, { "AvatarID", avatarID }, { "SeatPosition", seatposition }, { "Coins", coins } };

        player.SetCustomProperties(customProperties);
    }


    public override void OnReceivedRoomListUpdate()
    {
        randomRooms.Clear();
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            Debug.Log(room.Name);
            if (!room.IsOpen)
                continue;
            if (room.Name.Contains("Friend")) continue;
            randomRooms.Add(room.Name);
        }
    }

    string GreateValidRoomName(string origRoomName)
    {
        bool valid = true;
        while (true)
        {
            foreach (RoomInfo room in PhotonNetwork.GetRoomList())
            {
                if (!room.IsOpen)
                    continue;
                if (room.Name.Contains("Friend"))
                {
                    if (room.Name == origRoomName)
                    {
                        valid = false;
                    }
                }
            }
            if (valid == false)
            {
                origRoomName += GlobalValue.MyRandomInt(10).ToString();
            }
            else
            {
                break;
            }
        }
        return origRoomName;
    }
    public void CreatePokerRoom(string roomname)
    {

        RoomOptions roomOptions = new RoomOptions();
        int maxPlayers = GlobalValue.MaxPlayer;

        roomOptions.MaxPlayers = (byte)maxPlayers;

        roomOptions.IsOpen = true;
        GlobalValue.RoomID = GreateValidRoomName(roomname);
        PhotonNetwork.CreateRoom(GlobalValue.RoomID, roomOptions, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        IsCreater = true;
        PhotonNetwork.room.IsOpen = true;
        Debug.Log("created Room + " + PhotonNetwork.room.Name);
        SetCustomPropertiesCreater();
        GlobalValue.IsPokerController = true;
    }
    public override void OnPhotonCreateRoomFailed(object[] codeMessage)
    {

        if ((short)codeMessage[0] == ErrorCode.GameIdAlreadyExists)
        {
            GlobalValue.RoomID = GlobalValue.RoomID + "1";
            string roomname = "1";

            GlobalValue.RoomID = GlobalValue.RoomID + roomname;
            CreatePokerRoom(GlobalValue.RoomID);
        }
    }


    public void JoinRoomFriend(string roomname)
    {
        bool roomValid = false;
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {

            if (room.Name == roomname)
            {
                if (room.IsOpen == false)
                {
                    Debug.Log("Friend Room is not opened");
                }
                else
                {
                    roomValid = true;
                    Debug.Log("Friend Room is opened");
                }
            }


        }
        if (roomValid == true)
        {
            PhotonNetwork.JoinRoom(roomname);
        }
        else
        {

        }
        
    }


    public void JoinRandomGame()
    {

        if (LoadingView.Instance != null) LoadingView.Instance.Show("Joining to Random Room...");

        RoomOptions roomOptions = new RoomOptions();
        int maxPlayers = GlobalValue.MaxPlayer;

        roomOptions.MaxPlayers = (byte)maxPlayers;

        roomOptions.IsOpen = true;

        string _roomName = "";
        if (randomRooms.Count < 1)
        {
            _roomName = "Random" + GlobalValue.MyRandomInt(100) + 100;
            PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
        }
        else
        {
            int rrr = UnityEngine.Random.Range(0, randomRooms.Count);
            _roomName = randomRooms[rrr];
            PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
        }

    }

    // if we join (or create) a room, no need for the create button anymore;
    public override void OnJoinedRoom()
    {
        if (LoadingView.Instance != null) LoadingView.Instance.Hide();
        IsInRoom = true;
        Debug.Log("Joined Room + " + PhotonNetwork.room.name);
        SetCustomPropertiesCreater();

        if (PhotonNetwork.isMasterClient)
        {
            GlobalValue.IsPokerController = true;
        }
        else
        {
            GlobalValue.IsPokerController = false;
        }
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.ExternalCall("onJoinedRoom", PhotonNetwork.room.Name);
        }
        PhotonNetwork.LoadLevel("Game");
    }

    public int PlayersJoined()
    {
        return PhotonNetwork.playerList.Length;
    }

    public override void OnLeftRoom()
    {
        IsInRoom = false;

        // GameManager.Instance.OnGameOver_Event();
    }

    public virtual void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Failed Join Room + " + PhotonNetwork.room.name);

    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            if (p == PhotonNetwork.player) continue;
            else
            {

            }
        }
    }

    // when a player disconnects from the room, update the spawn/position order for all
    public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer)
    {
        if (disconnetedPlayer != PhotonNetwork.player)
        {
            
        }
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            {
                int seatPosition = (int)p.CustomProperties["SeatPosition"];
                string name = (string)p.CustomProperties["Name"];

            }
        }
    }


    
    public void SendTurn(string _color)
    {
        photonView.RPC("RPCTurn", PhotonTargets.All, _color);
    }
    [PunRPC]
    public void RPCTurn(string _color)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RemoteTurn(_color);
        }
    }

    public void SendClickedWord(string turnColor, int wordID)
    {
        photonView.RPC("RPCWordClicked", PhotonTargets.All, turnColor, wordID);
    }
    [PunRPC]
    public void RPCWordClicked(string _color, int wordID)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RemoteWordClicked(_color, wordID);
        }
    }

    public void SendGameOver( string losserColor)
    {
        photonView.RPC("RPCGameOver", PhotonTargets.All, losserColor);
    }

    [PunRPC]
    public void RPCGameOver(string losserColor)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RemoteGameOver(losserColor);
        }
    }
}
