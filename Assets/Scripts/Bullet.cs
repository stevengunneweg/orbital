using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 1f;
    public Vector3 direction;

    public bool isPlayer;
    public bool IsPlayer
    {
        get
        {
            return isPlayer;
        }
        set
        {
            isPlayer = value;
            updateLayer(value);
        }
    }

    private Rigidbody2D rb;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        updateLayer(IsPlayer);
    }

    private void updateLayer(bool isPlayer)
    {
        SetLayer(gameObject, isPlayer ? LayerMask.NameToLayer("Player_Projectiles") : LayerMask.NameToLayer("Enemy_Projectiles"));
    }

    private void SetLayer(GameObject root, int layer)
    {
        root.layer = layer;
        for (int i = 0; i < root.transform.childCount; ++i)
            SetLayer(transform.GetChild(i).gameObject, layer);
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
