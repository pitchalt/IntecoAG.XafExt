using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using IntecoAG.XafExt.LongOperation;

namespace IntecoAG.XafExt.LongOperation.Win
{
    public partial class LongOperationSplash : SplashScreen
    {
        public LongOperationTask LongOperationTask { get; private set; }

        private Boolean IsProgressBarConfigured;

        private void ProgressBarConfigure()
        {
            if (!IsProgressBarConfigured && LongOperationTask?.State == LongOperationState.RUNNING)
            {
                progressBarControl1.Properties.Minimum = 0;
                progressBarControl1.Properties.Maximum = LongOperationTask.MaxWorkItem;
                progressBarControl1.Properties.PercentView = true;
                progressBarControl1.Position = LongOperationTask.CurrentWorkItem;
                IsProgressBarConfigured = true;
            }
        }

        public LongOperationSplash()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);

            var splash_cmd = (SplashCommand) cmd;
            switch (splash_cmd)
            {
                case SplashCommand.ATTACH:
                    LongOperationTask = (LongOperation.LongOperationTask) arg;
                    this.labelStatus.Text = "Attached";
                    timer1.Enabled = true;
                    ProgressBarConfigure();
                    break;
                case SplashCommand.DETTACH:
                    timer1.Enabled = false;
                    LongOperationTask = null;
                    this.labelStatus.Text = "Detached";
                    break;
            }
        }

        //public void Attach(LongOperation.LongOperation longOperation)
        //{
        //    ProcessCommand(SplashCommand.ATTACH, longOperation);
        //}

        //public void Detach()
        //{
        //    ProcessCommand(SplashCommand.DETTACH, null);
        //}

        #endregion

        public enum SplashCommand
        {
            ATTACH = 1,
            DETTACH = 2, 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!IsProgressBarConfigured)
                ProgressBarConfigure();
            else
            {
                this.labelStatus.Text = "Process: " + LongOperationTask?.CurrentWorkItem ?? String.Empty;
                this.progressBarControl1.Position = LongOperationTask.CurrentWorkItem;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LongOperationTask.Cancel();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}