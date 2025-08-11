using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class ObjectEnableEvent : ButtonEvent
    {
        [SerializeField] GameObject targetObject;
        [SerializeField] bool desiredState = true;

        public override void Trigger()
        {
            if (targetObject != null)
            {
                targetObject.SetActive(desiredState);
            }
        }
    }
}