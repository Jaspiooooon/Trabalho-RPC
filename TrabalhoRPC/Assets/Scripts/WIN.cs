using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviourPun
{
    // Função que será chamada quando um jogador cruzar a linha de chegada
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que colidiu é o jogador local
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            // Chama o RPC para todos na sala
            photonView.RPC("PlayerWIN", RpcTarget.All, PhotonNetwork.NickName);
        }
    }

    // Método RPC que será chamado em todos os clientes
    [PunRPC]
    public void PlayerWIN(string playerName)
    {
        SceneManager.LoadScene("WIN");
        
    }
}