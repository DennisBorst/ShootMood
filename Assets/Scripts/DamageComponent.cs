using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageComponent : MonoBehaviour {

    public readonly UnityEvent<Collider> DamagedOtherCollider = new UnityEvent<Collider>();

    public bool CanDamage { get => canDamage; set => canDamage = value; }

    [SerializeField] private int damageAmount;
    [SerializeField] private bool canDamage;
    [SerializeField] private List<TagSO> hittableTags;

    private void OnTriggerEnter(Collider other) {
        if (!canDamage || !other.TryGetComponent(out TagsComponent tagComponent) || !tagComponent.ContainsAnyTag(hittableTags) || other.TryGetComponent(out HealthComponent healthComponent)) { return; }

        healthComponent.Damage(damageAmount);
        DamagedOtherCollider.Invoke(other);
    }

}