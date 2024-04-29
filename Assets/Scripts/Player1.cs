using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class Player1 : NetworkBehaviour
{

    Vector2 inputMove;
    Rigidbody2D myrigidbody2D;
    Animator myAnimaitor;
    LayerMask platform;
    CapsuleCollider2D player_collider;
    private int _score, _initial;
    [SerializeField] private int maxScore;
    public ulong playerID;

    [SerializeField] private Sprite[] Characters;
    [SerializeField] private RuntimeAnimatorController[] Animators;
    [SerializeField] private GameObject[] spawnPoints;

    float Speed = 10.0f;

    private static Audio audio;
    
    void Awake()
    {
        _score = 0;
        _initial = maxScore;
        // canvas = GameObject.FindGameObjectWithTag("Canvas");
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        myAnimaitor = GetComponent<Animator>();
        player_collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        Movement();
    }

    public override void OnNetworkSpawn()
    {
        // playerID = NetworkManager.Singleton.LocalClientId;
        // Debug.Log("Bullet_ID when network spawned for Player1: "+ playerID);
        Random_Configuration();
    }

    void Movement()
    {

        Flipper();

        if (Input.GetKey(KeyCode.W))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {

            if (Input.GetKey(KeyCode.A))
            {
                LeftMove();
            }
            else
            {
                RightMove();
            }

            audio.playwalk();
        }

    }

    public void Jump()
    {
        if (!player_collider.IsTouchingLayers(LayerMask.GetMask("Destructable")))
        { return; }

        myrigidbody2D.velocity = new Vector2(myrigidbody2D.velocity.x, Speed * 2.5f);
        myAnimaitor.SetTrigger("jump");
    }

    public void LeftMove()
    {
        myrigidbody2D.velocity = new Vector2(-Speed, myrigidbody2D.velocity.y);
    }

    public void RightMove()
    {
        myrigidbody2D.velocity = new Vector2(Speed, myrigidbody2D.velocity.y);
    }

    void Flipper()
    {
        Vector3 flip = transform.localScale;

        // Use a small threshold to account for floating-point precision
        float threshold = 0.01f;

        if (myrigidbody2D.velocity.x < -threshold)
        {
            flip.x = 2.5f;
            myAnimaitor.SetBool("Run", true);
        }
        else if (myrigidbody2D.velocity.x > threshold)
        {
            flip.x = -2.5f;
            myAnimaitor.SetBool("Run", true);
        }
        else
        {
            myAnimaitor.SetBool("Run", false);
        }

        transform.localScale = flip;
    }

    private void OnCollisionEnter2D(Collision2D collision) // PROBLEMATIC
    {
        /*        if(GetComponent<UltimateGunScript>().bullet_id != NetworkManager.Singleton.LocalClientId)
                {
                    Debug.Log("");// && collision.gameObject.GetComponent<Bullet>().id != playerID)
                }*/

        Debug.Log("Collision Object: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("Bullet_ID in Player1 class: " + collision.gameObject.GetComponent<Bullet>().id);
            Random_Configuration();
            --maxScore;

            _score = _initial - maxScore;

            if (_score == _initial)
            {

                DestroyCharacterRpc();
                Debug.Log("Max_Score Reached 0");

            }

        }
    }

    [Rpc(SendTo.Everyone)]
    public void DisconnectRpc()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("LeaderBoard");
    }

    [Rpc(SendTo.Server)]
    public void DestroyCharacterRpc()
    {
        DisconnectRpc();
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

    public void Random_Configuration()
    {
        int random = Random.Range(0, 4);

        /*GetComponent<SpriteRenderer>().sprite = Characters[random];
        Animator anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = Animators[random];
        ClientNetworkAnimator networkAnimator = GetComponent<ClientNetworkAnimator>();
        networkAnimator.Animator = anim;*/

        transform.position = new Vector3(spawnPoints[random].transform.position.x, spawnPoints[random].transform.position.y, 0);
    }

}


