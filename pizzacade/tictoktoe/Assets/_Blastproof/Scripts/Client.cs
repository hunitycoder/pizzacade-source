//using Photon.Realtime;
using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.UI.Message;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Client : MonoBehaviourPunCallbacks
{
    [SerializeField] private BoolVariable _canSelectTile;
    [SerializeField] private StringVariable _connectionStatus;
    [SerializeField] private StringVariable _connectedRoom;
    [SerializeField] private SimpleEvent _newGame;

    [SerializeField] private StringVariable _roomID;
    [SerializeField] private StringVariable _gameIDFilter;
    private List<string> randomRooms = new List<string>();
    public GameObject TurnIndicator;
    // ---- Exposed Methods  ---- 
    public void OnClickRandomButton()
    {
        JoinRandomRoom();
    }

    public void JoinFriendRoom()
    {
        JoinRoom();
    }

    public void CreateFriendRoom()
    {
        CreateRoom();
    }
    // -------------------------- 

    void Start()
    {
        TurnIndicator.SetActive(false);
        _connectionStatus.Value = "Connecting To Server...";
        _connectedRoom.Value = "";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        _connectionStatus.Value = "";
        _connectedRoom.Value = "";
        Application.ExternalEval("socketisready = true;");
    }

    string GenerateUniqueRoomName()
    {
        string ret = (GlobalValue.MyRandomInt(100) + 300).ToString();
        return ret;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _connectionStatus.Value = "Disconnected";
    }

    public void CreateRoom()
    {
        
        RoomOptions roomOptions = new RoomOptions();
        int maxPlayers = GlobalValue.MaxPlayer;

        roomOptions.MaxPlayers = (byte)maxPlayers;

        roomOptions.IsOpen = true;
        PhotonNetwork.CreateRoom("Friend"+GenerateUniqueRoomName(), roomOptions, Photon.Realtime.TypedLobby.Default);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Random"+ _roomID.Value);
    }

    [Button]
    public void JoinRandomRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
        RoomOptions roomOptions = new RoomOptions();
        int maxPlayers = GlobalValue.MaxPlayer;

        roomOptions.MaxPlayers = (byte)maxPlayers;

        roomOptions.IsOpen = true;
        string _roomName = "";
        if (randomRooms.Count < 1)
        {
            _roomName = "Random" + GlobalValue.MyRandomInt(100) + 300;
            PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
        }
        else
        {
            int rrr = UnityEngine.Random.Range(0, randomRooms.Count);
            _roomName = randomRooms[rrr];
            PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
        }

    }

    public override void OnJoinedRoom()
    {
        Message.Instance.ShowMessage("Joined room!", 2f);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("server");
        }
        _newGame?.Invoke();
        _canSelectTile.Value = false;
        _connectedRoom.Value = PhotonNetwork.CurrentRoom.Name;

        Application.ExternalCall("joinedRoom", PhotonNetwork.CurrentRoom.Name);

        _connectionStatus.Value = "Waiting For Opponent...";
        //TurnIndicator.SetActive(true);


        if (PhotonNetwork.CurrentRoom?.PlayerCount == PhotonNetwork.CurrentRoom?.MaxPlayers)
        {
            TurnIndicator.SetActive(true);
            _connectionStatus.Value = "";
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player Entered Room {newPlayer.ActorNumber}");
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom?.PlayerCount == PhotonNetwork.CurrentRoom?.MaxPlayers)
        {
            //start game
            TurnIndicator.SetActive(true);
            _connectionStatus.Value = "";
            bool startingPlayer = Random.value > .5f;
            if(startingPlayer)
            {
                photonView.RPC("StartGame", RpcTarget.Others, true);
            }
            else
            {
                _canSelectTile.Value = true;
                
            }
        }


    }

    [PunRPC]
    void StartGame(bool isStartingPlayer)
    {
        Debug.Log(isStartingPlayer);
        _canSelectTile.Value = isStartingPlayer;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Message.Instance.ShowMessage("Player left, closing room", 2f);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Message.Instance.ShowMessage("Joining random room failed!\n" + message, 2f);
        Debug.LogError($"{returnCode} {message}");
        if(returnCode == 32760)
        {
            CreateRoom();
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Message.Instance.ShowMessage("Joining room failed!\n" + message, 2f);
        Debug.LogError($"{returnCode} {message}");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (!room.IsOpen)
                continue;
            if (room.Name.Contains("Friend")) continue;
            randomRooms.Add(room.Name);
        }

        
    }
    
}
