using System;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using PMES_Respository.DataStruct;

namespace PMES_Respository.tbs_sqlserver
{

    [Table(Name = "t_plc_command", DisableSyncStructure = true)]
    public partial class T_plc_command
    {
        [Column(IsIdentity = true,IsPrimary = true)]
        public long Id { get; set; }

        public string AgvReqCode { get; set; }

        public string AttInfo { get; set; }

        public DateTime? CreateTime { get; set; }

        public string PlcComandContent { get; set; }

        /// <summary>
        /// plc消息类型:1.拆垛任务；2。码垛任务；3.组合机器人组托任务; 4.组合机器人- 申请母托盘组；5-组合机器人-申请【子托盘组】; 6.码垛位-上纸板；9.码垛清垛；10.组托清垛
        /// </summary>
        public int? PlcComandType { get; set; }

        public int? ReadWriteFlag { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public int? Status { get; set; } = 0;

        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 所属工位
        /// </summary>
        public int? WorkshopId { get; set; }

        public T Cmd<T>()
        {
            return JsonConvert.DeserializeObject<T>(PlcComandContent);
        }
    }

}
