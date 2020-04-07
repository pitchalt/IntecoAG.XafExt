using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;


namespace IntecoAG.XafExt.Spreadsheet.Win {
    public partial class SpreadsheetForm : RibbonForm {
        public SpreadsheetForm() {
            InitializeComponent();
            InitSkinGallery();
//            spreadsheetControl.Options.Behavior.Open = DocumentCapability.Hidden;
            spreadsheetControl.Options.Behavior.Save = DocumentCapability.Hidden;
//            spreadsheetControl.Options.Behavior.SaveAs = DocumentCapability.Hidden;
            spreadsheetControl.Options.Behavior.CreateNew = DocumentCapability.Hidden;
        }
        void InitSkinGallery() {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        public IWorkbook Document {
            get { return spreadsheetControl.Document; }
        }

        private void IagBarItemSave_ItemClick(object sender, ItemClickEventArgs e) {
            OnDocumentSave();
        }
        private void spreadsheetControl_CellValueChanged(object sender, SpreadsheetCellEventArgs e) {
            OnCellValueChanged(e);
        }

        public event EventHandler<EventArgs> DocumentSave;
        protected virtual void OnDocumentSave() {
            DocumentSave?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<SpreadsheetCellEventArgs> CellValueChanged;

        public event EventHandler<Cell> CellDataShow;

        protected virtual void OnCellValueChanged(SpreadsheetCellEventArgs e) {
            CellValueChanged?.Invoke(this, e);
        }

        protected void OnCellDataShow(Cell e) {
            CellDataShow?.Invoke(this, e);
        }

        private void MdfCellDataShow_ItemClick(object sender, ItemClickEventArgs e) {
            OnCellDataShow(spreadsheetControl.ActiveCell);
        }

    }
}