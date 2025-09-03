using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Health _bulletHealth;
    
    private float _bulletSpeed = 18.0f;
    private float _bulletDamage = 10.0f;
    MyTimer timer;

    private Vector3 _previousPosition;
    private Transform _previousHit;
    
    private float _hitSoundCooldown = 0.01f;
    public static float LastHitTime = 0;
    
    
    public void Instantiate(float bulletSpeed, float bulletDamage)
    {
        _bulletSpeed = bulletSpeed;
        _bulletDamage = bulletDamage;
    }

    private void Start()
    {
        timer = new MyTimer(2);
        _previousPosition = transform.position;
        _bulletHealth = GetComponent<Health>();
        _bulletHealth.Death += OnBulletDeath;
    }

    private void OnBulletDeath(string bulletName)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.up * (_bulletSpeed * Time.deltaTime);

        CheckHit();
        _previousPosition = transform.position;

        if (timer.TimerHasEnded())
        {
            Destroy(gameObject);
        }
    }

    private void CheckHit()
    {
        var direction = transform.position - _previousPosition;
        var hit = Physics2D.Raycast(_previousPosition, direction.normalized, direction.magnitude,
            LayerMask.GetMask("Zombie"));
        if (hit && hit.transform != _previousHit)
        {
            PlayHitSound();
            _bulletHealth.ChangeHealth(-10);
            _previousHit = hit.transform;
            hit.transform.GetComponent<Health>().ChangeHealth(-_bulletDamage);
            Zombie zombieComponent;
            hit.transform.TryGetComponent(out zombieComponent);
            if (zombieComponent != null)
                zombieComponent.Bleed(hit.point, transform.rotation);
        }
    }

    private void PlayHitSound()
    {
        if (Time.time - LastHitTime > _hitSoundCooldown)
        {
            AudioManager.instance.Play("Hit");
            LastHitTime = Time.time;
        }
    }
}