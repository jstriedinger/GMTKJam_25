using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody _rigidbody;
    private Vector2 _moveInputValue;
    private Vector3 _finalMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_finalMovement * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }


    private void Move()
    {
        Vector2 dir = _moveInputValue.normalized;
        //myAnim.SetFloat("Speed", magnitude);
        _finalMovement = new Vector3(dir.x, 0, dir.y) * speed;
        //_rigidbody.linearVelocity = new Vector3(dir.x * speed * Time.deltaTime,_rigidbody.linearVelocity.y, dir.y * speed * Time.deltaTime);
    }
    
    #region Input
    
    public void OnMove(InputValue value)
    {
        _moveInputValue = value.Get<Vector2>();
    }
    
    
    #endregion Input
}
