
using UnityEngine;

namespace Game.Run
{
    public class NormalBullet : Bullet
    {
        [SerializeField] private TrailRenderer _trailRenderer;

        public override void ResetBullet()
        {
            base.ResetBullet();
            _trailRenderer.Clear();
        }
        public override void UpdateBullet()
        {
            if (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime = 0;
                _pool.Release(this);
            }

            _transform.Translate(Vector3.forward * 60 * Time.deltaTime);
        }
        public override void OnHit()
        {
            _pool.Release(this);
        }
    }
}
