using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
public class QuestData : ScriptableObject
{
    public string Title;
    [ResizableTextArea]
    public string Description;

    public GameObject[] Steps;

}
