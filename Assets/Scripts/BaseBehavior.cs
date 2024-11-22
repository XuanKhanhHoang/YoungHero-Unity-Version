using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    private bool hasAwoken = false;

    protected virtual void Awake()
    {

        hasAwoken = true;
        OnAfterAwake();
    }

    protected virtual void Start()
    {
        if (!hasAwoken)
        {
            OnAfterAwake();
        }
    }

    protected virtual void OnAfterAwake()
    {

    }
}
