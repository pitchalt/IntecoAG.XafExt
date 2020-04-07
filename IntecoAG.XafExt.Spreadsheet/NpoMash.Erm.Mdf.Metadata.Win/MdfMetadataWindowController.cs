using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace NpoMash.Erm.Mdf.Metadata.Templates {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class MdfMetadataWindowController : WindowController {
        public MdfMetadataWindowController() {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated() {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }
        protected override void OnDeactivated() {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void DBMetadataBackupAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            try {
                var backupConn = new SqlConnection {
                    ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString
                };
                backupConn.Open();
                var backupcomm = backupConn.CreateCommand();
                var DBName = System.Configuration.ConfigurationManager.AppSettings["DBName"];
                var DBBackupPath = System.Configuration.ConfigurationManager.AppSettings["DBBackupPath"];
                var DBBackupTime = DateTime.Now.ToString("s").Replace(':', '-');
                var backupdb = $@"USE master; BACKUP DATABASE ""{DBName}"" TO DISK='{DBBackupPath}{DBName}-{DBBackupTime}.bak'";
                var backupcreatecomm = new SqlCommand(backupdb, backupConn);
                backupcreatecomm.ExecuteNonQuery();
                backupConn.Close();
            }
            catch (Exception ex) {
                if (ex.Message.Contains("Operating system error")) {
                    MessageBox.Show("Please chose a public folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DBMetadataRestoreAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            try {
                var backupConn = new SqlConnection {
                    ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString
                };
                backupConn.Open();
                var backupcomm = backupConn.CreateCommand();
                var DBName = System.Configuration.ConfigurationManager.AppSettings["DBName"];
                var backupdb = $@"USE master; RESTORE DATABASE ""{DBName}"" FROM DISK='{System.IO.Directory.GetCurrentDirectory()}\{DBName}.bak'";
                var backupcreatecomm = new SqlCommand(backupdb, backupConn);
                backupcreatecomm.ExecuteNonQuery();
                backupConn.Close();
            }
            catch (Exception ex) {
                if (ex.Message.Contains("Operating system error")) {
                    MessageBox.Show("Please chose a public folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
