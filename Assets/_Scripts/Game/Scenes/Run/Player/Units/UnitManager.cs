using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Game.Run
{
    public class UnitManager : MonoBehaviour
    {
        public TextMeshProUGUI unit;
        public GameObject UnitPrefab;

        int[] patternOrder = {
            21, 14, 11, 15, 22,
            13, 5, 3, 6, 16,
            9, 1, 0, 2, 10,
            20, 7, 4, 8, 17,
            24, 19, 12, 18, 23
        };
        Dictionary<int, Vector2> positionToCoordinates = new Dictionary<int, Vector2>();

        public void TakeHit(DamageCause damageCause, Unit unit)
        {
            if (ActiveUnits.Contains(unit))
            {
                unit.TakeHit(damageCause.Dmg);
            }
        }



        private int _gridHeight = 5;
        private int _gridWidth = 5;
        private int _maxUnitNumber = 25;

        private int _targetUnitNumber;
        private int _currentUnitNumber;
        private float _mapGridSize = 1;
        public float HalfWidth { get; private set; }
        public float LeftHalfPlayerSize { get; private set; }
        public float RightHalfPlayerSize { get; private set; }

        public List<Unit> ActiveUnits { get; private set; }

        private void Awake()
        {
            HalfWidth = .5f;
            LeftHalfPlayerSize = .5f;
            RightHalfPlayerSize = .5f;
            ActiveUnits = new List<Unit>();
            CreateSpawnPattern();
            SpawnUnit(1);
        }
        public void CreateSpawnPattern()
        {

            int halfGridHeight = _gridHeight / 2;
            int halfGridWidth = _gridWidth / 2;
            for (int i = halfGridHeight; i >= -halfGridHeight; i--)
            {
                for (int j = -halfGridWidth; j <= halfGridWidth; j++)
                {
                    positionToCoordinates.Add(patternOrder[(i + halfGridHeight) * _gridHeight + j + halfGridWidth], new Vector2(j * _mapGridSize, -i * _mapGridSize));
                }
            }
        }

        public void SpawnUnit(int amount)
        {
            if (_targetUnitNumber == 25)
            {
                return;
            }
            _targetUnitNumber += amount;
            Vector3 centerPosition = Vector3.zero;
            for (int position = _currentUnitNumber + 1; position <= _targetUnitNumber; position++)
            {
                Vector2 coordinates = positionToCoordinates[position - 1];
                Vector3 offset = new Vector3(coordinates.x, 0, coordinates.y);
                Vector3 spawnPosition = centerPosition + offset;
                GameObject character = Instantiate(UnitPrefab, transform);
                character.transform.localPosition = spawnPosition;
                character.transform.localRotation = Quaternion.identity;
                Unit newUnit = character.GetComponent<Unit>();
                ActiveUnits.Add(newUnit);
                _currentUnitNumber++;
                UpdateWidth(newUnit);
            }
        }
        public void UpdateWidth(Unit spawnedUnit)
        {
            if (spawnedUnit.transform.localPosition.x < 0)
            {
                if (Mathf.Abs(spawnedUnit.transform.localPosition.x - .5f) > LeftHalfPlayerSize)
                {
                    LeftHalfPlayerSize = Mathf.Abs(spawnedUnit.transform.localPosition.x - .5f);

                }
            }
            else
            {
                if (Mathf.Abs(spawnedUnit.transform.localPosition.x + .5f) > RightHalfPlayerSize)
                {
                    RightHalfPlayerSize = Mathf.Abs(spawnedUnit.transform.localPosition.x + .5f);
                }
            }
        }

        private void Update()
        {
            foreach (Unit unit in ActiveUnits)
            {
                if (unit.Ability.IsAbilityCooldown)
                {
                    unit.Ability.AbilityCooldownTime += Time.deltaTime;
                }
                if (unit.Ability.AbilityCooldownTime > unit.Ability.AbilityUseRate)
                {
                    unit.Ability.IsAbilityCooldown = false;
                    unit.Ability.AbilityCooldownTime = 0;
                }
                if (!unit.Ability.IsAbilityCooldown)
                {
                    unit.Ability.Use();
                    unit.Ability.IsAbilityCooldown = true;
                }
            }
        }
        public void AdjustAttackRate(float percentage)
        {
            foreach (Unit unit in ActiveUnits)
            {
                unit.Ability.AbilityUseRate -= unit.Ability.AbilityUseRate * percentage;
            }
        }
    }
}
