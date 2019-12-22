using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Drawing;
using TPublish.WinFormClientApp.Utils;

namespace TPublish.WinFormClientApp.Controls
{
    /// <summary>
    /// 步骤条控件
    /// </summary>
    [DefaultEvent("IndexChecked")]
    public partial class StepControl : UserControl
    {
        public StepControl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.MouseDown += Step_MouseDown;
        }

        /// <summary>
        /// The m step style
        /// </summary>
        private MetroColorStyle _mStepStyle = MetroColorStyle.Default;

        /// <summary>
        /// 步骤背景色
        /// </summary>
        /// <value>The color of the step back.</value>
        [Description("步骤Style"), Category("自定义")]
        public MetroColorStyle StepStyle
        {
            get
            {
                if (DesignMode || _mStepStyle != MetroColorStyle.Default)
                {
                    return _mStepStyle;
                }

                if (StyleManager != null && _mStepStyle == MetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && _mStepStyle == MetroColorStyle.Default)
                {
                    return MetroColorStyle.Default;
                }
                return _mStepStyle;
            }
            set
            {
                _mStepStyle = value;
                Refresh();
            }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        /// <summary>
        /// Occurs when [index checked].
        /// </summary>
        [Description("步骤更改事件"), Category("自定义")]
        public event EventHandler IndexChecked;

        /// <summary>
        /// The m step back color
        /// </summary>
        private Color _mStepBackColor = Color.FromArgb(189, 189, 189);

        /// <summary>
        /// 步骤背景色
        /// </summary>
        /// <value>The color of the step back.</value>
        [Description("步骤背景色"), Category("自定义")]
        public Color StepBackColor
        {
            get => _mStepBackColor;
            set
            {
                _mStepBackColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m step fore color
        /// </summary>
        private Color _mStepForeColor = Color.FromArgb(0, 174, 219);

        /// <summary>
        /// 步骤前景色
        /// </summary>
        /// <value>The color of the step fore.</value>
        [Description("步骤前景色"), Category("自定义")]
        public Color StepForeColor
        {
            get { return _mStepForeColor; }
            set
            {
                _mStepForeColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m step font color
        /// </summary>
        private Color _mStepFontColor = Color.White;

        /// <summary>
        /// 步骤文字颜色
        /// </summary>
        /// <value>The color of the step font.</value>
        [Description("步骤文字景色"), Category("自定义")]
        public Color StepFontColor
        {
            get => _mStepFontColor;
            set
            {
                _mStepFontColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m step width
        /// </summary>
        private int _mStepWidth = 35;
        /// <summary>
        /// 步骤宽度
        /// </summary>
        /// <value>The width of the step.</value>
        [Description("步骤宽度景色"), Category("自定义")]
        public int StepWidth
        {
            get => _mStepWidth;
            set
            {
                _mStepWidth = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m steps
        /// </summary>
        private string[] _mSteps = new[] { "step1", "step2", "step3" };

        /// <summary>
        /// Gets or sets the steps.
        /// </summary>
        /// <value>The steps.</value>
        [Description("步骤"), Category("自定义")]
        public string[] Steps
        {
            get => _mSteps;
            set
            {
                if (_mSteps == null || _mSteps.Length <= 1)
                    return;
                _mSteps = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m step index
        /// </summary>
        private int _mStepIndex;

        /// <summary>
        /// Gets or sets the index of the step.
        /// </summary>
        /// <value>The index of the step.</value>
        [Description("步骤位置"), Category("自定义")]
        public int StepIndex
        {
            get => _mStepIndex;
            set
            {
                if (value > Steps.Length)
                    return;
                _mStepIndex = value;
                Refresh();
                IndexChecked?.Invoke(this, null);
            }
        }

        /// <summary>
        /// The m line width
        /// </summary>
        private int _mLineWidth = 2;

        /// <summary>
        /// Gets or sets the width of the line.
        /// </summary>
        /// <value>The width of the line.</value>
        [Description("连接线宽度,最小2"), Category("自定义")]
        public int LineWidth
        {
            get => _mLineWidth;
            set
            {
                if (value < 2)
                    return;
                _mLineWidth = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m img completed
        /// </summary>
        private Image _mImgCompleted;
        /// <summary>
        /// Gets or sets the img completed.
        /// </summary>
        /// <value>The img completed.</value>
        [Description("已完成步骤图片，当不为空时，已完成步骤将不再显示数字,建议24*24大小"), Category("自定义")]
        public Image ImgCompleted
        {
            get => _mImgCompleted;
            set
            {
                _mImgCompleted = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m LST cache rect
        /// </summary>
        List<Rectangle> _mLstCacheRect = new List<Rectangle>();

        /// <summary>
        /// Handles the MouseDown event of the UCStep control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        void Step_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            var index = _mLstCacheRect.FindIndex(p => p.Contains(e.Location));
            if (index >= 0)
            {
                StepIndex = index + 1;
            }
        }

        /// <summary>
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SetGDIHigh();

            if (_mSteps != null && _mSteps.Length > 0)
            {
                var sizeFirst = g.MeasureString(_mSteps[0], this.Font);
                var y = (this.Height - _mStepWidth - 10 - (int)sizeFirst.Height) / 2;
                if (y < 0)
                    y = 0;

                var intTxtY = y + _mStepWidth + 10;
                var intLeft = 0;
                if (sizeFirst.Width > _mStepWidth)
                {
                    intLeft = (int)(sizeFirst.Width - _mStepWidth) / 2 + 1;
                }

                var intRight = 0;
                var sizeEnd = g.MeasureString(_mSteps[_mSteps.Length - 1], this.Font);
                if (sizeEnd.Width > _mStepWidth)
                {
                    intRight = (int)(sizeEnd.Width - _mStepWidth) / 2 + 1;
                }

                var intSplitWidth = (this.Width - _mSteps.Length - (_mSteps.Length * _mStepWidth) - intRight - intLeft) / (_mSteps.Length - 1);
                if (intSplitWidth < 20)
                    intSplitWidth = 20;
                _mLstCacheRect = new List<Rectangle>();
                for (var i = 0; i < _mSteps.Length; i++)
                {
                    #region 画圆，横线
                    var rectEllipse = new Rectangle(new Point(intLeft + i * (_mStepWidth + intSplitWidth), y), new Size(_mStepWidth, _mStepWidth));
                    _mLstCacheRect.Add(rectEllipse);
                    g.FillEllipse(new SolidBrush(_mStepBackColor), rectEllipse);

                    if (_mStepIndex > i)
                    {
                        g.FillEllipse(new SolidBrush(MetroPaint.GetStyleColor(StepStyle) ), new Rectangle(new Point(intLeft + i * (_mStepWidth + intSplitWidth) + 2, y + 2), new Size(_mStepWidth - 4, _mStepWidth - 4)));

                    }
                    if (_mStepIndex > i && _mImgCompleted != null)
                    {
                        g.DrawImage(_mImgCompleted, new Rectangle(new Point((intLeft + i * (_mStepWidth + intSplitWidth) + (_mStepWidth - 24) / 2), y + (_mStepWidth - 24) / 2), new Size(24, 24)), 0, 0, _mImgCompleted.Width, _mImgCompleted.Height, GraphicsUnit.Pixel, null);
                    }
                    else
                    {
                        var numSize = g.MeasureString((i + 1).ToString(), this.Font);
                        g.DrawString((i + 1).ToString(), Font, new SolidBrush(_mStepFontColor), new Point(intLeft + i * (_mStepWidth + intSplitWidth) + (_mStepWidth - (int)numSize.Width) / 2 + 1, y + (_mStepWidth - (int)numSize.Height) / 2 + 1));
                    }
                    #endregion

                    var sizeTxt = g.MeasureString(_mSteps[i], this.Font);
                    g.DrawString(_mSteps[i], Font, new SolidBrush(_mStepIndex > i ? MetroPaint.GetStyleColor(StepStyle) : _mStepBackColor), new Point(intLeft + i * (_mStepWidth + intSplitWidth) + (_mStepWidth - (int)sizeTxt.Width) / 2 + 1, intTxtY));
                }

                // 画进度线
                for (var i = 0; i < _mSteps.Length; i++)
                {
                    if (_mStepIndex > i)
                    {
                        if (i != _mSteps.Length - 1)
                        {
                            if (_mStepIndex == i + 1)
                            {
                                g.DrawLine(new Pen(MetroPaint.GetStyleColor(StepStyle), _mLineWidth), new Point(intLeft + i * (_mStepWidth + intSplitWidth) + _mStepWidth - 3, y + ((_mStepWidth) / 2)), new Point(intLeft + i * (_mStepWidth + intSplitWidth) + _mStepWidth + intSplitWidth / 2, y + ((_mStepWidth) / 2)));
                                g.DrawLine(new Pen(_mStepBackColor, _mLineWidth), new Point(intLeft + i * (_mStepWidth + intSplitWidth) + _mStepWidth + intSplitWidth / 2, y + ((_mStepWidth) / 2)), new Point(intLeft + (i + 1) * (_mStepWidth + intSplitWidth) + 10, y + ((_mStepWidth) / 2)));
                            }
                            else
                            {
                                g.DrawLine(new Pen(MetroPaint.GetStyleColor(StepStyle), _mLineWidth), new Point(intLeft + i * (_mStepWidth + intSplitWidth) + _mStepWidth - 3, y + ((_mStepWidth) / 2)), new Point(intLeft + (i + 1) * (_mStepWidth + intSplitWidth) + 10, y + ((_mStepWidth) / 2)));
                            }
                        }
                    }
                    else
                    {
                        if (i != _mSteps.Length - 1)
                        {
                            g.DrawLine(new Pen(_mStepBackColor, _mLineWidth), new Point(intLeft + i * (_mStepWidth + intSplitWidth) + _mStepWidth - 3, y + ((_mStepWidth) / 2)), new Point(intLeft + (i + 1) * (_mStepWidth + intSplitWidth) + 10, y + ((_mStepWidth) / 2)));
                        }
                    }
                }
            }

        }
    }
}
