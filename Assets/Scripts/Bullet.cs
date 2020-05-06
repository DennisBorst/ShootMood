using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour, IFire
{
    [SerializeField] private float force;
    [SerializeField] private float maxDuration;
    [Space]
    [SerializeField] private float screenTop;
    [SerializeField] private float screenBottom;
    [SerializeField] private float screenLeft;
    [SerializeField] private float screenRight;
    [SerializeField] private Vector3 offset;
    [Space]
    [SerializeField] private GameObject hitEnemyParticle;
    [SerializeField] private GameObject idleParticle;
    [SerializeField] private GameObject fireParticle;

    [Header("Sounds")]
    [SerializeField] private AudioClip pickUpBullet;
    [SerializeField] private AudioClip shootBullet;
    [SerializeField] private AudioClip hitOtherBullet;

    private float currentDuration;
    private bool fireTrue;
    private Transform newPosition;
    private AudioSource audioSource;

    private void Start()
    {
        currentDuration = maxDuration;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!fireTrue) {Switch(true); return; }
        else { Switch(false); }

        transform.Translate(Vector3.forward * force * Time.deltaTime);

        Vector3 newPos = transform.position;
        if (transform.position.z > screenTop) { newPos.z = screenBottom; }
        if (transform.position.z < screenBottom) { newPos.z = screenTop; }
        if (transform.position.x > screenRight) { newPos.x = screenLeft; }
        if (transform.position.x < screenLeft) { newPos.x = screenRight; }
        transform.position = newPos;

        currentDuration = Timer(currentDuration);

        if (currentDuration < 0)
        {
            currentDuration = maxDuration;
            fireTrue = false;
        }
    }
    public void OnUpdatePosition(Transform transform)
    {
        Transform newTransform = transform;
        this.transform.position = newTransform.position + transform.TransformDirection(offset);
        this.transform.rotation = transform.rotation;
    }

    public void Fire()
    {
        fireTrue = true;

        audioSource.clip = shootBullet;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerGun playerGun = collision.gameObject.GetComponent<PlayerGun>();

            if (!playerGun.fireObject)
            {
                playerGun.fireObject = true;
                playerGun.iFire = this;

                currentDuration = maxDuration;
                fireTrue = false;

                audioSource.clip = pickUpBullet;
                audioSource.Play();
            }
        }

        if (!fireTrue) { return; }

        if(collision.gameObject.tag == "EnemyBullet")
        {
            collision.gameObject.GetComponent<EnemyBulletScript>().DestroyObject();
            Instantiate(hitEnemyParticle, transform.position, hitEnemyParticle.transform.rotation);
            TimeManager.Instance.DoSlowMotionEnemyHit();
            Camera.main.transform.DOShakePosition(.4f, .5f, 20, 90, false, true);

            audioSource.clip = hitOtherBullet;
            audioSource.Play();
        }
    }

    private void Switch(bool on)
    {
        idleParticle.SetActive(on);
        fireParticle.SetActive(!on);
    }

    private float Timer(float timer)
    {
        timer -= Time.deltaTime;
        return timer;
    }
}
