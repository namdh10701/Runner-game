using TMPro;
using UnityEngine;

namespace Game.Run
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private Transform _transform;
        public Transform Transform => _transform;
        public Animator Animator => _animator;
        public LoopActBehaviour LoopActBehaviour;

        private void Awake()
        {
            _transform = transform;
            _animator = GetComponent<Animator>();
            LoopActBehaviour = GetComponent<LoopActBehaviour>();
        }
    }
}
