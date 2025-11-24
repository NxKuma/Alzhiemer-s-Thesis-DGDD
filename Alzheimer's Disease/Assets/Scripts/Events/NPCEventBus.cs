using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEventBus
{
    public static NPCEventBus current;

    public event Action onNPCInteract;
    public void NPCInteracted()
    {
        if (onNPCInteract != null)
        {
            onNPCInteract();
        }
    }
}