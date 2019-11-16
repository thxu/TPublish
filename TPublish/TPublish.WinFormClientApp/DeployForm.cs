using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp
{
    public partial class DeployForm : MetroForm
    {
        public DeployForm()
        {
            InitializeComponent();
        }


        #region 线程中操作UI控件

        delegate void SetProcessCallback(int val);
        private void SetProcessVal(int val)
        {
            if (this.buildProgressBar.InvokeRequired)
            {
                while (!this.buildProgressBar.IsHandleCreated)
                {
                    if (this.buildProgressBar.Disposing || this.buildProgressBar.IsDisposed)
                    {
                        return;
                    }
                }
                SetProcessCallback callback = new SetProcessCallback(SetProcessVal);
                this.buildProgressBar.Invoke(callback, new object[] { val });
            }
            else
            {
                this.buildProgressBar.Value = val;
            }
        }

        delegate void ProcessAutoIncrementCallback(int maxLimit);
        private void ProcessAutoIncrement(int maxLimit)
        {
            if (this.buildProgressBar.InvokeRequired)
            {
                while (!this.buildProgressBar.IsHandleCreated)
                {
                    if (this.buildProgressBar.Disposing || this.buildProgressBar.IsDisposed)
                    {
                        return;
                    }
                }
                ProcessAutoIncrementCallback callback = new ProcessAutoIncrementCallback(ProcessAutoIncrement);
                this.buildProgressBar.Invoke(callback, new object[] { maxLimit });
            }
            else
            {
                if (this.buildProgressBar.Value >= (int)maxLimit)
                {
                    return;
                }
                this.buildProgressBar.Value += 1;
            }
        }

        delegate void LogAppendCallback(string txt);
        private void LogAppend(string txt)
        {
            if (this.textLog.InvokeRequired)
            {
                while (!this.textLog.IsHandleCreated)
                {
                    if (this.textLog.Disposing || this.textLog.IsDisposed)
                    {
                        return;
                    }
                }

                LogAppendCallback callback = new LogAppendCallback(LogAppend);
                this.textLog.Invoke(callback, new object[] { txt });
            }
            else
            {
                this.textLog.AppendText($"{txt}{Environment.NewLine}");
            }
        }

        //private void SetStep(int stepIndex)
        //{
        //    if (this.ucStep.InvokeRequired)
        //    {
        //        while (!this.ucStep.IsHandleCreated)
        //        {
        //            if (this.ucStep.Disposing || this.ucStep.IsDisposed)
        //            {
        //                return;
        //            }
        //        }

        //        Action<string> actionDelegate = (x) => { this.ucStep.StepIndex = stepIndex; };
        //        this.ucStep.Invoke(actionDelegate, stepIndex);

        //        //LogAppendCallback callback = new LogAppendCallback(LogAppend);
        //        //this.textLog.Invoke(callback, new object[] { txt });
        //    }
        //    else
        //    {
        //        this.ucStep.StepIndex = stepIndex;
        //    }
        //}

        #endregion

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ControlHelper.ThreadRunExt(this, () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(200);
                    SetProcessVal(i);
                }
                ControlHelper.ThreadInvokerControl(this, () =>
                {
                    SetProcessVal(100);
                });
            }, null, this);
        }
    }
}
