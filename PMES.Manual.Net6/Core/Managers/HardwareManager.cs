using System.Text;
using Newtonsoft.Json;
using PMES_Respository.DataStruct;
using S7.Net;
using S7.Net.Types;
using Serilog;

namespace PMES.Manual.Net6.Core.Managers
{
    public class HardwareManager
    {
        public static ILogger Logger { get; set; }

        private static Lazy<HardwareManager> _holder = new Lazy<HardwareManager>(() => new HardwareManager());

        public static HardwareManager Instance => _holder.Value;

        private HardwareManager()
        {
        }
    }
}