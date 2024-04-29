using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    /*public UltimateGunScript parent;*/
    public float speed = 3f;
    public float lifetime = 5f;
    static Audio audio;
    public ulong id=0;

    void Awake()
    {
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("ID in Bullet class: " + id);
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyAmmoRpc), lifetime);
        audio.playpistol();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime * -1, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        Debug.Log("Collision Object: "+collision.gameObject.name);

        if(GetComponent<NetworkObject>().IsSpawned)
        {
            DestroyAmmoRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void DestroyAmmoRpc()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

}
