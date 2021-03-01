using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon;

/// <summary>
/// Manages the race gameplay, instantiating player car and sending/receiving
/// RPCs for race start/finish, timer, etc.
/// </summary>
public class PUNRaceManager: PunBehaviour {

	public static PUNRaceManager instance;

	// To sets player car as camera target
	
    private int loadedPlayers = 0;

    Transform PlayerSpawnPos;

	// reference to local player car
	[HideInInspector]

	// list of all player´s cars (for position calculation)
	
	public float raceTime = 0;
	public double startTimestamp = 0;

    public bool[] bLeaved;

	void Start () {

        
		instance = this;
        bLeaved = new bool[2];
        for (int i = 0; i < 2; i++) bLeaved[i] = false;

        PlayerSpawnPos = GameObject.Find("PlayerSpawnPos").transform;

        CreatePlayer();

		photonView.RPC ("ConfirmLoad", PhotonTargets.All);
	}

	void Update () {
		
	}
    
	// put the correct car references in position order to the appropriate GUIs
	// used on two panels (top-right, and final position/time)
	

	// called when a player computer finishes loading this scene...
	[PunRPC]
	public void ConfirmLoad () {
		loadedPlayers++;

	}

	// Instantiates player car on all peers, using the appropriate spawn point (based
	// on join order), and sets local camera target.
	private void CreatePlayer() {
        //string pathPlayer = "Player/" + GlobalValue.selectedplayer + "/Player" + GlobalValue.selectedplayer + "_" + GlobalValue.selectedbike;
        //GameObject mainPlayer = PhotonNetwork.Instantiate(pathPlayer, PlayerSpawnPos.position, PlayerSpawnPos.rotation, 0);
        //GameManager.Instance.SetMainPlayer(mainPlayer);
	}
    
    public void SendGameOver()
    {
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            //if( (int)(p.customProperties["playerid"])){
            //}
        }
        
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
       
    }

    // when a player disconnects from the room, update the spawn/position order for all
    public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer)
    {
        //int disconnectedplayerid = (int)disconnetedPlayer.customProperties["playerid"];
        //bLeaved[disconnectedplayerid] = true;
 
        if (PhotonNetwork.room.playerCount == 1)
        {
            //GameManager.Instance.GameOver();
        }
    }
    public void ResetToMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }
}
