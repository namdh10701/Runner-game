using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Run
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Transform _transform;
        protected ObjectPool<Projectile> _pool;
        [SerializeField] protected DamageCause _damageCause;
        [SerializeField] protected ProjectileTrajectory _projectileTrajectory;

        public DamageCause DamageCause => _damageCause;
        public ProjectileTrajectory Trajectory => _projectileTrajectory;
        protected virtual void Awake()
        {
            _transform = transform;
        }
        public virtual void ResetProjectile()
        {
            gameObject.SetActive(false);
        }

        public void SetPool(ObjectPool<Projectile> pool)
        {
            _pool = pool;
        }
        public virtual void UpdateProjectile()
        {
            _projectileTrajectory.UpdateTrajectory();
        }
        public abstract void OnHit();
    }
}