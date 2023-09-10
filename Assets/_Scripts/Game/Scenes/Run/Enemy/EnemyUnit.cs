using Game.Run;
using Game.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnit : MonoBehaviour
{
    ObjectPool<EnemyUnit> _pool;

    [SerializeField] SkinnedMeshRenderer _renderer;
    [SerializeField] Animator _animator;
    Transform _transform;
    public float _totalHp = 100;
    public float _currentHp = 100;
    public void SetPool(ObjectPool<EnemyUnit> pool)
    {
        _pool = pool;
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _transform = transform;
        
    }
    public float invisibleDestroyTime = .5f;
    public float elapsedTime;
    public void DestroyIfInvisible()
    {
        if (!_renderer.isVisible)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
        }
        if (elapsedTime >= invisibleDestroyTime)
        {
            Debug.Log("release");
            elapsedTime = 0;
            _pool.Release(this);
        }
    }
    public void AIAct()
    {
        _animator.SetFloat("Speed", 10);
        Vector3 direction = PlayerController.Instance.transform.position - _transform.position;
        direction.y = 0; // Ignore the Y-axis to prevent tilting
        direction.Normalize(); // Normalize the vector to get a unit vector

        // Rotate the enemy to face the player
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);
        }

        // Move the enemy towards the player
        transform.Translate(Vector3.forward * 3 * Time.deltaTime);
    }
    public void TakeHit(float dmg)
    {
        if (_currentHp > 0)
        {
            _currentHp -= dmg;
            if (_currentHp < 0)
            {
                _pool.Release(this);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.PLAYER_DMG_CAUSE_TAG))
        {
            Bullet projectileBehaviour = other.GetComponent<Bullet>();
            projectileBehaviour.OnHit();
            TakeHit(projectileBehaviour.Dmg);
        }
    }
}
