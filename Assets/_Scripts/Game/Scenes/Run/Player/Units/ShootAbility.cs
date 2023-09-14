using UnityEngine;

namespace Game.Run
{
    //TODO: FindTarget, assign target to projectile
    public class ShootAbility : Ability
    {
        private Projectile _projectile;
        private ProjectilePool _bulletPool;
        [SerializeField] private Transform _shotPos;
        [SerializeField] private string _shotPool;
        Transform _target;

        public Projectile Projectile => _projectile;
        private void Awake()
        {
            _bulletPool = GameObject.FindGameObjectWithTag(_shotPool).GetComponent<ProjectilePool>();
        }
        public override void Use()
        {
            _projectile = _bulletPool.Pool.Get();
            _projectile.Trajectory.LaunchedPos = _shotPos.position;
        }

        public Transform FindTarget()
        {
            // Find all colliders within the specified range
            Collider[] colliders = Physics.OverlapSphere(transform.position, (_projectile.Trajectory.Range));

            // Initialize variables to track the closest target and distance
            Transform closestTarget = null;
            float closestDistance = float.MaxValue;

            // Iterate through all colliders
            foreach (Collider collider in colliders)
            {
                // Check if the collider belongs to an enemy (you can use tags or layers)
                if (collider.CompareTag("Enemy"))
                {
                    // Calculate the distance between the current collider and this GameObject
                    float distance = Vector3.Distance(transform.position, collider.transform.position);

                    // If the distance is less than the closest distance found so far, update the closest target
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = collider.transform;
                    }
                }
            }

            // Return the closest target (or null if no target is found)
            return closestTarget;
        }
    }
}