using UnityEngine;

namespace Game.Run
{
    public class DamageCause : MonoBehaviour
    {
        [SerializeField] private float _dmg;
        public float Dmg
        {
            get
            {
                return _dmg;
            }
            set
            {
                _dmg = value;
            }
        }
    }
}