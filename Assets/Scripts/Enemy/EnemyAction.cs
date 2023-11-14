using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : ScriptableObject
{
    public bool IsComplete = false;
    public abstract void Act(EnemyController controller);
}


