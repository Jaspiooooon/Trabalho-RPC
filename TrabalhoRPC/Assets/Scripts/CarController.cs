using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarController : MonoBehaviourPun
{


    public float speed = 10f; // Vari�vel p�blica que define a velocidade do carro.
    public float turnSpeed = 5f; // Vari�vel p�blica que define a velocidade de rota��o do carro.
    Rigidbody rb;
    private PhotonView photonView; // Cria uma refer�ncia para o PhotonView, usado para verificar a propriedade do objeto.

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>(); // Inicializa o PhotonView ao iniciar o jogo.
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Move();
        }
    }

    void Move()
    {
         // Verifica se este carro pertence ao jogador local.
        {
            float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            // Captura o input vertical (teclas W/S ou setas) para movimentar o carro.

            float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            // Captura o input horizontal (teclas A/D ou setas) para girar o carro.

            transform.Translate(move, 0, 0 ); // Move o carro para frente ou para tr�s com base no input.
            transform.Rotate(0, turn, 0); // Rotaciona o carro com base no input de rota��o.
            photonView.RPC("moveRPC", RpcTarget.All, move, turn);
        }
    }
    [PunRPC]
    void moveRPC(float horizontal, float vertical)
    {
        Vector2 movement = new Vector2(horizontal, vertical);
        rb.velocity = movement * speed;
    }

    // Fun��o respons�vel por enviar e receber dados de rede.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Se este � o cliente local, envie dados.
        {
            stream.SendNext(transform.position); // Envia a posi��o do carro para outros jogadores.
            stream.SendNext(transform.rotation); // Envia a rota��o do carro para outros jogadores.
        }
        else // Se este � o cliente remoto, receba dados.
        {
            transform.position = (Vector3)stream.ReceiveNext(); // Recebe e atualiza a posi��o do carro.
            transform.rotation = (Quaternion)stream.ReceiveNext(); // Recebe e atualiza a rota��o do carro.
        }
    }
}

