using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarController : MonoBehaviourPun
{


    public float speed = 10f; // Variável pública que define a velocidade do carro.
    public float turnSpeed = 5f; // Variável pública que define a velocidade de rotação do carro.

    private PhotonView photonView; // Cria uma referência para o PhotonView, usado para verificar a propriedade do objeto.

    void Start()
    {
        photonView = GetComponent<PhotonView>(); // Inicializa o PhotonView ao iniciar o jogo.
    }

    void Update()
    {
        if (photonView.IsMine) // Verifica se este carro pertence ao jogador local.
        {
            float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            // Captura o input vertical (teclas W/S ou setas) para movimentar o carro.

            float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            // Captura o input horizontal (teclas A/D ou setas) para girar o carro.

            transform.Translate(0, 0, move); // Move o carro para frente ou para trás com base no input.
            transform.Rotate(0, turn, 0); // Rotaciona o carro com base no input de rotação.
        }
    }

    // Função responsável por enviar e receber dados de rede.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Se este é o cliente local, envie dados.
        {
            stream.SendNext(transform.position); // Envia a posição do carro para outros jogadores.
            stream.SendNext(transform.rotation); // Envia a rotação do carro para outros jogadores.
        }
        else // Se este é o cliente remoto, receba dados.
        {
            transform.position = (Vector3)stream.ReceiveNext(); // Recebe e atualiza a posição do carro.
            transform.rotation = (Quaternion)stream.ReceiveNext(); // Recebe e atualiza a rotação do carro.
        }
    }
}

