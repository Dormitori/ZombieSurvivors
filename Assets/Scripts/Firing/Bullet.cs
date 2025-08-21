using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _bulletSpeed = 18.0f;
    private float _bulletDamage = 10.0f;
    MyTimer timer;


    public void Instantiate(float bulletSpeed, float bulletDamage)
    {
        _bulletSpeed = bulletSpeed;
        _bulletDamage = bulletDamage;
    }

    private void Start()
    {
        timer = new MyTimer(2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.up * (_bulletSpeed * Time.deltaTime);
        
        if (timer.TimerHasEnded())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.GetComponent<Health>().ChangeHealth(-_bulletDamage);
        }
    }
}
