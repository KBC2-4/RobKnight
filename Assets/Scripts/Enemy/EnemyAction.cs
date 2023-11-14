using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : ScriptableObject
{
<<<<<<< Updated upstream
    public bool IsComplete = false;
=======
    public bool isCompletion;
>>>>>>> Stashed changes
    public abstract void Act(EnemyController controller);
}


