using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public RectTransform playerListContainer;
    public GameObject playerListPrefab; 
    public TMP_Text playerNameText; 

    public void PlayerEnterRoom()
    {
        Debug.Log("Player Entered Room");
        UpdatePlayerList();
        UpdatePlayerName(PhotonNetwork.NickName);
    }
    public void UpdatePlayerList()
    {
        Debug.Log("Updating Player List");
        foreach (Transform child in playerListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListEntry = Instantiate(playerListPrefab, playerListContainer);
            playerListEntry.GetComponentInChildren<TMP_Text>().text = player.NickName;
        }
    }
    private void UpdatePlayerName(string newName)
    {
        Debug.Log("Updating Player Name: " + newName);
        playerNameText.text = newName;
    }

}