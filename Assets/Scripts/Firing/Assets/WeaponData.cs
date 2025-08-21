using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float bulletSpeed;
    public float bulletDamage = 1.0f;
    public float bulletDelay;
    public int bulletsPerShot = 1;
    public float bulletSpread;

    public GameObject bulletObject;
}
