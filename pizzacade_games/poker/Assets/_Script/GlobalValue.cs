using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using Facebook.Unity;
using System.Collections.Generic;
using System;
//using Facebook.MiniJSON;
public class GlobalValue : MonoBehaviour {

    public static bool DebugMode = true;
    public static string RoomID = "ROOM";
    static int _coins = 1000;
    public static int MaxPlayer = 7;

    public static bool IsPokerController = false;

   

    public static int Coins
    {
        get
        {
            return _coins;
        }
        set
        {
            if (_coins != value)
            {
                _coins = value;
                PUNMenu.Instant.UpdateCoinsForPhotonPlayer(_coins);
            }
        }
    }
    
    public static bool SoundEnabled = true;
	public static bool MusicEnabled = true;

    static string _profileName;

    
    public static string ProfileName
    {
        set {

            _profileName = value;
            PlayerPrefs.SetString("PlayerName", _profileName);
            PlayerPrefs.Save();
        }

        get
        {
            if (PlayerPrefs.HasKey("PlayerName"))
            {
                _profileName = PlayerPrefs.GetString("PlayerName");
                
            }
            else
            {
                _profileName = "Player" + MyRandomInt(1000);
                PlayerPrefs.SetString("PlayerName", _profileName);
                PlayerPrefs.Save();
            }
            
            return _profileName;
        }
    }

    public static int AvatarID
    {

        get
        {
            if (PlayerPrefs.HasKey("AvatarID"))
            {
                return PlayerPrefs.GetInt("AvatarID", 0);

            }
            else
            {
                int randAvatar = MyRandomInt(65) + 1;
                PlayerPrefs.SetInt("AvatarID", randAvatar);
                return randAvatar;
            }
            
        }
        set
        {
            PlayerPrefs.SetInt("AvatarID", value);
            PlayerPrefs.Save();
        }
    }

    public static int SeatPosition = -1;

    public static int MyRandomInt(int modd)
    {
        byte[] gb;
        int tempi;

        gb = Guid.NewGuid().ToByteArray();
        tempi = Mathf.Abs(BitConverter.ToInt32(gb, 0));
        int ret = (tempi % modd);

        return ret;
    }
   
}
