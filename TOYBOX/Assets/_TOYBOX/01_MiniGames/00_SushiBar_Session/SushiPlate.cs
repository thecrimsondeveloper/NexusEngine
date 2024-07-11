using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiPlate : MonoBehaviour
{
    Animation conveyorBeltAnimation = null;

    public SushiPlate Initialize(AnimationClip conveyorBeltAnimationClip)
    {
        this.conveyorBeltAnimation.clip = conveyorBeltAnimationClip;
        conveyorBeltAnimation.Play();
        return this;
    }

}
