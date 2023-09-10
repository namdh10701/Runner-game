using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Run
{
    public abstract class Bullet : MonoBehaviour
    {
        protected Transform _transform;
        protected ObjectPool<Bullet> _pool;
        [SerializeField] protected float dmg;
        public float Dmg => dmg;
        public float elapsedTime = 0;
        [SerializeField] protected BulletTrajectory _bulletTrajectory;
        protected virtual void Awake()
        {
            _transform = transform;
        }
        public virtual void ResetBullet()
        {
            gameObject.SetActive(false);
            elapsedTime = 0;
        }

        public void SetPool(ObjectPool<Bullet> pool)
        {
            _pool = pool;
        }
        public abstract void UpdateBullet();
        public abstract void OnHit();
    }
}