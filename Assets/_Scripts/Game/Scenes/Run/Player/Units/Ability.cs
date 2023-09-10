using UnityEngine;

namespace Game.Run
{
    public abstract class Ability : MonoBehaviour
    {
        public bool IsAbilityCooldown;
        public float AbilityCooldownTime;
        public float AbilityUseRate;
        public abstract void Use();
    }
}