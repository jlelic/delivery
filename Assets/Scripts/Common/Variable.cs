using UnityEngine;
using UnityEngine.Events;

public abstract class Variable<T> : ScriptableObject
{
    [SerializeField] private bool resetOnSceneChange = false;
    [SerializeField] private UnityEvent onChangeEvent;
    public T initialValue;
    [Header("Runtime")]
    public T value;

    private void OnEnable()
    {
        if (!resetOnSceneChange)
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
        else
        {
            this.value = initialValue;
        }
    }

    public void SetValue(T value)
    {
        this.value = value;
        onChangeEvent?.Invoke();
    }

    public void ResetValue()
    {
        this.value = initialValue;
    }
}
