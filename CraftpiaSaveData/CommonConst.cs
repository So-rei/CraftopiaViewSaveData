using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraftpiaViewSaveData
{
    public static class CommonConst
    {
        //アイテムの種別と名称
        //inventorySaveData -> {itemListName} -> ItemInBox -> ItemInBoxValue -> Item -> (値)
        //inventorySaveData -> {enchantFragmentList} -> (値)
        public const string inventorySaveData = "inventorySaveData";
        public enum itemListName
        {
            equipmentList,
            buildingList, 
            consumptionList,
            materialList,
            petList,
            importantList,
            personalChestList,
            petChestList,
        }
        public enum itemListNameJa
        {
            装備,
            建物,
            食べ物,
            素材,
            ペット,
            大切なもの,
            クラウドストレージ,
            ペットストレージ,
        }
        public const string enchantFragmentList = "enchantFragmentList";

        //DBのテーブル名
        public const string CraftpiaTableName = "Entity";
        public const string PPSave_ID_Player = "PlayerSave";
        public const string PPSave_ID_InGame = "InGame";

        //メイン画面のDataGridView構造(隠し)
        public enum rowindex
        {
            CategoryName,
            DetailName,
            FullName,
            DetailName_Ja,
            Numbering,
            Value,
            Index,
            HexIndex
        }
    }
}
