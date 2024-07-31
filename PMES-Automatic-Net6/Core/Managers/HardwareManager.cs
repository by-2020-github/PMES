using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMES_Respository.DataStruct;
using S7.Net;
using S7.Net.Types;
using Serilog;

namespace PMES_Automatic_Net6.Core.Managers
{
    public class HardwareManager
    {
        private ILogger Logger => SerilogManager.GetOrCreateLogger();

        private static Lazy<HardwareManager> _holder = new Lazy<HardwareManager>(() => new HardwareManager());

        public static HardwareManager Instance => _holder.Value;

        private HardwareManager()
        {
        }


        private Lazy<Plc> _plc = new Lazy<Plc>(() => new Plc(CpuType.S7200, PMESConfig.Default.PlcIp,
            (short)PMESConfig.Default.PlcRack, (short)PMESConfig.Default.PlcSlot));

        public Plc Plc => _plc.Value;

        public bool InitPlc()
        {
            if (Plc.IsConnected)
            {
                return true;
            }

            try
            {
                Plc.Open();
                StartReading();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"打开PLC失败！{e.Message}");
                return false;
            }
        }

        /// <summary>
        ///     其它正常结构体
        /// </summary>
        public Action<Object> OnReceive { get; set; }

        /// <summary>
        ///     读到重量和条码
        /// </summary>
        public Action<List<DataItem>> OnWeightAndCodeChanged { get; set; }

        /// <summary>
        ///     读到盘码
        /// </summary>
        public Action<List<DataItem>> OnReelCodeChanged { get; set; }

        /// <summary>
        ///     读到箱外标签
        /// </summary>
        public Action<List<DataItem>> OnBoxBarCodeChanged { get; set; }

        public const int IntervalTime = 20;

        public void StartReading()
        {
            Task.Run(async () =>
            {
                while (Plc.IsConnected)
                {
                    await Task.Delay(IntervalTime);

                    #region 拆垛

                    //1 检测拆垛信息交互区
                    var pmesCmdUnStacking = new PmesCmdUnStacking();
                    Plc.ReadClass(pmesCmdUnStacking, 501);
                    if (!pmesCmdUnStacking.Equals(GlobalVar.PmesCmdUnStacking))
                        OnReceive?.Invoke(pmesCmdUnStacking);

                    //1.1 202工位
                    await Task.Delay(IntervalTime);
                    var plcCmdUnStacking = new PlcCmdUnStacking();
                    Plc.ReadClass(plcCmdUnStacking, 502);
                    if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking1))
                    {
                        GlobalVar.PlcCmdUnStacking1 = plcCmdUnStacking;
                        OnReceive?.Invoke(plcCmdUnStacking);
                    }

                    await Task.Delay(IntervalTime);
                    Plc.ReadClass(plcCmdUnStacking, 503);
                    if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking2))
                    {
                        GlobalVar.PlcCmdUnStacking2 = plcCmdUnStacking;
                        OnReceive?.Invoke(plcCmdUnStacking);
                    }

                    await Task.Delay(IntervalTime);
                    Plc.ReadClass(plcCmdUnStacking, 504);
                    if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking3))
                    {
                        GlobalVar.PlcCmdUnStacking3 = plcCmdUnStacking;
                        OnReceive?.Invoke(plcCmdUnStacking);
                    }

                    await Task.Delay(IntervalTime);
                    Plc.ReadClass(plcCmdUnStacking, 505);
                    if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking4))
                    {
                        GlobalVar.PlcCmdUnStacking4 = plcCmdUnStacking;
                        OnReceive?.Invoke(plcCmdUnStacking);
                    }

                    await Task.Delay(IntervalTime);
                    Plc.ReadClass(plcCmdUnStacking, 506);
                    if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking5))
                    {
                        GlobalVar.PlcCmdUnStacking5 = plcCmdUnStacking;
                        OnReceive?.Invoke(plcCmdUnStacking);
                    }

                    //1.6 207工位
                    await Task.Delay(IntervalTime);
                    Plc.ReadClass(plcCmdUnStacking, 507);
                    if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking6))
                    {
                        GlobalVar.PlcCmdUnStacking6 = plcCmdUnStacking;
                        OnReceive?.Invoke(plcCmdUnStacking);
                    }

                    #endregion

                    #region 带字符串的指令

                    await Task.Delay(IntervalTime);
                    var pmesDataItemList = new PmesDataItemList();

                    //读到重量和条码--->访问后台接口
                    Plc.ReadMultipleVars(pmesDataItemList.PmesWeightAndBarCode.ToList());
                    if (!ComparerDataList(pmesDataItemList.PmesWeightAndBarCode.ToList(),
                            GlobalVar.PmesDataItems.PmesWeightAndBarCode.ToList()))
                    {
                        GlobalVar.PmesDataItems.PmesWeightAndBarCode = pmesDataItemList.PmesWeightAndBarCode;
                        OnWeightAndCodeChanged?.Invoke(GlobalVar.PmesDataItems.PmesWeightAndBarCode.ToList());
                    }

                    //箱外标签校验
                    Plc.ReadMultipleVars(pmesDataItemList.PmesPackingBox.ToList());
                    if (!ComparerDataList(pmesDataItemList.PmesPackingBox.ToList(),
                            GlobalVar.PmesDataItems.PmesPackingBox.ToList()))
                    {
                        GlobalVar.PmesDataItems.PmesPackingBox = pmesDataItemList.PmesPackingBox;
                        OnBoxBarCodeChanged?.Invoke(GlobalVar.PmesDataItems.PmesPackingBox.ToList());
                    }

                    //条码复核
                    Plc.ReadMultipleVars(pmesDataItemList.PmesReelCodeCheck.ToList());
                    if (!ComparerDataList(pmesDataItemList.PmesReelCodeCheck.ToList(),
                            GlobalVar.PmesDataItems.PmesReelCodeCheck.ToList()))
                    {
                        GlobalVar.PmesDataItems.PmesReelCodeCheck = pmesDataItemList.PmesReelCodeCheck;
                        OnReelCodeChanged?.Invoke(GlobalVar.PmesDataItems.PmesReelCodeCheck.ToList());
                    }

                    #endregion
                }
            });
        }


        private bool ComparerDataList(List<DataItem> ls1, List<DataItem> ls2)
        {
            if (ls1.Count != ls2.Count)
                return false;
            for (int i = 0; i < ls1.Count; i++)
            {
                if (ls1[i].Value != ls2[i].Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}