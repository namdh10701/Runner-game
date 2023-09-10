using Common;
using Game.Run.Level;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Run
{
    public class PlayerController : Singleton<PlayerController>
    {
        private LevelManager _levelManager;
        private PlayerMovement _playerMovement;
        private UnitManager _unitManager;

        public PlayerMovement PlayerMovement => _playerMovement;
        public UnitManager UnitManager => _unitManager;

        protected override void Awake()
        {
            base.Awake();
            _playerMovement = GetComponent<PlayerMovement>();
            _unitManager = GetComponent<UnitManager>();
        }

        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            _levelManager = FindFirstObjectByType<LevelManager>();
            SetMaxXPosition();
            _playerMovement.ResetAll();
            _playerMovement.ResetSpeed();
        }
        public void SetMaxXPosition()
        {
            _playerMovement.SetMaxXPosition(_levelManager.LevelDefinition.LevelWidth * _unitManager.HalfWidth);
        }
        public void ResetPlayer()
        {
            _playerMovement.ResetAll();
            _playerMovement.ResetSpeed();
        }

        public void ChangeBullet(bool isBullet2)
        {

        }
    }
}