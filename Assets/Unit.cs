using Game.Run;
using UnityEngine;
namespace Game.Run
{
    class Unit : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;

        [SerializeField] Transform _shootPoint;
        public GameObject BulletPrefab;
        Transform _transform;


        public void Shoot()
        {
            GameObject o = Instantiate(BulletPrefab);
            o.transform.position = _shootPoint.position;
        }

        private bool _isShootCoolDown;
        private float _cooldownTime = 0;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if (_animator != null)
            {
                _animator.SetFloat("Speed", PlayerController.Instance.DistancePerSecond);
            }
            if (_isShootCoolDown)
            {
                _cooldownTime += Time.deltaTime;
            }
            if (_cooldownTime > PlayerController.Instance.AttackRate)
            {
                _isShootCoolDown = false;
                _cooldownTime = 0;
            }
            if (!_isShootCoolDown)
            {
                Shoot();
                _isShootCoolDown = true;
            }

        }

    }
}
