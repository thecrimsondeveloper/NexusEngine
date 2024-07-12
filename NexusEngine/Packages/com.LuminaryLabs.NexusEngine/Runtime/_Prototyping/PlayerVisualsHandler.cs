using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

public class PlayerVisualsHandler : MonoSequence
{
    public float scaleDuration = 1f;
    private Vector3 originalScale;

    protected override async UniTask Finish()
    {
        Debug.Log("PlayerVisualsHandler finished.");
        await UniTask.CompletedTask;
    }

    protected override async UniTask WhenLoad()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;

        //lerp the scale of the player back to the original scale
        for (float i = 0; i < scaleDuration; i += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, i / scaleDuration);
            await UniTask.NextFrame();
        }

        transform.localScale = originalScale;
    }

    protected override async UniTask Unload()
    {
        Debug.Log("PlayerVisualsHandler unloaded.");
        await UniTask.CompletedTask;
    }

    protected override void OnStart()
    {
        Debug.Log("PlayerVisualsHandler started.");
    }

    protected override void AfterLoad()
    {
        Debug.Log("PlayerVisualsHandler after load.");
    }

    protected override void OnFinished()
    {
        Debug.Log("PlayerVisualsHandler finished.");
    }

    protected override void OnUnload()
    {
        Debug.Log("PlayerVisualsHandler unloaded.");
    }
}
