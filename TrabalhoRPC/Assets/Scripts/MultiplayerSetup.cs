using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetup : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints; // Array com os pontos de spawn
    private List<int> usedSpawnIndices = new List<int>(); // Lista de �ndices j� usados


    // Start is called before the first frame update
    void Start()
    {
        // Agora o SpawnPlayer s� � chamado quando o jogador entra na sala
        if (PhotonNetwork.InRoom)
        {
            // Certifique-se de que apenas o jogador mestre fa�a isso no Start
            SpawnPlayer();
        }
    }

    // Chamar m�todo Spawn players quando entrar na Partida
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SpawnPlayer(); // Garantindo que o jogador spawne ao entrar na sala
    }

    // Dar spawn nos players em 1 dos spawn points diferentes
    [PunRPC]
    void SpawnPlayer()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Nenhum ponto de spawn foi atribu�do.");
            return;
        }

        // Embaralha os pontos de spawn no in�cio
        int spawnIndex = GetRandomSpawnPoint();
        Vector3 spawnPosition = spawnPoints[spawnIndex].position;

        // Instanciar o prefab do jogador
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

        if (player.GetComponent<PhotonView>().IsMine)
        {
            // Configurar a c�mera para seguir o jogador instanciado
            CameraController cameraController = FindObjectOfType<CameraController>();
            cameraController?.SetCameraFollow(player.transform);
        }

        // Notifica todos os clientes sobre o ponto de spawn usado
        photonView.RPC("MarkSpawnPointAsUsed", RpcTarget.AllBuffered, spawnIndex);
    }

    // Pega um ponto de spawn aleat�rio que ainda n�o foi usado
    int GetRandomSpawnPoint()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        } while (usedSpawnIndices.Contains(randomIndex));

        return randomIndex;
    }

    // Marca um ponto de spawn como usado globalmente
    [PunRPC]
    void MarkSpawnPointAsUsed(int spawnIndex)
    {
        if (!usedSpawnIndices.Contains(spawnIndex))
        {
            usedSpawnIndices.Add(spawnIndex);
        }
    }
}
