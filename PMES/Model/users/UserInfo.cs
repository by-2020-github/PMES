using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming
namespace PMES.Model.users
{
    public class UserInfo
    {
        public int authory { get; set; } = 1;
        public string belongLine { get; set; } = "人工线包装组";
        public string code { get; set; } = "TZQZ";
        public int isLoginAutoLine { get; set; } = 1;
        public string lineNo { get; set; } = "A1";
        public string loginUser { get; set; } = "18080808080";
        public string name { get; set; } = "aaa";
        public int userId { get; set; } = 1;
    }
}