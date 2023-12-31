using System.Collections.Generic;
using UnityEngine;
using Game.Run.Level;
using Common;
using Core;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Run
{
    /// <summary>
    /// A class used to store game state information, 
    /// load levels, and save/load statistics as applicable.
    /// The GameManager class manages all game-related 
    /// state changes.
    /// </summary>
    public class RunManager : Singleton<RunManager>
    {
        public GameObject winboard;
        LevelDefinition m_CurrentLevel;
        public LayerMask EnemyLayer;
        /// <summary>
        /// Returns true if the game is currently active.
        /// Returns false if the game is paused, has not yet begun,
        /// or has ended.
        /// </summary>
        public bool IsPlaying => _isPlaying;
        bool _isPlaying;
        GameObject _currentLevelGO;
        GameObject _currentTerrainGO;
        GameObject _levelMarkersGO;


#if UNITY_EDITOR
        bool m_LevelEditorMode;
#endif

        protected override void Awake()
        {
            base.Awake();
            Time.timeScale = 1;
#if UNITY_EDITOR
            // If LevelManager already exists, user is in the LevelEditorWindow
            /*            if (LevelManager.Instance != null)
                        {
                            StartGame();
                            m_LevelEditorMode = true;
                        }*/
#endif
            LoadLevel(LevelSelectionScreen.current);
        }
        private void Start()
        {
        }

        /// <summary>
        /// This method calls all methods necessary to load and
        /// instantiate a level from a level definition.
        /// </summary>
        public void LoadLevel(LevelDefinition levelDefinition)
        {
            Debug.Log("Load level");
            m_CurrentLevel = levelDefinition;
            LoadLevel(m_CurrentLevel, ref _currentLevelGO);
            CreateTerrain(m_CurrentLevel, ref _currentTerrainGO);
            PlaceLevelMarkers(m_CurrentLevel, ref _levelMarkersGO);
            StartGame();
        }

        /// <summary>
        /// This method calls all methods necessary to restart a level,
        /// including resetting the player to their starting position
        /// </summary>
        public void ResetLevel()
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.ResetPlayer();
            }

            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.ResetCamera();
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ResetSpawnables();
            }
        }

        /// <summary>
        /// This method loads and instantiates the level defined in levelDefinition,
        /// storing a reference to its parent GameObject in levelGameObject
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that holds all information needed to 
        /// load and instantiate a level.
        /// </param>
        /// <param name="levelGameObject">
        /// A new GameObject to be created, acting as the parent for the level to be loaded
        /// </param>
        public static void LoadLevel(LevelDefinition levelDefinition, ref GameObject levelGameObject)
        {
            if (levelDefinition == null)
            {
                Debug.LogError("Invalid Level!");
                return;
            }

            if (levelGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelGameObject);
                }
                else
                {
                    DestroyImmediate(levelGameObject);
                }
            }

            levelGameObject = new GameObject("LevelManager");
            LevelManager levelManager = levelGameObject.AddComponent<LevelManager>();
            levelManager.LevelDefinition = levelDefinition;
            Transform levelParent = levelGameObject.transform;

            for (int i = 0; i < levelDefinition.Spawnables.Length; i++)
            {
                LevelDefinition.SpawnableObject spawnableObject = levelDefinition.Spawnables[i];

                if (spawnableObject.SpawnablePrefab == null)
                {
                    continue;
                }

                Vector3 position = spawnableObject.Position;
                Vector3 eulerAngles = spawnableObject.EulerAngles;
                Vector3 scale = spawnableObject.Scale;

                GameObject go = null;

                if (Application.isPlaying)
                {
                    go = GameObject.Instantiate(spawnableObject.SpawnablePrefab, position, Quaternion.Euler(eulerAngles));
                }
                else
                {
#if UNITY_EDITOR
                    go = (GameObject)PrefabUtility.InstantiatePrefab(spawnableObject.SpawnablePrefab);
                    go.transform.position = position;
                    go.transform.eulerAngles = eulerAngles;
#endif
                }

                if (go == null)
                {
                    return;
                }

                // Set Base Color
                Spawnable spawnable = go.GetComponent<Spawnable>();
                if (spawnable != null)
                {
                    spawnable.SetBaseColor(spawnableObject.BaseColor);
                    spawnable.SetScale(scale);
                    levelManager.AddSpawnable(spawnable);
                }

                if (go != null)
                {
                    go.transform.SetParent(levelParent);
                }
            }
        }

        public void UnloadCurrentLevel()
        {
            if (_currentLevelGO != null)
            {
                GameObject.Destroy(_currentLevelGO);
            }

            if (_levelMarkersGO != null)
            {
                GameObject.Destroy(_levelMarkersGO);
            }

            if (_currentTerrainGO != null)
            {
                GameObject.Destroy(_currentTerrainGO);
            }

            m_CurrentLevel = null;
        }

        void StartGame()
        {
            ResetLevel();
            _isPlaying = true;
        }

        /// <summary>
        /// Creates and instantiates the StartPrefab and EndPrefab defined inside
        /// the levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the start and end prefabs.
        /// </param>
        /// <param name="levelMarkersGameObject">
        /// A new GameObject that is created to be the parent of the start and end prefabs.
        /// </param>
        public static void PlaceLevelMarkers(LevelDefinition levelDefinition, ref GameObject levelMarkersGameObject)
        {
            if (levelMarkersGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelMarkersGameObject);
                }
                else
                {
                    DestroyImmediate(levelMarkersGameObject);
                }
            }

            levelMarkersGameObject = new GameObject("Level Markers");

            GameObject start = levelDefinition.StartPrefab;
            GameObject end = levelDefinition.EndPrefab;

            if (start != null)
            {
                GameObject go = GameObject.Instantiate(start, new Vector3(start.transform.position.x, start.transform.position.y, 0.0f), Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);
            }

            if (end != null)
            {
                GameObject go = GameObject.Instantiate(end, new Vector3(end.transform.position.x, end.transform.position.y, levelDefinition.LevelLength), Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);
            }
        }

        /// <summary>
        /// Creates and instantiates a Terrain GameObject, built
        /// to the specifications saved in levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the terrain size.
        /// </param>
        /// <param name="terrainGameObject">
        /// A new GameObject that is created to hold the terrain.
        /// </param>
        public static void CreateTerrain(LevelDefinition levelDefinition, ref GameObject terrainGameObject)
        {
            TerrainGenerator.TerrainDimensions terrainDimensions = new TerrainGenerator.TerrainDimensions()
            {
                Width = levelDefinition.LevelWidth,
                Length = levelDefinition.LevelLength,
                StartBuffer = levelDefinition.LevelLengthBufferStart,
                EndBuffer = levelDefinition.LevelLengthBufferEnd,
                Thickness = levelDefinition.LevelThickness
            };
            TerrainGenerator.CreateTerrain(terrainDimensions, levelDefinition.TerrainMaterial, ref terrainGameObject);
        }

        public void Win()
        {
/*            PlayerController.Instance.gameObject.SetActive(false);
            Time.timeScale = 0;
            winboard.gameObject.SetActive(true);*/
            //TODO: Handle progressing
            /*var levelProgress = SaveManager.Instance.LevelProgress;
              if (currentLevelIndex == levelProgress && currentLevelIndex < m_LevelStates.Count - 1)
              SaveManager.Instance.LevelProgress = levelProgress + 1;
            m_WinEvent.Raise();*/
        }

        public void Lose()
        {
        }
        public void OnCollect(Collectable collectable)
        {
            switch (collectable.ID)
            {
                case Collectable.CollectableID.Bonus:
                    break;
                case Collectable.CollectableID.Extra_Unit:
                    break;
                case Collectable.CollectableID.Coin:
                    Inventory.Instance.OnGoldPicked(collectable);
                    break;
            }
        }

    }
}