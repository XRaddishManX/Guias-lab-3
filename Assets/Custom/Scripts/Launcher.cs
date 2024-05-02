using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{

    [SerializeField] TMP_InputField _roomNameInputfield;
    [SerializeField] TMP_Text _roomName;
    [SerializeField] TMP_Text _errorMessage;

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
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorMessage.text = "Error al crear la sala " + message;
        MenuManager.Instance.OpenMenuName("Error");
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

}
 