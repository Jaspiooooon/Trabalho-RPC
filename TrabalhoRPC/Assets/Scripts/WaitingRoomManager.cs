using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text playerListText; // TextMeshPro para exibir os jogadores
    public GameObject startGameButton; // Bot�o para o Master Client iniciar o jogo
    public GameObject LeaveLobbyButton; // Bit�ao para sair do lobby
    public TMP_Text roomNameText;  // Refer�ncia ao TMP_Text para mostrar o nome da sala
    private MapType selectedMap = MapType.Jogo;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerList();
        roomNameText.text = "Sala: " + PhotonNetwork.CurrentRoom.Name;

        // Apenas o Master Client pode ver o bot�o de iniciar o jogo
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerList(); // Atualiza a lista de jogadores a cada frame
    }

    // Atualiza a lista de jogadores exibida
    void UpdatePlayerList()
    {
        playerListText.text = "Jogadores na sala:\n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";
        }
    }

    // M�todo chamado quando um novo jogador entra na sala
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList(); // Atualiza a lista de jogadores
    }

    // M�todo chamado quando um jogador sai da sala
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList(); // Atualiza a lista de jogadores
    }

    // Bot�o para voltar ao lobby
    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom(); // Sai da sala e volta ao lobby
    }

    // Callback chamado quando o jogador sai da sala
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby"); // Carrega a cena do lobby
    }
    // Bot�o que o Master Client usa para iniciar o jogo
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length < 1)
            {
                Debug.Log("� necess�rio pelo menos 2 jogadores para iniciar o jogo.");
                return;
            }

            // Chama o RPC para iniciar o jogo
            photonView.RPC("gameStart", RpcTarget.All, (int)selectedMap); // Passa o mapa selecionado
        }
    }

    //� chamado pelo startGame para mudar a cena de todos para o mapa escolhido
    [PunRPC]
    public void gameStart(int mapindex)
    {
        // Carrega o mapa selecionado
        PhotonNetwork.LoadLevel("Jogo");     
    }
    public enum MapType
    {
        Jogo
    }
}
