
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;

namespace Xpand.ExpressApp.ImportWizard.Win {
    [ToolboxBitmap(typeof(ImportWizardWindowsFormsModule))]
    [ToolboxItem(true)]
    public sealed partial class ImportWizardWindowsFormsModule : ModuleBase {
        public const string XpandImportWizardWin = "eXpand.ImportWizard.Win";
        public ImportWizardWindowsFormsModule() {
            InitializeComponent();
        }
    }
}