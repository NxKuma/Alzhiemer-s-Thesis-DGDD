using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvents
{
    public static NPCEvents current;

    public event Action onNPCInteract;
    public void NPCInteracted()
    {
        if (onNPCInteract != null)
        {
            onNPCInteract();
        }
    }
}