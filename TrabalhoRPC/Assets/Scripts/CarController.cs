using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static UnityEngine.RuleTile.TilingRuleOutput;



public class CarController : MonoBehaviour
{
    float turnSpeed = 50f;
    public float moveSpeed = 10f;  // Velocidade de movimento
    private Rigidbody2D rb;       // Referência ao Rigidbody2D
    private Vector2 movement;     // Direção do movimento
    public float maxSpeed = 20f;  // Velocidade máxima
    public float acceleration = 5f;  // Aceleração do carro
    public float deceleration = 10f;  // Taxa de desaceleração quando não estiver acelerando
    private float currentSpeed = 0f;  // Velocidade atual
    private PhotonView photonView;
    void Start()
    {
        // Pegando a referência do Rigidbody2D do objeto
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();

        // Verifica se o jogador é o cliente local
        if (!photonView.IsMine)
        {
            // Se não for o cliente local, desabilita os controles de entrada
            Destroy(this);
        }
    }

    void Update()
    {
        // Capturando o input do eixo Horizontal e Vertical
        movement.x = Input.GetAxis("Horizontal");  // Para os lados
        movement.y = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);
    }
    public void Move()
    {
        if (movement.y > 0)
        {
            // Acelera gradativamente até o máximo
            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);  // Limita a velocidade ao máximo permitido
        }
        else if (movement.y < 0)
        {
            // Desaceleração gradativa quando não estiver acelerando
            currentSpeed -= deceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);  // Garante que não fique negativo
        }
        // Movendo o carro para frente
        rb.MovePosition(rb.position + (Vector2)(transform.up * currentSpeed * Time.deltaTime));

        // Rotacionando o carro
        rb.MoveRotation(rb.rotation - movement.x * turnSpeed * Time.fixedDeltaTime);  // Gira de acordo com o input horizontal
        photonView.RPC("MoveRPC", RpcTarget.All);
    }
    [PunRPC]
    public void MoveRPC()
    {

    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }





    }
}