using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarController : MonoBehaviour
{
    float turnSpeed = 50f;
    public float moveSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float deceleration = 10f;
    private float currentSpeed = 0f;
    private PhotonView photonView;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();

        // Verifica se o jogador é o cliente local
        if (!photonView.IsMine)
        {
            // Se não for o cliente local, ainda queremos que o carro seja sincronizado
            // Desabilitar controle de input, mas manter física e sincronização
            GetComponent<Rigidbody2D>().isKinematic = true; // Para impedir movimentação por forças locais
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Capturando o input do eixo Horizontal e Vertical
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);
        }
    }

    public void Move()
    {
        if (movement.y > 0)
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }
        else if (movement.y < 0)
        {
            currentSpeed -= deceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }

        // Movendo o carro para frente
        rb.MovePosition(rb.position + (Vector2)(transform.up * currentSpeed * Time.deltaTime));

        // Rotacionando o carro
        rb.MoveRotation(rb.rotation - movement.x * turnSpeed * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }
    }
}
