using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    // different ai can be run using different scripts, they should all inherit from this
    public abstract class AIBaseBehavior : MonoBehaviour
    {
        public abstract void run(AICore aiCore);
    }
}
