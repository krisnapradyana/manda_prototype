using Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonPlayerControl : MonoBehaviour
{
    [SerializeField] GameHandler _inspectManager;
    [SerializeField] Animator _characterAnim;
    [SerializeField] float _speed = 200f;

    Rigidbody _rb;
    float _turnSmoothTime = .05f;
    float _turnSmoothVelocity;
    float _horizontal;
    float _vertical;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inspectManager._centralSystem.CurrentState != GameState.inspect)
        {
            return;
        }
        ControlInput(_inspectManager._inputListener._moveComposite.x, _inspectManager._inputListener._moveComposite.y);
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        AnimateCharacter();
    }

    void ControlInput(float horizontal, float vertical)
    {
        _horizontal = horizontal;
        _vertical = vertical;
    }

    void MoveCharacter()
    {
        Vector3 _direction = new Vector3(_horizontal, 0f, _vertical);

        if (_direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _rb.velocity = moveDir.normalized * _speed * Time.fixedDeltaTime;
        }
        else
        {
            _rb.velocity -= 0.05f * _rb.velocity;
        }
    }

    void AnimateCharacter()
    {
        _characterAnim.SetFloat("Velocity", Mathf.Clamp01( _rb.velocity.magnitude));
    }
}
