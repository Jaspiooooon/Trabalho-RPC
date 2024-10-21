using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class CarController : MonoBehaviour
{
    float turnSpeed = 120f;
    private Rigidbody2D rb;         // Refer�ncia ao Rigidbody2D
    private Vector2 movement;       // Dire��o do movimento
    public float maxSpeed = 20f;    // Velocidade m�xima
    public float acceleration = 5f; // Acelera��o do carro
    public float deceleration = 10f;// Taxa de desacelera��o
    private float currentSpeed = 0f;// Velocidade atual
    private CinemachineVirtualCamera cineMachineCam;
    public PhotonView photonView;
    private Vector2 networkPosition; // Para armazenar a posi��o recebida pela rede
    private float networkRotation;   // Para armazenar a rota��o recebida pela rede
    
    void Start()
    {
        // Pegando a refer�ncia do Rigidbody2D do objeto
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();

        // Inicializa as vari�veis de rede
        networkPosition = rb.position;
        networkRotation = rb.rotation;
        if (photonView.IsMine)
        {
            cineMachineCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (cineMachineCam != null ) 
            {
                cineMachineCam.Follow = this.transform;
            }
        }
    }
    

    void Update()
    {
        if (photonView.IsMine)
        {
            // Captura de input apenas no jogador local
            movement.x = Input.GetAxis("Horizontal");  // Para os lados (girar)
            movement.y = Input.GetAxis("Vertical");    // Para frente e para tr�s (acelera��o)

            // Manda a atualiza��o de posi��o e rota��o via RPC
            photonView.RPC("UpdateMovement", RpcTarget.Others, rb.position, rb.rotation, movement.x, movement.y);
        }
        else
        {
            // Caso n�o seja o jogador local, interpola a posi��o e rota��o para suavizar o movimento
            rb.position = Vector2.Lerp(rb.position, networkPosition, Time.deltaTime * 10);
            rb.rotation = Mathf.Lerp(rb.rotation, networkRotation, Time.deltaTime * 10);
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            // Acelera��o
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

            // Movimenta o carro
            rb.MovePosition(rb.position + (Vector2)(transform.up * currentSpeed * Time.fixedDeltaTime));

            // Rotaciona o carro
            rb.MoveRotation(rb.rotation - movement.x * turnSpeed * Time.fixedDeltaTime);
        }
    }

    // M�todo chamado via RPC para sincronizar movimento e rota��o
    [PunRPC]
    public void UpdateMovement(Vector2 newPosition, float newRotation, float horizontalInput, float verticalInput)
    {
        networkPosition = newPosition;  // Atualiza a posi��o recebida
        networkRotation = newRotation;  // Atualiza a rota��o recebida
    }
}
