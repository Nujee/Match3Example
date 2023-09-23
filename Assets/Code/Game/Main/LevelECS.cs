using Code.MySubmodule.Main;

namespace Code.Game.Main
{
    public sealed class LevelECS : ECS
    {
        /// <summary>
        /// Add your ECS systems here.
        /// </summary>
        protected override void AddGameplaySystems()
        {
            // _systems[SystemType.Update]
            //     .Add(new s_SomeSystem());
        }
    }
}