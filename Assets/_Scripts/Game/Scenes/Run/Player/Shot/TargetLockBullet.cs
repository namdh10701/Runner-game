
using UnityEngine;

namespace Game.Run
{
    public class TargetLockBullet : Bullet
    {
        private float _flyTime;
        public enum State
        {
            LAUNCHED, ROTATING, LOCKED
        }
        public Transform target;
        public State CurrentState;
        [SerializeField] private TrailRenderer _trailRenderer;

        protected override void Awake()
        {
            base.Awake();
            _transform.forward = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(.3f, 1f), 1);
        }

        private void OnEnable()
        {
            CurrentState = State.LAUNCHED;
            target = null;
        }

        public override void OnHit()
        {
            _pool.Release(this);
        }
        public override void ResetBullet()
        {
            base.ResetBullet();
            _trailRenderer.Clear();
            _flyTime = 0;
        }

        public override void UpdateBullet()
        {
            UpdateState();
            if (CurrentState == State.LAUNCHED)
            {
                FindClosestEnemy();
            }
            else if (CurrentState == State.ROTATING)
            {
                RotateBullet();
            }
            _transform.Translate(Vector3.forward * 60 * Time.deltaTime);
        }
        private void UpdateState()
        {
            _flyTime += Time.deltaTime;
            if (_flyTime < .5f)
            {
                CurrentState = State.LAUNCHED;
            }
            else if (_flyTime > .5f && _flyTime < 1f)
            {
                CurrentState = State.ROTATING;
            }
            else
            {
                CurrentState = State.LOCKED;
            }
        }

        private void RotateBullet()
        {
            if (target != null)
            {
                Vector3 direction = target.position - _transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 40);
            }
        }

        private void FindClosestEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 1000, RunManager.Instance.EnemyLayer);
            float closestDistance = Mathf.Infinity;

            foreach (Collider enemy in enemies)
            {
                if (enemy.transform.position.z > _transform.position.z)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        target = enemy.transform;
                    }
                }
            }
        }
    }
}
