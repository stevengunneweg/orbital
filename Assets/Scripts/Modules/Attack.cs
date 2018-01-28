using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public float angle;
    public float rotationSpeed = 1f;
    public float reloadDuration = 1f;
    public GameObject bulletPrefab;
    GameObject axl;

    private Timer isReloading;

    private BulletParent bulletParent;

    protected void Start()
    {
        angle = Random.value * 360f;
        bulletParent = Hierarchy.GetComponentWithTag<BulletParent>();
        isReloading = new Timer(reloadDuration);        
    }

    int framesSinceLastShot = 0;
    protected void Update()
    {
        framesSinceLastShot++;
        if (framesSinceLastShot > 30)
        {
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(transform.position + (new Vector3(1, 0, 0) * 0.25f), new Vector3(1, 0, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(1, 0, 0) * 0.25f), new Vector3(1, 0, 0));
            }
            else if (hit = Physics2D.Raycast(transform.position + (new Vector3(-1, 0, 0) * 0.25f), new Vector3(-1, 0, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(-1, 0, 0) * 0.25f), new Vector3(-1, 0, 0));
            }
            else if (hit = Physics2D.Raycast(transform.position + (new Vector3(0, 1, 0) * 0.25f), new Vector3(0, 1, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(0, 1, 0) * 0.25f), new Vector3(0, 1, 0));

            }
            else if (hit = Physics2D.Raycast(transform.position + (new Vector3(0, -1, 0) * 0.25f), new Vector3(0, -1, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(0, -1, 0) * 0.25f), new Vector3(0, -1, 0));
            }

            else if (hit = Physics2D.Raycast(transform.position + (new Vector3(1, 1, 0) * 0.25f), new Vector3(1, 1, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(1, 1, 0) * 0.25f), new Vector3(1, 1, 0));
            }
            else if (hit = Physics2D.Raycast(transform.position + (new Vector3(-1, 1, 0) * 0.25f), new Vector3(-1, 1, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(-1, 1, 0) * 0.25f), new Vector3(-1, 1, 0));
            }
            else if (hit = Physics2D.Raycast(transform.position + (new Vector3(1, -1, 0) * 0.25f), new Vector3(1, -1, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(1, -1, 0) * 0.25f), new Vector3(1, -1, 0));

            }
            else if (hit = Physics2D.Raycast(transform.position + (new Vector3(-1, -1, 0) * 0.25f), new Vector3(-1, -1, 0)))
            {
                if (hit.transform != this.transform)
                    Shoot(transform.position + (new Vector3(-1, -1, 0) * 0.25f), new Vector3(-1, -1, 0));
            }
        }
        
    }


    private void Shoot(Vector3 position, Vector3 direction)
    {
        //Shoot
        // Create the bullet
        var bulletInstance = Instantiate(bulletPrefab, position, Quaternion.identity, bulletParent.transform);
        var bullet = bulletInstance.GetComponent<Bullet>();

        // Apply the bullet direction to the bullet
        bullet.SetDirection(direction);
        framesSinceLastShot = 0;
    }

}
