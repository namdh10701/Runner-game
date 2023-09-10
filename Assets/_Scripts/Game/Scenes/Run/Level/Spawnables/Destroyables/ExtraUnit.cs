using UnityEngine;
namespace Game.Run.Level.Destroyables
{
    public class ExtraUnit : Destroyable
    {
        //TODO: UI here
        [SerializeField] private int _numberOfUnit;

        public override void OnDestroyed()
        {
            base.OnDestroyed();
            PlayerController.Instance.UnitManager.SpawnUnit(_numberOfUnit);
        }
    }
}
