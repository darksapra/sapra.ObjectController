using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.ObjectController
{
    [System.Serializable]
    public class PCustomGravity : AbstractPassive
    {
        public override PassivePriority whenDo => PassivePriority.LastOne;
        public Vector3 direction;
        public float gravityMultiplierBase;
        public bool useGravity;
        public bool alwaysUpRight = false;
        private PDirectionManager _pDirectionManager;
        public override void DoPassive(Vector3 position, InputValues input)
        {
            if(rb.useGravity)
                rb.useGravity = false;
            //direction = -camera.transform.up;
            controller.gravityDirection = direction.normalized;
            controller.gravityMultiplier = gravityMultiplierBase;
            if(useGravity)
                rb.AddForce(direction.normalized*gravityMultiplierBase, ForceMode.Acceleration);
            if(alwaysUpRight)
                _pDirectionManager.ForcedRotation(0.1f,  RotationMode.DesiredAxisAndTarget, transform.up,-controller.gravityDirection);
            useGravity = true;
        }


        protected override void AwakeRoutine(AbstractCObject controller)        {
            direction = controller.gravityDirection;
            gravityMultiplierBase = controller.gravityMultiplier;
            _pDirectionManager = controller.RequestModule<PassiveModule>().RequestRoutine<PDirectionManager>(true);
        }
    }
}
