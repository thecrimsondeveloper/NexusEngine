using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Extras;
using Toolkit.NexusEngine;
using ToyBox;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class PositionSetter : NexusBlock
    {

        [Title("Settings")]
        [SerializeField] Space space = Space.World;
        [SerializeField] NexusVector3 position;

        [BoxGroup("Random Offset")]
        [SerializeField, HideLabel, BoxGroup("Random Offset/X")] NumberRange xRandomeRange = new NumberRange(-1, 1);
        [SerializeField, HideLabel, BoxGroup("Random Offset/Y")] NumberRange yRandomeRange = new NumberRange(-1, 1);
        [SerializeField, HideLabel, BoxGroup("Random Offset/Z")] NumberRange zRandomeRange = new NumberRange(-1, 1);
        public void SetPosition()
        {
            Vector3 pos = position + new Vector3(xRandomeRange.Random(), yRandomeRange.Random(), zRandomeRange.Random());

            if (space == Space.World)
            {
                entity.transform.position = pos;
            }
            else
            {
                entity.transform.localPosition = pos;
            }
        }
    }
}
