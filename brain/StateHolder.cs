public interface StateHolder {
    void HandleStateChange(StateManager.State state); //tell the thing that its state has changed
    bool CanChangeState(StateManager.State state); //to tell the manager if can switch to given state
}