using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Toolkit.Entity;
using Toolkit.Samples;
using Toolkit.Sequences;
using UnityEngine;
using Sequence = Toolkit.Sequences.Sequence;
using System;

namespace ToyBox
{
    public class TVChannelNumberPuzzle : MonoSequence
    {
        [SerializeField, HideLabel, BoxGroup("Sequence Controller")]
        protected NumberSequenceCompletable sequenceCompletable;
        public NumberSequenceCompletable SequenceCompletable => sequenceCompletable;

        [SerializeField] DialCompletable antennaDialCompletable;
        [SerializeField] Animation anims = null;
        [SerializeField] AnimationClip activateAnim = null;
        [SerializeField] AnimationClip deactivateAnim = null;
        [SerializeField] Transform antennaRotationTarget = null;
        [SerializeField] TMP_Text channelNumberText = null;
        [SerializeField] MeshRenderer channelScreen = null;

        private Material screenMaterial;

        private void Awake()
        {
            sequenceCompletable = ScriptableObject.CreateInstance<NumberSequenceCompletable>();
            sequenceCompletable.sequenceLength = 4;
            sequenceCompletable.InitializeObject();

            screenMaterial = channelScreen.materials[1];
        }

        private void Start()
        {
            antennaDialCompletable.OnComplete.AddListener(OnAntennaDialComplete);
            antennaDialCompletable.OnReset.AddListener(OnAntennaDialReset);
            sequenceCompletable.OnComplete.AddListener(() => Sequence.Finish(this as IBaseSequence));
        }

        private void Update()
        {
            float yValOfTarget = antennaRotationTarget.localEulerAngles.y;
            float value = yValOfTarget / 360;
            antennaDialCompletable.SetDialValue(value);
        }

        void OnAntennaDialComplete()
        {
            float value = antennaDialCompletable.CurrentValue;
            int nextChannel = UnityEngine.Random.Range(10, 99);
            sequenceCompletable.CycleSequence(nextChannel);
            channelNumberText.text = nextChannel.ToString();

            screenMaterial.DOFloat(0.02f, "Static", 0.2f).SetEase(Ease.Linear);
        }

        void OnAntennaDialReset()
        {
            DOTween.Kill(screenMaterial);
            screenMaterial.DOFloat(1, "Static", 0.2f).SetEase(Ease.Linear);
        }

        [System.Serializable]
        struct ChannelContainer
        {
            public string channelName;
            public int channelNumber;
            public int channelContent;
            public GameObject parent;
            public TMP_Text nameText;
            public TMP_Text contentText;

            public void SetContent(int channelContent, bool isActive)
            {
                this.channelContent = channelContent;
                contentText.text = channelContent.ToString();
                parent.SetActive(isActive);
            }
        }

        public bool TryGetSequenceCompletable(out NumberSequenceCompletable sequence)
        {
            if (sequenceCompletable == null)
            {
                sequence = null;
                return false;
            }

            sequence = sequenceCompletable;
            return true;
        }

        protected override UniTask Finish()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            anims.Play(deactivateAnim.name);
            return UniTask.WaitWhile(() => anims.isPlaying);
        }

        protected override void AfterLoad()
        {
            anims.Play(activateAnim.name);
        }

        protected override void OnStart()
        {
            gameObject.SetActive(true);
        }

        protected override void OnFinished()
        {
            // Logic for when sequence is finished
        }

        protected override void OnUnload()
        {
            gameObject.SetActive(false);
        }
    }
}
