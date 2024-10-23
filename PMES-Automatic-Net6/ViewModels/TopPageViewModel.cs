using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PMES_Automatic_Net6.Core;
using S7.Net;
using PMES_Automatic_Net6.Core.Managers;
using PMES_Respository.DataStruct;
using S7.Net.Types;
using DateTime = System.DateTime;
using PMES_Respository.reportModel;
using PMES.UC.reports;
using System.Drawing;
using System.IO;
using DevExpress.XtraReports.UI;
using HslCommunication.ModBus;
using FluentModbus;
using Newtonsoft.Json;
using DevExpress.XtraReports;
using static DevExpress.XtraEditors.Filtering.DataItemsExtension;
using DevExpress.Diagram.Core.Shapes;
using DevExpress.Utils.About;
using ProductInfo = PMES_Automatic_Net6.Model.ProductInfo;
using System.Reflection;
using DevExpress.Mvvm.Native;
using DevExpress.XtraSplashScreen;
using PMES_Common;
using DevExpress.Utils.Extensions;
using DevExpress.Pdf;
using PMES_Automatic_Net6.Model;
using Google.Protobuf.WellKnownTypes;
using System.Net.Http;
using PMES_Respository;
using DevExpress.XtraRichEdit.Layout.Engine;
using System.Xml.Xsl;
using PMES_Respository.tbs_sqlserver;
using System.Data.Common;
using System.Data;
using WebService = PMES_Automatic_Net6.Core.WebService;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class TopPageViewModel : ObservableObject
    {
        #region taskQueue

        [ObservableProperty] private int _taskQueueSelectedIndex = 0;

        #endregion

        public int Count { get; set; } = 0;
        private Plc Plc => HardwareManager.Instance.Plc;
        private ModbusTcpNet PlcXj => HardwareManager.Instance.PlcXj;
        public Serilog.ILogger Logger => SerilogManager.GetOrCreateLogger("Running");

        /// <summary>
        ///     记录工位上的信息 清空托盘后也清空
        /// </summary>
        private Dictionary<int, List<ProductInfo>> _stackProductInfos = new Dictionary<int, List<ProductInfo>>();

        /// <summary>
        ///     正常换跺数量
        /// </summary>
        [ObservableProperty] private int _exchangeStackNum;

        /// <summary>
        ///     异常换跺数量
        /// </summary>
        [ObservableProperty] private int _errorExchangeStackNum;

        private Queue<XtraReport> _reportsBox = new Queue<XtraReport>();
        private FSqlMysqlHelper _fSqlHelper = FreeSqlManager.FSqlMysqlHelper;
        private IFreeSql _fSql = FreeSqlManager.FSqlServer;
        private const byte _readFlag = 2;
        private ushort _reverse1 = 0;
        private ushort _reverse2 = 0;
        [ObservableProperty] private int _okCount = 0;
        [ObservableProperty] private int _errorCount = 0;
        [ObservableProperty] private int _totalCount = 0;
        [ObservableProperty] private int _totalOkCount = 0;
        [ObservableProperty] private int _totalNgCount = 0;
        private string _virtualBoxId = Guid.NewGuid().ToString();

        [ObservableProperty] private int _reelCountNoChange = 0;


        private readonly PmesStacking _pmesStackingError;

        /// <summary>
        ///     记录当前的任务队列
        /// </summary>
        private readonly List<T_plc_command> _plcCommandsRunning = new List<T_plc_command>();

        [ObservableProperty] private string? _productCode = "";
        [ObservableProperty] private string? _preScanCode1 = "";
        [ObservableProperty] private string? _preScanCode2 = "";
        [ObservableProperty] private string? _boxCode1 = "";
        [ObservableProperty] private string? _boxCode2 = "";
        [ObservableProperty] private string? _weight1 = "0";
        [ObservableProperty] private string? _weight2 = "0";

        /// <summary>
        ///     当前结果队列
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<TopGridModel> _listProcess = new ObservableCollection<TopGridModel>();

        /// <summary>
        ///     历史结果队列
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<TopGridModel> _listHistory = new ObservableCollection<TopGridModel>();

        public TopPageViewModel()
        {
            WebService.Logger = Logger;
            _pmesStackingError = new()
            {
                DeviceId = 2,
                WorkPositionId = 251,
                ReelSpecification = 1,
                StackModel = 1,
                StackingSpeed = 20,
                Reserve1 = 0,
                Reserve2 = 2,
                PmesAndPlcReadWriteFlag = 2
            };
            HardwareManager.Instance.OnReceive += Handler;
            HardwareManager.Instance.OnWeightAndCodeChanged += WeightAndCodeChanged;
            HardwareManager.Instance.OnReelCodeChanged += ReelCodeChanged;
            HardwareManager.Instance.OnBoxBarCodeChanged += BoxBarCodeChanged;
            HardwareManager.Instance.OnBoxArrived += BoxArrived;
            HardwareManager.Instance.OnBoxStacked += BoxStacked;
            HardwareManager.Instance.OnNoPageBoard += OnNoPageBoard;
            StartMonitorPlcCommonTb();
            SerilogManager.LogViewTextBox.TextChanged += ((sender, args) =>
                    GlobalVar.MainView.Dispatcher.Invoke(() => { SerilogManager.LogViewTextBox.ScrollToEnd(); })
                );
            WebService.Logger = Logger;
        }

        private async Task OnNoPageBoard()
        {
            var success = WebService.Instance.ClearAndApplyPageBoard();
        }

        /// <summary>
        ///     开始检测指令表
        /// </summary>
        private void StartMonitorPlcCommonTb()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await Task.Delay(2000);

                        var stationStatusList = await _fSql.Select<T_station_status>().ToListAsync();
                        //if (can540 && stationStatusList[2].Status == 1)
                        //{
                        //    try
                        //    {
                        //        cmdStack1.Reserve2 = cmdStack1.WorkPositionId;
                        //        await Plc.WriteClassAsync(cmdStack1, 540);
                        //        Logger?.Information(
                        //            $"Plc写入清垛指令，cmdStack:{cmdStack1.WorkPositionId}，cmdStack.Reserve2 ：{cmdStack1.Reserve2}");
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        Logger?.Error(LogInfo.Error($"PLC写入清垛指令失败：{cmdStack1} \nex:{e}"));
                        //    }
                        //}

                        //查出来
                        var cmdNeedExec = await _fSql.Select<T_plc_command>().Where(s => s.Status == 0).ToListAsync();
                        //拆垛[1]---一个拆完了才能下一个
                        {
                            //执行中的队列
                            if (_plcCommandsRunning.Count(s => s.PlcComandType == 1) == 0) //添加任务
                            {
                                var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 1 && s.Status == 0);
                                if (plcCommand != null)
                                {
                                    if ((await WaitStation(plcCommand.Cmd<PmesCmdUnStacking>().WorkPositionId, 1,
                                            2))) //只等2s 如果不行就检查下一个指令
                                    {
                                        _plcCommandsRunning.Add(plcCommand);
                                    }
                                }
                            }
                            else //执行任务
                            {
                                //其实拆垛队列只有一个任务
                                var plcCommand =
                                    _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 1, Status: 0 });
                                if (plcCommand != null)
                                {
                                    var cmd = plcCommand.Cmd<PmesCmdUnStacking>();
                                    plcCommand.Status = 2; //执行 状态改为执行中
                                    await _fSql.Update<T_plc_command>().Set(s => s.Status, 2)
                                        .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                                    await Plc.WriteClassAsync(cmd, 501);

                                    _ = Task.Run(async () =>
                                    {
                                        while (true)
                                        {
                                            Thread.Sleep(3000);
                                            var plcCommandWorkshopId = (int)plcCommand.WorkshopId!;
                                            try
                                            {
                                                var pmesCmdUnStacking =
                                                    Plc.ReadClass<PmesCmdUnStacking>(plcCommandWorkshopId + 300, 0);
                                                Logger?.Verbose(
                                                    $"当前拆垛工位:{plcCommandWorkshopId}，检测DB：{plcCommandWorkshopId + 300} ,还剩:{pmesCmdUnStacking!.ReelNum}");
                                                if (pmesCmdUnStacking!.ReelNum != 0) continue;
                                                Logger?.Verbose("拆跺完毕，开始申请入库！");

                                                plcCommand.Status = 1;
                                                Logger?.Information($"拆垛指令执行完毕：{plcCommand.PlcComandContent}");
                                                await WebService.Instance.EmptyTrayUnStacking(cmd!.WorkPositionId);
                     
                                                //1 修改数据库状态--->拆垛指令执行完毕
                                                var ret = await _fSql.Update<T_plc_command>()
                                                    .Set(s => s.Status, 1)
                                                    .Where(s => s.Id == plcCommand.Id)
                                                    .ExecuteAffrowsAsync();
                                                Logger?.Information(
                                                    $"拆垛指令，数据库更改指令为完成状态，结果:{ret}\t{plcCommand.PlcComandContent}");

                                                //2 队列移除拆垛指令
                                                _plcCommandsRunning.Remove(plcCommand);
                                                Logger?.Information(
                                                    $"[KEY STEP]队列移除拆垛指令:{plcCommand.PlcComandContent}");

                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                Logger?.Error($"检查工位状态失败！\n{e.Message}");
                                            }
                                        }
                                    });
                                }
                            }
                        }


                        //码垛[2]---码完了才能下一个   |   清垛
                        {
                            if (_plcCommandsRunning.Count(s => s.PlcComandType == 2 && s.Status == 0) == 0)
                            {
                                var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 2 && s.Status == 0);
                                if (plcCommand != null)
                                {
                                    if ((await WaitStation(plcCommand.Cmd<PmesStacking>().WorkPositionId, 1,
                                            2))) //只等2s 如果不行就检查下一个指令
                                    {
                                        plcCommand.Status = 2; //执行 状态改为执行中
                                        await _fSql.Update<T_plc_command>().Set(s => s.Status, 2)
                                            .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                                        _plcCommandsRunning.Add(plcCommand);
                                    }
                                }
                            }

                            //todo: 2024年9月26日 16点00分 新增
                            var plcCommondClearStack =
                                cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 9 && s.Status == 0);
                            if (plcCommondClearStack != null)
                            {
                                var r = await _fSql.Update<T_plc_command>().Set(s => s.Status, 2)
                                    .Where(s => s.Id == plcCommondClearStack.Id).ExecuteAffrowsAsync();
                                if (r <= 0)
                                {
                                    MessageBox.Show("发送清垛指令时更新数据库失败，程序终止！");
                                    return;
                                }

                                var cmd = plcCommondClearStack.Cmd<PmesStacking>();
                                Plc.WriteClass(cmd, 540);
                                Plc.ReadClass(cmd, 540);
                                Logger?.Information($"发送清垛指令结果:\n{cmd}");
                            }
                        }

                        //子母托[3]---拆完了才能下一个
                        {
                            if (_plcCommandsRunning.Count(s => s.PlcComandType == 3) == 0)
                            {
                                var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 3 && s.Status == 0);
                                if (plcCommand != null)
                                {
                                    var pmesCmdCombinationMotherChildTray =
                                        plcCommand.Cmd<PmesCmdCombinationMotherChildTray>();
                                    //TODO:子母托的位置释放了才可以组子母托
                                    if ((await WaitStation(
                                            pmesCmdCombinationMotherChildTray.ChildMotherTrayWorkPositionId, 3,
                                            2))) //只等2s 如果不行就检查下一个指令
                                    {
                                        _plcCommandsRunning.Add(plcCommand);
                                    }
                                }
                            }
                            else
                            {
                                var plcCommand =
                                    _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 3, Status: 0 });
                                if (plcCommand != null)
                                {
                                    var cmd = plcCommand.Cmd<PmesCmdCombinationMotherChildTray>();
                                    Logger?.Information(LogInfo.Info($"执行组子母托指令:{plcCommand.PlcComandContent}"));
                                    plcCommand.Status = 2;
                                    await _fSql.Update<T_plc_command>().Set(s => s.Status, 2)
                                        .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                                    await Plc.WriteClassAsync(cmd, 550);


                                    //TODO: 执行了之后要检测工位表，看什么时候组托完成，然后通知后台
                                    //QA:在哪个工位组 PmesCmdCombinationMotherChildTray 没有说明
                                    _ = Task.Run(async () =>
                                    {
                                        var pos = cmd.ChildMotherTrayWorkPositionId;
                                        var start = DateTime.Now;
                                        // while ( (DateTime.Now - start).TotalMinutes < 10 )
                                        while (true)
                                        {
                                            await Task.Delay(3000);
                                            var pmesMotherTrayBarcode = PmesDataItemList.PmesMotherTrayBarcode.ToList();
                                            try
                                            {
                                                await Plc.ReadMultipleVarsAsync(pmesMotherTrayBarcode);
                                            }
                                            catch (Exception)
                                            {
                                                Logger?.Error($"读plc失败！pos:{pos}");
                                            }

                                            if (int.Parse(pmesMotherTrayBarcode[1].Value.ToString()!) != 1)
                                            {
                                                Logger?.Information($"组子母托还未完成，读不到条码！");
                                                continue;
                                            }

                                            //1 申请拉走
                                            //TODO:这里后台接口暂未实现
                                            Logger?.Information($"组成功，调用后台接口拉走！pos:{pos}");
                                            var success =
                                                await WebService.Instance.PostMotherTrayCode(
                                                    cmd.ChildTrayWorkPositionId, pos,
                                                    pmesMotherTrayBarcode[0].Value.ToString()!,
                                                    cmd.MotherTrayWorkShopId);
                                            if (!success)
                                            {
                                                //TODO:agv异常处理
                                                Logger?.Error($"调用接口失败，agv拉走子母托失败！pos:{pos}");
                                                return;
                                            }

                                            if (await WaitStation(pos, 0, 900))
                                            {
                                                Logger?.Information($"子母托[3]工位{pos}释放！");
                                            }
                                            else
                                            {
                                                Logger?.Information($"子母托[3]等待工位{pos}释放失败！");
                                                return;
                                            }

                                            //2 更新数据库---组子母托指令执行完成
                                            await _fSql.Update<T_plc_command>().Set(s => s.Status, 1)
                                                .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                                            //3 移除指令
                                            _plcCommandsRunning.Remove(plcCommand);
                                            //4 告诉PLC----[娄工会主动等10s赋值0 2024年9月15日 19点39分] 
                                            pmesMotherTrayBarcode[1].Value = (byte)0;
                                            await Plc.WriteAsync(pmesMotherTrayBarcode.TakeLast(3).ToArray());
                                            //5 子母托清垛
                                            //cmd.ClearStack = pos;
                                            //await Plc.WriteClassAsync(cmd, 550);

                                            break; //执行成功的话 主动结束循环
                                        }
                                    });
                                }
                            }


                            //todo: 2024年9月27日 09:38:59 新增 独立清空组子母拖标志位
                            var plcCommandClearCom =
                                cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 10 && s.Status == 0);
                            if (plcCommandClearCom != null)
                            {
                                var r = await _fSql.Update<T_plc_command>().Set(s => s.Status, 2)
                                    .Where(s => s.Id == plcCommandClearCom.Id).ExecuteAffrowsAsync();
                                if (r <= 0)
                                {
                                    MessageBox.Show("发送清空组子母拖标志位指令时更新数据库失败，程序终止！");
                                    return;
                                }

                                var cmd = plcCommandClearCom.Cmd<PmesCmdCombinationMotherChildTray>();
                                Plc.WriteClass(cmd, 550);
                                Plc.ReadClass(cmd, 550);
                                Logger?.Information($"发送清空组子母指令结果:\n{cmd}");
                            }
                        }


                        //[4]子母托，子母托盘上料
                        {
                            if (_plcCommandsRunning.Count(s => s.PlcComandType is 4 or 5) == 0)
                            {
                                var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType is 4 or 5);
                                if (plcCommand != null)
                                {
                                    try
                                    {
                                        var pmesCmdTrayFeeding = plcCommand.Cmd<PmesCmdTrayFeeding>();
                                        //直接写入PLC上料数据即可
                                        await Plc.WriteClassAsync(pmesCmdTrayFeeding!, 551);
                                        var ret = await _fSql.Update<T_plc_command>().Set(s => s.Status, 1)
                                            .Where(s => s.Id == plcCommand.Id)
                                            .ExecuteAffrowsAsync();
                                        Logger?.Verbose($"执行指令:{plcCommand.PlcComandContent}，结果：{ret}");
                                    }
                                    catch (Exception e)
                                    {
                                        Logger?.Verbose($"执行指令:{plcCommand!.PlcComandContent}，失n败.\n{e}");
                                    }
                                }
                            }
                        }

                        //[5]上纸板
                        {
                            if (_plcCommandsRunning.Count(s => s.PlcComandType == 6) == 0)
                            {
                                var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 6 && s.Status == 0);
                                if (plcCommand != null)
                                {
                                    try
                                    {
                                        var cmd = plcCommand
                                            .Cmd<PlcCmdStacking5>(); ////这里是不是可以直接把后台读到的类  直接发给plc  而不是先读plc再改变其中一个值

                                        var plcCmdStacking5 = new PlcCmdStacking5();
                                        Plc.ReadClass(plcCmdStacking5, 545);
                                        plcCmdStacking5.Reserve1 = plcCommand.Cmd<PlcCmdStacking5>().Reserve1;
                                        //直接写入PLC上料数据即可
                                        //await Plc.WriteClassAsync(plcCmdStacking5!, 545);
                                        await Plc.WriteClassAsync(cmd!, 545);

                                        var ret = await _fSql.Update<T_plc_command>().Set(s => s.Status, 1)
                                            .Where(s => s.Id == plcCommand.Id)
                                            .ExecuteAffrowsAsync();
                                        Logger?.Verbose($"执行指令:{plcCommand.PlcComandContent}，结果：{ret}");
                                    }
                                    catch (Exception e)
                                    {
                                        Logger?.Verbose($"执行指令:{plcCommand!.PlcComandContent}，失n败.\n{e}");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger?.Error($"任务队列异常:\te");
                    }
                }
            });
        }

        /// <summary>
        ///     等待某个位置某个状态
        /// </summary>
        /// <param name="pos">目标位置</param>
        /// <param name="status">工位状态：0. 可用; 1.物料占用；2.空闲; 3.锁定；4.维护；5.暂停关闭</param>
        /// <param name="seconds">等待时间</param>
        /// <returns></returns>
        public async Task<bool> WaitStation(int pos, int status, int seconds = 900)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMinutes < seconds)
            {
                await Task.Delay(2000);
                Logger?.Information($"等待:{pos}状态变为:{status}");
                var station = await _fSql.Select<T_station_status>().Where(s => s.WorkshopId == pos).FirstAsync();
                if (station == null)
                {
                    Logger?.Error(LogInfo.Error($"station:{pos} 不存在！"));
                    return false;
                }

                if (station.Status == status)
                {
                    Logger?.Information(LogInfo.Info($"station:{pos},status ok！"));
                    return true;
                }
            }

            Logger?.Error(LogInfo.Error($"station:{pos},{seconds} 秒内等不到状态:{status}，返回false！"));
            return false;
        }

        #region PLC状态改变处理事件

        [ObservableProperty] private ObservableCollection<MyProductTaskInfo>
            _taskQueue = new();

        /// <summary>
        ///     PLC状态监测
        /// </summary>
        /// <param name="obj"></param>
        private async Task Handler(object obj)
        {
            //Logger?.Verbose($"状态改变:\n\t{obj}");
            try
            {
                switch (obj)
                {
                    case PmesCmdUnStacking pmesCmdUnStacking:
                    {
                        if (pmesCmdUnStacking.PmesAndPlcReadWriteFlag == 1) //PLC读走指令后置1 表示空闲
                        {
                        }

                        break;
                    }
                    case PlcCmdUnStacking plcUnStacking:
                        break;
                    case PlcCmdStacking plcCmdStacking:
                        if (plcCmdStacking.StackingFinished == 2)
                        {
                            var firstOrDefault =
                                _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 0 });
                            if (firstOrDefault != null)
                            {
                                _plcCommandsRunning.Remove(firstOrDefault);
                                var deserializeObject =
                                    JsonConvert.DeserializeObject<PmesStacking>(firstOrDefault.PlcComandContent);
                                if (_fSql.Update<T_plc_command>().Where(s => s.Id == firstOrDefault.Id)
                                        .Set(s => s.Status, 1).ExecuteAffrows() > 0)
                                {
                                    Logger?.Verbose($"T_plc_command,Id = {firstOrDefault.Id} ,数据库更新成功");
                                }
                                else
                                {
                                    Logger?.Verbose($"T_plc_command,Id = {firstOrDefault.Id} ,数据库更新失败");
                                }
                            }


                            ShowAlarm($"{plcCmdStacking.WorkPositionId} 码垛完成！请发送清跺指令，调度Agv小车拉走。");
                        }

                        break;
                    //组合机器人 -- 组合工位571、572
                    case PlcCmdCombinationMotherChildTray1 plcCmdCombinationMotherChild:
                        //if (plcCmdCombinationMotherChild.Num == 1)
                        //{
                        //    var plcCommand =
                        //   _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 3, Status: 2 });
                        //    if (plcCommand != null)
                        //    {
                        //        var cmd = plcCommand.Cmd<PmesCmdCombinationMotherChildTray>();
                        //        var pos = cmd.ChildMotherTrayWorkPositionId;
                        //        var pmesMotherTrayBarcode =
                        //                    await Plc.ReadMultipleVarsAsync(PmesDataItemList
                        //                        .PmesMotherTrayBarcode);
                        //        var success =
                        //           await WebService.Instance.PostMotherTrayCode(
                        //               cmd.ChildTrayWorkPositionId, pos,
                        //               pmesMotherTrayBarcode.ToString()!, cmd.MotherTrayWorkShopId);


                        //        if (!success)
                        //        {
                        //            //TODO:agv异常处理
                        //            Logger?.Error($"agv拉走子母托失败！pos:{pos}");
                        //        }

                        //        //2 更新数据库---组子母托指令执行完成
                        //        await _fSql.Update<T_plc_command>().Set(s => s.Status, 1)
                        //            .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                        //        //3 移除指令
                        //        _plcCommandsRunning.Remove(plcCommand);
                        //        //4 告诉PLC
                        //        pmesMotherTrayBarcode[1].Value = (ushort)2;
                        //        await Plc.WriteAsync(pmesMotherTrayBarcode.TakeLast(3).ToArray());
                        //        var pmesCmdCombinationMotherChildTray = new PmesCmdCombinationMotherChildTray();
                        //        Plc.ReadClass(pmesCmdCombinationMotherChildTray, 550);
                        //        //5 子母托清垛
                        //        pmesCmdCombinationMotherChildTray.ClearStack = pos;
                        //        await Plc.WriteClassAsync(pmesCmdCombinationMotherChildTray, 550);
                        //    }

                        //组盘完成, 呼叫agv，拉走【子母托盘组】到缓存位
                        ////?????,buys
                        /// ShowAlarm($"{plcCmdCombinationMotherChild.ChildMotherWorkPostionId} 组盘完成, 呼叫agv，拉走【子母托盘组】到缓存位。");
                        //}

                        break;

                    case PlcCmdCombinationMotherChildTray2 plcCmdCombinationMotherChild:
                        break;
                }
            }
            catch (Exception e)
            {
                ShowError($"TopViewModel.Handler Exception!\n{e}");
            }
        }

        /// <summary>
        ///     1 --> 读取重量和条码
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async Task WeightAndCodeChanged(List<DataItem> obj)
        {
            Logger?.Information($"WeightAndCodeChanged,\n{HardwareManager.PrintDataItems(obj)}");
            try
            {
                Logger?.Verbose($"WeightAndCodeChanged,当前标志位是：{obj[4].Value}");

                Weight1 = ((double.Parse(obj[2].Value!.ToString()!)) / 100d).ToString("f2");
                Weight2 = ((double.Parse(obj[3].Value!.ToString()!)) / 100d).ToString("f2");

                ProductCode = obj[1].Value!.ToString();

                var product = new ProductInfo();
                if (ProductCode!.StartsWith("888"))
                {
                    ErrorStop((int.Parse(obj[0].Value!.ToString()!)), "称重读取条码失败，采用默认条码888");
                    product.product_order_no = $"888{(DateTime.Now)}";
                }
                else
                {
                    product = await WebService.Instance.Get<ProductInfo>(
                                  $"{ApiUrls.QueryOrder}{ProductCode}&format=json") ??
                              new ProductInfo()
                              {
                                  product_order_no = $"999{DateTime.Now}"
                              };
                }


                try
                {
                    //任务队列添加对象
                    var taskInfo = new MyProductTaskInfo
                    {
                        Weight1 = (double.Parse(obj[2].Value!.ToString()!)),
                        Weight2 = (double.Parse(obj[3].Value!.ToString()!)),
                        ProductOrderBarCode = ProductCode,
                        WorkShop = EnumWorkShop.Weighted,
                        ProductOrderInfo = product
                    };

                    Application.Current.Dispatcher.Invoke(() => { TaskQueue.Add(taskInfo); });


                    var topGridModel = await UpdateProductInfo(taskInfo);
                    topGridModel.MyProductTaskInfo = taskInfo;
                    try
                    {
                        GlobalVar.MainView.Dispatcher.Invoke(() =>
                        {
                            ListHistory.Add(topGridModel);
                            ListProcess.Add(topGridModel);
                        });
                    }
                    catch (Exception e)
                    {
                        Logger?.Error($"更新界面失败！\n{e}");
                    }


                    //这里判断标签类型（是否需要装箱）
                    var items = obj.Select(s => new DataItem
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
                    items[^2].Value = !taskInfo.TbReelInfo.IsQualified
                        ? (short)2
                        : short.Parse(taskInfo.ProductOrderInfo.package_info.is_naked ? "2" : "1");
                    await Plc.WriteAsync(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    await Plc.ReadMultipleVarsAsync(items);
                    Logger?.Information(
                        ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
                }
                catch (Exception e)
                {
                    Logger?.Error($"添加任务队列失败：{e}");
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"WeightAndCodeChanged，异常：{e}");
            }
        }

        private async Task<TopGridModel> UpdateProductInfo(MyProductTaskInfo productTaskInfo)
        {
            var product = productTaskInfo.ProductOrderInfo;
            if (!double.TryParse(product!.package_info.tare_weight, out double packageWeight))
            {
                Logger?.Error("获取皮重失败，默认是0.");
            }

            if (!double.TryParse(product!.xpzl_weight, out double reelWeight))
            {
                Logger?.Error("获取reelWeight失败，默认是0.");
            }

            Logger?.Information(
                $"[KEY STEP]ProductCode:{ProductCode},tare_weight:{packageWeight},xpzl_weight:{reelWeight}");
            var netWeight = double.Parse(Weight1 ?? "0") - reelWeight - packageWeight;

            var tReelCode = GetTReelCode(product, netWeight);
            productTaskInfo.TbReelInfo = tReelCode;

            //这里判断是否合格
            var weightValidate = await productTaskInfo.WeightValidate();

            //add 2024年8月29日 校验合格之后检查是否有对应的托盘
            //add 2024年8月30日 校验合格之后不检查是否有对应的托盘，码垛区会一直码满空托盘，所以码垛的时候等待就可以了
            //if (weightValidate)
            //{
            //    if (_stackingDic.Count == 0)
            //    {
            //        //TODO: 呼叫子母托盘的时候只需要发送物料型号
            //        //按照现在的逻辑
            //        var applyTray2Storage = await WebService.Instance.ApplyTray2Storage("", product.package_info.delivery_sub_tray_spec!, "");
            //        Logger?.Information($"码垛工位没有木托盘，申请一个，执行结果：{applyTray2Storage}");
            //    }
            //}

            #region 打印盘码（合格证）

            // 箱码 最后五位是重量 放到插入数据那里更新
            // 52.00 -> [0.05200] ->  05200  #####
            //包装条码；产品助记码 + 线盘分组代码 + 用户标准代码 + 包装组编号 + 年月 + 4位流水号 + 装箱净重，
            //eg1:{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{weight}"

            var count = GetCountBySpec(productTaskInfo);
            count = count > 9999 ? 1 : count;
            var wei = (int)(100 * netWeight);
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:yyMM}{count:D4}-{wei:D5}";
            productTaskInfo.GenerateBoxCode = boxCode;
            productTaskInfo.GenerateReelCode = boxCode;

            productTaskInfo.TbBoxInfo = new T_box
            {
                CreateTime = DateTime.Now,
                IsDel = 0,
                LabelId = 1, //标签Id
                LabelName = "",
                PackagingCode = GlobalVar.CurrentUserInfo.packageGroupCode,
                PackagingSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{_okCount:D4}",
                PackagingWorker = GlobalVar.CurrentUserInfo.username,
                PackingBarCode = boxCode,
                PackingQty = product.package_info.packing_quantity.ToString(),
                PackingWeight = netWeight,
                PackingGrossWeight = double.Parse(Weight1),
                UpdateTime = DateTime.Now
            };

            //打印标签
            //--如果失败，打印空标签  2024年8月29日
            var xianPanReportModel = productTaskInfo.TbReelInfo.IsQualified
                ? new XianPanReportModel
                {
                    Title = "",
                    MaterialNo = product.material_number,
                    Model = product.material_name,
                    Specifications = product.material_spec,
                    GrossWeight = tReelCode.GrossWeight,
                    NetWeight = tReelCode.NetWeight,
                    BatchNum = tReelCode.BatchNO,
                    No = tReelCode.PSN,
                    BoxCode = boxCode,
                    DateTime = DateTime.Now.ToString("yy-MM-dd"),
                    WaterMark = tReelCode.NoQualifiedReason
                }
                : new XianPanReportModel
                {
                    Title = "",
                    MaterialNo = "",
                    Model = "",
                    Specifications = "",
                    GrossWeight = 0,
                    NetWeight = 0,
                    BatchNum = "",
                    WaterMark = tReelCode.NoQualifiedReason,
                    No = "",
                    BoxCode = "",
                    DateTime = DateTime.Now.ToString("yy-MM-dd"),
                    Reji = ""
                };
            var xianPanReportModels = new List<XianPanReportModel>() { xianPanReportModel };
            var reportReel = new ReelReportAuto();
            reportReel.SetQCR(productTaskInfo.TbReelInfo.IsQualified);
            reportReel.DataSource = xianPanReportModels;
            reportReel.Watermark.Text = xianPanReportModel.WaterMark;
            reportReel.Watermark.ShowBehind = false;
            reportReel.DrawWatermark = !tReelCode.IsQualified;

            productTaskInfo.ReportReel = reportReel;
            Directory.CreateDirectory($"D:\\ReportView");
            reportReel.ExportToImage($"D:\\ReportView\\reel_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\ReportView\\xp_{ProductCode}.png");
            //TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 

            try
            {
                reportReel.Print("152");
                Logger?.Information($"打印盘码（合格证）成功，打印机：152!");
                SendXinJieCmd();
            }
            catch (Exception e)
            {
                Logger?.Information($"打印盘码（合格证）失败，打印机：152！\n{e}");
            }

            #endregion

            #region boxCode

            var boxReportModels = new List<BoxReportModel>()
            {
                new BoxReportModel
                {
                    MaterialNo = tReelCode.CustomerMaterialCode,
                    Model = product.material_name,
                    GBNo = product.material_ns_model,
                    Specifications = product.material_spec,
                    NetWeight = netWeight,
                    BatchNum = product.product_order_no,
                    No = tReelCode.PSN, //包装线真实的编码
                    Standard = tReelCode.ProductStandardName,
                    ProductNo = tReelCode.ProductCode,
                    DateTime = DateTime.Now.ToString("yy-MM-dd"),
                    GrossWeight = double.Parse(Weight1),
                    BoxCode = boxCode,
                    WaterMark = tReelCode.NoQualifiedReason,
                    //BoxCode = (new Random().Next(100_000_000, 200_000_000)).ToString(),
                    ReJi = product.material_thermal_grade,
                    JsbzShortName = product.jsbz_short_name,
                },
            };
            Logger?.Information(
                $"[KEY STEP][PRINT RECORD]:ReJi:{product.material_thermal_grade},JsbzShortName{product.jsbz_short_name}\nproduct:{JsonConvert.SerializeObject(product)}");
            var reportP = new BoxReportAuto();
            reportP.DataSource = boxReportModels;
            reportP.Watermark.Text = boxReportModels.Last().WaterMark;
            reportP.Watermark.ShowBehind = false;
            reportP.DrawWatermark = !tReelCode.IsQualified;
            await reportP.ExportToImageAsync($"D:\\ReportView\\box_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\ReportView\\box.png");
            _reportsBox.Enqueue(reportP);
            productTaskInfo.ReportEdge = reportP;


            var reportTop = new BoxReportTop();
            reportTop.SetQrCode(boxCode);
            productTaskInfo.ReportTop = reportTop;

            #endregion


            if (product.package_info.packing_quantity == 0)
            {
                product.package_info.packing_quantity = 1;
                product.package_info.stacking_layers = 2;
                product.package_info.stacking_per_layer = 4;
            }

            #region 更新到界面表格

            var topGridModel = new TopGridModel
            {
                BarCode = tReelCode.PSN,
                ProductCode = tReelCode.ProductCode,
                Weight1 = double.Parse(Weight1 ?? "0"),
                Weight2 = double.Parse(Weight2 ?? "0"),
                NetWeight = (double)tReelCode.NetWeight,
                Result = tReelCode.IsQualified ? "合格" : "不合格",
                Reason = tReelCode.NoQualifiedReason
            };
            return topGridModel;

            #endregion
        }

        private T_preheater_code GetTReelCode(ProductInfo product, double netWeight)
        {
            var tReelCode = new T_preheater_code
            {
                BatchNO = @$"{ProductCode}-{DateTime.Now: MMdd}A",
                CreateTime = DateTime.Now,
                CustomerCode = product.customer_number,
                CustomerId = product.customer_id,
                CustomerMaterialCode = product.customer_material_number,
                CustomerMaterialName = product.customer_material_name,
                CustomerMaterialSpec = product.customer_material_spec,
                CustomerName = product.customer_name,
                GrossWeight = double.Parse(Weight1 ?? "0"),
                ICMOBillNO = product.product_order_no, //生产工单 之前是null，todo:确认是否需要
                IsDel = 0,
                IsQualified = true, //是否合格 默认是合格的
                MachineCode = product.machine_number,
                MachineId = product.machine_id,
                MachineName = product.machine_name,
                NetWeight = netWeight,
                NoQualifiedReason = "",
                OperatorCode = product.operator_code,
                OperatorName = product.operator_name,
                PreheaterCode = product.xpzl_number,
                PreheaterId = product.xpzl_id,
                PreheaterName = product.xpzl_name,
                PreheaterSpec = product.xpzl_spec,
                PreheaterWeight = double.Parse(product.xpzl_weight),
                ProductCode = product.material_number,
                ProductDate = DateTime.Parse(product.product_date), // product.product_date
                ProductGBName = product.material_ns_model,
                ProductId = product.material_id,
                ProductionBarcode = ProductCode,
                ProductionOrgNO = product.product_org_number,
                ProductMnemonicCode = product.material_mnemonic_code,
                ProductName = product.material_name,
                ProductSpec = product.customer_material_spec,
                ProductStandardName = product.material_execution_standard,
                PSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:MMdd}{0001}",
                Status = 1, //装箱状态
                StockCode = product.stock_number,
                StockId = product.stock_id,
                StockName = product.stock_name,
                UpdateTime = DateTime.Now,
                UserStandardCode = product.jsbz_number,
                UserStandardId = product.jsbz_id,
                UserStandardName = product.jsbz_name,
                WeightUserId = GlobalVar.CurrentUserInfo.userId,
                Material_themal_grade = product.material_thermal_grade,
                Material_spec = product.material_spec,
                Jsbz_short_name = product.jsbz_short_name,
                MachineNose = product.machine_nose
            };
            return tReelCode;
        }

        /// <summary>
        ///     所有任务的异常处理
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="msg"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ErrorStop(int deviceId, string msg)
        {
            Logger.Error($"设备{deviceId}出错，错误信息：{msg}");
        }

        /// <summary>
        ///     2 --> 条码校验
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task ReelCodeChanged(List<DataItem> obj)
        {
            try
            {
                PreScanCode1 = obj[1].Value!.ToString();
                PreScanCode2 = obj[2].Value!.ToString();
                var info = _taskQueue?.FirstOrDefault(s => s.WorkShop == EnumWorkShop.Weighted);
                if (info == null)
                {
                    Logger?.Error($"【ReelCodeChanged】error，找不到工位为 weighted 的产品信息！");
                }

                info.WorkShop = EnumWorkShop.ReelCodeChecked;

                info.ReelCode1 = PreScanCode1;
                info.ReelCode2 = PreScanCode2;

                if (PreScanCode2 != info.GenerateReelCode && PreScanCode1 != info.GenerateReelCode)
                {
                    ErrorStop((int.Parse(obj[0].Value!.ToString()!)),
                        $"当前条码：{PreScanCode2}|{PreScanCode1} 不等于上位机生成的条码：{info.GenerateReelCode}。");
                    info.TbReelInfo.IsQualified = false;
                    info.TbReelInfo.NoQualifiedReason = "ReelValidateError";
                }

                //这里判断标签类型（是否需要装箱）
                var items = obj.Select(s => new DataItem
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
                ////items[^2].Value = short.Parse("2");
                if (info.ProductOrderInfo!.package_info.is_naked)
                {
                    Logger?.Error($"{info.ProductOrderBarCode} :不应该出现裸装！");
                    //todo:实际生产要以ERP为准 2024年9月21日 14点14分
                    info.ProductOrderInfo!.package_info.is_naked = false;
                }

                items[^2].Value = !info.TbReelInfo.IsQualified
                    ? short.Parse("2")
                    : short.Parse(info.ProductOrderInfo!.package_info.is_naked ? "2" : "1");
                await Plc.WriteAsync(items.TakeLast(3).ToArray());
                Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                await Plc.ReadMultipleVarsAsync(items);
                Logger?.Information(
                    ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
        }

        public ManualResetEvent _waitStaking = new ManualResetEvent(true);

        /// <summary>
        ///     3 --> 条箱子到达位置，贴箱码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task BoxArrived(List<DataItem> arg)
        {
            //if (!_waitStaking.WaitOne(180_000))
            //{
            //    ErrorStop(250, $"贴箱码位置等待超时，码垛完成没有释放信号量！检查清垛是否有问题或者码垛是否完成！");
            //    MessageBox.Show("贴箱码位置等待超时，等待超过3分钟都没有码垛完成！检查清垛是否有问题或者码垛是否完成！软件停止，请重启软件！");
            //    return;
            //}
            //_waitStaking.Reset();

            try
            {
                //1 首先把码垛指令清空，等待箱码校验完成之后写入新的指令
                ClearPlcStackCmdWhenBoxArrived();

                var info = _taskQueue.FirstOrDefault(s => s.WorkShop == EnumWorkShop.ReelCodeChecked);
                if (info == null)
                {
                    Logger?.Error("_productInfoQueue队列里没有【WorkShop == EnumWorkShop.ReelCodeChecked】任务");
                    return;
                }

                if (_reportsBox.Count == 0)
                {
                    Logger?.Error($"_reportsBox队列中没有对应的箱码标签，不做任何处理！");
                    return;
                }

                var reportP = _reportsBox.Dequeue();

                if (info.TbReelInfo.IsQualified)
                {
                    //这里判断标签类型（打印几个，然后通知PLC放行）
                    //if ((bool)!info.ProductOrderInfo!.package_info.is_naked) //如果不是裸装 打印标签
                    Logger?.Information(
                        $"{info.ProductOrderBarCode} 是否裸装：{info.ProductOrderInfo!.package_info.is_naked}");
                    if (true) //TODO: 测试 PT25都不是裸装的  2024年9月20日 09点56分 
                    {
                        await info.ReportEdge?.PrintAsync("161")!;
                        info.ReportTop?.ExportToImage($"D:\\ReportView\\{info.ProductOrderBarCode}_edge.png");

                        await info.ReportTop?.PrintAsync("160")!;
                        info.ReportTop?.ExportToImage($"D:\\ReportView\\{info.ProductOrderBarCode}_top.png");
                        Thread.Sleep(6000);
                    }
                }


                //1 发送规格之后才能放行
                var pmesStacking = new PmesStacking
                {
                    WorkPositionId = 0,
                    ReelSpecification = 1,
                    StackModel = 0,
                    PmesAndPlcReadWriteFlag = 2
                };
                var plcCmdAttribute = pmesStacking!.GetType().GetCustomAttribute<PlcCmdAttribute>();
                if (plcCmdAttribute != null)
                {
                    var dbBlock = plcCmdAttribute.DbBlock;
                    await Plc.WriteClassAsync(pmesStacking, dbBlock);
                }

                //2 放行
                var items = arg.Select(s => new DataItem
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
                _reverse2 = (ushort)(!info.TbReelInfo.IsQualified ? 2 :
                    info.ProductOrderInfo!.package_info.is_naked ? 2 : 1);
                items[^1].Value = _reverse2;
                await Plc.WriteAsync(items.TakeLast(3).ToArray());
                Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                await Plc.ReadMultipleVarsAsync(items);
                Logger?.Information(
                    ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
        }

        /// <summary>
        ///     4 --> 贴完箱码，箱码校验
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task BoxBarCodeChanged(List<DataItem> obj)
        {
            try
            {
                BoxCode1 = obj[1].Value!.ToString();
                BoxCode2 = obj[2].Value!.ToString();
                var info = _taskQueue.First(s => s.WorkShop == EnumWorkShop.ReelCodeChecked);

                //不会出现
                if (string.IsNullOrEmpty(BoxCode1) && string.IsNullOrEmpty(BoxCode2))
                {
                    info.TbReelInfo.IsQualified = false;
                    info.TbReelInfo.NoQualifiedReason = "NoBoxCode";
                }

                //TODO: 不校验顶贴
                if (!BoxCode1!.Equals(info.GenerateBoxCode) && !BoxCode2!.Equals(info.GenerateBoxCode))
                {
                    info.TbReelInfo.IsQualified = false;
                    info.TbReelInfo.NoQualifiedReason = "ScanError";
                }

                info.WorkShop = EnumWorkShop.BoxCodeChecked;


                //判断是否Ng
                var items = obj.Select(s => new DataItem
                {
                    DataType = s.DataType,
                    VarType = s.VarType,
                    DB = s.DB,
                    StartByteAdr = s.StartByteAdr,
                    BitAdr = s.BitAdr,
                    Count = s.Count,
                    Value = s.Value
                }).ToList();
                items[^3].Value = _readFlag; //已读

                if (info.TbReelInfo.IsQualified) //合格的话就根据是否装箱来
                {
                    //1-->执行正常码垛指令
                    if (!ExecuteStack(info))
                    {
                        await Task.Delay(2500);
                        if (!ExecuteStack(info))
                        {
                            Logger?.Error("码垛指令执行失败");
                            MessageBox.Show("码垛指令执行失败，程序退出！");
                            return;
                        }
                    }

                    //1装箱  2不装箱
                    _reverse2 = (ushort)(!info.TbReelInfo.IsQualified ? 2 :
                        info.ProductOrderInfo!.package_info.is_naked ? 2 : 1);
                    items[^1].Value = _reverse2;
                    //2-->放行
                    await Plc.WriteAsync(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    await Plc.ReadMultipleVarsAsync(items);
                    Logger?.Information(
                        ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
                }
                else //不合格
                {
                    if (await WaitStation(251, 1, 900))
                    {
                    }
                    else
                    {
                        Logger?.Error("异常码垛未状态错误！不能码垛，程序终止！");
                        return;
                    }

                    //2 发送错误的码垛指令
                    await Plc.WriteClassAsync(_pmesStackingError, 540);
                    Logger?.Information($"发送新的码垛指令,码垛到异常工位:\n{_pmesStackingError}");

                    //2 通知放行
                    _reverse2 = 2; //Ng,不装箱
                    items[^1].Value = _reverse2;
                    await Plc.WriteAsync(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    await Plc.ReadMultipleVarsAsync(items);
                    Logger?.Information(
                        ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
        }

        /// <summary>
        ///     5 --> 码垛完成后
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task BoxStacked(List<DataItem> arg)
        {
            //return;
            var info = _taskQueue.FirstOrDefault(s => s.WorkShop == EnumWorkShop.BoxCodeChecked);
            if (info == null)
            {
                Logger?.Error(LogInfo.Error("触发了码垛完成，"));
                return;
            }

            try
            {
                GlobalVar.MainView.Dispatcher.Invoke(() => { TotalCount++; });

                info.WorkShop = EnumWorkShop.Stacked;
                Logger?.Information($"{info.ProductOrderBarCode} 码垛完成！码垛位置：{arg[1]?.Value}");
                try
                {
                    GlobalVar.MainView.Dispatcher.Invoke(() =>
                    {
                        //2024年10月17日  正在测试的列表去除码完的
                        ListProcess.Remove(s => s.MyProductTaskInfo?.WorkShop == EnumWorkShop.Stacked);
                    });
                }
                catch (Exception e)
                {
                    Logger?.Error($"更新界面失败！\n{e}");
                }

                if (info.TbReelInfo.IsQualified)
                {
                    info.TbReelInfo.VirtualBoxCode = _virtualBoxId;
                    InsertReeTb(info); //插入盘码和装箱单数据到数据库
                    _okCount++;
                    GlobalVar.MainView.Dispatcher.Invoke(() => { TotalOkCount++; });
                }
                else
                {
                    _errorCount++;
                    GlobalVar.MainView.Dispatcher.Invoke(() => { TotalNgCount++; });
                }


                //1 首先判断是否需要换垛
                if (_taskQueue.Count > 1)
                {
                    var plcCommandStacking =
                        _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 2 });
                    var workPositionId = plcCommandStacking.Cmd<PmesStacking>().WorkPositionId;
                    if (_stackProductInfos[workPositionId].Count > 0)
                    {
                        var current = _stackProductInfos[workPositionId].First();
                        var index = _taskQueue.IndexOf(info);
                        if (index + 1 < _taskQueue.Count && _taskQueue[(index + 1)].ProductOrderBarCode != "888")
                        {
                            var after = _taskQueue[(index + 1)].ProductOrderInfo;
                            var needExchange = current.NeedChange(after);

                            Logger?.Verbose($"判断是否需要换跺:{needExchange}");
                            Logger?.Verbose(
                                $"判断是否需要换跺-Cur:{info.ProductOrderBarCode},customer_number:{current.customer_number},material_number:{current.material_number},xpzl_spec:{current.xpzl_spec},jsbz_number:{current.jsbz_number}");
                            Logger?.Verbose(
                                $"判断是否需要换跺-Pre:{_taskQueue[(index + 1)].ProductOrderBarCode},customer_number:{after.customer_number},material_number:{after.material_number},xpzl_spec:{after.xpzl_spec},jsbz_number:{after.jsbz_number}");

                            //需要换垛
                            if (needExchange)
                            {
                                Logger?.Information(LogInfo.Info("触发换垛进入清垛流程"));
                                await ClearStack();
                                _okCount = 0;
                            }
                        }
                    }
                }


                //2 清垛 --- 包装满了
                //TODO: 调试的时候把装箱个数设置为2，2个就触发换垛
                if (ExchangeStackNum > 1) //如果是调试模式 手工换跺
                {
                    if (_okCount == ExchangeStackNum)
                    {
                        Logger?.Verbose(LogInfo.Info($"满足包装要求换跺-手动：换跺所需要的数量是：{ExchangeStackNum},当前ok:{_okCount}"));
                        await ClearStack();
                        _okCount = 0;
                    }
                }
                else
                {
                    var count = info.ProductOrderInfo.package_info.stacking_layers *
                                info.ProductOrderInfo.package_info.stacking_per_layer;
                    if (_okCount == count)

                    {
                        Logger?.Information($"满足包装要求换跺-自动：换跺所需要的数量是：{count},当前ok:{_okCount}");
                        Logger?.Verbose(LogInfo.Info("满16个进入清垛流程"));
                        await ClearStack();
                        _okCount = 0;
                    }
                }


                //3 判断是否清Error 工位
                if (ErrorCount == (ErrorExchangeStackNum < 1 ? 8 : ExchangeStackNum)) //1. 满一层剔除；线盘换型；人工触发..
                {
                    #region 等待是否码垛完成

                    var cmd = new PmesStacking();
                    var db = 548;
                    Plc.ReadClass(cmd, db);
                    var initNum = cmd.Reserve1;
                    while (true)
                    {
                        Thread.Sleep(1500);
                        Plc.ReadClass(cmd, db);
                        if (cmd.Reserve1 > initNum)
                        {
                            Logger?.Information($"db:{db}计数器已经加1，异常码垛位置开始申请入库！");
                            break;
                        }
                    }

                    #endregion

                    var success = WebService.Instance.ClearErrorStack(_pmesStackingError.WorkPositionId);
                    Logger?.Information($"清空异常位结果;{success}");
                    if (!success)
                    {
                        Logger?.Error("清空异常位失败！程序终止！");
                    }

                    _errorCount = 0;
                    //_pmesStackingError.ClearStack = _pmesStackingError.WorkPositionId;
                    //await Plc.WriteClassAsync(_pmesStackingError, 540);
                    //_pmesStackingError.ClearStack = 1;
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
            finally
            {
                _waitStaking.Set();
                Logger?.Information($"{info.ProductOrderBarCode} 码垛完成!");
            }
        }

        private bool can540 = false;
        private PmesStacking cmdStack1;

        /// <summary>
        ///     执行【申请入库】【PLC清垛】
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ClearStack()
        {
            var plcCommandStacking = _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 2 });
            if (plcCommandStacking == null)
            {
                ErrorStop(0, $"找不到拆垛指令！");
                return false;
            }

            //1 修改数据库状态--->码垛指令执行完毕
            var ret = await _fSql.Update<T_plc_command>().Set(s => s.Status, 1)
                .Where(s => s.Id == plcCommandStacking.Id).ExecuteAffrowsAsync();
            plcCommandStacking.Status = 1;
            var cmdStack = plcCommandStacking.Cmd<PmesStacking>();
            Logger?.Information($"数据库更改码垛指令为完成状态，结果:{ret}\t{plcCommandStacking.PlcComandContent}");

            #region 2024年9月27日 09:28:21 新增 拉走前判断是否码垛完成 因为信号是提前发的

            var cmd = new PmesStacking();
            var db = cmdStack.WorkPositionId - 250 + 540;
            //读取当前码垛指令
            if (db < 542 || db > 547)
            {
                var msg = $"码垛工位错误:{cmdStack.WorkPositionId}，程序终止！";
                MessageBox.Show(msg);
                Logger?.Error(msg);
                return false;
            }

            Plc.ReadClass(cmd, db);
            var initNum = cmd.Reserve1;
            while (true)
            {
                Thread.Sleep(1500);
                Plc.ReadClass(cmd, db);
                if (cmd.Reserve1 > initNum)
                {
                    Logger?.Information($"db:{db}计数器已经加1，开始申请入库！");
                    break;
                }
            }

            #endregion

            var station = plcCommandStacking.WorkshopId!;
            try
            {
                _stackProductInfos[(int)station].Clear();
                Logger?.Information($"_stackProductInfos已满，清空:\n{JsonConvert.SerializeObject(_stackProductInfos)}");
            }
            catch (Exception e)
            {
                Logger?.Information(
                    $"_stackProductInfos已满，清空失败！\n{e.Message}\n{JsonConvert.SerializeObject(_stackProductInfos)}");
            }

            //2 agv拉走入库
            if (PMESConfig.Default.EmptyStacking)
            {
                var r = _fSql.Update<T_station_status>().Set(s => s.Status, 3)
                    .Where(s => s.WorkshopId == cmdStack.WorkPositionId).ExecuteAffrows();
                if (r <= 0)
                {
                    Logger?.Error($"锁定工位失败：{cmdStack.WorkPositionId}！");
                    return false;
                }


                var success = WebService.Instance.ApplyTray2Storage(cmdStack.WorkPositionId);
                Logger?.Information($"调用后台Api执行清垛指令，结果:{success}，cmdStack:{cmdStack.WorkPositionId}");
                if (!success)
                {
                    Logger?.Error($"调用后台Api执行清垛指令失败！");
                    return false;
                }
                else
                {
                    can540 = true;
                    cmdStack1 = cmdStack;
                }
            }

            //3 调度清垛指令  16点17分  2024年9月26日  todo: 付发送清垛指令
            try
            {
                //cmdStack.ClearStack = cmdStack.WorkPositionId;
                //await Plc.WriteClassAsync(cmdStack, 540);
                //Logger?.Information($"Plc写入清垛指令，cmdStack:{cmdStack.WorkPositionId}，cmdStack.ClearStack ：{cmdStack.ClearStack}");
            }
            catch (Exception e)
            {
                Logger?.Error(LogInfo.Error($"PLC写入清垛指令失败：{cmdStack} \nex:{e}"));
            }

            return true;
        }

        /// <summary>
        ///     执行队列中正常的堆垛指令
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool ExecuteStack(MyProductTaskInfo info)
        {
            T_plc_command plcCommand;
            while (true)
            {
                Thread.Sleep(1000);
                plcCommand = _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 2 });
                if (plcCommand == null) //如果队列中没有需要的指令，一直等待
                {
                    Logger?.Error("队列中没有满足条件的码垛指令,继续等待....");
                }
                else
                {
                    Logger?.Information($"找到正常码垛指令:{plcCommand.PlcComandContent}");
                    break;
                }
            }

            try
            {
                var pmesStacking = plcCommand.Cmd<PmesStacking>();
                if (!_stackProductInfos.ContainsKey(pmesStacking.WorkPositionId))
                {
                    _stackProductInfos.Add(pmesStacking.WorkPositionId, new List<ProductInfo>());
                }

                _stackProductInfos[pmesStacking.WorkPositionId].Add(info.ProductOrderInfo);

                if (!WaitStation(pmesStacking.WorkPositionId, 1, 3600).Result)
                {
                    Logger?.Error("工位状态不对，无法码垛！");
                    return false;
                }

                Logger?.Verbose($"WorkPositionId:{pmesStacking.WorkPositionId} 状态为1，发送码垛指令。");
                //todo: 在这里确认数据库里工位状态是否正确 pmesStacking.WorkPositionId
                Plc.WriteClass(pmesStacking, 540);
            }
            catch (Exception e)
            {
                Logger?.Error(LogInfo.Error($"执行PLC执行失败 Cmd：\n{plcCommand.PlcComandContent}"));
            }

            return true;
        }

        /// <summary>
        ///     当box到达的时候，清空码垛指令。
        ///     当箱码校验完毕，发送新的指令。
        /// </summary>
        public void ClearPlcStackCmdWhenBoxArrived()
        {
            var pmesStacking = new PmesStacking
            {
                WorkPositionId = 0,
                ReelSpecification = 0,
                StackModel = 0,
                PmesAndPlcReadWriteFlag = 2
            };
            var plcCmdAttribute = pmesStacking!.GetType().GetCustomAttribute<PlcCmdAttribute>();
            if (plcCmdAttribute != null)
            {
                var dbBlock = plcCmdAttribute.DbBlock;
                Plc.WriteClass(pmesStacking, dbBlock);
            }
        }

        #endregion

        #region 表示状态

        /// <summary>
        ///     第一次扫码
        /// </summary>
        [ObservableProperty] private Status? _barCodeScannerStatus1 = Status.Unknown;

        /// <summary>
        ///     扫描校验
        /// </summary>
        [ObservableProperty] private Status? _barCodeScannerStatus2 = Status.Unknown;

        /// <summary>
        ///     第一次称重
        /// </summary>
        [ObservableProperty] private Status? _weighingStatus1 = Status.Unknown;

        /// <summary>
        ///     称重校验
        /// </summary>
        [ObservableProperty] private Status? _weighingStatus2 = Status.Unknown;

        /// <summary>
        ///     打印机
        /// </summary>
        [ObservableProperty] private Status? _printerStatus = Status.Unknown;

        /// <summary>
        ///     条码校验结果
        /// </summary>
        [ObservableProperty] private bool? _barCodeCheck = false;

        /// <summary>
        ///     PLC状态
        /// </summary>
        [ObservableProperty] private Status? _plcStatus = Status.Unknown;

        /// <summary>
        ///     服务器状态
        /// </summary>
        [ObservableProperty] private Status? _serviceStatus = Status.Unknown;

        /// <summary>
        ///     所有设备（工作指示灯）
        /// </summary>
        [ObservableProperty] private Status? _allStatus = Status.Unknown;

        #endregion

        #region cmds

        [RelayCommand]
        private void Close()
        {
            Application.Current.Shutdown();
            //Application.Current.Shutdown();
        }

        #endregion

        #region 信捷PLC

        [ObservableProperty] private ObservableCollection<ModbusCmd> _modbusCmds = new ObservableCollection<ModbusCmd>()
        {
            new() //数量
            {
                DeviceId = 1,
                Address = 1000,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 1
            },
            new() //类型
            {
                DeviceId = 1,
                Address = 1002,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 1
            },
            new() //偏移量
            {
                DeviceId = 1,
                Address = 1004,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new() //预留位
            {
                DeviceId = 1,
                Address = 1006,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new() //预留位
            {
                DeviceId = 1,
                Address = 1008,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new() //是否贴完
            {
                DeviceId = 1,
                Address = 800,
                Method = ModbusMethods.ReadCoils,
                Value = 0
            },
        };


        [RelayCommand]
        private void SendXinJieCmd()
        {
            Logger?.Verbose($"信捷PLC写指令，长度：{ModbusCmds.Count}");
            foreach (var modbusCmd in ModbusCmds)
            {
                try
                {
                    var isSuccess = PlcXj.WriteOneRegister(modbusCmd.Address.ToString(), modbusCmd.Value)
                        .IsSuccess;
                    Logger?.Information(
                        $"信捷PLC写指令,address = {modbusCmd.Address},value = {modbusCmd.Value}执行结果:{isSuccess}");


                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    Logger?.Error($"信捷写入错误:{e.Message}");
                }
            }
        }

        [RelayCommand]
        private void ReadXinJieCmd()
        {
            foreach (var modbusCmd in ModbusCmds)
            {
                switch (modbusCmd.Method)
                {
                    case ModbusMethods.ReadCoils:
                        var ret1 = PlcXj.ReadCoil(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = (short)(ret1[0] ? 1 : 0);
                        break;
                    case ModbusMethods.ReadHoldingRegisters:
                        var ret2 = PlcXj.Read(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = ret2[0];
                        break;
                    case ModbusMethods.ReadInputRegisters:
                        var ret3 = PlcXj.Read(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = ret3[0];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Thread.Sleep(50);
            }
        }

        #endregion

        #region Debug

        private T_preheater_code GetReelCode(ProductInfo product)
        {
            var t = new T_preheater_code
            {
                BatchNO = @$"{ProductCode}-{DateTime.Now: MMdd}A",
                CreateTime = DateTime.Now,
                CustomerCode = product.customer_number,
                CustomerId = product.customer_id,
                CustomerMaterialCode = product.customer_material_number,
                CustomerMaterialName = product.customer_material_name,
                CustomerMaterialSpec = product.customer_material_spec,
                CustomerName = product.customer_name,
                GrossWeight = double.Parse(Weight1 ?? "0"),
                ICMOBillNO = product.product_order_no, //生产工单 之前是null，todo:确认是否需要
                IsDel = 0,
                IsQualified = true, //是否合格 默认是合格的
                MachineCode = product.machine_number,
                MachineId = product.machine_id,
                MachineName = product.machine_name,
                NetWeight = double.Parse(Weight1 ?? "0") - 0d,
                NoQualifiedReason = "TEST",
                OperatorCode = product.operator_code,
                OperatorName = product.operator_name,
                PreheaterCode = product.xpzl_number,
                PreheaterId = product.xpzl_id,
                PreheaterName = product.xpzl_name,
                PreheaterSpec = product.xpzl_spec,
                PreheaterWeight = double.Parse(product.xpzl_weight),
                ProductCode = product.material_number,
                ProductDate = DateTime.Parse(product.product_date), // product.product_date
                ProductGBName = product.material_ns_model,
                ProductId = product.material_id,
                ProductionBarcode = ProductCode,
                ProductionOrgNO = product.product_org_number,
                ProductMnemonicCode = product.material_mnemonic_code,
                ProductName = product.material_name,
                ProductSpec = product.customer_material_spec,
                ProductStandardName = product.material_execution_standard,
                PSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:MMdd}{0001}",
                Status = 1, //装箱状态
                StockCode = product.stock_number,
                StockId = product.stock_id,
                StockName = product.stock_name,
                UpdateTime = DateTime.Now,
                UserStandardCode = product.jsbz_number,
                UserStandardId = product.jsbz_id,
                UserStandardName = product.jsbz_name,
                Weight1 = 0,
                WeightUserId = GlobalVar.CurrentUserInfo.userId
            };
            return t;
        }

        [RelayCommand]
        private void PrintReelCode()
        {
            var product = new ProductInfo();
            var tPreheaterCode = GetReelCode(product);
            var wei = (int)(100 * double.Parse(Weight1));
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{1000}-{wei:D4}";
            //打印标签
            var xianPanReportModel = new XianPanReportModel
            {
                Title = "",
                MaterialNo = product.material_number,
                Model = product.xpzl_spec,
                Specifications = tPreheaterCode.PreheaterSpec,
                GrossWeight = tPreheaterCode.GrossWeight,
                NetWeight = tPreheaterCode.NetWeight,
                BatchNum = tPreheaterCode.BatchNO,
                No = tPreheaterCode.PSN,
                BoxCode = boxCode,
                DateTime = DateTime.Now.ToString("yy-MM-dd"),
                WaterMark = tPreheaterCode.NoQualifiedReason
            };
            var xianPanReportModels = new List<XianPanReportModel>() { xianPanReportModel };
            var reportReel = new ReelReportAuto();
            reportReel.DataSource = xianPanReportModels;
            reportReel.Watermark.Text = xianPanReportModel.WaterMark;
            reportReel.Watermark.ShowBehind = false;
            reportReel.DrawWatermark = !tPreheaterCode.IsQualified;
            reportReel.ExportToImage($"D:\\reel_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\xp_{ProductCode}.png");
            //TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 

            reportReel.Print("152");

            Logger?.Information($"调用默认打印机打印!");
            SendXinJieCmd();
        }

        [RelayCommand]
        private void PrintBoxCode()
        {
            if (_reportsBox.Count == 0)
            {
                var boxReportModels = new List<BoxReportModel>()
                {
                    new BoxReportModel
                    {
                        MaterialNo = "22222222222",
                        Model = "22222222222",
                        Specifications = "22222222222",
                        NetWeight = 1d,
                        BatchNum = "22222222222",
                        No = "22222222222",
                        Standard = "22222222222",
                        ProductNo = "22222222222",
                        BoxCode = Guid.NewGuid().ToString().Substring(1, 10),
                        DateTime = "22222222222",
                        GrossWeight = 1d,
                        WaterMark = "TEST"
                    }
                };
                var reportP = new BoxReportAuto();
                //var reportP = new TemplateBox();
                reportP.Watermark.ShowBehind = false;
                reportP.DataSource = boxReportModels;
                reportP.Watermark.Text = boxReportModels.Last().WaterMark;
                reportP.Watermark.ShowBehind = false;
                reportP.DrawWatermark = true;
                reportP.ExportToImage($"D:\\box_{ProductCode}.png");
                reportP.Print("161");
                reportP.Print("160");
                _reportsBox.Enqueue(reportP);
            }


            var reportP1 = _reportsBox.Dequeue();
            reportP1.Print("161");
            reportP1.Print("160");
        }

        [RelayCommand]
        private void AddData()
        {
            var topGridModel = new TopGridModel
            {
                BarCode = Guid.NewGuid().ToString().Substring(1, 10),
                ProductCode = Guid.NewGuid().ToString().Substring(1, 10),
                Weight1 = new Random().NextDouble() * 25,
                Weight2 = new Random().NextDouble() * 25,
                NetWeight = 0,
                CreateTime = DateTime.Now,
                Result = null
            };
            ListProcess.Add(topGridModel);
            ListHistory.Add(topGridModel);
        }

        #endregion

        #region 任务管理界面

        [RelayCommand]
        private void AddTask()
        {
            var myProductTaskInfo = new MyProductTaskInfo
            {
                ProductOrderBarCode = "test",
                WorkShop = EnumWorkShop.None,
                ProductOrderInfo = new ProductInfo(),
                Weight1 = 0,
                Weight2 = 0,
                ReelCode1 = "t1",
                ReelCode2 = "t2",
                BoxCode1 = "null",
                BoxCode2 = "null",
                GenerateReelCode = "null",
                GenerateBoxCode = "null",
                TbReelInfo = new T_preheater_code(),
                TbBoxInfo = new T_box(),
                ReportReel = new ReelReportAuto(),
                ReportTop = new BoxReportAuto(),
                ReportEdge = new BoxReportAuto(),
                NeedPack = true
            };
            GlobalVar.MainView.Dispatcher.Invoke(() => { TaskQueue.Add(myProductTaskInfo); });
        }

        [RelayCommand]
        private void DeleteTask()
        {
            if (TaskQueue.Count < 0)
                return;
            if (TaskQueueSelectedIndex < 0)
                return;
            TaskQueue.RemoveAt(TaskQueueSelectedIndex);
        }

        [RelayCommand]
        private void PrintTaskTop()
        {
            if (TaskQueue.Count == 0)
            {
                var re = new BoxReportTop();
                re.Print("160");
                return;
            }

            if (TaskQueueSelectedIndex == 0)
                return;
            TaskQueue[TaskQueueSelectedIndex].ReportTop?.Print("160");
        }

        [RelayCommand]
        private void PrintTaskEdge()
        {
            if (TaskQueue.Count < 0)
                return;
            if (TaskQueueSelectedIndex < 0)
                return;
            TaskQueue[TaskQueueSelectedIndex].ReportEdge?.Print("161");
        }

        [RelayCommand]
        private void PrintTaskReel()
        {
            if (TaskQueue.Count < 0)
                return;
            if (TaskQueueSelectedIndex < 0)
                return;
            TaskQueue[TaskQueueSelectedIndex].ReportReel?.Print("152");
        }

        #endregion

        private void ShowError(string msg)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowAlarm(string msg)
        {
            Logger?.Warning(msg);
            MessageBox.Show(msg, "Alarm", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void InsertReeTb(MyProductTaskInfo info)
        {
            try
            {
                Logger?.Verbose(LogInfo.Info($"准备插入数据库,数据：\n{JsonConvert.SerializeObject(info)}"));
                var boxInfo = info.TbBoxInfo;
                var plcCommandStack = _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 2 });

                var cmd = plcCommandStack.Cmd<PmesStacking>();
                var stationStatus = _fSql.Select<T_station_status>().Where(s => s.WorkshopId == cmd.WorkPositionId)
                    .First();
                boxInfo.TrayBarcode = stationStatus?.AttachInfo;
                boxInfo.Status = 1; //合托进行中

                var id = _fSql.Insert(boxInfo).ExecuteIdentity();
                boxInfo.Id = (uint)id;
                Logger?.Verbose(LogInfo.Info($"插入BoxId:{id}"));

                var reelInfo = info.TbReelInfo;
                reelInfo.BoxId = (int)id;
                var reelId = (int)_fSql.Insert(reelInfo).ExecuteIdentity();
                reelInfo.Id = (uint)reelId;
                Logger?.Verbose(LogInfo.Info($"插入盘码Id:{reelId}"));

                var product = info.ProductOrderInfo;
                var order = new T_order_package
                {
                    CreateTime = DateTime.Now,
                    DeliverySubTrayName = product.package_info.delivery_sub_tray_name,
                    FullCoilWeight = product.package_info.cu_full_coil_weight,
                    MaxWeight = double.Parse(product.package_info.cu_max_weight ?? "0"),
                    MinWeight = double.Parse(product.package_info.cu_min_weight ?? "0"),
                    PackagingReqCode = product.package_info.code,
                    PackagingReqName = product.package_info.name,
                    PackingQuantity = product.package_info.packing_quantity,
                    Paperboard_name = product.package_info.delivery_sub_tray_name,
                    Paperboard_number = 1,
                    Paperboard_spec = "1",
                    PreheaterCodeId = reelId,
                    PreheaterInsidePackageName = product.package_info.wire_reel_inside_package_name,
                    PreheaterOutsidePackageName = product.package_info.wire_reel_external_package_name,
                    StackingLayers = product.package_info.stacking_layers,
                    StackingPerLayer = product.package_info.stacking_per_layer,
                    Super_wide_sub_tray = false,
                    TareWeight = double.Parse(product.package_info.tare_weight ?? "0"),
                    UpdateTime = DateTime.Now
                };
                var orderId = _fSql.Insert(order).ExecuteIdentity();
                Logger?.Verbose(LogInfo.Info($"插入包装 T_order_package Id:{orderId}"));
                //2024年10月17日 06:25:43 顺便插入老数据库
                UpdateOldDbSqlServer(reelInfo, boxInfo, order);
            }
            catch (Exception e)
            {
                Logger?.Error(LogInfo.Error($"插入盘码到数据库失败！\n{e}"));
            }
        }

        private void InsertTrayTb(MyProductTaskInfo info)
        {
            try
            {
                var boxInfo = info.TbBoxInfo;
                var plcCommandStack = _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 2 });
                var cmd = plcCommandStack.Cmd<PmesStacking>();
                var stationStatus = _fSql.Select<T_station_status>().Where(s => s.Id == cmd.WorkPositionId).First();
                boxInfo.TrayBarcode = stationStatus.AttachInfo;
                var id = _fSql.Insert(boxInfo).ExecuteIdentity();
                boxInfo.Id = (uint)id;
                Logger?.Verbose($"插入TrayId:{id}");
                var infos = _taskQueue.Where(s => s.TbReelInfo.VirtualBoxCode == _virtualBoxId)
                    .Select(s => s.TbReelInfo)
                    .ToList();
                foreach (var item in infos)
                {
                    item.BoxId = (int)id;
                    var ret = _fSql.Update<T_preheater_code>().Set(s => s.BoxId, id).ExecuteAffrows();
                    Logger?.Information($"更改盘码Id={item.Id}，的BoxId为：{id}，执行结果：{ret}");
                }

                _virtualBoxId = Guid.NewGuid().ToString();
            }
            catch (Exception e)
            {
                Logger?.Error($"插入箱码到数据库失败！{e}");
            }
        }

        private int GetCountBySpec(MyProductTaskInfo productTaskInfo)
        {
            var path = ".//config//count.json";
            var jsonDic = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(path));
            if (jsonDic == null)
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(new Dictionary<string, int>() { { "1", 1 } }));
            }

            var key = productTaskInfo.ProductOrderInfo.GetSpec;
            if (jsonDic.ContainsKey(key))
            {
                jsonDic[key] = jsonDic[key] + 1;
            }
            else
            {
                jsonDic.Add(key, 1);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(jsonDic));
            return jsonDic[key];
        }

        /// <summary>
        ///     更新到老数据库
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateOldDbSqlServer(T_preheater_code preheaterCode, T_box boxCode, T_order_package packageOrder)
        {
            try
            {
                var view = _fSql.Select<U_VW_DBCP>().WithSql("SELECT * FROM U_VW_DBCP").ToList()
                    .First(s => s.FItemID.ToString().Equals(preheaterCode.ProductId.ToString()));
                if (view == null)
                {
                    ShowError("视图查询为空！");
                    return;
                }

                var old = new OldTest
                {
                    FItemID = preheaterCode.ProductId,
                    FNumber = preheaterCode.ProductCode,
                    FCZM = preheaterCode.ProductMnemonicCode,
                    FGJTYXH = preheaterCode.ProductGBName,
                    FType = preheaterCode.ProductName,
                    FTypeID = view.产品型号ID,
                    FTypeNO = view.产品型号代号,
                    FQMDJID = view.产成品漆膜等级ID,
                    FQMDJNO = view.产成品漆膜等级代号,
                    FQMDJ = view.产成品漆膜等级,
                    FCPGGID = view.产品规格ID,
                    FCPGGNO = view.产品规格代号,
                    FCZID = view.产品材质ID,
                    FCZNO = view.产品材质代号,
                    FCZName = view.产品材质,
                    FXTID = view.产成品形态ID,
                    FXTNO = view.产成品形态代号,
                    FXTName = view.产成品形态,
                    FCPGG = preheaterCode.Material_spec,
                    FXPItemID = preheaterCode.PreheaterId,
                    FXPNumber = preheaterCode.PreheaterCode,
                    FXPName = preheaterCode.PreheaterName,
                    FXPGG = preheaterCode.PreheaterSpec,
                    FXPQty = (decimal)preheaterCode.NetWeight,
                    FBZZPZQty = (decimal)packageOrder.TareWeight, //修改为包装代码里的【皮重】。
                    FJTH = preheaterCode.MachineCode,
                    FBH = boxCode.PackagingSN, //以天为单位进行联系，保留4位。
                    FBHMX = preheaterCode.PSN,
                    FPCH = preheaterCode.BatchNO, //修改为：FIcmibILLno-天A(上午A，下午M），day保留2位..

                    FDate = boxCode.CreateTime, //只写日期，
                    FSXH = 0, //一天的连续号..

                    FHGZQty = packageOrder.TareWeight,
                    FJYR = preheaterCode.OperatorName,
                    FZXBZID = 0,
                    FZXBZ = preheaterCode.ProductStandardName,
                    FMZQty = (decimal)boxCode.PackingGrossWeight,
                    FPZQty = (decimal)preheaterCode.PreheaterWeight,
                    FJZQty = (decimal)preheaterCode.NetWeight,
                    FStrip = boxCode.PackingBarCode,
                    FComputerName = "2#自动包装线", //boxCode.PackagingCode,

                    FZQty = (decimal)boxCode.PackingWeight,
                    FBQID = boxCode.LabelTemplateId,
                    FBQJS = int.Parse(boxCode.PackingQty),
                    FTypeTemp = preheaterCode.ProductName,
                    FICMOID = preheaterCode.ICMOBillNO,

                    FICMOBillNO = preheaterCode.ICMOBillNO,

                    FStrip2 = preheaterCode.ProductionBarcode,
                    FDate2 = preheaterCode.CreateTime,
                    FJSBZID = preheaterCode.UserStandardId,
                    FJSBZNumber = preheaterCode.UserStandardCode,
                    FSCorgno = preheaterCode.ProductionOrgNO, //
                    FStockID = preheaterCode.StockId,
                    FCustomer = preheaterCode.CustomerId,
                    FLinkStacklabel = $"{GetTimeStamp}-{boxCode.TrayBarcode}", //时间戳+实托盘条码
                    FSPTime = DateTime.Now
                };
                _fSql.Insert(old).ExecuteAffrows();
            }
            catch (Exception e)
            {
                ShowError($"插入老数据库失败！\n{e.Message}");
            }
        }


        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.bool bflag = true</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }


        private bool ExecProcedureAutoInput(string linkStackLabel)
        {
            DbParameter p2 = null;
            DataTable dt = _fSql.Ado.CommandFluent("P_AutoInput")
                .CommandType(CommandType.StoredProcedure)
                .CommandTimeout(60)
                .WithParameter("id", null, p =>
                {
                    p2 = p; //输入 参数   
                    p.DbType = DbType.AnsiString;
                    p.Direction = ParameterDirection.Input;
                    p.Value = linkStackLabel;
                })
                .ExecuteDataTable();
            var result = "";
            if (dt.Rows.Count > 0)
            {
                Logger?.Information($"存储过程执行结果：{dt.Rows[0][0].ToString()}.linkStackLabel{linkStackLabel}");
                return (dt.Rows[0][0].ToString() ?? "").Equals("导入成功");
            }
            else
            {
                Logger?.Error($"存储过程执行返回为空！linkStackLabel:{linkStackLabel}");
                return false;
            }
        }
    }
}