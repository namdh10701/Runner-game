using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
namespace Game.Run
{
    public class ProjectilePool : MonoBehaviour
    {
        private ObjectPool<Projectile> _pool;
        public ObjectPool<Projectile> Pool => _pool;
        [SerializeField] private Projectile _projectilePrefab;
        public TextMeshProUGUI bulletCount;
        public int count = 0;


        public List<Projectile> activeProjectiles;
        private void Start()
        {
            _pool = new ObjectPool<Projectile>(OnCreate, OnGet, OnRelease, OnDestroyBullet, false, 150, 200);
            activeProjectiles = new List<Projectile>();
        }

        private Projectile OnCreate()
        {
            Projectile bullet;
            bullet = Instantiate(_projectilePrefab);
            bullet.SetPool(_pool);
            return bullet;
        }
        private void OnGet(Projectile bullet)
        {
            count++;
            activeProjectiles.Add(bullet);
            bullet.gameObject.SetActive(true);
            bulletCount.text = count.ToString();
        }
        private void OnRelease(Projectile projectile)
        {
            count--;
            projectile.ResetProjectile();
            activeProjectiles.Remove(projectile);
            bulletCount.text = count.ToString();
        }
        private void OnDestroyBullet(Projectile projectile)
        {
            count--;
            activeProjectiles.Remove(projectile);
            Destroy(projectile.gameObject);
        }


        private void Update()
        {
            foreach (Projectile prjoectile in activeProjectiles.ToArray())
            {
                prjoectile.UpdateProjectile();
            }
        }
    }
}
