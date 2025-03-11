using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState _currenPlayerState=PlayerState.Idle;

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }
    public void ChangeState(PlayerState newPlayerState)
    {
        if(_currenPlayerState==newPlayerState)
        {
            return;
        }

        _currenPlayerState=newPlayerState;
    }

    public PlayerState GetCurrentState()
    {
        return _currenPlayerState;
    }
}

