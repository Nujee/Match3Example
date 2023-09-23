using Code.MySubmodule.Main;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.Main
{
    [DisallowMultipleComponent]
    public sealed class ExtendedSessionCompositionRoot : BaseSessionCompositionRoot
    {
        /// <summary>
        /// Use this method to load adressables and inject them into ECS.
        /// </summary>
        protected override async UniTask LoadAndInjectAdressables()
        {
            
        }
        
        /// <summary>
        /// Use this method to load pools and inject them into ECS.
        /// </summary>
        protected override async UniTask LoadAndInjectPools()
        {
            
        }
    }
}