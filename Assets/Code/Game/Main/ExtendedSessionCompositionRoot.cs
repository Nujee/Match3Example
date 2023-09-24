using Code.Game.Items;
using Code.MySubmodule.Main;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Game.Main
{
    [DisallowMultipleComponent]
    public sealed class ExtendedSessionCompositionRoot : BaseSessionCompositionRoot
    {
        [field: SerializeField] public AssetReference SymbolSet { get; private set; }
        /// <summary>
        /// Use this method to load adressables and inject them into ECS.
        /// </summary>
        protected override async UniTask LoadAndInjectAdressables()
        {
            // pool injection
            var symbolsSet = await SymbolSet.LoadAssetAsync<ItemDataSet>();
            _injectParameters.Add(symbolsSet);

            foreach (var itemData in symbolsSet.Set)
            {
                await new SymbolPool(itemData)
                    .SetStartingSize(50)
                    .SetRefillCount(20)
                    .Init(itemData.Prefab);
            }
        }
    }
}