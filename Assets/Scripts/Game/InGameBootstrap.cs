using Dice.Registry;
using UnityEngine;

namespace GameLayer
{
    public sealed class InGameBootstrap : MonoBehaviour
    {
        void Awake() { DiceRegistry.Init(); }
    }
}