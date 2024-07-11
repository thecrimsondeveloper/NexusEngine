using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Users
{

    public class UserAction
    {

        public UserAction(string actionName, string actionDescription)
        {
            this.timestamp = Time.time;
            this.actionName = actionName;
            this.actionDescription = actionDescription;
        }

        public float timestamp;
        public string actionName;
        public string actionDescription;
    }
}
