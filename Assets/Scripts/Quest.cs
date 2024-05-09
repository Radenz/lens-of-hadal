using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
public class Quest : ScriptableObject
{
    public string Title;
    [ResizableTextArea]
    public string Description;

    public List<QuestItem> Items;
}

[Serializable]
public class QuestItem
{
    public string Description;
    public bool Countable;

    [HideInInspector]
    public int Counter;
    [HideInInspector]
    public int Target;
    [HideInInspector]
    public bool IsCompleted;

    public QuestValidator Validator;
}

public class QuestTarget<T>
{
    private T value;

    public QuestTarget(T value)
    {
        this.value = value;
    }

    public void Set(T value)
    {
        this.value = value;
        QuestSystem.Instance?.TryValidate();
    }

    public static implicit operator T(QuestTarget<T> value)
    {
        return value.value;
    }
}
