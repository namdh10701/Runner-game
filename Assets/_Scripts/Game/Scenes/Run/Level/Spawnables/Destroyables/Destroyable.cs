using UnityEngine;
using Game.Run.UI;
using Game.Shared;

namespace Game.Run.Level
{
    public abstract class Destroyable : Spawnable
    {
        [SerializeField] private float _totalHp;
        [SerializeField] private float _currentHp;
        private HpBar _hpBar;
        private bool _destroyed;

        Renderer[] _renderers;

        protected override void Awake()
        {
            base.Awake();
            _hpBar = GetComponentInChildren<HpBar>();
            _renderers = GetComponentsInChildren<Renderer>();
            _destroyed = false;
        }
        public void TakeHit(float dmg)
        {
            if (_currentHp > 0 && !_destroyed)
            {
                _currentHp -= dmg;
                _hpBar.OnUpdateHp(_currentHp / _totalHp);
                if (_currentHp < 0)
                {
                    OnDestroyed();
                }
            }
        }

        public virtual void OnDestroyed()
        {
            _destroyed = true;
            gameObject.SetActive(false);
        }
        public override void ResetSpawnable()
        {
            _destroyed = false;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constant.PLAYER_DMG_CAUSE_TAG))
            {
                Projectile projectile = other.GetComponent<Bullet>();
                projectile.OnHit();
                TakeHit(projectile.DamageCause.Dmg);
            }
        }
    }
}
