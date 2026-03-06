using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

public class DialogueVariables
{
    private static Dictionary<string, Ink.Runtime.Object> variables;

    public bool isListening {get; private set;} = false;
    public Action<int> GamePhaseChanged; // subscribers receive the new game phase as an int (cast from enum)
    public Action<string, bool> ItemVisibilityChanged; // subscribers receive the new visibility status of the item whose variable changed
    private Dictionary<string, Ink.Runtime.Object> _setVariableCache = new Dictionary<string, Ink.Runtime.Object>();
    private Story _listeningStory;
    
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
        Debug.Log($"SetBool called for variable '{name}' with value {value}. isListening={isListening}");
        SetVariable(name, new BoolValue(value));
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

        SetVariable(name, new IntValue(value));
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

        SetVariable(name, new FloatValue(value));

    }

    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        if (variables != null)
        {
            return;
        }

        Story globalVariablesStory = new Story(loadGlobalsJSON.text);

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
        if (story == null)
        {
            Debug.LogWarning("StartListening was called with a null story.");
            return;
        }

        _listeningStory = story;

        // VariablesToStory() must be called before subscribing to the event
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
        isListening = true;
        if(_setVariableCache.Count > 0)
        {
            Debug.Log("Applying cached variable changes to story:");
            foreach (KeyValuePair<string, Ink.Runtime.Object> kvp in _setVariableCache)
            {
                Debug.Log($"Applying cached variable: {kvp.Key} = {kvp.Value}");
                story.variablesState.SetGlobal(kvp.Key, kvp.Value);
                variables[kvp.Key] = kvp.Value;
            }
            _setVariableCache.Clear();
        }

    }

    public void StopListening(Story story)
    {
        isListening = false;
        if (story != null)
        {
            story.variablesState.variableChangedEvent -= VariableChanged;
        }
        _listeningStory = null;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        Debug.Log("Variable changed: " + name + " = " + value);
        variables[name] = value;
        NotifyVariableUpdated(name, value);


    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> var in variables)
        {
            story.variablesState.SetGlobal(var.Key, var.Value);
        }
    }

    private void SetVariable(string name, Ink.Runtime.Object value)
    {
        variables[name] = value;

        if (isListening && _listeningStory != null)
        {
            _listeningStory.variablesState.SetGlobal(name, value);
            return;
        }

        NotifyVariableUpdated(name, value);
        _setVariableCache[name] = value;
    }

    private void NotifyVariableUpdated(string name, Ink.Runtime.Object value)
    {
        if (name.Equals("gamePhase", StringComparison.Ordinal))
        {
            int? gamePhaseValue = value switch
            {
                IntValue intValue => intValue.value,
                FloatValue floatValue => floatValue.value % 1 == 0 ? (int)Mathf.Floor(floatValue.value) : null,
                _ => null
            };

            if (gamePhaseValue.HasValue)
            {
                GamePhaseChanged?.Invoke(gamePhaseValue.Value);
            }
        }

        if (name.StartsWith("p", StringComparison.Ordinal))
        {
            string itemName = name;
            bool isVisible = value switch
            {
                BoolValue boolValue => boolValue.value,
                IntValue intValue => intValue.value != 0,
                FloatValue floatValue => Mathf.Abs(floatValue.value) > Mathf.Epsilon,
                _ => false
            };

            ItemVisibilityChanged?.Invoke(itemName, isVisible);
        }
    }
}