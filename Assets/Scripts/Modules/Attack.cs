using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public float angle;
    public float rotationSpeed = 1f;
    public float reloadDuration = 1f;
    public GameObject bulletPrefab;

    private Timer isReloading;

    private BulletParent bulletParent;

    protected void Start()
    {
        angle = Random.value * 360f;
        bulletParent = Hierarchy.GetComponentWithTag<BulletParent>();
        isReloading = new Timer(reloadDuration);
    }

    protected void Update()
    {
        // Turn gradually each step
        angle += rotationSpeed * Time.deltaTime;

        // Check wether reloading
        if (!isReloading)
        {
            // Reset reloading timer
            isReloading = new Timer(reloadDuration);

            // Calculate the bullet direction
            var bulletDirection = (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up).normalized;

            // Create the bullet
            var bulletInstance = Instantiate(bulletPrefab, transform.position + bulletDirection * 0.2f, Quaternion.identity, bulletParent.transform);
            var bullet = bulletInstance.GetComponent<Bullet>();

            // Apply the bullet direction to the bullet
            bullet.SetDirection(bulletDirection);
            
        }
    }

}
