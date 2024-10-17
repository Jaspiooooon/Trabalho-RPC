using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Importa Photon para funcionalidades de multiplayer.

public class SpawnCar : MonoBehaviourPunCallbacks // Define a classe para gerenciar o setup do multiplayer.
{
    public GameObject carPrefab; // Referência ao prefab do carro que será instanciado.

    void Start()
    {
        if (PhotonNetwork.IsConnected) // Verifica se o jogador está conectado à rede Photon.
        {
            PhotonNetwork.Instantiate(carPrefab.name, GetRandomSpawnPosition(), Quaternion.identity);
            // Instancia o carro para o jogador com uma posição aleatória e rotação padrão.
        }
    }

    // Função que retorna uma posição aleatória para spawnar o carro.
    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)); // Retorna uma posição aleatória no eixo X e Z.
    }
}
