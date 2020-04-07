using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;

using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public static class MdfTemplateTableLogic {

        public static void Render(this MdfTemplateTable _this, IObjectSpace os) {
            _this.Table.Render(os);
        }

    }
}
