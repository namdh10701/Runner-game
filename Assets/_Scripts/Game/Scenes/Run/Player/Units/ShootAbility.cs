using UnityEngine;

namespace Game.Run
{
    public class ShootAbility : Ability
    {
        private Bullet _bullet;
        private BulletPool _bulletPool;
        [SerializeField] private Transform _shotPos;
        [SerializeField] private string _shotPool;


        private void Awake()
        {
            _bulletPool = GameObject.FindGameObjectWithTag(_shotPool).GetComponent<BulletPool>();
        }
        public override void Use()
        {
            _bullet = _bulletPool.Pool.Get();
            _bullet.transform.position = _shotPos.position;
        }
    }
}