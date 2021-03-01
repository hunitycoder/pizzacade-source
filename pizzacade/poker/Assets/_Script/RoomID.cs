using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomID : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.connected)
        {
            if (PhotonNetwork.room.Name.Contains("Friend")){
                GetComponent<Text>().text = "ROOM ID " + PhotonNetwork.room.Name.Substring(6);
            }
            else
            {
                GetComponent<Text>().text = "RANDOM ROOM";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
