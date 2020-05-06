using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private float maxThrust;
    [SerializeField] private float force;
    [Space]
    [SerializeField] private float screenTop;
    [SerializeField] private float screenBottom;
    [SerializeField] private float screenLeft;
    [SerializeField] private float screenRight;
    [Space]
    [SerializeField] private Transform playerHitTransform;
    [SerializeField] private GameObject impactParticle;
    [SerializeField] private GameObject playerParticle;

    private float particleCooldown = 0.3f;
    private float cooldown;
    private Rigidbody rb;
    private Vector3 thrust;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.transform.eulerAngles = RandomRotation();
        cooldown = particleCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.forward * force * Time.deltaTime);
        Vector3 newPos = transform.position;

        cooldown -= Time.deltaTime;
        if(cooldown < 0) { cooldown = 0; }

        if (transform.position.z > screenTop || transform.position.z < screenBottom || transform.position.x > screenRight || transform.position.x < screenLeft)
        {
            if (ParticleReady()) { Instantiate(impactParticle, transform.position, transform.rotation); }
            cooldown = particleCooldown;
            this.transform.eulerAngles = RandomRotation();
        }

        if (transform.position.z > screenTop) { newPos.z = screenBottom; }
        if (transform.position.z < screenBottom) { newPos.z = screenTop; }
        if (transform.position.x > screenRight) { newPos.x = screenLeft; }
        if (transform.position.x < screenLeft) { newPos.x = screenRight; }

        transform.position = newPos;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Damage();
            Instantiate(playerParticle, playerHitTransform.position, playerHitTransform.rotation, player.gameObject.transform);
            TimeManager.Instance.DoSlowMotionPlayerHit();
            int randomNumber = Random.Range(0, 3);
            this.gameObject.transform.position = SpawnBullet.Instance.spawnPosition[randomNumber].position;
            Camera.main.transform.DOShakePosition(.4f, .5f, 20, 90, false, true);
        }
    }

    public void DestroyObject()
    {
        ScoreManager.Instance.IncreaseScore(score);
        Destroy(this.gameObject);
    }

    private Vector3 RandomRotation()
    {
        return new Vector3(0, Random.Range(0, 360), 0);
    }

    private bool ParticleReady()
    {
        return cooldown <= 0;
    }
}
