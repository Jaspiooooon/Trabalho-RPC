using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput; // Usando TMP_InputField para o TextMeshPro
    public TMP_Text roomListText; // Usando TMP_Text para a lista de salas
    public byte maxPlayers = 4;
    public TMP_Text statusText;
    public GameObject roomButtonPrefab; // Prefab de um bot�o para sala
    public Transform roomListContainer; // Container onde os bot�es ser�o instanciados

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Conecta-se ao Photon
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao Master Server!");
        PhotonNetwork.JoinLobby(); // Entra no lobby principal
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Entrou no lobby!");
        statusText.text = "Lobby: " + PhotonNetwork.CurrentLobby.Name; // Exibe o nome do lobby atual
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Limpa os bot�es antigos
        /*foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject); // Remove todos os bot�es existentes
        }*/

        // Verifica se h� salas dispon�veis
        if (roomList.Count == 0)
        {
            roomListText.text = "Nenhuma sala dispon�vel.";
        }
        else
        {
            roomListText.text = ""; // Limpa a mensagem de "nenhuma sala" se houver salas dispon�veis

            // Cria um bot�o para cada sala na lista
            foreach (RoomInfo room in roomList)
            {
                GameObject roomButton = Instantiate(roomButtonPrefab, roomListContainer);
                roomButton.GetComponentInChildren<TMP_Text>().text = room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")";
                roomButton.GetComponent<Button>().onClick.AddListener(() => JoinRoom(room.Name)); // Adiciona a fun��o para entrar na sala
            }
        }
    }

    public void CreateRoom()
    {
        string roomName = roomNameInput.text; // Obt�m o nome da sala
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Sala" + Random.Range(1000, 9999); // Nome aleat�rio se o campo estiver vazio
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        statusText.text = "Criando sala...";
    }

    public void JoinRoom(string roomName)
    {
        statusText.text = "Tentando entrar na sala " + roomName + "...";
        PhotonNetwork.JoinRoom(roomName);
    }
    
    public override void OnJoinedRoom()
    {
        statusText.text = "Entrando na sala " + PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.LoadLevel("WaitingRoom"); // Carrega a cena de espera
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Falha ao entrar na sala: " + message);
        statusText.text = "Falha ao entrar na sala: " + message; // Exibe mensagem de erro
    }
}
