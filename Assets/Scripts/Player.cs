using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private PlayerGun gun;
    [SerializeField] private HealthComponent healthComponent;

    private Camera mainCamera;
    private AudioSource audioSource;

    private Plane groundPlane;

    private void OnEnable() {
        healthComponent.HealthChanged.AddListener(OnHealthChanged);
        healthComponent.NoHealthLeft.AddListener(OnDeath);
    }

    private void OnDisable() {
        healthComponent.HealthChanged.RemoveListener(OnHealthChanged);
        healthComponent.NoHealthLeft.RemoveListener(OnDeath);
    }

    private void Awake() {
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (TimeManager.Instance.timeStopped) { return; }
        LookAtMouse();
    }

    private void FixedUpdate() {
        if (TimeManager.Instance.timeStopped) { return; }
        Movement();
    }

    private void OnTriggerEnter(Collider other) {
        if (!gun.HasBullet && other.TryGetComponent(out Bullet bullet)) {
            gun.Equip(bullet);
        }
    }

    private void Movement() {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputDirection = inputDirection.sqrMagnitude > 1 ? inputDirection.normalized : inputDirection;
        rigidbody.velocity = inputDirection * movementSpeed;
    }

    private void LookAtMouse() {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(cameraRay, out float rayLength)) {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    private void OnHealthChanged(int currentHealth) {
        audioSource.Play();
        UIManager.Instance.UpdateHealth(currentHealth);
    }

    private void OnDeath() {
        playerObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

}