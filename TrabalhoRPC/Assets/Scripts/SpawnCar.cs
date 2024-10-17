using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Importa Photon para funcionalidades de multiplayer.

public class SpawnCar : MonoBehaviourPunCallbacks // Define a classe para gerenciar o setup do multiplayer.
{
    public GameObject carPrefab; // Refer�ncia ao prefab do carro que ser� instanciado.

    void Start()
    {
        if (PhotonNetwork.IsConnected) // Verifica se o jogador est� conectado � rede Photon.
        {
            PhotonNetwork.Instantiate(carPrefab.name, GetRandomSpawnPosition(), Quaternion.identity);
            // Instancia o carro para o jogador com uma posi��o aleat�ria e rota��o padr�o.
        }
    }

    // Fun��o que retorna uma posi��o aleat�ria para spawnar o carro.
    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)); // Retorna uma posi��o aleat�ria no eixo X e Z.
    }
}
