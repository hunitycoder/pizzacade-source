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
    [SerializeField] private StringVariable _gameID;
    [SerializeField] private StringVariable _gameIDFilter;
    [SerializeField] private StringVariable _gameState;
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
        
        //use to control gamemode from code or interface
        _gameIDFilter.Value = "m";
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
        string[] numbers = new string[3];
        for(int i=0; i<numbers.Length; i++)
        {
            numbers[i] = Random.Range(4, 10).ToString();
        }
        return System.String.Format("{0}{1}{2}", numbers);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _connectionStatus.Value = "Disconnected";
    }

    public void CreateRoom()
    {
        Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("Friend"+GenerateUniqueRoomName(), roomOptions, Photon.Realtime.TypedLobby.Default);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Friend"+_roomID.Value);
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
            _roomName = "Random" + GlobalValue.MyRandomInt(100) + 400;
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
        _connectionStatus.Value = "Waiting for opponent...";
        _gameID.Value = PhotonNetwork.CurrentRoom.Name;
        _newGame?.Invoke();
        _canSelectTile.Value = false;
        _connectedRoom.Value = "ROOM: " + PhotonNetwork.CurrentRoom.Name;
        _gameState.Value = "";

        Application.ExternalCall("joinedRoom", PhotonNetwork.CurrentRoom.Name);

        if(PhotonNetwork.CurrentRoom?.PlayerCount == PhotonNetwork.CurrentRoom?.MaxPlayers)
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
