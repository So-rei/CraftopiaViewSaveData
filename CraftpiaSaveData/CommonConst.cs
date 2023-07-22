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
        public enum itemListName
        {
            equipmentList,
            buildingList, 
            consumptionList,
            personalChestList,
            petList,
            materialList,
            petChestList,
            importantList
        }
        public enum itemListNameJa
        {
            装備,
            建物,
            食べ物,
            クラウドストレージ,
            ペット,
            素材,
            ペットストレージ,
            大切なもの
        }

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
