using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMES.TemplteEdit
{
    public partial class CustomEdit : Form
    {
        public CustomEdit()
        {
            InitializeComponent();
            this.propertyGrid1.AutoScroll = true;

        }

        public void SetObj(object obj)
        {
            this.propertyGrid1.SelectedObject = obj;
        }
        public object GetObj( )
        {
            return this.propertyGrid1.SelectedObject;
        }
    }
}
