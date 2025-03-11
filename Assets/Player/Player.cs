using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        float vertical = Input.GetAxis("Vertical");

        Vector3 horizontalDirection = horizontal * _camera.right;

        Vector3 verticalDirection = vertical * _camera.forward;

        verticalDirection.y = 0;

        horizontalDirection.y = 0;
        Vector3 movementDirection = horizontalDirection + verticalDirection;
        _rigidBody.velocity = movementDirection * _speed * Time.deltaTime;
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

        if (OnPowerUpStart != null)

        {

            OnPowerUpStart();

        }

        yield return new WaitForSeconds(_powerUpDuration);

        _isPowerUpActive = false;

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

        if (_health > 0)

        {

            transform.position = _respawnPoint.position;

        }

        else

        {

            _health = 0;

            Debug.Log("Lose");

        }

        UpdateUI();

    }

    private void UpdateUI()

    {

        _healthText.text = "Health: " + _health;

    }
}
