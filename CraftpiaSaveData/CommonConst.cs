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
        public static string[] listname = { "equipmentList", "buildingList", "consumptionList", "personalChestList", "petList", "materialList", "petChestList", "importantList" };
        public static string[] listnameja = { "装備", "建物", "食べ物", "クラウドストレージ", "ペット", "素材", "ペットストレージ", "大切なもの" };

        public const string CraftpiaTableName = "Entity";
        public const string PPSave_ID_Player = "PlayerSave";
        public const string PPSave_ID_InGame = "InGame";

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
