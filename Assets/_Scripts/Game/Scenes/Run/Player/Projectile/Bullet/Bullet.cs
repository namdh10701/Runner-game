
using UnityEngine;

namespace Game.Run
{
    public class Bullet : Projectile
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private float _maxTravelDistance = 10;
        public override void ResetProjectile()
        {
            base.ResetProjectile();
            _trailRenderer.Clear();
        }
        public override void UpdateProjectile()
        {
            base.UpdateProjectile();
            _projectileTrajectory.TraveledDistance = Vector3.Distance(_projectileTrajectory.LaunchedPos, transform.position);
            if (_projectileTrajectory.TraveledDistance > _maxTravelDistance)
            {
                _pool.Release(this);
            }
        }
        public override void OnHit()
        {
            _pool.Release(this);
        }
    }
}
