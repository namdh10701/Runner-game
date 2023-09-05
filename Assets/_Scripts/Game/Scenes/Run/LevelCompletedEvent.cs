using Game.Run;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelCompletedEvent),
    menuName = "Runner/" + nameof(LevelCompletedEvent))]
public class LevelCompletedEvent : AbstractGameEvent
{
    public override void Reset()
    {
    }
}
