using System;
using UnityEngine;
using static Game.Run.InputManager;

namespace Game.Run
{
    public class PlayerMovement : MonoBehaviour
    {
        enum PlayerSpeedPreset
        {
            Slow,
            Medium,
            Fast,
            Custom
        }

        [SerializeField] PlayerSpeedPreset _playerSpeed = PlayerSpeedPreset.Medium;
        [SerializeField] float _customPlayerSpeed = 2f;
        [SerializeField] float _accelerationSpeed = 10.0f;
        [SerializeField] float _decelerationSpeed = 20.0f;
        [SerializeField] float _horizontalSpeedFactor = 0.5f;
        [SerializeField] bool _autoMoveForward = true;
        [SerializeField] float _horizontalSpeed;
        private Transform _transform;
        private bool _hasInput;
        private float _maxXPosition;
        private float _xPos;
        private float _zPos;
        private float _targetPosition;
        private float _speed;

        private float _targetSpeed;
        private Vector3 _startPosition;
        private Vector3 _lastPosition;
        public float DistancePerSecond { get; private set; }
        private void Awake()
        {
            _transform = transform;
            _lastPosition = _transform.position;
            _startPosition = _transform.position;
        }
        public float GetDefaultSpeed()
        {
            switch (_playerSpeed)
            {
                case PlayerSpeedPreset.Slow:
                    return 5.0f;
                case PlayerSpeedPreset.Medium:
                    return 10.0f;
                case PlayerSpeedPreset.Fast:
                    return 20.0f;
            }
            return _customPlayerSpeed;
        }

        internal void ResetAll()
        {
            _xPos = 0.0f;
            _zPos = _startPosition.z;
            _targetPosition = 0.0f;
            _lastPosition = _transform.position;
            _hasInput = false;
        }
        public void ResetSpeed()
        {
            _speed = 0.0f;
            _targetSpeed = GetDefaultSpeed();
        }

        public void AdjustSpeed(float speed)
        {
            _targetSpeed += speed;
            _targetSpeed = Mathf.Max(0.0f, _targetSpeed);
        }
        void Accelerate(float deltaTime, float targetSpeed)
        {
            _speed += deltaTime * _accelerationSpeed;
            _speed = Mathf.Min(_speed, targetSpeed);
        }

        void Decelerate(float deltaTime, float targetSpeed)
        {
            _speed -= deltaTime * _decelerationSpeed;
            _speed = Mathf.Max(_speed, targetSpeed);
        }

        bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }

        public void SetDeltaPosition(float swipeDelta)
        {
            if (_maxXPosition == 0.0f)
            {
                Debug.LogError("Player cannot move because SetMaxXPosition has never been called or Level Width is 0. If you are in the LevelEditor scene, ensure a level has been loaded in the LevelEditor Window!");
            }

            _targetPosition = _targetPosition + (_horizontalSpeed * swipeDelta);
            _targetPosition = Mathf.Clamp(_targetPosition, -_maxXPosition + PlayerController.Instance.UnitManager.LeftHalfPlayerSize, _maxXPosition - PlayerController.Instance.UnitManager.RightHalfPlayerSize);
            _xPos += _horizontalSpeed * Time.deltaTime * swipeDelta;
            _xPos = Mathf.Clamp(_xPos, -_maxXPosition + PlayerController.Instance.UnitManager.LeftHalfPlayerSize, _maxXPosition - PlayerController.Instance.UnitManager.RightHalfPlayerSize);
            _hasInput = true;
        }


        public void CancelMovement()
        {
            _hasInput = false;
        }

        public void SetMaxXPosition(float maxXPosition)
        {
            _maxXPosition = maxXPosition;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (!_autoMoveForward && !_hasInput)
            {
                Decelerate(deltaTime, 0.0f);
            }
            else if (_targetSpeed < _speed)
            {
                Decelerate(deltaTime, _targetSpeed);
            }
            else if (_targetSpeed > _speed)
            {
                Accelerate(deltaTime, _targetSpeed);
            }

            float speed = _speed * deltaTime;

            _zPos += speed;

            if (_hasInput)
            {
                //TODO if want to accelarate
                /*float horizontalSpeed = speed * _horizontalSpeedFactor;
                float newPositionTarget = Mathf.Lerp(_xPos, _targetPosition, horizontalSpeed);
                float newPositionDifference = newPositionTarget - _xPos;
                newPositionDifference = Mathf.Clamp(newPositionDifference, -horizontalSpeed, horizontalSpeed);
                _xPos += newPositionDifference;*/
            }
            else
            {
                _targetPosition = transform.position.x;
            }

            _transform.position = new Vector3(_xPos, _transform.position.y, _zPos);

            if (deltaTime > 0.0f)
            {
                float distanceTravelledSinceLastFrame = (_transform.position - _lastPosition).magnitude;
                DistancePerSecond = distanceTravelledSinceLastFrame / deltaTime;
            }


            /*   if (_transform.position != _lastPosition)
               {
                   _transform.forward = Vector3.Lerp(_transform.forward, (_transform.position - _lastPosition).normalized, speed);
               }*/

            _lastPosition = _transform.position;

        }
    }
}
