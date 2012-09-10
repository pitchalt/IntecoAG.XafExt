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
    public partial class BookViewController : ObjectViewController {
        public BookViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        private void Action1_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View != null && View.CurrentObject as Book != null) {
                (View.CurrentObject as Book).Status = BookState.Prochitana;
                DevExpress.XtraEditors.XtraMessageBox.Show("Книга прочитана!");
            } else {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка выполнения действия!");
            }
        }

        private void Action2_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View != null && View.CurrentObject as Book != null) {
                (View.CurrentObject as Book).Status = BookState.Porvana;
                DevExpress.XtraEditors.XtraMessageBox.Show("Книга изуверски порвана!");
            } else {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка выполнения действия!");
            }
        }

        private void Action3_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (View != null && View.CurrentObject as Book != null) {
                (View.CurrentObject as Book).Status = BookState.Sohzena;
                DevExpress.XtraEditors.XtraMessageBox.Show("Книга с наслаждением сожжена!");
            } else {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка выполнения действия!");
            }
        }
    }
}
