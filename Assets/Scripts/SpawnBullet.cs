using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    [SerializeField] private GameObject EnemyBullet;
    public Transform[] spawnPosition;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float increaseSpawnSpeed;

    private float timer = 30;
    private float spawnSpeed = 1;
    private float currentTime;

    private void Start() {
        currentTime = spawnTimer;
    }

    private void Update() {
        currentTime = Timer(currentTime);
        timer = Timer(timer);

        if (currentTime <= 0) {
            currentTime = spawnTimer;
            int randomNumber = Random.Range(0, 3);
            Instantiate(EnemyBullet, spawnPosition[randomNumber].position, spawnPosition[randomNumber].rotation);
        }

        if (timer <= 0) {
            timer = 30;
            spawnSpeed += increaseSpawnSpeed;
        }
    }

    private float Timer(float timer)  {
        timer -= Time.deltaTime * spawnSpeed;
        return timer;
    }

    #region Singleton
    private static SpawnBullet instance;
    private void Awake() {
        instance = this;
    }

    public static SpawnBullet Instance {
        get {
            if (instance == null) {
                instance = new SpawnBullet();
            }
            return instance;
        }
    }
    #endregion

}