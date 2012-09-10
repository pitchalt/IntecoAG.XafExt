using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.XtraEditors;

using IntecoAG.XAFExt.Security.Tests.Module.BusinessObjects;

namespace IntecoAG.XAFExt.Security.Tests.Module.Controllers {
    public partial class FictionBookViewController : ObjectViewController {
        public FictionBookViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void FictionAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View != null && View.CurrentObject as Book != null) {
                (View.CurrentObject as Book).Status = BookState.Obgazhena;
                DevExpress.XtraEditors.XtraMessageBox.Show("Книга цинично обгажена!");
            } else {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка выполнения действия!");
            }
        }

        private void FictionAction2_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View != null && View.CurrentObject as Book != null) {
                (View.CurrentObject as Book).Status = BookState.Reabilitirovana;
                DevExpress.XtraEditors.XtraMessageBox.Show("Книга реабилитирована!");
            } else {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка выполнения действия!");
            }
        }
    }
}
