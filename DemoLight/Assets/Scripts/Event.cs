using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer.Gameplay
{
    public abstract class Event : MonoBehaviour
    {
        public abstract void Trigger();
    }
}