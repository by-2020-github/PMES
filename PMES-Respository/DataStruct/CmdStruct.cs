using System;
using System.Collections.Generic;
using System.Text;
using S7.Net;
using S7.Net.Types;

namespace PMES_Respository.DataStruct
{
    //产品名称 Product range--PR、
    //产品规格 Product specification--PSN、
    //产品型号 Product model--PML。
    //Reel n. 卷轴; 卷盘; 卷筒; 绕在卷轴上的线（或金属丝、胶卷等）---->线盘

    #region 拆垛机器人

    /// <summary>
    ///     DB501
    /// </summary>
    public class PmesCmdUnStacking
    {
        /// <summary>
        /// 00001 - 拆垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public byte WorkPositionId { get; set; }

        /// <summary>
        /// 线盘规格 物料规格类型对应1-8，分别如下：
        ///1. PT25
        ///2. PT45
        ///3.PT60
        ///4.PT90
        ///5.PT200
        ///6.PT270
        ///7.355*180木盘
        ///8. 500*210木盘
        /// </summary>
        public byte ReelSpecification { get; set; }


        /// <summary>
        ///     线盘数量
        /// </summary>
        public byte ReelNum { get; set; }

        public byte UnStackSpeed { get; set; }
        public short ReelHeight { get; set; }
        public short Reserve1 { get; set; }
        public short Reserve2 { get; set; }

        /// <summary>
        ///     相等，则认为已处理过；否则，未处理;  判定下次是否可以（上位机）写入的标志
        /// </summary>
        public byte WriteFlag { get; set; }

        /// <summary>
        ///     上位机写入为0，plc处理后值为1.
        /// </summary>
        public short PlcProcessFlag { get; set; }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    public class PlcCmdUnStacking
    {
        /// <summary>
        /// 00001 - 拆垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public byte WorkPositionId { get; set; }

        /// <summary>
        ///     线盘规格
        /// </summary>
        public byte ReelSpecification { get; set; }

        /// <summary>
        ///     线盘数量
        /// </summary>
        public byte ReelNum { get; set; }

        public byte UnStackSpeed { get; set; }
        public short ReelHeight { get; set; }
        public short Reserve1 { get; set; }
        public short Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }
    }

    #endregion

    #region 带字符串的指令

    public class PmesDataItemList
    {
        /// <summary>
        ///     DB510
        /// </summary>
        public static List<DataItem> PmesWeightAndBarCode { get; } = new List<DataItem>()
        {
            //id
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 10,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = 1//暂时未知
            },
            //code 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 10,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //weight 1 重量：4个byte,1个双字，如：120023（代表1200.23KG，两位小数点）；
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.DWord,
                DB = 10,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            // weight 2 重量：4个byte,1个双字，如：120023（代表1200.23KG，两位小数点）；
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.DWord,
                DB = 10,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            // read flag 1.上位机未读；2.上位机已读
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 10,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
   
            //预留位
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 10,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //预留位
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 10,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
        };

        /// <summary>
        ///     线盘barcode check
        /// </summary>
        public static List<DataItem> PmesReelCodeCheck { get; } = new List<DataItem>()
        {
            //id
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Int,
                DB = 520,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = 1//暂时未知
            },
            //code1 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.S7String,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 50,
                Value = new object()
            },
            //code2 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.S7String,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 50,
                Value = new object()
            },
            //1.上位机未读；2.上位机已读
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve1
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve2
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
        };

        /// <summary>
        ///     装箱
        /// </summary>
        public static List<DataItem> PmesPackingBox { get; } = new List<DataItem>()
        {
            //id
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Int,
                DB = 520,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = 1//暂时未知
            },
            //code1 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.S7String,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 50,
                Value = new object()
            },
            //code2 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.S7String,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 50,
                Value = new object()
            },
            //1.上位机未读；2.上位机已读
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve1
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve2
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 520,
                StartByteAdr = 2,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
        };
    }

    #endregion

    #region 码垛

    public class PmesStacking
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public byte WorkPositionId { get; set; }

        /// <summary>
        /// 线盘规格 物料规格类型对应1-8，分别如下：
        ///1. PT25
        ///2. PT45
        ///3.PT60
        ///4.PT90
        ///5.PT200
        ///6.PT270
        ///7.355*180木盘
        ///8. 500*210木盘
        /// </summary>
        public byte ReelSpecification { get; set; }


        /// <summary>
        ///     垛型对应数值：1-；2-；3-
        /// </summary>
        public byte StackModel { get; set; }

        public short Reserve1 { get; set; }
        public short Reserve2 { get; set; }
        /// <summary>
        ///     相等，则认为已处理过；否则，未处理;  判定下次是否可以（上位机）写入的标志
        /// </summary>
        public byte WriteFlag { get; set; }

        /// <summary>
        ///     上位机写入为0，plc处理后值为1.
        /// </summary>
        public short PlcProcessFlag { get; set; }
    }

    #endregion



}