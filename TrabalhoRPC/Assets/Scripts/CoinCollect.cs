using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CoinCollect : MonoBehaviourPun 
{
    public int coins = 0; // Variável pública que armazena o número de moedas coletadas.

    private PhotonView photonView; // Referência ao PhotonView para verificar a propriedade do carro.

    void Start()
    {
        photonView = GetComponent<PhotonView>(); // Inicializa o PhotonView ao iniciar o jogo.
    }

    void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && other.gameObject.CompareTag("Coin")) // Verifica se o carro é do jogador local e se colidiu com uma moeda.
        {
            coins++; // Aumenta o contador de moedas.
            PhotonNetwork.Destroy(other.gameObject); // Destrói a moeda na rede (para todos os jogadores).
            // Aqui você poderia adicionar melhorias, como aumentar a velocidade ou manuseio.
        }
    }
}
