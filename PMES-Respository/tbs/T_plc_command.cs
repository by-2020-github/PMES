using FreeSql.DatabaseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using Newtonsoft.Json.Serialization;

namespace PMES.Model.tbs
{
    /// <summary>
    /// Plc命令
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_plc_command", DisableSyncStructure = true)]
    public partial class T_plc_command
    {
        /// <summary>
        /// agv表主键id
        /// </summary>
        [JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// AGV调度请求ID
        /// </summary>
        [JsonProperty, Column(Name = "agvReqCode", StringLength = 20)]
        public string AgvReqCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty, Column(Name = "attInfo")]
        public string AttInfo { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime", DbType = "datetime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// json格式的命令内容
        /// </summary>
        [JsonProperty, Column(Name = "plcComandContent")]
        public string PlcComandContent { get; set; }

        public T Cmd<T>()
        {
            // 设置序列化选项为小写驼峰命名风格
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.DeserializeObject<T>(PlcComandContent, settings);
        }

        /// <summary>
        /// 命令类型:1.拆垛任务；2。码垛任务；3.组合机器人组托任务; 4.组合机器人- 申请母托盘组；5-组合机器人-申请【子托盘组】
        /// </summary>
        [JsonProperty, Column(Name = "plcComandType", DbType = "int")]
        public int? PlcComandType { get; set; }

        [JsonProperty, Column(Name = "readWriteFlag", DbType = "int unsigned")]
        public uint? ReadWriteFlag { get; set; } = 0;

        /// <summary>
        /// 0.未执行；1.执行完毕；2.执行中；3.暂停
        /// </summary>
        [JsonProperty, Column(Name = "status", DbType = "int")]
        public int? Status { get; set; } = 0;

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
        public DateTime? UpdateTime { get; set; }
    }
}