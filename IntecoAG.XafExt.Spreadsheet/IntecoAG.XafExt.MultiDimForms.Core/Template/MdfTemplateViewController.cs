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

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {


    public class MdfTemplateViewController : IagObjectViewController<MdfCoreTemplate> {

        public SimpleAction RenderAction { get; protected set; }

        public MdfTemplateViewController(): base() {

            RenderAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(RenderAction)}", PredefinedCategory.RecordEdit) {
                Caption = "Render",
//                ImageName = "BO_Skull",
                PaintStyle = ActionItemPaintStyle.Image,
                ToolTip = "Render template axis and tables",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
                TargetViewType = ViewType.DetailView,
                TargetViewNesting = Nesting.Root,
            };
            RenderAction.Execute += RenderAction_Execute; 
        }

        private void RenderAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (this.CurrentObject == null)
                return;
            ObjectSpace.CommitChanges();
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                MdfCoreTemplate template = os.GetObject(this.CurrentObject);
                template.Render(os);
                os.CommitChanges();
            }
        }

    }

}
