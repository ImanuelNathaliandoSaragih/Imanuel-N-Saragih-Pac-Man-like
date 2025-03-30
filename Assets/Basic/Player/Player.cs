using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [SerializeField]

    private float _speed;
    [SerializeField]

    private Transform _camera;
    private Rigidbody _rigidBody;

    [SerializeField]
    private float _powerUpDuration;

    private Coroutine _powerUpCoroutine;

    public Action OnPowerUpStart;

    public Action OnPowerUpStop;
    private bool _isPowerUpActive;

    [SerializeField]
    private Transform _respawnPoint;

    [SerializeField]
    private int _health;

    [SerializeField]
    private TMP_Text _healthText;

    [SerializeField]
    private float _rotationTime = 0.1f;

    private float _rotationVelocity;

    [SerializeField]
    private Animator _animator;

    public GameObject plain;
    public GameObject armor;
    [SerializeField]
    private AudioSource _powerUpActiveSFX;
    [SerializeField]
    private AudioSource _powerUpDeactiveSFX;
    [SerializeField]
    private AudioSource _deathSFX;
    private void Start()

    {
        UpdateUI();
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

    }
    private void Awake()

    {

        _rigidBody = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);

        if (movementDirection.magnitude >= 0.1)
        {
            float rotationAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) *
            Mathf.Rad2Deg + _camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle,
            ref _rotationVelocity, _rotationTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
        }

        Vector3 velocity = movementDirection * _speed * Time.deltaTime;
        velocity.y = _rigidBody.velocity.y; // Keep gravity effect

        _rigidBody.velocity = velocity;
        Debug.Log("Y Velocity: " + _rigidBody.velocity.y);


        _animator.SetFloat("Velocity", _rigidBody.velocity.magnitude);
    }
    void FixedUpdate()
    {
        _rigidBody.AddForce(Vector3.down * 100f, ForceMode.Acceleration);
    }

    public void PickPowerUp()

    {

        Debug.Log("Pick Power Up");
        if (_powerUpCoroutine != null)

        {

            StopCoroutine(_powerUpCoroutine);

        }

        _powerUpCoroutine = StartCoroutine(StartPowerUp());
    }
    private IEnumerator StartPowerUp()

    {
        _isPowerUpActive = true;
        plain.SetActive(false);
        armor.SetActive(true);

        _powerUpActiveSFX.Play();
        if (OnPowerUpStart != null)

        {

            OnPowerUpStart();
        }

        yield return new WaitForSeconds(_powerUpDuration);

        _isPowerUpActive = false;

        plain.SetActive(true);
        armor.SetActive(false);
        _powerUpDeactiveSFX.Play();
        if (OnPowerUpStop != null)

        {

            OnPowerUpStop();
        }

    }
    private void OnCollisionEnter(Collision collision)

    {

        if (_isPowerUpActive)

        {

            if (collision.gameObject.CompareTag("Enemy"))

            {

                collision.gameObject.GetComponent<Enemy>().Dead();

            }

        }

    }

    public void Dead()

    {

        _health -= 1;
        _deathSFX.Play();
        if (_health > 0)

        {

            transform.position = _respawnPoint.position;

        }

        else

        {

            _health = 0;

            Debug.Log("Lose");
            SceneManager.LoadScene("LoseScreen");
        }

        UpdateUI();

    }

    private void UpdateUI()

    {

        _healthText.text = "" + _health;

    }
}
