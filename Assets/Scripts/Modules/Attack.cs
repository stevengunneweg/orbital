using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Satelite))]
public class Attack : MonoBehaviour {

    public float angle;
    public float rotationSpeed = 1f;
    public float reloadDuration = 1f;
    public GameObject bulletPrefab;

    private bool isPlayer { get { return satelite.IsPlayer; } }

    private Timer isReloading;

    private BulletParent bulletParent;
    private Satelite satelite;

    protected void Start()
    {
        satelite = GetComponent<Satelite>();
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
            bullet.isPlayer = isPlayer;

            // Apply the bullet direction to the bullet
            bullet.SetDirection(bulletDirection);
            
        }
    }

}
