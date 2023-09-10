using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
namespace Game.Run
{
    public class BulletPool : MonoBehaviour
    {
        private ObjectPool<Bullet> _pool;
        public ObjectPool<Bullet> Pool => _pool;
        [SerializeField] private Bullet _bulletPrefab;
        public TextMeshProUGUI bulletCount;
        public int count = 0;


        public List<Bullet> activeBullet;
        private void Start()
        {
            _pool = new ObjectPool<Bullet>(OnCreate, OnGet, OnRelease, OnDestroyBullet, false, 150, 200);
            activeBullet = new List<Bullet>();
        }

        private Bullet OnCreate()
        {
            Bullet bullet;
            bullet = Instantiate(_bulletPrefab);
            bullet.SetPool(_pool);

            return bullet;
        }
        private void OnGet(Bullet bullet)
        {
            count++;
            activeBullet.Add(bullet);
            bullet.gameObject.SetActive(true);
            bulletCount.text = count.ToString();
        }
        private void OnRelease(Bullet bullet)
        {
            count--;
            bullet.ResetBullet();
            activeBullet.Remove(bullet);
            bulletCount.text = count.ToString();
        }
        private void OnDestroyBullet(Bullet bullet)
        {
            count--;
            activeBullet.Remove(bullet);
            Destroy(bullet.gameObject);
        }


        private void Update()
        {
            foreach (Bullet bullet in activeBullet.ToArray())
            {
                bullet.UpdateBullet();
            }
        }
    }
}
