using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject carPrefab;  // Refer�ncia ao prefab do carro (deve estar na pasta "Resources")
    public Transform spawnPoint;  // Ponto onde o carro vai aparecer
    #region
    // Inst�ncia est�tica do GameManager
    public static GameManager instance;

    // M�todo Awake � chamado antes de Start
    void Awake()
    {
        // Se n�o existir uma inst�ncia, defina essa como a �nica
        if (instance == null)
        {
            instance = this;  // Define a inst�ncia atual
            DontDestroyOnLoad(gameObject);  // Impede que o objeto seja destru�do ao mudar de cena
        }
        else
        {
            Destroy(gameObject);  // Destr�i qualquer inst�ncia extra
        }
    }
    #endregion
    
    

    void Start()
    {
        if (PhotonNetwork.IsConnected)  // Verifica se est� conectado � Photon Network
        {
            if (PhotonNetwork.IsMasterClient)  // Apenas o Master Client cria o carro
            {
                // Instancia o carro via PhotonNetwork, sincronizando para todos os jogadores
                PhotonNetwork.Instantiate(carPrefab.name, spawnPoint.position, spawnPoint.rotation);
            }
        }
        else
        {
            // Instancia localmente se n�o estiver conectado ao Photon (para teste local)
            Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

public void carregarLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

}
