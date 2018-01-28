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
	public Transform gfx;

	private Timer lifetime;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		updateLayer(IsPlayer);
		lifetime = new Timer(5);
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
		var angle = Vector3.SignedAngle(direction, Vector3.up, Vector3.forward);
		gfx.Rotate(0, 0, -angle);
    }

    protected void Update()
    {
		rb.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
		if (!lifetime) {
			Destroy(gameObject);
		}
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(this.gameObject);
    }
}
