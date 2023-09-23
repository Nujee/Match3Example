using Code.MySubmodule.Main;
using UnityEngine;

namespace Code.Game.Main
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LevelSettings))]
    public sealed class  ExtendedSceneCompositionRoot : BaseSceneCompositionRoot
    {
        /// <summary>
        /// Use this method to inject scene dependencies into ECS.
        /// </summary>
        protected override void InjectSceneObjects()
        {
            _injectParameters.Add(FindObjectOfType<LevelSettings>());
        }
    }
}