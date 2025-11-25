using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    private Dictionary<string, Ink.Runtime.Object> variables;

    public void StartListening(Story story)
    {
        story.variableState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variableState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        Debug.Log("Variable changed: " + name + " = " + value);
    }
}