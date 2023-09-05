using Game.Run;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelLostEvent),
        menuName = "Runner/" + nameof(LevelLostEvent))]
public class LevelLostEvent : AbstractGameEvent
{
    public override void Reset()
    {
    }
}
