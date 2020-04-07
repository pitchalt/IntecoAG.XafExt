using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;

using IntecoAG.XafExt.Spreadsheet.MultiDimForms;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;

namespace IntecoAG.XafExt.Spreadsheet.Test.MultiDimForms {


    public class MdfTemplateTestDetailViewController : IagObjectDetailViewController<MdfCoreTemplate> {

        public SimpleAction TestCoreFillAction { get; protected set; }

        public MdfTemplateTestDetailViewController(): base() {

            TestCoreFillAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(TestCoreFillAction)}", PredefinedCategory.Tools) {
                Caption = "Test Core Fill",
//                ImageName = "BO_Skull",
                PaintStyle = ActionItemPaintStyle.Image,
                ToolTip = "Fill template with MDF Core test data",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            };

            TestCoreFillAction.Execute += TestCoreFillAction_Execute; 
        }

        private void TestCoreFillAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (this.CurrentObject == null)
                return;
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                MdfCoreTemplate template = os.GetObject(this.CurrentObject);
                MdfTemplateTestLogic.FillTestTemplate(template, os);
                os.CommitChanges();
            }
        }

    }

}
