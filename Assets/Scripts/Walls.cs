using Oasez.Extensions.List;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    public enum HitAction {

        OppositeSide    = 0,
        RandomPosition  = 1,

    }

    [SerializeField] private HitAction hitAction;
    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private Transform mapCenter;
    [SerializeField] private float randomAngle;

    private Collider[] wallColliders;
    private List<Collider> collidersInTeleportation = new List<Collider>();

    private void Awake() {
        wallColliders = GetComponents<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (collidersInTeleportation.Contains(other) || !IsBulletFired(other)) { return; }
        Debug.Log("bullet entered trigger");
        HandleCollision(other);
        collidersInTeleportation.Add(other);
    }

    private void OnTriggerStay(Collider other) {
        if (collidersInTeleportation.Contains(other) || !IsBulletFired(other)) { return; }
        HandleCollision(other);
        collidersInTeleportation.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("bullet exited trigger");
        collidersInTeleportation.Remove(other);
    }

    private void HandleCollision(Collider other) {
        switch (hitAction) {
            case HitAction.OppositeSide:
                TeleportToOppositePosition(other.transform);
                break;
            case HitAction.RandomPosition:
                TeleportToRandomPosition(other.transform);
                break;
            default:
                break;
        }
    }

    private bool IsBulletFired(Collider colldier) => !colldier.TryGetComponent(out Bullet bullet) || bullet.IsFired;

    private void TeleportToRandomPosition(Transform transform) {
        Collider randomCollider = wallColliders.GetRandom();
        Bounds spawnBounds = randomCollider.bounds;

        Vector3 newPosition = new Vector3(
            Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            transform.position.y,
            Random.Range(spawnBounds.min.z, spawnBounds.max.z)
        );

        newPosition = randomCollider.transform.TransformPoint(newPosition);
        Vector3 centerPosition = mapCenter.position;
        centerPosition.y = newPosition.y;
        Vector3 newDirection = centerPosition - newPosition;
        //newDirection = Quaternion.AngleAxis(Random.Range(-randomAngle / 2, randomAngle / 2), Vector3.up) * newDirection;

        transform.position = newPosition;
        Debug.Log("Teleported bullet");
        transform.forward = newDirection;
    }

    private void TeleportToOppositePosition(Transform transform) {
        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, Mathf.Infinity, raycastLayer)) {
            transform.position = hit.point;
        }
    }

}