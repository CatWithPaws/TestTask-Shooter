using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text logger;

    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(100, 9999);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
        Log("Waiting for Connection");
    }

    public override void OnConnected()
    {
        Log("Connected to Lobby.\nJoin to a game or create own!");
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "Room",new Photon.Realtime.RoomOptions { MaxPlayers = 2 });


    public void JoinToRoom() => PhotonNetwork.JoinRandomRoom();

    public override void OnJoinedRoom()
    {
        Log("Joined Room");
        PhotonNetwork.LoadLevel("Level");

    }

    private void Log(string text)
    {
        logger.text = text;
        Debug.Log(text);
    }

}
