using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;

    InputAction fireAction;

    bool canShoot = true;
    MyTimer shootTimer;
    private Transform shootingPosition;
    private ParticleSystem flashParticles;

    void Awake()
    {
        fireAction = InputSystem.actions.FindAction("Attack");
        shootTimer = new MyTimer(weaponData.bulletDelay);
        shootingPosition = transform.Find("ShootingPosition");
        flashParticles = transform.Find("FlashParticles").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (fireAction.IsPressed() && canShoot)
        {
            Shoot(shootingPosition);
            shootTimer.Reset(weaponData.bulletDelay);
            canShoot = false;
        }
        if (shootTimer.TimerHasEnded())
        {
            canShoot=true;
        }
    }
    
    public void Shoot(Transform shootPosition)
    {
        for (int i = 0; i < weaponData.bulletsPerShot; i++) {
            Quaternion randomDeviation = Quaternion.Euler(
                Random.Range(-weaponData.bulletSpread, weaponData.bulletSpread),
                Random.Range(-weaponData.bulletSpread, weaponData.bulletSpread),
                0
            );
            var bullet = Instantiate(
                weaponData.bulletObject,
                shootPosition.position,
                shootPosition.rotation * randomDeviation
            );
            bullet.GetComponent<Bullet>().Instantiate(weaponData.bulletSpeed, weaponData.bulletDamage);

        }
        flashParticles.Play();
    }
}
