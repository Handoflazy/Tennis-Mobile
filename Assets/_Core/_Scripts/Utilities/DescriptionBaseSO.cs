using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DescriptionBaseSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string Description;
}
public abstract class RuntimeScriptableObject : ScriptableObject
{
    static readonly List<RuntimeScriptableObject> Instances = new();

    private void OnEnable() => Instances.Add(this);

    private void OnDisable() => Instances.Remove(this);
    protected abstract void OnReset();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetAllInstance() {
        Instances.ForEach(i => i.OnReset());
    }
}