using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static UnityEngine.RuleTile.TilingRuleOutput;



public class CarController : MonoBehaviour
{
    float turnSpeed = 6.0f;
    public float moveSpeed = 5f;  // Velocidade de movimento
    private Rigidbody2D rb;       // Refer�ncia ao Rigidbody2D
    private Vector2 movement;     // Dire��o do movimento
    public float maxSpeed = 20f;  // Velocidade m�xima
    public float acceleration = 5f;  // Acelera��o do carro
    public float deceleration = 10f;  // Taxa de desacelera��o quando n�o estiver acelerando
    private float currentSpeed = 0f;  // Velocidade atual
    void Start()
    {
        // Pegando a refer�ncia do Rigidbody2D do objeto
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Capturando o input do eixo Horizontal e Vertical
        movement.x = Input.GetAxis("Horizontal");  // Para os lados
        movement.y = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
    }

    void FixedUpdate()
    {
        if (movement.y > 0)
        {
            // Acelera gradativamente at� o m�ximo
            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);  // Limita a velocidade ao m�ximo permitido
        }
        else
        {
            // Desacelera��o gradativa quando n�o estiver acelerando
            currentSpeed -= deceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);  // Garante que n�o fique negativo
        }
        // Movendo o carro para frente
        rb.MovePosition(rb.position + (Vector2)(transform.up * movement.y * moveSpeed * Time.deltaTime));

        // Rotacionando o carro
        rb.MoveRotation(rb.rotation - movement.x * turnSpeed * Time.fixedDeltaTime);  // Gira de acordo com o input horizontal

       
        

    }
}






