using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour {

    public readonly UnityEvent<int> HealthChanged = new UnityEvent<int>();
    public readonly UnityEvent NoHealthLeft = new UnityEvent();

    [SerializeField] private int health;

    public void Damage(int amount) {
        health = Mathf.Clamp(health - amount, 0, health);
        HealthChanged.Invoke(health);

        if (health == 0) {
            NoHealthLeft.Invoke();
        }
    }

}