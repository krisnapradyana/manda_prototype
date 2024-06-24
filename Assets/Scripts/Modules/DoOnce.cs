using System;

public class DoOnce
{
    // Flag to check if the action has already been executed
    private bool hasExecuted = false;

    // Action to be executed only once
    private Action doOnceAction;
    // Assign the action to be executed
    public void SetDoOnceAction(Action action)
    {
        doOnceAction = action;
    }

    // Execute the action if it hasn't been executed before
    public void Execute()
    {
        if (!hasExecuted)
        {
            doOnceAction?.Invoke();
            hasExecuted = true;
        }
    }

    // Reset the flag to allow the action to be executed again
    public void Reset()
    {
        hasExecuted = false;
    }
}