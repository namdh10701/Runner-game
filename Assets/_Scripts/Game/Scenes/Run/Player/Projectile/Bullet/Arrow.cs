
using UnityEngine;

namespace Game.Run
{
    public class Arrow : Projectile
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        public override void ResetProjectile()
        {
            base.ResetProjectile();
            _trailRenderer.Clear();
        }
        public override void UpdateProjectile()
        {
            base.UpdateProjectile();
        }
        public override void OnHit()
        {
            _pool.Release(this);
        }
    }
}
