using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.VFX;
using Unity.VisualScripting;

[RequireComponent(typeof(Collider))]
public abstract class TriggerButton : MonoBehaviour
{
    [Title("Settings")]
    public LayerMask layerMask;
    [Range(0, 1)] public float clickThreshold = 0.8f;
    [Range(0, 1)] public float resetClickThreshold = 0.1f;
    [SerializeField] VisualEffect visual;

    [Title("Events")]
    public UnityEvent<float> OnSqueeze;
    public UnityEvent OnClick;
    Collider currentHand = null;


    private void OnValidate()
    {
        //make sure the collider is a trigger
        Collider collider = GetComponent<Collider>() ?? gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    bool hasResetClick = true;

    void OnTriggerStay(Collider other)
    {
        int hand = other.CompareTag("LeftHandInteractor") ? 0 :
                    other.CompareTag("RightHandInteractor") ? 1 :
                    -1;

        if (hand == -1) return;

        currentHand = other;
        OVRInput.Controller controller = hand == 0 ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        float value = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        TriggerSqueeze(other, value, controller);
        OVRInput.SetControllerVibration(0.5f, 0.2f, controller);
        if (visual) visual.SetBool("IsHovering", true);
        if (visual) visual.SetVector3("Position", other.transform.position);
    }


    void TriggerSqueeze(Collider squeezer, float squeezeValue, OVRInput.Controller controller = OVRInput.Controller.RTouch)
    {

        OnTriggerSqueeze(squeezeValue);
        OnSqueeze.Invoke(squeezeValue);
        if (squeezeValue > clickThreshold)
        {
            Click();
        }
        else if (squeezeValue < resetClickThreshold)
        {
            ResetButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool isLeftHand = other.CompareTag("LeftHandInteractor");
        bool isRightHand = other.CompareTag("RightHandInteractor");
        bool isHand = isLeftHand || isRightHand;
        if (isHand == false) return;


        if (isLeftHand)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }
        else if (isRightHand)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }


        currentHand = null;
        if (visual) visual.SetBool("IsHovering", false);
        if (hasResetClick == false)
            ResetButton();



    }

    async void Click(OVRInput.Controller controller = OVRInput.Controller.RTouch)
    {
        if (hasResetClick)
        {
            Debug.Log("Click");
            hasResetClick = false;
            OnClickAction();
            OnClick.Invoke();

            await UniTask.RunOnThreadPool(async () =>
            {
                OVRInput.SetControllerVibration(0.5f, 1f, controller);
                await UniTask.Delay(100);
                OVRInput.SetControllerVibration(0, 0, controller);
            });
        }
    }

    void ResetButton()
    {
        if (hasResetClick == false) return;
        hasResetClick = true;
        OnResetButton();
    }






    [Button]
    protected abstract void OnClickAction();
    protected abstract void OnTriggerSqueeze(float squeezeValue);

    [Button]
    protected abstract void OnResetButton();




}
