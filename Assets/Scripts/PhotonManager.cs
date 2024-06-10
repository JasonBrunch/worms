using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private string roomName = "MainRoom";
    public LobbyManager lobbyManager; // Reference to the LobbyManager

    void Start()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        }
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        JoinOrCreateRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        JoinOrCreateRoom();
    }

    void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8; // Set the maximum number of players
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    //Called when the local player joins a room.
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
         lobbyManager.PlayerEnterRoom(); // Delegate the update to the LobbyManager
    }

    //alled when another player joins the room that the local player is already in.
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player Entered Room: " + newPlayer.NickName);
       
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        lobbyManager.UpdatePlayerList(); // Delegate the update to the LobbyManager
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join room failed: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create room failed: " + message);
    }
}