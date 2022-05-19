using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sapra.ObjectController.Samples
{
    [System.Serializable]
    public class StatModule : AbstractModule<AbstractStat, CObject>
    {        
        public void Run(bool continuosCheck)
        {
            if(continuosCheck)
                InitializeComponents(this.cObject);
            foreach(AbstractStat stat in onlyEnabledComponents)
            {
                stat.DoExtra();
            }
        }
    }
}