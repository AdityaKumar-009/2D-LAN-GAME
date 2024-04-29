using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nade : MonoBehaviour
{
    public float Force =10f;
    Rigidbody2D rb;
    Vector2 angle=Vector2.zero;
    public float lifetime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("Explosion", lifetime);
        angle.x = transform.rotation.z;
        rb.AddForce(angle*Force);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Explosion()
    {
        
        Invoke("destroyammo",0);
    }
    public void destroyammo()
    {
        Destroy(gameObject);
    }
}
