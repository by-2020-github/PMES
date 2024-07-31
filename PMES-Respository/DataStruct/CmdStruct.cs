using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PMES_Common;
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
    ///     DB500
    /// </summary>
    [PlcCmdAttribute(501)]
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        ///     上位机写，入；置2; PLC写入，置1.
        /// </summary>
        public byte PmesAndPlcReadWriteFlag { get; set; }

        protected bool Equals(PmesCmdUnStacking other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && PmesAndPlcReadWriteFlag == other.PmesAndPlcReadWriteFlag;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PmesCmdUnStacking)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ PmesAndPlcReadWriteFlag.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    [PlcCmdAttribute(502)]
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }

        protected bool Equals(PlcCmdUnStacking other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && UnStackingFinished == other.UnStackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdUnStacking)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    [PlcCmdAttribute(502)]
    public class PlcCmdUnStacking1
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }

        protected bool Equals(PlcCmdUnStacking1 other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && UnStackingFinished == other.UnStackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdUnStacking1)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    [PlcCmdAttribute(503)]
    public class PlcCmdUnStacking2
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }

        protected bool Equals(PlcCmdUnStacking1 other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && UnStackingFinished == other.UnStackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdUnStacking1)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    [PlcCmdAttribute(504)]
    public class PlcCmdUnStacking3
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }

        protected bool Equals(PlcCmdUnStacking1 other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && UnStackingFinished == other.UnStackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdUnStacking1)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    [PlcCmdAttribute(505)]
    public class PlcCmdUnStacking4
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }

        protected bool Equals(PlcCmdUnStacking1 other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && UnStackingFinished == other.UnStackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdUnStacking1)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    [PlcCmdAttribute(506)]
    public class PlcCmdUnStacking5
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }

        protected bool Equals(PlcCmdUnStacking1 other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && UnStackingFinished == other.UnStackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdUnStacking1)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///  DB502----DB506 202---206
    /// </summary>
    [PlcCmdAttribute(507)]
    public class PlcCmdUnStacking6
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
        public ushort ReelHeight { get; set; }
        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否拆垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte UnStackingFinished { get; set; }

        protected bool Equals(PlcCmdUnStacking1 other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && ReelNum == other.ReelNum &&
                   UnStackSpeed == other.UnStackSpeed && ReelHeight == other.ReelHeight && Reserve1 == other.Reserve1 &&
                   Reserve2 == other.Reserve2 && UnStackingFinished == other.UnStackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdUnStacking1)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelNum.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelHeight.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ UnStackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    #endregion

    #region 带字符串的指令

    public class PmesDataItemList
    {
        /// <summary>
        ///     DB510
        /// </summary>
        public ObservableCollection<DataItem> PmesWeightAndBarCode { get; set; } = new ObservableCollection<DataItem>()
        {
            //id
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 510,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = 218,
            },
            //code 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 510,
                StartByteAdr = 1,
                BitAdr = 0,
                Count = 50,
                Value = new object()
            },
            //weight 1 重量：4个byte,1个双字，如：120023（代表1200.23KG，两位小数点）；
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.DWord,
                DB = 510,
                StartByteAdr = 52,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            // weight 2 重量：4个byte,1个双字，如：120023（代表1200.23KG，两位小数点）；
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.DWord,
                DB = 510,
                StartByteAdr = 56,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            // read flag 1.上位机未读；2.上位机已读
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Byte,
                DB = 510,
                StartByteAdr = 60,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },

            //预留位
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 510,
                StartByteAdr = 61,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //预留位
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 510,
                StartByteAdr = 63,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
        };

        /// <summary>
        ///     线盘barcode check
        /// </summary>
        public ObservableCollection<DataItem> PmesReelCodeCheck { get; set; } = new ObservableCollection<DataItem>()
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
                Value = 221 //暂时未知,为平面图上的设备号,PLC需要，用于确定是那个设备，进而监控设备状态
            },
            //code1 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.S7String,
                DB = 520,
                StartByteAdr = 1,
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
                StartByteAdr = 52,
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
                StartByteAdr = 103,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve1
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 520,
                StartByteAdr = 104,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve2
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 520,
                StartByteAdr = 106,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
        };

        /// <summary>
        ///     装箱
        /// </summary>
        public ObservableCollection<DataItem> PmesPackingBox { get; set; } = new ObservableCollection<DataItem>()
        {
            //id
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Int,
                DB = 530,
                StartByteAdr = 0,
                BitAdr = 0,
                Count = 1,
                Value = 1 //暂时未知
            },
            //code1 条码数据，PMES直接读取
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.S7String,
                DB = 520,
                StartByteAdr = 1,
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
                StartByteAdr = 52,
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
                StartByteAdr = 103,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve1
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 520,
                StartByteAdr = 104,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
            //1.reserve2
            new DataItem
            {
                DataType = DataType.DataBlock,
                VarType = VarType.Word,
                DB = 520,
                StartByteAdr = 106,
                BitAdr = 0,
                Count = 1,
                Value = new object()
            },
        };

        protected bool Equals(PmesDataItemList other)
        {
            return Equals(PmesWeightAndBarCode, other.PmesWeightAndBarCode) &&
                   Equals(PmesReelCodeCheck, other.PmesReelCodeCheck) && Equals(PmesPackingBox, other.PmesPackingBox);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PmesDataItemList)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (PmesWeightAndBarCode != null ? PmesWeightAndBarCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PmesReelCodeCheck != null ? PmesReelCodeCheck.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PmesPackingBox != null ? PmesPackingBox.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    #endregion

    #region 码垛

    [PlcCmdAttribute(540)]
    public class PmesStacking
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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
        ///     备注：横竖码垛方向。垛型，代表那种子托盘. 
        ///    1. PT25裸装-2层
        ///    2. PT25裸装-3层
        ///     3. PT25箱装-2层
        ///    4. PT25箱装-3层
        ///    5. PT45
        ///    6.PT60
        ///    7.PT90
        ///    8.PT200
        ///    9.PT270
        ///    10.355*180木盘
        ///    11. 500*210木盘
        /// </summary>
        public byte StackModel { get; set; }


        //
        //码垛速度
        /*1. P25/PT45为60%

            2. PT60/PT90为50%

            3. PT200/PT270为40%

            4. 木托盘为355：50%

            5. 木托盘500： 为40%*/
        public ushort StackingSpeed { get; set; }


        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }


        /// <summary>
        ///     上位机写，入；置2; PLC写入，置1.
        /// </summary>
        public byte PmesAndPlcReadWriteFlag { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(541)]
    public class PlcCmdStacking
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }

        protected bool Equals(PlcCmdStacking other)
        {
            return DeviceId == other.DeviceId && WorkPositionId == other.WorkPositionId &&
                   ReelSpecification == other.ReelSpecification && StackModel == other.StackModel &&
                   Reserve1 == other.Reserve1 && Reserve2 == other.Reserve2 &&
                   StackingFinished == other.StackingFinished;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlcCmdStacking)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeviceId.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkPositionId.GetHashCode();
                hashCode = (hashCode * 397) ^ ReelSpecification.GetHashCode();
                hashCode = (hashCode * 397) ^ StackModel.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve1.GetHashCode();
                hashCode = (hashCode * 397) ^ Reserve2.GetHashCode();
                hashCode = (hashCode * 397) ^ StackingFinished.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(541)]
    public class PlcCmdStacking1
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(542)]
    public class PlcCmdStacking2
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(543)]
    public class PlcCmdStacking3
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(544)]
    public class PlcCmdStacking4
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(545)]
    public class PlcCmdStacking5
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(546)]
    public class PlcCmdStacking6
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(547)]
    public class PlcCmdStacking7
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    /// <summary>
    ///     DB541~DB548
    /// </summary>
    [PlcCmdAttribute(548)]
    public class PlcCmdStacking8
    {
        /// <summary>
        /// 码垛机器人
        /// </summary>
        public byte DeviceId { get; set; }

        public ushort WorkPositionId { get; set; }

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

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否码垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    #endregion

    #region 组合子母托盘

    [PlcCmdAttribute(550)]
    public class PmesCmdCombinationMotherChildTray
    {
        /// <summary>
        /// 00001 - 组合子母托盘 机器人
        /// </summary>
        public byte DeviceId { get; set; }

        /// <summary>
        ///     母托盘工位号
        /// </summary>
        public ushort MotherStayWorkPositionId { get; set; }

        /// <summary>
        ///  子托盘规格类型
        ///     备注：横竖码垛方向。垛型，代表那种子托盘.
        ///     1. PT25裸装-2层
        ///     2. PT25裸装-3层
        ///     3. PT25箱装-2层
        ///     4. PT25箱装-3层
        ///     5. PT45
        ///     6.PT60
        ///     7.PT90
        ///     8.PT200
        ///     9.PT270
        ///     10.355*180木盘
        ///     11. 500*210木盘
        /// </summary>
        public byte ChildStaySpecification { get; set; }

        /// <summary>
        ///     子托盘工位号
        /// </summary>
        public ushort ChildStayWorkPositionId { get; set; }

        /// <summary>
        ///     子托盘个数
        /// </summary>
        public byte ChildStayNum { get; set; }

        /// <summary>
        ///     子母托盘工位号
        /// </summary>
        public ushort ChildMontherStayWorkPositionId { get; set; }

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        ///     相等，则认为已处理过；否则，未处理;  判定下次是否可以（上位机）写入的标志
        /// </summary>
        public byte WriteFlag { get; set; }

        /// <summary>
        ///     上位机写入为0，plc处理后值为1.
        /// </summary>
        public byte PlcProcessFlag { get; set; }
    }

    [PlcCmdAttribute(551)]
    public class PlcCmdCombinationMotherChildTray
    {
        /// <summary>
        /// 00001 - 组合子母托盘 机器人
        /// </summary>
        public byte DeviceId { get; set; }

        /// <summary>
        ///     母托盘工位号
        /// </summary>
        public ushort MotherStayWorkPositionId { get; set; }

        /// <summary>
        ///  子托盘规格
        ///     备注：横竖码垛方向。垛型，代表那种子托盘. 
        ///     1. PT25裸装-2层
        ///     2. PT25裸装-3层
        ///     3. PT25箱装-2层
        ///     4. PT25箱装-3层
        ///     5. PT45
        ///     6.PT60
        ///     7.PT90
        ///     8.PT200
        ///     9.PT270
        ///     10.355*180木盘
        ///     11. 500*210木盘
        /// </summary>
        public byte ChildStaySpecification { get; set; }

        /// <summary>
        ///     子托盘工位号
        /// </summary>
        public ushort ChildStayWorkPositionId { get; set; }

        /// <summary>
        ///     子母托盘工位号
        /// </summary>
        public ushort ChildMontherStayWorkPositionId { get; set; }

        public ushort Reserve1 { get; set; }
        public ushort Reserve2 { get; set; }

        /// <summary>
        /// 若【是否组垛完成】字段为2. 则上位机读取此块数据，并清零（设备号不清零）。
        /// </summary>
        public byte StackingFinished { get; set; }
    }

    #endregion

    #region 整条线物料信息交互区

    [PlcCmdAttribute(560)]
    public class ValidateInfo
    {
        public byte DeviceId { get; set; }

        /// <summary>
        /// 物料规格类型对应1-8，
        /// 分别如下：
        /// 1. PT25
        /// 2. PT45
        /// 3.PT60
        /// 4.PT90
        /// 5.PT200
        /// 6.PT270
        /// 7.355*180木盘
        /// 8. 500*210木盘
        /// </summary>
        public byte PSN { get; set; }

        public byte IsOk { get; set; }
        public ushort Reverse1 { get; set; }
        public ushort Reverse2 { get; set; }
    }

    #endregion
}