using UnityEngine;

namespace Game.Run
{
    public abstract class ProjectileTrajectory : MonoBehaviour
    {
        protected Transform _transform;
        protected Vector3 _launchedPos;
        [SerializeField] private float _range;
        public float Range => _range;
        public Vector3 LaunchedPos
        {
            get
            {
                return _launchedPos;
            }
            set
            {
                _launchedPos = value;
                transform.position = value;
            }
        }
        public float TraveledDistance;
        protected virtual void Awake()
        {
            _transform = transform;
        }
        public abstract void UpdateTrajectory();
    }
}
