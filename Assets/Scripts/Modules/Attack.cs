using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Satelite))]
public class Attack : MonoBehaviour {
    public Satelite satelite;
    public float angle;
    public float rotationSpeed = 1f;
    public float reloadDuration = 1f;
    public GameObject bulletPrefab;

    private Timer isReloading;

    private BulletParent bulletParent;

    protected void Start()
    {
        satelite = GetComponent<Satelite>();
        angle = Random.value * 360f;
        bulletParent = Hierarchy.GetComponentWithTag<BulletParent>();
        isReloading = new Timer(reloadDuration);        
    }
    
    protected void Update()
    {
        if (!isReloading)
        {
            const float startOffset = 0.3f;
            const float lookDistance = 8f;
            var allColliders = Physics2D.OverlapCircleAll(transform.position, lookDistance);

            Collider2D closestCollider = null;
            float closestDistance = float.MaxValue;
            foreach (var collider in allColliders)
            {
                // TODO check for friendly or enemy colliders

                if (collider.transform == transform)
                    continue;

                var dist = (collider.transform.position - transform.position).magnitude;
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestCollider = collider;
                }
            }
            
            // Create the bullet
            if (closestCollider != null)
            {
                var direction = (closestCollider.transform.position - transform.position).normalized;
                Shoot(transform.position + direction * startOffset, direction);
            }

        }
        
    }
    
    private void Shoot(Vector3 position, Vector3 direction)
    {
        // Create the bullet
        var bulletInstance = Instantiate(bulletPrefab, position, Quaternion.identity, bulletParent.transform);
        var bullet = bulletInstance.GetComponent<Bullet>();
        bullet.isPlayer = satelite.IsPlayer;

        // Apply the bullet direction to the bullet
        bullet.SetDirection(direction);

        // Restart the reload
        isReloading = new Timer(reloadDuration);
    }

}
