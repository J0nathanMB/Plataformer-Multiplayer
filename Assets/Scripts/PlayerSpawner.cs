using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviour
{
    //Player[] allPlayers;
    public GameObject[] playerPrefabs;
    public Transform []spawnPoints;

    private void Awake()
    {
        //allPlayers = PhotonNetwork.PlayerList
        //foreach (Player p in allPlayers)
        //{
        //    if (p != PhotonNetwork.LocalPlayer)
        //    {
        //        playerMovement.instance.myNumberInroom++;
        //    }
        //}
        //if (photonView.IsMine)
        //{
        //    playerMovement.LocalPlayerInstance = this.gameObject;
        //}
        //DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;
        SpawnPlayer();
    }

    

    void SpawnPlayer()
    {
       int randomNumber = Random.Range(0, spawnPoints.Length);
        GameObject playerToSpawm = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];


        if (playerMovement.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(playerToSpawm.name, spawnPoints[randomNumber].position, Quaternion.identity);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }


        //if (PhotonNetwork.MasterClient is ) 
        //{
        //    PhotonNetwork.Instantiate(playerToSpawm.name, spawnPoint1.position, Quaternion.identity);
        //}
        //else
        //{
        //    PhotonNetwork.Instantiate(playerToSpawm.name, spawnPoint2.position, Quaternion.identity);
        //}

    }

}
