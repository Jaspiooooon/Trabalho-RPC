using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CoinCollect : MonoBehaviourPun 
{
    public int coins = 0; // Vari�vel p�blica que armazena o n�mero de moedas coletadas.

    private PhotonView photonView; // Refer�ncia ao PhotonView para verificar a propriedade do carro.

    void Start()
    {
        photonView = GetComponent<PhotonView>(); // Inicializa o PhotonView ao iniciar o jogo.
    }

    void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && other.gameObject.CompareTag("Coin")) // Verifica se o carro � do jogador local e se colidiu com uma moeda.
        {
            coins++; // Aumenta o contador de moedas.
            PhotonNetwork.Destroy(other.gameObject); // Destr�i a moeda na rede (para todos os jogadores).
            // Aqui voc� poderia adicionar melhorias, como aumentar a velocidade ou manuseio.
        }
    }
}
