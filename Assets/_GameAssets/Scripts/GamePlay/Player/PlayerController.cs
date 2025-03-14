
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;
    [Header("References")]
   [SerializeField] private Transform _orientationTransform;

   [Header("Movement Settings")]

   [SerializeField] private float _movementSpeed;
    [SerializeField] private KeyCode _movementKey;
   
   [Header("JumpSettings")]
   [SerializeField] private KeyCode _jumpKey;
   [SerializeField] private float _jumpForce;
   [SerializeField] private bool _canJump=true;
   [SerializeField] private float _jumpCooldown;
   [SerializeField] private float _airMultiplayer;
   [SerializeField] private float _airDrag;


   [Header("SlidingSettings")]
   [SerializeField] private KeyCode _slideKey;
   [SerializeField] private float _slideMultiplayer;
    [SerializeField] private float _slideDrag;
   
   [Header("Ground Check Settings")]
  [SerializeField] private float _playerHeight;
  [SerializeField] private LayerMask _groundLayer;
  [SerializeField] private float _groundDrag;

  private StateController _stateController;
   private Rigidbody _playerRigidbody;
   private float _startingMovementSpeed,_startingJumpForce;
   private float _horizontalInput,_verticalInput;
   private bool _isSliding;

    

    private Vector3 _movementDirection;

   private void Awake()
    {
        _playerRigidbody=GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation=true;
        _stateController=GetComponent<StateController>();
        _startingMovementSpeed=_movementSpeed;
        _startingJumpForce=_jumpForce;
        
        
        
    }
    private void Update()
    {
        SetInputs();
        SetState();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }

   private void FixedUpdate()
    {
        SetPlayerMovement();
    }
    private void SetInputs(){
    _horizontalInput=Input.GetAxisRaw("Horizontal");
    _verticalInput=Input.GetAxisRaw("Vertical");
    if(Input.GetKeyDown(_slideKey))
    {
      _isSliding=true;
     
    }
    else if(Input.GetKeyDown(_movementKey))
    {
     _isSliding=false;

    }
   else if(Input.GetKey(_jumpKey)  && _canJump && IsGrounded()){

        _canJump=false;
        SetPlayerJumping();
        Invoke(nameof(ResetJumping),_jumpCooldown);
        
    }
    }

    private void SetPlayerMovement(){
        _movementDirection=_orientationTransform.forward*_verticalInput + _orientationTransform.right*_horizontalInput;
        
       float forceMultipler=_stateController.GetCurrentState() switch
       {
            PlayerState.Idle=>1f,
            PlayerState.Slide=>_slideMultiplayer,
            PlayerState.Jump=>_airMultiplayer,
            _=>1f
       };
         _playerRigidbody.AddForce(_movementDirection.normalized*_movementSpeed*forceMultipler,ForceMode.Force);
    }


    private void SetPlayerDrag()
    {

          _playerRigidbody.linearDamping= _stateController.GetCurrentState()  switch
          {
            PlayerState.Move=>_groundDrag,
            PlayerState.Slide=>_slideDrag,
            PlayerState.Jump=>_airDrag,

            _=>_playerRigidbody.linearDamping
          };
       

    }
   private void  LimitPlayerSpeed()
   {
    
    Vector3 flatVelocity=new Vector3(_playerRigidbody.linearVelocity.x,0f,_playerRigidbody.linearVelocity.z);

    if(flatVelocity.magnitude>_movementSpeed)
    {

        Vector3 limitedVelocity=flatVelocity.normalized*_movementSpeed;
        _playerRigidbody.linearVelocity=limitedVelocity;
    }

   }
    private void SetPlayerJumping(){
        OnPlayerJumped?.Invoke();
        _playerRigidbody.linearVelocity=new Vector3(_playerRigidbody.linearVelocity.x,0f,_playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up*_jumpForce,ForceMode.Impulse);
    }


private void SetState()
{
  Vector3 movementDirection=GetMovmentDirection();
  bool isGrounded =IsGrounded();

  PlayerState currentState=_stateController.GetCurrentState();

  var newState=currentState switch
  {
    _ when movementDirection==Vector3.zero && isGrounded && !_isSliding=>PlayerState.Idle,
    _ when movementDirection!=Vector3.zero && isGrounded && !_isSliding=>PlayerState.Move, 
    _ when movementDirection!=Vector3.zero && isGrounded &&  _isSliding=>PlayerState.Slide,
     _ when movementDirection==Vector3.zero && isGrounded && _isSliding=>PlayerState.SlideIdle, 
     _ when !_canJump && !isGrounded=>PlayerState.Jump,
     _ =>currentState

  };
     if(newState!=currentState)
     {
        _stateController.ChangeState(newState);
     }
      



}


private void ResetJumping()
{

        _canJump = true;
}

#region Helper Function
private bool IsGrounded()
{
    return Physics.Raycast(transform.position,Vector3.down,_playerHeight*0.5f+0.2f,_groundLayer);

}

private Vector3 GetMovmentDirection()
{
    return _movementDirection.normalized;
}

public void SetMovementSpeed(float speed,float duration)
{
    _movementSpeed+=speed;
    Invoke(nameof(ResetMovementSpeed),duration);
}
private void ResetMovementSpeed()
{
    _movementSpeed=_startingMovementSpeed;
}
public void SetJumpingForce(float force,float duration)
{
    _jumpForce+=force;
     Invoke(nameof(ResetJumpingForce),duration);
}

private void ResetJumpingForce()
{
    _jumpForce=_startingJumpForce;
}


#endregion
}
