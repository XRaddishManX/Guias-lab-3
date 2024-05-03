using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{

    [SerializeField] TMP_InputField _roomNameInputfield;
    [SerializeField] TMP_Text _roomName;
    [SerializeField] TMP_Text _errorMessage;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomItemPrefab;
    public static Launcher Instance;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerItemPrefab;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // conectar al master
        Debug.Log("Conectando");
        MenuManager.Instance.OpenMenuName("Loading"); //(paso 60)
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        //borran la siguiente linea es el comportamiento base del metodo
        // base.OnConnectedToMaster();
        Debug.Log("Conectado");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        //borran la base
        // base.OnJoinedLobby();
        MenuManager.Instance.OpenMenuName("Home"); //(paso 61)
        Debug.Log("Conectado al lobby ");
        PhotonNetwork.NickName = "player" + Random.Range
        (0, 1000).ToString("0000");

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomNameInputfield.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(_roomNameInputfield.text);

        MenuManager.Instance.OpenMenuName("Loading");

    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenuName("Room");
        _roomName.text = PhotonNetwork.CurrentRoom.Name;
        foreach (Transform playerT in PlayerListContent)
        { Destroy(playerT.gameObject); }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerItemPrefab,
           PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorMessage.text = "Error al crear la sala " + message;
        MenuManager.Instance.OpenMenuName("Error");
    }
    public void JoinRoom(RoomInfo _info)
    {
        PhotonNetwork.JoinRoom(_info.Name);
        MenuManager.Instance.OpenMenuName("Loading");
    }
    public void Leaveroom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenuName("Home");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenuName("Home");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transfo in roomListContent)
        {
            //destruimos prefabs de todas las salas anteriores para
            //cargar la nueva lista de salas
            Destroy(transfo.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            { continue; }
            // Creamos un prefab por cada sala que exista
            //Se Agrega la informacion de la sala
            // LLamamos Setup para que agrege las salas existentes
            Instantiate(roomItemPrefab,
           roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerItemPrefab,
       PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

}
