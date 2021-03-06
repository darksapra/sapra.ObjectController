using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.ObjectController
{
    [System.Serializable]
    public class StatModule : AbstractModule<AbstractStat>
    {        
        public void Run()
        {
            foreach(AbstractStat stat in onlyEnabledRoutines)
            {
                stat.DoExtra();
            }
        }
    }
}
