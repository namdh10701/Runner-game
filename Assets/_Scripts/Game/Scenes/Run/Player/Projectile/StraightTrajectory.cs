using UnityEngine;

namespace Game.Run
{
    public class StraightTrajectory : ProjectileTrajectory
    {
        [SerializeField] private float speed = 60;
        public override void UpdateTrajectory()
        {
            _transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}