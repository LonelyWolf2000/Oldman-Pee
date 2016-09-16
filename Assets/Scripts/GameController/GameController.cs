using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController
{
    public class GameController : MonoBehaviour
    {
        public int SegmentsOfLevel = 1;     // Количество сегментов (экранов) левела
        public int StressOfLevel;
        public int AmountEnemies = 2;       // Количество врагов на левеле
        public float SafeRadius = 6.5f;     // Минимальное удаление от точки старта плеера для спауна противника
        public Transform CorridorPrefab;    // Префаб коридора
        public Transform DoorPrefab;        // Префаб двери
        public Transform Background;    
        public GameObject Player;           // Префаб игрока
        public float CatsStepOffsetY = 0.1f;
        public GameObject[] Enemies;
        public int MinCats;
        public int MaxCats;
        public int MinSpiders;
        public int MaxSpiders;
        private const float _MININTERVAL = 0.2f; // Минимальный интервал между спаунищимися противниками
        private SoundSystemEventListener _soundSystemEventListener;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _soundSystemEventListener = new SoundSystemEventListener(_audioSource, FindObjectOfType<SettingSysScript>());

            if (Enemies.Length == 0)
                Enemies = Resources.LoadAll<GameObject>("Enemies");

            _GenerateLevel();
            SpawnEnemies();
            Instantiate(Player);

            DoorScript.PlayerInDoorEvent += PlayerWin;
            PlayerController.FullStressEvent += OnFullStressEvent;
        }

        private void OnFullStressEvent()
        {
            GUIMessages guiMessages = GetComponent<GUIMessages>();
            guiMessages.Lose.enabled = true;
            _PlaySound(guiMessages.LoseSound);
            OnDestroy();
        }

        private void PlayerWin(GameObject door)
        {
            GUIMessages guiMessages = GetComponent<GUIMessages>();
            guiMessages.Win.enabled = true;
            _PlaySound(guiMessages.WinSound);
            OnDestroy();
        }  

        /// <summary>
        /// Построение уровня
        /// </summary>
        private void _GenerateLevel()
        {
            GameObject levelContainer = new GameObject("Level");
            levelContainer.AddComponent<LevelData>();
            Transform prevSegment = null;

            for (int i = 0; i < SegmentsOfLevel; i++)
            {
                Transform currentSegment = Instantiate(CorridorPrefab);
                currentSegment.parent = levelContainer.transform;

                if (prevSegment != null)
                {
                    float offsetX = prevSegment.position.x - prevSegment.GetComponent<CorridorScript>().LeftPoint.position.x;
                    Vector3 offset = new Vector3(offsetX, 0);
                    currentSegment.transform.position = prevSegment.GetComponent<CorridorScript>().RightPoint.position + offset;

                }
                else
                {
                    //Устанавливаем левый край сцены
                    LevelData.LeftLimiter = currentSegment.GetComponent<CorridorScript>().LeftPoint;
                }

                prevSegment = currentSegment;
            }

            //Устанавливаем правый край сцены
            LevelData.RightLimiter = prevSegment.GetComponent<CorridorScript>().RightPoint;
            LevelData.HightLevel = prevSegment.FindChild("ceiling").position.y;
            LevelData.StressOfLevel = StressOfLevel;

            _InstDoor("StartDoor", levelContainer, LevelData.LeftLimiter);
            _InstDoor("EndDoor", levelContainer, LevelData.RightLimiter);
            _InstBackground(LevelData.RightLimiter, levelContainer);
        }

        /// <summary>
        /// Инстанцируем двери
        /// </summary>
        /// <param name="name"></param>
        /// <param name="levelContainer"></param>
        /// <param name="currentSegment"></param>
        private void _InstDoor(string name, GameObject levelContainer, Transform doorPosition)
        {
            Transform door = Instantiate(DoorPrefab);
            door.name = name;
            door.parent = levelContainer.transform;
            door.transform.position = doorPosition.transform.position;
        }

        private void _InstBackground(Transform endPosition, GameObject levelContainer)
        {
            if(!Background) return;

            Sprite sprite = Background.GetComponent<SpriteRenderer>().sprite;
            float offsetBackgraund = sprite.rect.width / sprite.pixelsPerUnit + 1.5f;
            float positionX = 3;

            while (positionX < endPosition.position.x)
            {
                Transform back = Instantiate(Background);
                back.name = "bakcground";
                back.parent = levelContainer.transform;
                back.transform.position = new Vector3(positionX, back.transform.position.y, back.transform.position.z);
                positionX += offsetBackgraund;
            }
        }

        public void SpawnEnemies()
        {
            if (Enemies == null || Enemies.Length == 0) return;

            GameObject containerAllEnemy = new GameObject("Enemies");

            if (MaxCats != 0)
                _SpawnSpecificEnemies(Enemies[0], Random.Range(MinCats, MaxCats + 1), containerAllEnemy.transform);
            if (MaxSpiders != 0)
                _SpawnSpecificEnemies(Enemies[1], Random.Range(MinSpiders, MaxSpiders + 1), containerAllEnemy.transform);
        }

        /// <summary>
        /// Спауним количество AmountEnemies противников, рандомного типа в рандомной точке.
        /// </summary>
        private void _SpawnRandomEnemies()
        {
            MaxCats = MinCats = AmountEnemies - Random.Range(1, AmountEnemies + 1);
            MaxSpiders = MinSpiders = AmountEnemies - MaxCats;
            SpawnEnemies();
        }

        private void _SpawnSpecificEnemies(GameObject enemy, int amount, Transform containerAllEnemy)
        {
            if (enemy == null || amount == 0) return;

            GameObject containerSpecificEnemy = new GameObject(enemy.name);
            containerSpecificEnemy.transform.parent = containerAllEnemy;

            float[] spawnedEnemies = new float[amount];
            float catsOffsetY = 0;

            for (int i = 0; i < amount; i++)
            {
                float y = 0;
                GameObject newEnemy = Instantiate(enemy);

                if (newEnemy.name == "cat(Clone)")
                {
                    catsOffsetY += CatsStepOffsetY;
                    y = catsOffsetY;
                }
                newEnemy.transform.position = _GenerateUnicCoord(spawnedEnemies, y, i);
                newEnemy.transform.parent = containerSpecificEnemy.transform;
            }
        }

        /// <summary>
        /// Генерируем уникальную координату Х удаленную на не ближе, чем _MININTERVAL от ближайшего объекта.
        /// </summary>
        /// <param name="spawnedEnemies"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private Vector3 _GenerateUnicCoord(float[] spawnedEnemies, float y, int currentIndex)
        {
            float x = -1;
            while (x < 0)
            {
                x = Random.Range(LevelData.LeftLimiter.position.x + SafeRadius, LevelData.RightLimiter.position.x);
                foreach (var variable in spawnedEnemies)
                {
                    if (Mathf.Abs(variable - x) < _MININTERVAL)
                    {
                        x = -1;
                        break;
                    }
                }
            }

            spawnedEnemies[currentIndex] = x;

            return new Vector3(x, y);
        }

        private void _PlaySound(AudioClip audioClip)
        {
            if (_audioSource != null)
            {
                _audioSource.clip = audioClip;
                _audioSource.Play();
            }
        }

        private void OnDestroy()
        {
            DoorScript.PlayerInDoorEvent -= PlayerWin;
            PlayerController.FullStressEvent -= OnFullStressEvent;
            _soundSystemEventListener.DestroyListener();
        }
    }
}