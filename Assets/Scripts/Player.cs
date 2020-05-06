using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private int health;
    [SerializeField] private GameObject playerObject;

    private CharacterController characterController;
    private Camera mainCamera;
    private AudioSource audioSource;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = FindObjectOfType<Camera>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (TimeManager.Instance.timeStopped) { return; }
        Movement();
    }
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3.Normalize(move);
        characterController.Move(move * movementSpeed * Time.deltaTime);

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            playerObject.transform.LookAt(new Vector3(pointToLook.x, playerObject.transform.position.y, pointToLook.z));
        }
    }

    public void Damage()
    {
        Debug.Log("Damaged");
        health--;
        audioSource.Play();
        UIManager.Instance.UpdateHealth(health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        playerObject.SetActive(false);
        GameManager.Instance.GameOver();
    }
}
