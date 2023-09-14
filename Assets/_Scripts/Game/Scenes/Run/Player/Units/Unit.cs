using Game.Run.UI;
using TMPro;
using UnityEngine;

namespace Game.Run
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private float TotalHp;
        [SerializeField] private float CurrentHp;
        [SerializeField] private Animator _animator;
        private Transform _transform;
        public Transform Transform => _transform;
        public Animator Animator => _animator;
        public Ability Ability;
        public HpBar hpBar;

        public void TakeHit(float dmg)
        {
            CurrentHp -= dmg;
            hpBar.OnUpdateHp(CurrentHp);
            hpBar.gameObject.SetActive(false);
        }

        private void Awake()
        {
            _transform = transform;
            _animator = GetComponent<Animator>();
            Ability = GetComponent<Ability>();
            hpBar.gameObject.SetActive(false);
        }
    }
}
