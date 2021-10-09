using UnityEngine;

public class PlayerGun : MonoBehaviour {

    public bool HasBullet => equippedBullet != null;

    [SerializeField] private Transform firePoint;

    public Bullet equippedBullet;

    public void Equip(Bullet bullet) {
        equippedBullet = bullet;

        bullet.transform.SetParent(firePoint);
        bullet.transform.localPosition = bullet.Offset;
        bullet.transform.localRotation = Quaternion.identity;

        bullet.OnEquip();
    }

    private void Update() {
        if (!HasBullet) { return; }
        CheckInput();
    }

    private void CheckInput() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Fire();
        }
    }

    private void Fire() {
        equippedBullet.transform.SetParent(null, true);
        equippedBullet.OnFire();
        equippedBullet = null;
    }

}