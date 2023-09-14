using Game.Run;
using Game.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnit : MonoBehaviour
{
    public enum State
    {
        None, Chasing, Attacking
    }

    public void Attack()
    {
        CurrentState = State.Attacking;
        _animator.SetTrigger("Attack");
    }

    public void OnFinishAttack()
    {
        //Intermediate
        PlayerController.Instance.UnitManager.TakeHit(_dmgCause, AttackTrigger.Target);
        CurrentState = State.None;
    }

    ObjectPool<EnemyUnit> _pool;

    [SerializeField] SkinnedMeshRenderer[] _renderers;
    [SerializeField] Animator _animator;
    Transform _transform;
    [SerializeField] private float _dmg = 30;
    public float _totalHp = 100;
    public float _currentHp = 100;
    public State CurrentState;
    public AttackTrigger AttackTrigger;
    private DamageCause _dmgCause;
    public void SetPool(ObjectPool<EnemyUnit> pool)
    {
        _pool = pool;
    }
    private void OnEnable()
    {
        CurrentState = State.None;
    }
    private void Awake()
    {
        _dmgCause = gameObject.AddComponent<DamageCause>();
        _dmgCause.Dmg = _dmg;
        _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _animator = GetComponent<Animator>();
        _transform = transform;


    }
    public float invisibleDestroyTime = .5f;
    public float elapsedTime;
    public void DestroyIfInvisible()
    {
        if (!_renderers[0].isVisible)
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
        if (CurrentState == State.None)
        {
            CurrentState = State.Chasing;
        }
        if (CurrentState == State.Chasing)
        {
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
            Projectile projectile = other.GetComponent<Projectile>();
            projectile.OnHit();
            TakeHit(projectile.DamageCause.Dmg);
        }
    }
}
