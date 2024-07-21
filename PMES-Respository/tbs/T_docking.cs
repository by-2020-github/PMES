using FreeSql.DatabaseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs
{

    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_docking" )]
    public partial class T_docking
    {

        [JsonProperty, Column(Name = "id" , IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 停驳位名称
        /// </summary>
        [JsonProperty, Column(Name = "dockingPosition", StringLength = 11)]
        public string DockingPosition { get; set; }

        /// <summary>
        /// 停驳位编号
        /// </summary>
        [JsonProperty, Column(Name = "dockingPositionCode" )]
        public string DockingPositionCode { get; set; }

        /// <summary>
        /// 产线号
        /// </summary>
        [JsonProperty, Column(Name = "lineNo" )]
        public int? LineNo { get; set; }

        /// <summary>
        /// 机台编号
        /// </summary>
        [JsonProperty, Column(Name = "name" )]
        public string Name { get; set; }

    }

}
