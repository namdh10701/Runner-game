using Game.Run;
using UnityEngine;
public class AttackTrigger : MonoBehaviour
{
    [SerializeField] EnemyUnit _enemyUnit;
    public Unit Target;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = other.GetComponent<Unit>();
            Debug.Log(Target);
            _enemyUnit.Attack();
        }
    }
}