using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    
   [SerializeField] private Animator _playerAnimator;
   private PlayerController  _playerController;

   private StateController _stateController;

    private void Awake()
    {
        _playerController=GetComponent<PlayerController>();
        _stateController=GetComponent<StateController>();
    }

    private void Start()
    {
        _playerController.OnPlayerJumped+=PlayerController_OnPlayerJumped;
    }

   

    private void Update()
    {
        SetPlayerAnimations();
    }

     private void PlayerController_OnPlayerJumped()
    {
       _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Jumping,true);
       Invoke(nameof(ResetJumping),0.5f);
    }

    private void ResetJumping()
    {
        _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Jumping,false);
    }

    private void SetPlayerAnimations()
    {
        var currentState=_stateController.GetCurrentState();
        switch(currentState)
        {
            case PlayerState.Idle:
              _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Sliding,false);
              _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Moving,false);
            break;

            case PlayerState.Move:
                 _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Sliding,false);
                 _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Moving,true);
               break;

            case PlayerState.SlideIdle:
                 _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Sliding,true);
                 _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Sliding_Active,false);
               break;

            case PlayerState.Slide:
                 _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Sliding,true);
                 _playerAnimator.SetBool(Consts.PlayerAnimations.Is_Sliding_Active,true);
               break;   
        }
    }
}
