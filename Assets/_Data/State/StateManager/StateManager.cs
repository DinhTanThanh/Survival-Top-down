using UnityEngine;

public class StateManager : MonoBehaviour
{
    private IState _currentState;

    public IState CurrentState => _currentState;

    public void ChangeState(IState state)
    {
        if (state == null) return;
        if (_currentState != null && state.GetType() == _currentState.GetType())
            return;

        if (_currentState != null)
            _currentState.Exit();

        _currentState = state;

        if (_currentState != null)
            _currentState.Enter();
    }
    private void FixedUpdate()
    {
        if (_currentState != null)
            _currentState.Execute();
    }
}
