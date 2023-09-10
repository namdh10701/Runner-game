using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Run
{
    public class EnemyPool : MonoBehaviour
    {
        ObjectPool<EnemyUnit> _pool;
        public ObjectPool<EnemyUnit> Pool => _pool;
        public EnemyUnit enemyPrefab;
        List<EnemyUnit> activeUnits;

        public TextMeshProUGUI enemyCount;
        public TextMeshProUGUI rateText;
        public int count = 0;
        private void Start()
        {
            _pool = new ObjectPool<EnemyUnit>(OnCreate, OnGet, OnRelease, OnDestroyBullet, false, 250, 300);
            activeUnits = new List<EnemyUnit>();
        }

        private EnemyUnit OnCreate()
        {
            EnemyUnit unit = Instantiate(enemyPrefab);
            unit.SetPool(_pool);
            return unit;
        }
        private void OnGet(EnemyUnit unit)
        {
            count++;
            activeUnits.Add(unit);
            unit.gameObject.SetActive(true);
            enemyCount.text = activeUnits.Count.ToString();
            unit.transform.position = PlayerController.Instance.transform.position + new Vector3(Random.Range(-7.0f, 7.0f), 0, 70);
        }
        private void OnRelease(EnemyUnit unit)
        {
            count--;
            activeUnits.Remove(unit);
            unit._currentHp = 100;
            unit.gameObject.SetActive(false);
            enemyCount.text = activeUnits.Count.ToString();
        }
        private void OnDestroyBullet(EnemyUnit unit)
        {
            count--;
            activeUnits.Remove(unit);
            Destroy(unit.gameObject);
        }

        public float spawnRate = .7f;
        public float elapsedTime;

        private void Update()
        {
            if (elapsedTime <= spawnRate)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                _pool.Get();
                elapsedTime = 0;
            }

            foreach (EnemyUnit unit in activeUnits.ToArray())
            {
                unit.DestroyIfInvisible();
                unit.AIAct();
            }
        }

        public void IncreaseRate()
        {
            spawnRate += .1f;
            rateText.text = spawnRate.ToString("0.0");
        }
        public void DecreaseRate()
        {

            spawnRate -= .1f;
            rateText.text = spawnRate.ToString("0.0");
        }
    }
}
