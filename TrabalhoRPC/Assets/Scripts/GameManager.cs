using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject carPrefab;  // Referência ao prefab do carro (deve estar na pasta "Resources")
    public Transform spawnPoint;  // Ponto onde o carro vai aparecer
    #region
    // Instância estática do GameManager
    public static GameManager instance;

    // Método Awake é chamado antes de Start
    void Awake()
    {
        // Se não existir uma instância, defina essa como a única
        if (instance == null)
        {
            instance = this;  // Define a instância atual
            DontDestroyOnLoad(gameObject);  // Impede que o objeto seja destruído ao mudar de cena
        }
        else
        {
            Destroy(gameObject);  // Destrói qualquer instância extra
        }
    }
    #endregion
    
    

    void Start()
    {
        if (PhotonNetwork.IsConnected)  // Verifica se está conectado à Photon Network
        {
            if (PhotonNetwork.IsMasterClient)  // Apenas o Master Client cria o carro
            {
                // Instancia o carro via PhotonNetwork, sincronizando para todos os jogadores
                PhotonNetwork.Instantiate(carPrefab.name, spawnPoint.position, spawnPoint.rotation);
            }
        }
        else
        {
            // Instancia localmente se não estiver conectado ao Photon (para teste local)
            Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

public void carregarLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

}
