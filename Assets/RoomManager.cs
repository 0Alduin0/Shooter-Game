using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject player;

    [Space]
    public Transform[] spawnPoints;

    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;

    public GameObject connectingUI;

    public GameObject crossHair;

    public GameObject quitgame;

    private string nickname = "unnamed";

    public string roomNameToJoin = "test";


    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            quitgame.SetActive(true);
        }
    }

    public void quit()
    {
        Application.Quit();
    }
    public void resumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        quitgame.SetActive(false);
    }

    public void ChangeNickname(string _name)
    {
        nickname = _name;
    }

    public void JoinRoomButtonPressed()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting..");
        connectingUI.SetActive(true);
        nameUI.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected server");

        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("connected lobby");

        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Connected room");

        roomCam.SetActive(false);

        SpawnPlayer();

        crossHair.SetActive(true);

    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();

        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);

        PhotonNetwork.LocalPlayer.NickName = nickname;

    }


    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

            hash["kills"] = kills;
            hash["deaths"] = deaths;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        catch
        {
        }
    }
}
