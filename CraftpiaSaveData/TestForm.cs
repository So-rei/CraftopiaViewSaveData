using CraftpiaViewSaveData.File;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraftpiaViewSaveData
{
    public partial class TestForm : Form
    {
        byte[] originalData;
        Dictionary<int, CraftpiaParams> convertData;

        public TestForm()
        {
            InitializeComponent();
        }

        //インポート
        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var ocss = files.Where(f => Path.GetExtension(f) == ".db");
            if (ocss.Count() != 1) return;

            originalData = ImportFile.Import(ocss.First());
            convertData = ImportFile.GetList(originalData, ocss.First());

            //dgvにセットしていく...
            var numberingdic = new Dictionary<string, int>();//アイテム等のときのindexふり用dictionary
            dgv1.Rows.Clear();
            var viewlist = GetResourceFile.GetFile();
            int row = 0;
        }
    }
}
