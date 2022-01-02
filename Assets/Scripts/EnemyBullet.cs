using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private float speed;
    [Space]
    [SerializeField] private float extraRadius;
    [Space]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform playerHitTransform;
    [SerializeField] private GameObject impactParticle;
    [SerializeField] private GameObject playerParticle;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private DamageComponent damageComponent;

    private float particleCooldown = 0.3f;
    private float cooldown;
    private bool wasInMap = false;

    private void OnEnable() {
        healthComponent.NoHealthLeft.AddListener(OnDeath);
        damageComponent.DamagedOtherCollider.AddListener(OnDamagedOtherCollider);
    }

    private void OnDisable() {
        healthComponent.NoHealthLeft.RemoveListener(OnDeath);
        damageComponent.DamagedOtherCollider.RemoveListener(OnDamagedOtherCollider);
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        this.transform.eulerAngles = RandomRotation();
        cooldown = particleCooldown;

        UpdateVelocity();
    }

    private void Update() {
        UpdateVelocity();
        //cooldown = Mathf.Clamp(cooldown - Time.deltaTime, 0, Mathf.Infinity);

        //bool isInMap = Map.Instance.IsInMap(transform.position, extraRadius);

        //if (!isInMap && wasInMap) {
        //    if (cooldown <= 0) { 
        //        Instantiate(impactParticle, transform.position, transform.rotation); 
        //    }
        //    cooldown = particleCooldown;

        //    transform.position = Map.Instance.GetOppositePosition(transform.position);
        //    this.transform.eulerAngles = RandomRotation();
        //    UpdateVelocity();

        //    wasInMap = false;
        //}

        //wasInMap = isInMap;
    }

    private void UpdateVelocity() {
        rb.velocity = transform.forward * speed;
    }

    private void OnDamagedOtherCollider(Collider other) {
        Instantiate(playerParticle, playerHitTransform.position, playerHitTransform.rotation, other.gameObject.transform);
        TimeManager.Instance.DoSlowMotionPlayerHit();
        int randomNumber = Random.Range(0, 3);
        this.gameObject.transform.position = SpawnBullet.Instance.spawnPosition[randomNumber].position;
        Camera.main.transform.DOShakePosition(.4f, .5f, 20, 90, false, true);
    }

    private void OnDeath() {
        ScoreManager.Instance.IncreaseScore(score);
        Destroy(this.gameObject);
    }

    private Vector3 RandomRotation() {
        (float minAngle, float maxAngle) angles = Map.Instance.GetAnglesTowardsMap(transform.position);
        Vector3 randomAngle = new Vector3(0, Random.Range(angles.minAngle, angles.maxAngle), 0);
        return randomAngle;
    }

    private void OnDrawGizmosSelected() {
        Handles.color = Map.Instance.IsInMap(transform.position, extraRadius) ? Color.green : Color.red;

        Handles.DrawWireCube(Map.Instance.Middle.position, Map.Instance.GetMapSize(extraRadius));
    }

}