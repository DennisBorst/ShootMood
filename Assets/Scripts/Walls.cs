using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    [SerializeField] private LayerMask raycastLayer;

    private List<Collider> collidersInTeleportation = new List<Collider>();

    private void OnTriggerEnter(Collider other) {
        if (collidersInTeleportation.Contains(other) || !IsBulletFired(other)) { return; }
        TeleportToOppositePosition(other.transform);
        collidersInTeleportation.Add(other);
    }

    private void OnTriggerStay(Collider other) {
        if (collidersInTeleportation.Contains(other) || !IsBulletFired(other)) { return; }
        TeleportToOppositePosition(other.transform);
        collidersInTeleportation.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        collidersInTeleportation.Remove(other);
    }

    private bool IsBulletFired(Collider colldier) => colldier.TryGetComponent(out Bullet bullet) && bullet.IsFired;

    private void TeleportToOppositePosition(Transform transform) {
        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, Mathf.Infinity, raycastLayer)) {
            transform.position = hit.point;
        }
    }

}