using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;

public class SpawningPlayer1 : NetworkBehaviour
{
    public GameObject Panel;
    public Text win;
    public Text score;
    int sc = -1;
    public bool playerIsDead = true;
    public GameObject[] spawnPoints;
    public GameObject[] Characters;

    public static SpawningPlayer1 instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    public override void OnNetworkSpawn()
    {
               
        sc++;
        score.text = "Player1 Score: " + sc;

        // SpawnPlayerRpc(NetworkManager.LocalClientId);

        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;

        playerIsDead = false;

        

    }

    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsHost)
        {
            foreach (ulong client in clientsCompleted)
            {
                Debug.Log("SpawnPlayerCalled: " + client);
                int randomSpawn = Random.Range(0, 4);
                int randomChar = Random.Range(0, 4);
                GameObject newPlayer = Instantiate(Characters[randomChar], spawnPoints[randomSpawn].transform.position, Quaternion.identity);
                NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
                netObj.SpawnAsPlayerObject(client, true);
            }
        }

    }

    [Rpc(SendTo.Server)] //server owns this object but client can request a spawn
    public void SpawnPlayerRpc(ulong clientId)
    {
        Debug.Log("SpawnPlayerCalled: " + clientId);
        int randomSpawn = Random.Range(0, 4);
        int randomChar = Random.Range(0, 4);
        GameObject newPlayer = Instantiate(Characters[randomChar], spawnPoints[randomSpawn].transform.position, Quaternion.identity);
        NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (sc == 10)
        {
            /*Panel.SetActive(true);
            Time.timeScale = 0.0f;
            win.text = "Player 1 Wins";*/
            Debug.Log("Player 1 wins!");
        }
        if (playerIsDead)
        {
            sc++;
            score.text = "Player1 Score: " + sc;
            int randomSpawn = Random.Range(0, 4);
            int randomChar = Random.Range(0, 4); 
            Instantiate(Characters[randomChar], spawnPoints[randomSpawn].transform.position, Quaternion.identity);
            playerIsDead = false;
        }
    }

    public void OnMainMenu()
    {
        
        SceneManager.LoadScene(0);
        
    }

}
