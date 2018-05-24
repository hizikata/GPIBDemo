using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NationalInstruments.NI4882;

namespace GPIBDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        Device Device;
        #endregion Fields
        #region Properties
        public List<string> ListBoardAdd { get; set; }
        public byte[] PrimaryAddress { get; set; }
        #endregion Properties
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int boardAddress = cmbBoardAddress.SelectedIndex;
                byte primaryAddress = (byte)cmbPriamryAddress.SelectedItem;
                Device = new Device(boardAddress, primaryAddress);
                Device.Write("*IDN?");
                string msg = Device.ReadString();
                if (string.IsNullOrWhiteSpace(msg))
                {
                    MessageBox.Show("初始化失败", "系统提示");
                }
                else
                {
                    tbIDN.Text = msg;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }

        }

        private void btnClosed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListBoardAdd = new List<string>
            {
                "GPIB0","GPIB1","GPIB2","GPIB3","GPIB4",
                "GPIB5","GPIB6","GPIB7","GPIB4","GPIB9"
            };
            PrimaryAddress = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            string[] ModeStr = { "DFBAN", "FPAN" };
            cmbBoardAddress.ItemsSource = ListBoardAdd;
            cmbPriamryAddress.ItemsSource = PrimaryAddress;
            cmbMode.ItemsSource = ModeStr;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (Device == null)
            {
                MessageBox.Show("仪器未初始化！", "系统提示");
                return;
            }
            if (!string.IsNullOrWhiteSpace(tbCommand.Text.Trim()))
            {
                Device.Write(tbCommand.Text.Trim());
            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            if (Device == null)
            {
                MessageBox.Show("仪器未初始化！", "系统提示");
                return;
            }
            else
            {
                this.tbReadArea.Text = Device.ReadString();
            }

        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Device == null)
                {
                    MessageBox.Show("仪器未初始化！", "系统提示");
                    return;
                }
                if (Validation())
                {
                    Device.Write(cmbMode.SelectedItem.ToString().Trim());
                    Device.Write(string.Format(@"CTRWL {0}", this.tbCenterLength.Text.Trim()));
                    Device.Write(string.Format(@"SPAN {0}", this.tbSpan.Text.Trim()));
                    Device.Write(string.Format(@"LSCL {0}", this.tbSpan.Text.Trim()));
                    if (!string.IsNullOrEmpty(this.tbRes.Text.Trim()))
                    {
                        Device.Write(string.Format(@"RESLN {0}", this.tbRefLevel.Text.Trim()));
                    }
                    if (!string.IsNullOrEmpty(this.tbRefLevel.Text.Trim()))
                    {
                        Device.Write(string.Format(@"REFL {0}", this.tbRefLevel.Text.Trim()));
                    }
                    MessageBox.Show("OSA初始化成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }
        }
        bool Validation()
        {
            if (cmbMode.SelectedIndex == -1)
            {
                MessageBox.Show("请选择光谱模式", "系统提示");
                return false;
            }
            if (string.IsNullOrEmpty(tbCenterLength.Text.Trim()) || string.IsNullOrEmpty(tbSpan.Text.Trim())
                || string.IsNullOrEmpty(tbScale.Text.Trim()))
            {
                MessageBox.Show("请填写中心波长、Span、Scale","系统提示");
                    return false;
            }
            else
            {
                return true;
            }
        }

    }
}

