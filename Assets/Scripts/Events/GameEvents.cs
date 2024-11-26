using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<float> OnSoundEmitted;
    
    public static void EmitSoundLevel(float level)
    {
        OnSoundEmitted?.Invoke(level);
    }
}
