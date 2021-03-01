
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;
using poker;

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
        SetCustomPropertiesPlayer(PhotonNetwork.player, GlobalValue.ProfileName, GlobalValue.AvatarID, GlobalValue.SeatPosition, GlobalValue.Coins, 0, 0, "notavailable", GlobalValue.IsPokerController);
    }

    public void SetCustomPropertiesPlayer(PhotonPlayer player, string name, int avatarID, int seatposition, int coins, int bet, int dealer, string action, bool bControler)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { 
            { PConst.NAME, name },
            { PConst.AVATARID, avatarID },
            { PConst.SEATPOS, seatposition }, 
            { PConst.COINS, coins },
            { PConst.BET, bet },
            { PConst.DEALER, dealer },
            { PConst.ACTION, action },
            { PConst.CONTROLLER, bControler },
        };

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
            _roomName = "Random" + GlobalValue.MyRandomInt(100) + 200;
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
            int seatPosition = (int)disconnetedPlayer.CustomProperties[PConst.SEATPOS];
            Debug.Log("DisconnectedplayerPOS: " + seatPosition);
            bool pokerController = (bool )disconnetedPlayer.CustomProperties[PConst.CONTROLLER];
            if (TexasHoldEm.Instance != null)
            {
                TexasHoldEm.Instance.RemovePlayer(seatPosition);
            }

            if( pokerController == true)
            {
                for (int i = 0; i < GlobalValue.MaxPlayer; i++)
                {
                    if (TexasHoldEm.Instance.PlayerSeated(i) == false || seatPosition == i)
                    {
                        continue;
                    }
                    Debug.Log("Available Player: " + i);
                    if (i == (int)PhotonNetwork.player.CustomProperties[PConst.SEATPOS])
                    {
                        Debug.Log("Update Player: " + i);
                        UpdatePokerController();
                        if (TexasHoldEm.Instance) TexasHoldEm.Instance.Deal(true);
                    }
                    break;
                }
            }

            

        }
    }

    public void Seat(int seatID)
    {
        int seatPosition = (int)PhotonNetwork.player.CustomProperties[PConst.SEATPOS];
        int prevSeatPosition = seatPosition;
        if (seatPosition != -1)
        {

        }

        string name = (string)PhotonNetwork.player.CustomProperties[PConst.NAME];
        int coins = (int)PhotonNetwork.player.CustomProperties[PConst.COINS];
        int avatarID = (int)PhotonNetwork.player.CustomProperties[PConst.AVATARID];
        int bet = (int)PhotonNetwork.player.CustomProperties[PConst.BET];
        string action = (string)PhotonNetwork.player.CustomProperties[PConst.ACTION];
        int dealer = (int)PhotonNetwork.player.CustomProperties[PConst.DEALER];
        bool bController = (bool)PhotonNetwork.player.CustomProperties[PConst.CONTROLLER];
        seatPosition = seatID;

        SetCustomPropertiesPlayer(PhotonNetwork.player, name, avatarID, seatPosition, coins, bet, dealer, action, bController);

        photonView.RPC("RPCSeat", PhotonTargets.All, name, seatID, avatarID, coins, bet, dealer, action, prevSeatPosition);

    }
    [PunRPC]
    public void RPCSeat(string playername, int seatid, int avatarid, int coins, int bet, int dealer, string action, int prevPosition)
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.SeatPlayer(playername, seatid, avatarid, coins, bet, dealer, action, prevPosition);
        }
    }

    public void UpdateCoinsForPhotonPlayer( int _coins)
    {
        int seatPosition = (int)PhotonNetwork.player.CustomProperties[PConst.SEATPOS];
        string name = (string)PhotonNetwork.player.CustomProperties[PConst.NAME];
        int coins = (int)PhotonNetwork.player.CustomProperties[PConst.COINS];
        int avatarID = (int)PhotonNetwork.player.CustomProperties[PConst.AVATARID];
        int bet = (int)PhotonNetwork.player.CustomProperties[PConst.BET];
        string action = (string)PhotonNetwork.player.CustomProperties[PConst.ACTION];
        int dealer = (int)PhotonNetwork.player.CustomProperties[PConst.DEALER];
        bool bController = (bool)PhotonNetwork.player.CustomProperties[PConst.CONTROLLER];
        if (coins != _coins)
        {
            coins = _coins;
            SetCustomPropertiesPlayer(PhotonNetwork.player, name, avatarID, seatPosition, coins, bet, dealer, action, bController);
        }
    }

    public void UpdatePokerController()
    {
        int seatPosition = (int)PhotonNetwork.player.CustomProperties[PConst.SEATPOS];
        string name = (string)PhotonNetwork.player.CustomProperties[PConst.NAME];
        int coins = (int)PhotonNetwork.player.CustomProperties[PConst.COINS];
        int avatarID = (int)PhotonNetwork.player.CustomProperties[PConst.AVATARID];
        int bet = (int)PhotonNetwork.player.CustomProperties[PConst.BET];
        string action = (string)PhotonNetwork.player.CustomProperties[PConst.ACTION];
        int dealer = (int)PhotonNetwork.player.CustomProperties[PConst.DEALER];
        GlobalValue.IsPokerController = true;
        
        SetCustomPropertiesPlayer(PhotonNetwork.player, name, avatarID, seatPosition, coins, bet, dealer, action, true);
        
    }
    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            int seatPosition = (int)p.CustomProperties[PConst.SEATPOS];
            string name = (string)p.CustomProperties[PConst.NAME];
            int coins = (int)p.CustomProperties[PConst.COINS];
            int avatarID = (int)p.CustomProperties[PConst.AVATARID];
            int bet = (int)p.CustomProperties[PConst.BET];
            string action = (string)p.CustomProperties[PConst.ACTION];
            int dealer = (int)p.CustomProperties[PConst.DEALER];
            if (TexasHoldEm.Instance != null)
            {
                TexasHoldEm.Instance.UpdatePlayerCoins(seatPosition, coins);
            }
        }
    }

    public void SendCardData(string data)
    {
        if (GlobalValue.IsPokerController)
        {
            photonView.RPC("RPCCardData", PhotonTargets.All, data);
        }
    }
    [PunRPC]
    public void RPCCardData(string data)
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.CardDataRemote(data);
        }
    }

    public void SendBetTurn( int seatid)
    {
        if (GlobalValue.IsPokerController)
        {
            photonView.RPC("RPCBetTurn", PhotonTargets.All, seatid);
        }
    }
    [PunRPC]
    public void RPCBetTurn(int seatid)
    {
        int seatPosition = (int)PhotonNetwork.player.CustomProperties["SeatPosition"];
        if (TexasHoldEm.Instance.ReadySeat(seatid))
        {
            if (TexasHoldEm.Instance)
            {
                TexasHoldEm.Instance.StopTimer();
                TexasHoldEm.Instance.StartTimer(seatid);
            }
        }
        if ( seatPosition == seatid)
        {
            if (TexasHoldEm.Instance.ReadySeat(seatid))
            {
                BettingManager.Instance.StartBetTurn();
            }
        }
    }

    public void SendCalc()
    {
        photonView.RPC("RPCCalc", PhotonTargets.All);
    }
    [PunRPC]
    public void RPCCalc()
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.RemoteCalculate();
        }
    }
    public void SendFlop()
    {
        photonView.RPC("RPCFlop", PhotonTargets.All);
    }

    [PunRPC]
    public void RPCFlop()
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.RemoteFlop();
        }
    }

    public void SendTurn()
    {
        photonView.RPC("RPCTurn", PhotonTargets.All);
    }
    [PunRPC]
    public void RPCTurn()
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.RemoteTurn();
        }
    }

    public void SendRiver()
    {
        photonView.RPC("RPCRiver", PhotonTargets.All);
    }

    [PunRPC]
    public void RPCRiver()
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.RemoteRiver();
        }
    }

    public void SendReset()
    {
        photonView.RPC("RPCReset", PhotonTargets.All);
    }

    [PunRPC]
    public void RPCReset()
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.RemoteReset();
        }

    }


    public void SendBetAmount(int sid, int betamount, bool ballin)
    {
        photonView.RPC("RPCBetDone", PhotonTargets.All, sid, betamount, ballin);
    }
    [PunRPC]
    public void RPCBetDone(int sid, int betamount, bool ballin)
    {
        if (TexasHoldEm.Instance != null)
        {
            TexasHoldEm.Instance.RemoteBet(sid,betamount, ballin);
        }

    }
}
