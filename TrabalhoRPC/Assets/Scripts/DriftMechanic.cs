using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class DriftMechanic : MonoBehaviourPun // Define a classe para a mecânica de drift.
{
    public float driftFactor = 0.95f; // Define o fator de drift (perda de tração).

    private PhotonView photonView; // Cria uma referência para o PhotonView.

    void Start()
    {
        photonView = GetComponent<PhotonView>(); // Inicializa o PhotonView ao iniciar o jogo.
    }

    void Update()
    {
        if (photonView.IsMine) // Verifica se este carro pertence ao jogador local.
        {
            if (Input.GetKey(KeyCode.Space)) // Verifica se a barra de espaço está sendo pressionada (drift).
            {
                GetComponent<Rigidbody>().drag = driftFactor; // Aplica o drift alterando o "drag" do carro.
            }
            else
            {
                GetComponent<Rigidbody>().drag = 1f; // Retorna o "drag" ao valor normal quando não está driftando.
            }
        }
    }
}
