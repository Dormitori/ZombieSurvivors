using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public enum ZombieState
{
    Moving,
    Attacking,
    Dead
}

public class Zombie : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float damage = 3.0f;
    
    [SerializeField] private float stunnedSpeed = 0.5f;
    [SerializeField] private float stunnedTime = 1f;
    
    private float _currentSpeed;
    private GameObject _player;
    private Health _playerHealthComponent;
    private ParticleSystem _bloodParticles;
    
    private ZombieState _state;

    private Animator _animator;


    private void Awake()
    {
        _state = ZombieState.Moving;
        _currentSpeed = speed;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealthComponent = _player.GetComponent<Health>();
        GetComponent<Health>().Death += ZombieDeath;
        _animator = GetComponentInChildren<Animator>();
        _bloodParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void ZombieDeath(string obj)
    {
        speed = 0;
        _state = ZombieState.Dead;
        AudioManager.instance.Play("ZombieDeath");
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this);
    }

    private void Update()
    {
        switch (_state)
        {
            case ZombieState.Moving:
                _animator.Play("Move");
                FollowPlayer();
                LookAtPlayer();
                break;
            case ZombieState.Attacking:
                _animator.Play("Attack");
                EatingPlayer();
                break;
            case ZombieState.Dead:
                _animator.Play("Dying");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Bleed(Vector3 point, Quaternion rotation)
    {
        _bloodParticles.transform.position = point;
        _bloodParticles.transform.rotation = rotation;
        _bloodParticles.Play();
    }

    private void EatingPlayer()
    {
        _playerHealthComponent.ChangeHealth(-damage * Time.deltaTime);
    }

    private void FollowPlayer()
    {
        var directionVector = _player.transform.position - transform.position;
        transform.position += directionVector / directionVector.magnitude * (Time.deltaTime * _currentSpeed);
    }

    private void LookAtPlayer()
    {
        transform.up = _player.transform.position - transform.position;
    }

    private IEnumerator Stun()
    {
        _currentSpeed = stunnedSpeed;
        yield return new WaitForSeconds(stunnedTime);
        _currentSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _state != ZombieState.Dead)
        {
            _state = ZombieState.Attacking;
        } else if (collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(Stun());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _state != ZombieState.Dead)
        {
            _state = ZombieState.Moving;
        }
    }
}
