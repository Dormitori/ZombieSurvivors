using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangingWeapons : MonoBehaviour
{
    public List<Weapon> weapons;
    public Weapon currentWeapon;

    private InputAction scrollAction;
    private int scrollPos;
    private Transform _weaponPosition;

    void Awake()
    { 
        scrollAction = InputSystem.actions.FindAction("ScrollWheel");
        _weaponPosition = transform.Find("WeaponPosition");
        currentWeapon = Instantiate(currentWeapon, _weaponPosition.position, Quaternion.identity, _weaponPosition);
    }
    
    void Update()
    {
        var scroll = scrollAction.ReadValue<Vector2>()[1];
        if (scroll != 0f)
        {
            scrollPos += (int)scroll;

            scrollPos += weapons.Count;
            scrollPos %= weapons.Count;
            ChangeWeapon(weapons[scrollPos]);
        }
    }

    void ChangeWeapon(Weapon weapon)
    {
        var instance = Instantiate(weapon, _weaponPosition.position, _weaponPosition.transform.rotation, _weaponPosition);
        Destroy(currentWeapon.transform.gameObject);
        currentWeapon = instance;
    }
}
