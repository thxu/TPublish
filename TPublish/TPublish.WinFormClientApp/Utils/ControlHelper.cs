using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TPublish.WinFormClientApp.Utils
{
    public static class ControlHelper
    {
        /// <summary>
        /// 使用一个线程执行一个操作
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="func">执行内容</param>
        /// <param name="callback">执行后回调</param>
        /// <param name="enableControl">执行期间禁用控件列表</param>
        /// <param name="intSplashScreenDelayTime">延迟显示等待提示时间</param>
        public static void ThreadRunExt(
          Control parent,
          Action func,
          Action<object> callback,
          Control enableControl = null,
          int intSplashScreenDelayTime = 200)
        {
            ThreadRunExt(parent, func, callback, new[] { enableControl }, intSplashScreenDelayTime);
        }
        /// <summary>
        /// 使用一个线程执行一个操作
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="func">执行内容</param>
        /// <param name="callback">执行后回调</param>
        /// <param name="enableControl">执行期间禁用控件列表</param>
        /// <param name="intSplashScreenDelayTime">延迟显示等待提示时间</param>
        public static void ThreadRunExt(
            Control parent,
            Action func,
            Action<object> callback,
            Control[] enableControl = null,
            int intSplashScreenDelayTime = 200)
        {
            if (enableControl != null)
            {
                List<Control> lstCs = new List<Control>();
                foreach (var c in enableControl)
                {
                    if (c == null)
                        continue;
                    if (c is Form)
                    {
                        lstCs.AddRange(c.Controls.ToArray());
                    }
                    else
                    {
                        lstCs.Add(c);
                    }
                }
                SetControlEnableds(lstCs.ToArray(), false);
            }
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    func();
                    if (callback != null)
                    {
                        callback(null);
                    }
                }
                catch (Exception obj)
                {
                    if (callback != null)
                    {
                        callback(obj);
                    }
                    else
                    {
                        ThreadBaseCallBack(parent, obj);
                    }
                }
                finally
                {
                    if (parent != null)
                    {
                        ThreadInvokerControl(parent, delegate
                        {
                            CloseProcessPanel(parent);
                            SetForegroundWindow(parent.Handle);
                        });
                    }
                    if (enableControl != null)
                    {
                        if (parent != null)
                        {
                            ThreadInvokerControl(parent, delegate
                            {
                                List<Control> lstCs = new List<Control>();
                                foreach (var c in enableControl)
                                {
                                    if (c == null)
                                        continue;
                                    if (c is Form)
                                    {
                                        lstCs.AddRange(c.Controls.ToArray());
                                    }
                                    else
                                    {
                                        lstCs.Add(c);
                                    }
                                }
                                SetControlEnableds(lstCs.ToArray(), true);
                            });
                        }
                    }
                }
            });
        }

        #region 设置控件Enabled，切不改变控件颜色
        /// <summary>
        /// 功能描述:设置控件Enabled，切不改变控件颜色
        /// 作　　者:HZH
        /// 创建日期:2019-03-04 13:43:32
        /// 任务编号:POS
        /// </summary>
        /// <param name="c">c</param>
        /// <param name="enabled">enabled</param>
        public static void SetControlEnabled(this Control c, bool enabled)
        {
            if (!c.IsDisposed)
            {
                if (enabled)
                {
                    ControlHelper.SetWindowLong(c.Handle, -16, -134217729 & ControlHelper.GetWindowLong(c.Handle, -16));
                }
                else
                {
                    ControlHelper.SetWindowLong(c.Handle, -16, 134217728 + ControlHelper.GetWindowLong(c.Handle, -16));
                }
            }
        }

        /// <summary>
        /// Sets the window long.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nIndex">Index of the n.</param>
        /// <param name="wndproc">The wndproc.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll ")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int wndproc);

        /// <summary>
        /// Gets the window long.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nIndex">Index of the n.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll ")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// 功能描述:设置控件Enabled，切不改变控件颜色
        /// 作　　者:HZH
        /// 创建日期:2019-03-04 13:43:32
        /// 任务编号:POS
        /// </summary>
        /// <param name="cs">cs</param>
        /// <param name="enabled">enabled</param>
        public static void SetControlEnableds(Control[] cs, bool enabled)
        {
            for (int i = 0; i < cs.Length; i++)
            {
                Control c = cs[i];
                SetControlEnabled(c, enabled);
            }
        }
        #endregion

        /// <summary>
        /// Threads the base call back.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="obj">The object.</param>
        private static void ThreadBaseCallBack(Control parent, object obj)
        {
            if (obj is Exception)
            {
                if (parent != null)
                {
                    ThreadInvokerControl(parent, delegate
                    {
                    });
                }
            }
        }
        /// <summary>
        /// 委托调用主线程控件
        /// </summary>
        /// <param name="parent">主线程控件</param>
        /// <param name="action">修改控件方法</param>
        public static void ThreadInvokerControl(Control parent, Action action)
        {
            if (parent != null)
            {
                if (parent.InvokeRequired)
                {
                    parent.BeginInvoke(action);
                }
                else
                {
                    action();
                    SetForegroundWindow(parent.Handle);
                }
            }
        }

        /// <summary>
        /// Sets the foreground window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Closes the process panel.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public static void CloseProcessPanel(Control parent)
        {
            if (parent.InvokeRequired)
            {
                parent.BeginInvoke(new MethodInvoker(delegate
                {
                    CloseProcessPanel(parent);
                }));
            }
            else
            {
                Control control = HaveProcessPanelControl(parent);
                if (control != null)
                {
                    Form frm = control.Tag as Form;
                    if (frm != null && !frm.IsDisposed && frm.Visible)
                    {
                        if (frm.InvokeRequired)
                        {
                            frm.BeginInvoke(new MethodInvoker(delegate
                            {
                                frm.Hide();
                            }));
                        }
                        else
                        {
                            frm.Hide();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Haves the process panel control.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>Control.</returns>
        public static Control HaveProcessPanelControl(Control parent)
        {
            Control[] array = parent.Controls.Find("myprogressPanelext", false);
            Control result;
            if (array.Length > 0)
            {
                result = array[0];
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <returns>Control[].</returns>
        public static Control[] ToArray(this Control.ControlCollection controls)
        {
            if (controls == null || controls.Count <= 0)
                return new Control[0];
            List<Control> lst = new List<Control>();
            foreach (Control item in controls)
            {
                lst.Add(item);
            }
            return lst.ToArray();
        }

        /// <summary>
        /// 设置GDI高质量模式抗锯齿
        /// </summary>
        /// <param name="g">The g.</param>
        public static void SetGDIHigh(this Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
        }
    }
}
