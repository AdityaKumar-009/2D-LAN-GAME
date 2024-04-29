using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructableTiles : NetworkBehaviour
{
    public Tilemap destructableTilemap;
    static Audio audio;

    NetworkList<Vector2> destructionLocation = new();

    void Awake()
    {
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }
    // Start is called before the first frame update
    void Start()
    {
        destructableTilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Vector2 hitPoint in destructionLocation)
        {
            destructableTilemap.SetTile(destructableTilemap.WorldToCell(hitPoint), null);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOwner && collision.gameObject.CompareTag("Bullet"))
        {
            Vector3 hitPosition = Vector3.zero;
            
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                // Debug.Log("HITPOINT X: "+hitPosition.x+" | HITPOINT Y: "+hitPosition.y);
                DestructionLocationAddRpc(hitPosition.x, hitPosition.y);

            }
            
        }
    }

    [Rpc(SendTo.Server)]
    private void DestructionLocationAddRpc(float hitPositionX, float hitPositionY)
    {
        destructionLocation ??= new();
        destructionLocation.Add(new Vector2(hitPositionX,hitPositionY));
        // Debug.Log("Added in VAR -------- > HITPOINT X: " + hitPositionX + " | HITPOINT Y: " + hitPositionY);
    }

}
