using System;
using System.Collections.Generic;
using System.Text;

namespace PMES_Common
{
    public class PlcCmdAttribute : System.Attribute
    {
        public int DbBlock { get; set; }

        public PlcCmdAttribute(int dbBlock)
        {
            DbBlock = dbBlock;
        }
    }
}