using Core;
using Data;
using Items.Registry;
using Items.Runtime;
using Items.Abstractions;
using UnityEngine;

namespace Bootstrap
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        public TesseraService tesseraService;

        private GameContext context;

        private void Awake()
        {
            context = new GameContext();

            if (!tesseraService)
            {
                tesseraService = new GameObject("TesseraService").AddComponent<TesseraService>();
            }

            tesseraService.LoadFromStreamingAssets();

            TesseraRegistry.Init();
        }

        // 간단한 동작 테스트용
        private void Start()
        {
            if (!tesseraService.TryGet("num_boost", out var rec)) return;

            var runtime = new TesseraRuntime(rec);
            ITessera effect = TesseraRegistry.Create(rec.effectId);

            if (effect.CanTrigger(context, runtime))
            {
                effect.Trigger(context, runtime);
            }

            Debug.Log($"chips={context.chipCount}, mult={context.multiplier}");
        }
    }
}