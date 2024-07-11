using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using DG.Tweening;

namespace ToyBox
{
    public class ThumbTack : MonoBehaviour
    {
        [SerializeField] Transform intersectionPoint;

        public Transform InterSectionPoint => intersectionPoint;
        public bool isPinned = false;
    }
}
