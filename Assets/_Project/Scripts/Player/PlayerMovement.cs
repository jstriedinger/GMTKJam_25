using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int tiltAngle = 20;
    private float _targetYRot = 0;
    [SerializeField] private Transform character;
    [SerializeField] private Animator CharacterAnimator;
    [SerializeField] private Animator FlipAnimator;
    public bool debugMode = false;
    public static event Action<bool> isMovingEvent;

    private Rigidbody _rigidbody;
    private Vector2 _moveInputValue;
    private Vector3 _finalMovement;
    private bool _isMoving;

    public bool canMove = true;

    //flipping stuff
    private bool _isFlipped = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

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
        if (!_isFlipped && _finalMovement.x < 0)
        {
            _isFlipped = true;
            FlipAnimator.SetTrigger("Flip");

            if (debugMode == true)
            {
                Debug.Log("Flip!");
            }
        }
        else if (_isFlipped && _finalMovement.x > 0)
        {
            _isFlipped = false;
            FlipAnimator.SetTrigger("Flip");
            
            if (debugMode == true)
            {
                Debug.Log("Flip!");
            }
        }

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
        if (canMove == true)
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
            CharacterAnimator.SetBool("Moving", false);
            _isMoving = false;
        }
        else
        {
            CharacterAnimator.SetBool("Moving", true);
            _isMoving = true;
        }

        isMovingEvent?.Invoke(_isMoving);

        if (_finalMovement.z > 0f)
        {
            _targetYRot = character.localScale.x > 0 ? -tiltAngle : tiltAngle;
        }
        else if (_finalMovement.z < 0f) // below zero
        {
            _targetYRot = character.localScale.x > 0 ? tiltAngle : -tiltAngle;
        }
        else
        {
            _targetYRot = 0; // neutral when input.y == 0            
        }

        // Smoothly interpolate to target rotation
        Quaternion currentRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0f, _targetYRot, 0f);

        transform.rotation = Quaternion.Lerp(currentRot, targetRot, Time.deltaTime * 10);
    }

    private void CanMoveOnDraw(bool onDraw)
    {
        canMove = !onDraw;
        Debug.Log("Can move on draw: " + canMove);
    }

    private void CanMoveOnSequence(bool isActive)
    {
        canMove = !isActive;
    }
    
    public void ToggleCanMove(bool canMove)
    {
        this.canMove = canMove;
        
    }

    private void OnEnable()
    {
        //event for when drawing on screen
        PlayerAttack.OnDraw += CanMoveOnDraw;
        //event for when learning melody on screen
        MusicSheet.onSequenceActive += CanMoveOnSequence;
    }

    private void OnDisable()
    {
        PlayerAttack.OnDraw -= CanMoveOnDraw;
        MusicSheet.onSequenceActive -= CanMoveOnSequence;
    }

    public IEnumerator OnTakingDamage()
    {
        CharacterAnimator.SetTrigger("Hurt");
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;

    }
    
    public void TogglePlayerInput(bool toggle)
    {
        _playerInput.enabled = toggle;
        
    }

    #region Input

    public void OnMove(InputValue value)
    {
        _moveInputValue = value.Get<Vector2>();
    }


    #endregion Input
}
