using Player;
using UnityEngine;

namespace GameController
{
    public class GameController : MonoBehaviour
    {
        public int SegmentsOfLevel = 1; // Количество сегментов (экранов) левела
        public int AmountEnemies = 2; // Количество врагов на левеле
        public float SafeRadius = 6.5f; // Минимальное удаление от точки старта плеера для спауна противника
        public Transform CorridorPrefab; // Префаб коридора
        public Transform DoorPrefab; // Префаб двери
        public GameObject Player; // Префаб игрока
        public GameObject[] Enemies;

        private const float _MININTERVAL = 1.0f; // Минимальный интервал между спунищимися противниками

        private void Awake()
        {
            if (Enemies.Length == 0)
                Enemies = Resources.LoadAll<GameObject>("Enemies");

            _GenerateLevel();
            _SpawnEnemies();
            Instantiate(Player);

            DoorScript.PlayerInDoorEvent += PlayerWin;
            PlayerController.FullStressEvent += OnFullStressEvent;
        }

        private void OnFullStressEvent()
        {
            GetComponent<GUIMessages>().Lose.enabled = true;
        }

        private void PlayerWin(GameObject door)
        {
            GetComponent<GUIMessages>().Win.enabled = true;
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

            _InstDoor("StartDoor", levelContainer, LevelData.LeftLimiter);
            _InstDoor("EndDoor", levelContainer, LevelData.RightLimiter);
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

        /// <summary>
        /// Спауним количество AmountEnemies противников, рандомного типа в рандомной точке.
        /// </summary>
        private void _SpawnEnemies()
        {
            if (AmountEnemies == 0 || Enemies == null || Enemies.Length == 0) return;

            GameObject containerEnemy = new GameObject("Enemies");
            float[] spawnedEnemies = new float[AmountEnemies];

            for (int i = 0; i < AmountEnemies; i++)
            {
                GameObject enemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)]);
                enemy.transform.position = _GenerateUnicCoord(spawnedEnemies, i);
                enemy.transform.parent = containerEnemy.transform;
            }
        }

        /// <summary>
        /// Генерируем уникальную координату Х удаленную на не ближе, чем _MININTERVAL от ближайшего объекта.
        /// </summary>
        /// <param name="spawnedEnemies"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private Vector3 _GenerateUnicCoord(float[] spawnedEnemies, int currentIndex)
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

            return new Vector3(x, 0);
        }

        private void OnDestroy()
        {
            DoorScript.PlayerInDoorEvent -= PlayerWin;
            PlayerController.FullStressEvent -= OnFullStressEvent;
        }
    }
}