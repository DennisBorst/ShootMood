using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour {

    public Vector3 Offset => offset;

    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float maxDuration;
    [Space]
    [SerializeField] private float extraRadius;
    [SerializeField] private Vector3 offset;

    [Header("Resources")]
    [SerializeField] private GameObject hitEnemyParticle;
    [Space]
    [SerializeField] private AudioClip pickUpBullet;
    [SerializeField] private AudioClip shootBullet;
    [SerializeField] private AudioClip hitOtherBullet;

    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject idleParticle;
    [SerializeField] private GameObject fireParticle;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private DamageComponent damageComponent;

    private bool IsFired {
        get => isFired;
        set {
            damageComponent.CanDamage = value;
            isFired = value;
        } 
    }

    private float currentDuration;
    private bool isFired;
    private bool wasInMap = false;

    private void OnEnable() {
        damageComponent.DamagedOtherCollider.AddListener(OnDamagedOtherCollider);
    }

    private void OnDisable() {
        damageComponent.DamagedOtherCollider.RemoveListener(OnDamagedOtherCollider);
    }

    private void Start() {
        currentDuration = maxDuration;
    }

    private void Update() {
        if (!IsFired) { return; }

        bool isInMap = Map.Instance.IsInMap(transform.position);

        if (!isInMap && wasInMap) {
            transform.position = Map.Instance.GetOppositePosition(transform.position);
            transform.Rotate(Vector3.up, 180f);
        }

        wasInMap = isInMap;

        currentDuration -= Time.deltaTime;

        if (currentDuration < 0) {
            OnDurationEnded();
        }
    }

    public void OnDurationEnded() {
        IsFired = false;

        rigidbody.velocity = Vector3.zero;

        currentDuration = maxDuration;
        UpdateVisuals();
    }

    public void OnEquip() {
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
        IsFired = false;

        currentDuration = maxDuration;

        UpdateVisuals();

        audioSource.clip = pickUpBullet;
        audioSource.Play();
    }

    public void OnFire() {
        rigidbody.isKinematic = false;
        IsFired = true;

        rigidbody.velocity = transform.forward * speed;

        UpdateVisuals();

        audioSource.clip = shootBullet;
        audioSource.Play();
    }

    private void OnDamagedOtherCollider(Collider other) {
        Instantiate(hitEnemyParticle, transform.position, hitEnemyParticle.transform.rotation);
        TimeManager.Instance.DoSlowMotionEnemyHit();
        Camera.main.transform.DOShakePosition(.4f, .5f, 20, 90, false, true);

        audioSource.clip = hitOtherBullet;
        audioSource.Play();
    }

    private void UpdateVisuals() {
        idleParticle.SetActive(!IsFired);
        fireParticle.SetActive(IsFired);
    }

}