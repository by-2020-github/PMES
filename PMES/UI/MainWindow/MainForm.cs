using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PMES.Model;

namespace PMES.UI.MainWindow
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private ProductInfo _productInfo = new ProductInfo();
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     选择不同的打印标签模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelSettings(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     串口通讯设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommunicationSettings(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     打印机设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrinterSettings(object sender, EventArgs e)
        {

        }
    }
}