using Photon.Pun;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints; // Array com os pontos de spawn
    private List<int> usedSpawnIndices = new List<int>(); // Lista de índices já usados


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }
    //Chamar metodo Spawn players quando entrar na Partida
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SpawnPlayer(); // Garantindo que o jogador spawne ao entrar na sala
    }

    //Dar spawn nos players em 1 dos 4 spawn points diferentes
    void SpawnPlayer()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Nenhum ponto de spawn foi atribuído.");
            return;
        }

        // Embaralha os pontos de spawn no início
        int spawnIndex = GetRandomSpawnPoint();
        Vector3 spawnPosition = spawnPoints[spawnIndex].position;

        GameObject player = PhotonNetwork.Instantiate("carPrefab", spawnPosition, Quaternion.identity);

        if (player.GetComponent<PhotonView>().IsMine)
        {
            CameraController CameraController = FindObjectOfType<CameraController>();
            CameraController?.SetCameraFollow(player.transform);
        }

        photonView.RPC("MarkSpawnPointAsUsed", RpcTarget.AllBuffered, spawnIndex);
    }

    //Pegar um dos 4 spawpoints para dar spawn 
    int GetRandomSpawnPoint()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        } while (usedSpawnIndices.Contains(randomIndex));

        return randomIndex;
    }

    //Marca um spawn como usado para não ocorrer mais de um player por spawn
    [PunRPC]
    void MarkSpawnPointAsUsed(int spawnIndex)
    {
        if (!usedSpawnIndices.Contains(spawnIndex))
        {
            usedSpawnIndices.Add(spawnIndex);
        }
    }
}