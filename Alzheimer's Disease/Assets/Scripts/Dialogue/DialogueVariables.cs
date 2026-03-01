using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using System;

public class DialogueVariables
{
    private static Dictionary<string, Ink.Runtime.Object> variables;

    public bool isListening {get; private set;} = false;
    public Action<int> GamePhaseChanged; // subscribers receive the new game phase as an int (cast from enum)
    private Dictionary<string, string> _setVariableCache = new Dictionary<string, string>(); 
    
    public bool ContainsVariable(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && variables.ContainsKey(name);
    }

    public bool TryGetBool(string name, out bool value)
    {
        value = default;
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (!variables.TryGetValue(name, out Ink.Runtime.Object inkValue) || inkValue == null)
        {
            return false;
        }

        if (inkValue is BoolValue boolValue)
        {
            value = boolValue.value;
            return true;
        }

        Debug.LogWarning($"Ink variable '{name}' exists but is not a bool (type={inkValue.GetType().Name}).");
        return false;
    }

    public void SetBool(string name, bool value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("Tried to set an Ink bool with an empty name.");
            return;
        }

        if(!isListening) _setVariableCache[name] = value.ToString();
        else variables[name] = new BoolValue(value);
        // Debug.Log("Variable changed: " + name + " = " + value);
    }

    public bool TryGetInt(string name, out int value)
    {
        value = default;
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (!variables.TryGetValue(name, out Ink.Runtime.Object inkValue) || inkValue == null)
        {
            return false;
        }

        if (inkValue is IntValue intValue)
        {
            value = intValue.value;
            return true;
        }

        Debug.LogWarning($"Ink variable '{name}' exists but is not an int (type={inkValue.GetType().Name}).");
        return false;
    }

    public void SetInt(string name, int value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("Tried to set an Ink int with an empty name.");
            return;
        }

        if(!isListening) _setVariableCache[name] = value.ToString();
        else variables[name] = new IntValue(value);
        // Debug.Log("Variable changed: " + name + " = " + value);
    }

    public bool TryGetFloat(string name, out float value)
    {
        value = default;
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (!variables.TryGetValue(name, out Ink.Runtime.Object inkValue) || inkValue == null)
        {
            return false;
        }

        if (inkValue is FloatValue floatValue)
        {
            value = floatValue.value;
            return true;
        }

        // Convenience: allow an int Ink variable to be read as a float.
        if (inkValue is IntValue intValue)
        {
            value = intValue.value;
            return true;
        }

        Debug.LogWarning($"Ink variable '{name}' exists but is not a float (type={inkValue.GetType().Name}).");
        return false;
    }

    public void SetFloat(string name, float value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("Tried to set an Ink float with an empty name.");
            return;
        }

        if(!isListening) _setVariableCache[name] = value.ToString();
        else variables[name] = new FloatValue(value);

    }

    public DialogueVariables(string globalsFilePath)
    {
        if (variables != null)
        {
            return;
        }

        // compile the story
        string inkFileContents = File.ReadAllText(globalsFilePath);
        Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
        Story globalVariablesStory = compiler.Compile();

        // initialize the story
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void StartListening(Story story)
    {
        // VariablesToStory() must be called before subscribing to the event
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
        isListening = true;
        if(_setVariableCache.Count > 0)
        {
            Debug.Log("Applying cached variable changes to story:");
            foreach (KeyValuePair<string, string> kvp in _setVariableCache)
            {
                Debug.Log($"Evaluating: {kvp.Key} = {kvp.Value}");
                // Attempt to parse the value as int or float before setting as string
                if (int.TryParse(kvp.Value, out int intValue))
                {
                    Debug.Log($"Setting int variable: {kvp.Key} = {intValue}");
                    story.variablesState.SetGlobal(kvp.Key, new IntValue(intValue)); 
                    // SetInt(kvp.Key, intValue);
                }
                else if (float.TryParse(kvp.Value, out float floatValue))
                {
                    story.variablesState.SetGlobal(kvp.Key, new FloatValue(floatValue)); 
                    // SetFloat(kvp.Key, floatValue);
                }
                else if (bool.TryParse(kvp.Value, out bool boolValue))
                {
                    story.variablesState.SetGlobal(kvp.Key, new BoolValue(boolValue)); 
                    // SetBool(kvp.Key, new BoolValue(boolValue));
                }
            }
            _setVariableCache.Clear();
        }

    }

    public void StopListening(Story story)
    {
        isListening = false;
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (name.Equals("gamePhase", StringComparison.Ordinal))
        {
            int? gamePhaseValue = value switch
            {
                IntValue intValue => intValue.value,
                FloatValue floatValue => (int)Mathf.Floor(floatValue.value),
                _ => null
            };

            if (gamePhaseValue.HasValue)
            {
                GamePhaseChanged?.Invoke(gamePhaseValue.Value);
            }
        }

        Debug.Log("Variable changed: " + name + " = " + value);
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }


    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> var in variables)
        {
            story.variablesState.SetGlobal(var.Key, var.Value);
        }
    }
}