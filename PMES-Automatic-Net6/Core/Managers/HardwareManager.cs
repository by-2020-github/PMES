using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FluentModbus;
using Newtonsoft.Json;
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


        private Lazy<Plc> _plc = new Lazy<Plc>(() => new Plc(CpuType.S71200, PMESConfig.Default.PlcIp,
            (short)PMESConfig.Default.PlcRack, (short)PMESConfig.Default.PlcSlot));

        //private Lazy<ModbusTcpClient> _modbusTcpClient = new Lazy<ModbusTcpClient>(new ModbusTcpClient());
        private Lazy<HslCommunication.ModBus.ModbusTcpNet> _modbusTcpClient =
            new Lazy<HslCommunication.ModBus.ModbusTcpNet>(
                new HslCommunication.ModBus.ModbusTcpNet(PMESConfig.Default.PlcXinJieIp,
                    PMESConfig.Default.PlcXinJiePort));

        public Plc Plc => _plc.Value;
        public HslCommunication.ModBus.ModbusTcpNet PlcXj => _modbusTcpClient.Value;

        public bool InitPlc()
        {
            var logNetSingle = new HslCommunication.LogNet.LogNetSingle(@"\log\modbus.log");
            PlcXj.LogNet = logNetSingle;

            try
            {
                if (PlcXj.ConnectServer().IsSuccess)
                {
                    Logger?.Information($"打开信捷PLC成功");
                }
                else
                {
                    Logger?.Error($"打开信捷PLC失败");
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"打开信捷PLC失败！\n{e.Message}");
            }

            try
            {
                Plc.Open();
                StartReading();
                Logger?.Information($"打开西门子PLC成功");
                return true;
            }
            catch (Exception e)
            {
                Logger?.Error($"打开西门子PLC失败！参数：\n{e.Message}");
            }

            return false;
        }

        /// <summary>
        ///     其它正常结构体
        /// </summary>
        public Func<Object, Task> OnReceive { get; set; }

        /// <summary>
        ///     读到重量和条码
        /// </summary>
        public Func<List<DataItem>, Task> OnWeightAndCodeChanged { get; set; }

        /// <summary>
        ///     读到盘码
        /// </summary>
        public Func<List<DataItem>, Task> OnReelCodeChanged { get; set; }

        /// <summary>
        ///     读到箱外标签
        /// </summary>
        public Func<List<DataItem>, Task> OnBoxBarCodeChanged { get; set; }


        /// <summary>
        ///     Box到达
        /// </summary>
        public Func<List<DataItem>, Task> OnBoxArrived { get; set; }

        public const int IntervalTime = 50;

        public void StartReading()
        {
            Task.Run(async () =>
            {
                var second = false;
                while (Plc.IsConnected)
                {
                    try
                    {
                        await Task.Delay(IntervalTime);

                        #region 拆垛

                        //1 检测拆垛信息交互区
                        var pmesCmdUnStacking = new PmesCmdUnStacking
                        {
                            DeviceId = 1,
                            WorkPositionId = 201,
                            ReelSpecification = 1,
                            ReelNum = 1,
                            UnStackSpeed = 20,
                            ReelHeight = 12,
                            Reserve1 = 0,
                            Reserve2 = 0,
                            PmesAndPlcReadWriteFlag = 0
                        };
                        Plc.ReadClass(pmesCmdUnStacking, 501);
                        Logger?.Verbose($"pmesCmdUnStacking:{pmesCmdUnStacking}");
                        if (!pmesCmdUnStacking.Equals(GlobalVar.PmesCmdUnStacking))
                        {
                            GlobalVar.PmesCmdUnStacking = pmesCmdUnStacking;
                            if (second)
                                OnReceive?.Invoke(pmesCmdUnStacking);
                        }

                        //1.1 202工位
                        await Task.Delay(IntervalTime);
                        var plcCmdUnStacking = new PlcCmdUnStacking
                        {
                            DeviceId = 1,
                            WorkPositionId = 202,
                        };
                        Plc.ReadClass(plcCmdUnStacking, 502);
                        Logger?.Verbose($"plcCmdUnStacking1:{plcCmdUnStacking}");
                        if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking1))
                        {
                            GlobalVar.PlcCmdUnStacking1 = plcCmdUnStacking;
                            if (second)
                                OnReceive?.Invoke(plcCmdUnStacking);
                        }

                        await Task.Delay(IntervalTime);
                        plcCmdUnStacking = new PlcCmdUnStacking
                        {
                            DeviceId = 1,
                            WorkPositionId = 203,
                        };
                        Plc.ReadClass(plcCmdUnStacking, 503);
                        Logger?.Verbose($"plcCmdUnStacking2:{plcCmdUnStacking}");
                        if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking2))
                        {
                            GlobalVar.PlcCmdUnStacking2 = plcCmdUnStacking;
                            if (second)
                                OnReceive?.Invoke(plcCmdUnStacking);
                        }

                        await Task.Delay(IntervalTime);
                        plcCmdUnStacking = new PlcCmdUnStacking
                        {
                            DeviceId = 1,
                            WorkPositionId = 204,
                        };
                        Plc.ReadClass(plcCmdUnStacking, 504);
                        Logger?.Verbose($"plcCmdUnStacking3:{plcCmdUnStacking}");
                        if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking3))
                        {
                            GlobalVar.PlcCmdUnStacking3 = plcCmdUnStacking;
                            if (second)
                                OnReceive?.Invoke(plcCmdUnStacking);
                        }

                        await Task.Delay(IntervalTime);
                        plcCmdUnStacking = new PlcCmdUnStacking
                        {
                            DeviceId = 1,
                            WorkPositionId = 205,
                        };
                        Plc.ReadClass(plcCmdUnStacking, 505);
                        Logger?.Verbose($"plcCmdUnStacking4:{plcCmdUnStacking}");
                        if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking4))
                        {
                            GlobalVar.PlcCmdUnStacking4 = plcCmdUnStacking;
                            if (second)
                                OnReceive?.Invoke(plcCmdUnStacking);
                        }

                        await Task.Delay(IntervalTime);
                        plcCmdUnStacking = new PlcCmdUnStacking
                        {
                            DeviceId = 1,
                            WorkPositionId = 206,
                        };
                        Plc.ReadClass(plcCmdUnStacking, 506);
                        Logger?.Verbose($"plcCmdUnStacking5:{plcCmdUnStacking}");
                        if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking5))
                        {
                            GlobalVar.PlcCmdUnStacking5 = plcCmdUnStacking;
                            if (second)
                                OnReceive?.Invoke(plcCmdUnStacking);
                        }

                        //1.6 207工位
                        await Task.Delay(IntervalTime);
                        plcCmdUnStacking = new PlcCmdUnStacking
                        {
                            DeviceId = 1,
                            WorkPositionId = 207,
                        };
                        Plc.ReadClass(plcCmdUnStacking, 507);
                        Logger?.Verbose($"plcCmdUnStacking6:{plcCmdUnStacking}");

                        if (!plcCmdUnStacking.Equals(GlobalVar.PlcCmdUnStacking6))
                        {
                            GlobalVar.PlcCmdUnStacking6 = plcCmdUnStacking;
                            if (second)
                                OnReceive?.Invoke(plcCmdUnStacking);
                        }

                        #endregion

                        #region 带字符串的指令

                        await Task.Delay(IntervalTime);
                        var pmesDataItemList = new PmesDataItemList();


                        //读到重量和条码--->访问后台接口
                        await Task.Delay(IntervalTime);

                        Plc.ReadMultipleVars(pmesDataItemList.PmesWeightAndBarCode);
                        Logger?.Verbose(
                            $"读取重量成功,PmesWeightAndBarCode:{PrintDataItems(pmesDataItemList.PmesWeightAndBarCode)}");
                        if (pmesDataItemList.PmesWeightAndBarCode[4].Value.ToString() == "1")
                        {
                            if (!ComparerDataList(pmesDataItemList.PmesWeightAndBarCode,
                                    GlobalVar.PmesDataItems.PmesWeightAndBarCode))
                            {
                                GlobalVar.PmesDataItems.PmesWeightAndBarCode = pmesDataItemList.PmesWeightAndBarCode;
                                if (second)
                                {
                                    if (int.Parse(pmesDataItemList.PmesWeightAndBarCode[3].Value.ToString()) != 0
                                        && int.Parse(pmesDataItemList.PmesWeightAndBarCode[4].Value.ToString()) != 0
                                        && !string.IsNullOrEmpty(pmesDataItemList.PmesWeightAndBarCode[2].Value
                                            .ToString()))
                                    {
                                        OnWeightAndCodeChanged?.Invoke(
                                            GlobalVar.PmesDataItems.PmesWeightAndBarCode.ToList());
                                        Response(pmesDataItemList.PmesWeightAndBarCode);
                                    }
                                }
                            }
                        }


                        //条码复核
                        await Task.Delay(IntervalTime);

                        Plc.ReadMultipleVars(pmesDataItemList.PmesReelCodeCheck);
                        Logger?.Verbose(
                            $"读取条码成功,PmesReelCodeCheck:{PrintDataItems(pmesDataItemList.PmesReelCodeCheck.ToList())}");

                        if (pmesDataItemList.PmesReelCodeCheck[3].Value.ToString() == "1")
                        {
                            if (!ComparerDataList(pmesDataItemList.PmesReelCodeCheck.ToList(),
                                    GlobalVar.PmesDataItems.PmesReelCodeCheck.ToList()))
                            {
                                GlobalVar.PmesDataItems.PmesReelCodeCheck = pmesDataItemList.PmesReelCodeCheck;

                                if (second)
                                {
                                    OnReelCodeChanged?.Invoke(
                                        GlobalVar.PmesDataItems.PmesReelCodeCheck.ToList());
                                    Response(pmesDataItemList.PmesReelCodeCheck);
                                }
                            }
                        }

                        //箱外标签校验
                        await Task.Delay(IntervalTime);
                        Plc.ReadMultipleVars(pmesDataItemList.PmesPackingBox.ToList());
                        Logger?.Verbose(
                            $"读取箱码成功,PmesPackingBox:{PrintDataItems(pmesDataItemList.PmesPackingBox.ToList())}");
                        if (pmesDataItemList.PmesPackingBox[3].Value.ToString() == "1")
                        {
                            if (!ComparerDataList(pmesDataItemList.PmesPackingBox.ToList(),
                                    GlobalVar.PmesDataItems.PmesPackingBox.ToList()))
                            {
                                GlobalVar.PmesDataItems.PmesPackingBox = pmesDataItemList.PmesPackingBox;
                                if (second)
                                {
                                    OnBoxBarCodeChanged?.Invoke(GlobalVar.PmesDataItems.PmesPackingBox.ToList());
                                    Response(pmesDataItemList.PmesPackingBox);
                                }
                            }
                        }

                        if (!pmesDataItemList.PmesPackingBox[^2].Value.ToString().Equals(GlobalVar.IsBoxOnPos))
                        {
                            GlobalVar.IsBoxOnPos = pmesDataItemList.PmesPackingBox[^2].Value.ToString();
                            if (GlobalVar.IsBoxOnPos.Equals("1"))
                                OnBoxArrived?.Invoke(pmesDataItemList.PmesPackingBox);
                        }

                        #endregion


                        //1 码垛
                        await Task.Delay(IntervalTime);
                        var pmesStacking = new PmesStacking
                        {
                            DeviceId = 2,
                            WorkPositionId = 241,
                            ReelSpecification = 1,
                            StackModel = 1,
                            StackingSpeed = 20,
                        };
                        Plc.ReadClass(pmesStacking, 540);
                        Logger?.Verbose($"pmesStacking:{pmesStacking}");
                        if (!pmesStacking.Equals(GlobalVar.PmesStacking))
                        {
                            GlobalVar.PmesStacking = pmesStacking;
                            if (second)
                                OnReceive?.Invoke(pmesStacking);
                        }


                        //251工位  异常剔除位
                        await Task.Delay(IntervalTime);
                        var plcCmdStacking1 = new PlcCmdStacking();
                        Plc.ReadClass(plcCmdStacking1, 541);
                        Logger?.Verbose($"plcCmdStacking1:{plcCmdStacking1}");
                        if (!plcCmdStacking1.Equals(GlobalVar.PlcCmdStacking1))
                        {
                            GlobalVar.PlcCmdStacking1 = plcCmdStacking1;
                            if (second)
                                OnReceive?.Invoke(plcCmdStacking1);
                        }

                        //252工位  正常码垛位
                        await Task.Delay(IntervalTime);
                        var plcCmdStacking2 = new PlcCmdStacking();
                        Plc.ReadClass(plcCmdStacking2, 542);
                        Logger?.Verbose($"plcCmdStacking2:{plcCmdStacking2}");
                        if (!plcCmdStacking2.Equals(GlobalVar.PlcCmdStacking2))
                        {
                            GlobalVar.PlcCmdStacking1 = plcCmdStacking2;
                            if (second)
                                OnReceive?.Invoke(plcCmdStacking2);
                        }


                        //253工位  正常码垛位
                        await Task.Delay(IntervalTime);
                        var plcCmdStacking3 = new PlcCmdStacking();
                        Plc.ReadClass(plcCmdStacking3, 543);
                        Logger?.Verbose($"plcCmdStacking3:{plcCmdStacking3}");
                        if (!plcCmdStacking3.Equals(GlobalVar.PlcCmdStacking3))
                        {
                            GlobalVar.PlcCmdStacking1 = plcCmdStacking3;
                            if (second)
                                OnReceive?.Invoke(plcCmdStacking3);
                        }

                        await Task.Delay(IntervalTime);
                        var plcCmdStacking4 = new PlcCmdStacking();
                        Plc.ReadClass(plcCmdStacking4, 544);
                        Logger?.Verbose($"plcCmdStacking4:{plcCmdStacking4}");
                        if (!plcCmdStacking4.Equals(GlobalVar.PlcCmdStacking4))
                        {
                            GlobalVar.PlcCmdStacking1 = plcCmdStacking4;
                            if (second)
                                OnReceive?.Invoke(plcCmdStacking4);
                        }

                        await Task.Delay(IntervalTime);
                        var plcCmdStacking5 = new PlcCmdStacking();
                        Plc.ReadClass(plcCmdStacking5, 545);
                        Logger?.Verbose($"plcCmdStacking5:{plcCmdStacking5}");
                        if (!plcCmdStacking5.Equals(GlobalVar.PlcCmdStacking5))
                        {
                            GlobalVar.PlcCmdStacking1 = plcCmdStacking5;
                            if (second)
                                OnReceive?.Invoke(plcCmdStacking5);
                        }

                        await Task.Delay(IntervalTime);
                        var plcCmdStacking6 = new PlcCmdStacking();
                        Plc.ReadClass(plcCmdStacking6, 546);
                        Logger?.Verbose($"plcCmdStacking6:{plcCmdStacking6}");
                        if (!plcCmdStacking6.Equals(GlobalVar.PlcCmdStacking6))
                        {
                            GlobalVar.PlcCmdStacking1 = plcCmdStacking6;
                            if (second)
                                OnReceive?.Invoke(plcCmdStacking6);
                        }

                        await Task.Delay(IntervalTime);
                        var plcCmdStacking7 = new PlcCmdStacking();
                        Plc.ReadClass(plcCmdStacking7, 547);
                        Logger?.Verbose($"plcCmdStacking7:{plcCmdStacking7}");
                        if (!plcCmdStacking7.Equals(GlobalVar.PlcCmdStacking7))
                        {
                            GlobalVar.PlcCmdStacking1 = plcCmdStacking7;
                            if (second)
                                OnReceive?.Invoke(plcCmdStacking7);
                        }

                        second = true;
                    }
                    catch (Exception e)
                    {
                        Logger?.Error($"读取异常。\n{e.Message}");
                    }
                }
            });
        }

        public static string PrintDataItems(List<DataItem> dataItems)
        {
            var sb = new StringBuilder();
            dataItems.ForEach(s =>
            {
                sb.Append(
                    $"\nDataType = {s.DataType},\tVarType = {s.VarType},\tDB = {s.DB},\tStartByteAdr = {s.StartByteAdr},\tBitAdr = {s.BitAdr},\tCount = 1,\tValue = {s.Value}");
            });
            return sb.ToString();
        }

        public bool ComparerDataList(List<DataItem> ls1, List<DataItem> ls2)
        {
            if (ls1.Count != ls2.Count)
                return false;

            var lsItem1 = JsonConvert.SerializeObject(ls1);
            var lsItem2 = JsonConvert.SerializeObject(ls2);
            Logger.Verbose($"比较两个对象的值(1)：\n{lsItem1}");
            Logger.Verbose($"比较两个对象的值(2)：\n{lsItem2}");
            Logger.Verbose($"比较两个对象的结果：{lsItem1 == lsItem2}");
            return lsItem1 == lsItem2;

            //Logger.Verbose($"比较两个对象的值(1)：\n{PrintDataItems(ls1)}");
            //Logger.Verbose($"比较两个对象的值(2)：\n{PrintDataItems(ls2)}");
            //for (int i = 0; i < ls1.Count; i++)
            //{
            //    var lsItem1 = ls1[i].Value?.ToString() ?? "";
            //    var lsItem2 = ls2[i].Value?.ToString() ?? "";
            //    Logger.Verbose($"比较第{i}个值：{lsItem1} <--?--> {lsItem2}");
            //    if (lsItem1 != lsItem2)
            //    {
            //        Logger.Information($"第{i}个对象，{ls1[i].Value} != {ls2[i]}");
            //        return false;
            //    }
            //}

            return true;
        }

        public void Response(List<DataItem> dataItems)
        {
            var items = dataItems.Select(s => new DataItem
            {
                DataType = s.DataType,
                VarType = s.VarType,
                DB = s.DB,
                StartByteAdr = s.StartByteAdr,
                BitAdr = s.BitAdr,
                Count = s.Count,
                Value = s.Value
            }).ToList();
            items[^3].Value = byte.Parse("2");
            Plc.Write(items.TakeLast(3).ToArray());
            Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
            Plc.ReadMultipleVarsAsync(items);
            Logger?.Information(
                ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
        }
    }
}