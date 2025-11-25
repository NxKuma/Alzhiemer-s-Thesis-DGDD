using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEventBus
{
    public static NPCEventBus current;

    public event Action<string> onNPCInteract;
    public void NPCInteracted(string npcName)
    {
        if (onNPCInteract != null)
        {
            onNPCInteract(npcName);
        }
    }

    
}