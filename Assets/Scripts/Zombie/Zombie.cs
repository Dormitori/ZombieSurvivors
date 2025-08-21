using System;
using System.Collections;
using UnityEngine;

public enum ZombieState
{
    Moving,
    Attacking
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
        Destroy(gameObject);
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
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _state = ZombieState.Attacking;
        } else if (collision.gameObject.CompareTag("Bullet"))
        {
            _bloodParticles.transform.position = collision.transform.position;
            _bloodParticles.transform.rotation = collision.transform.rotation;
            _bloodParticles.Play();
            StartCoroutine(Stun());
        }
    }

    private IEnumerator Stun()
    {
        _currentSpeed = stunnedSpeed;
        yield return new WaitForSeconds(stunnedTime);
        _currentSpeed = speed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _state = ZombieState.Moving;
        }
    }

}
