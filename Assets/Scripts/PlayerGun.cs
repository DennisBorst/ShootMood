using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    //public GameObject bullet;

    [HideInInspector] public bool fireObject = false;
    public IFire iFire;

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (!fireObject) { return; }

        if (CanShoot())
        {
            Debug.Log("Able To Shoot");
            iFire.OnUpdatePosition(firePoint);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Shooting");

                //Shooting
                fireObject = false;
                iFire.Fire();
                iFire = null;
            }
        }
    }

    public bool CanShoot()
    {
        return iFire != null;
    }
}
