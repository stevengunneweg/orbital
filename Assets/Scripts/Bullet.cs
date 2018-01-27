using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 1f;
    public Vector3 direction;

    private Rigidbody2D rb;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    protected void Update()
    {
        rb.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
    }

}
