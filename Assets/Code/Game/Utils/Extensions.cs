namespace Code.Game.Utils
{
    public static class Extensions
    {
        // public static void SetRandomItem(this Slot slot, 
        //     IEnumerable<ItemData> itemDatasSample, Vector3 whereTo, IEnumerable<ItemData> itemDatas, 
        //     LevelSettings levelSettings, EcsWorld world, bool doIncludeBigBonusItemsInSample = true)
        // {
        //     var samplableItemDatas = itemDatasSample
        //         .Where(x => x.IsSamplable)
        //         .ToList();
        //     
        //     var randomItem = RandomMethods
        //         .GetSelectionArray(samplableItemDatas.ToArray(), x => x.Weight)
        //         .Random()!;
        //     
        //     var randomItemPool = randomItem.Pool;
        //
        //     var bigBonusPool = itemDatas.First(x => x.BonusType == BonusType.BigBonus).Pool;
        //     var bigBonusChance = doIncludeBigBonusItemsInSample ? levelSettings.BigBonusItemAppearPercent * 0.01f : -1f;
        //
        //     var currentItemEntity = Random.value > bigBonusChance
        //         ? randomItemPool.Get(whereTo, Quaternion.identity)
        //         : bigBonusPool.Get(whereTo, Quaternion.identity);
        //     
        //     ref var c_item = ref world.GetPool<c_Item>().Get(currentItemEntity);
        //
        //     ref var c_transform = ref world.GetPool<c_Transform>().Get(currentItemEntity);
        //     c_transform.Transform.localScale = Vector3.one * levelSettings.SymbolSize;
        //
        //     slot.AttachedItemEntity = currentItemEntity;
        //     slot.AttachedItemType = c_item.Data.Type;
        // }
    }
}