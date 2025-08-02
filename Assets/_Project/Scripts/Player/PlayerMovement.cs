using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform character;
    [SerializeField] private Animator animator;

    private Rigidbody _rigidbody;
    private Vector2 _moveInputValue;
    private Vector3 _finalMovement;

    private bool _canMove = true;

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

        FlipCharacterBasedOnSpeed();
    }

    private void FlipCharacterBasedOnSpeed()
    {
        if (_rigidbody.linearVelocity.x >= 0)
        {
            if (character.localScale.x < 0)
            {
                character.localScale = new Vector3(character.localScale.x * -1, character.localScale.y, character.localScale.z);
            }
        }
        else
        {
            if (character.localScale.x > 0)
            {
                character.localScale = new Vector3(character.localScale.x * -1, character.localScale.y, character.localScale.z);
            }
        }
    }

    private void Move()
    {
        if (_canMove == true)
        {
            Vector2 dir = _moveInputValue.normalized;
            //myAnim.SetFloat("Speed", magnitude);
            _finalMovement = new Vector3(dir.x, 0, dir.y) * speed;
            //_rigidbody.linearVelocity = new Vector3(dir.x * speed * Time.deltaTime,_rigidbody.linearVelocity.y, dir.y * speed * Time.deltaTime);
            
        }
        else
        {
            _finalMovement = new Vector3(0, 0, 0);
        }

        if (_finalMovement.sqrMagnitude < Mathf.Epsilon)
        {
            //small movement, change anim
            animator.SetBool("Moving", false);
        }
        else
            animator.SetBool("Moving", true);
    }

    private void CanMoveOnDraw(bool onDraw)
    {
        _canMove = !onDraw;
    }

    private void CanMoveOnSequence(bool isActive)
    {
        _canMove = !isActive;
    }

    private void OnEnable()
    {
        //event for when drawing on screen
        DrawOnScreen.onDraw += CanMoveOnDraw;
        //event for when learning melody on screen
        MusicSheet.onSequenceActive += CanMoveOnSequence;
    }

    private void OnDisable()
    {
        DrawOnScreen.onDraw -= CanMoveOnDraw;
        MusicSheet.onSequenceActive -= CanMoveOnSequence;
    }


    #region Input

    public void OnMove(InputValue value)
    {
        _moveInputValue = value.Get<Vector2>();
    }


    #endregion Input
}
