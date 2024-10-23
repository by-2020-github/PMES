// ReSharper disable InconsistentNaming

namespace PMES.Manual.Net6.Model;

public class ProductInfo
{
    public string product_date { get; set; } = "";
    public string product_org_number { get; set; } = "";

    /// <summary>
    ///     生产工单 G24040656 如果是new 创建的是777 作为异常判断标识
    /// </summary>
    public string product_order_no { get; set; } = "";

    public int customer_id { get; set; } 

    public string customer_number { get; set; } = "";
    public string customer_name { get; set; } = "";
    public int material_id { get; set; } 

    /// <summary>
    ///     产品代码
    /// </summary>
    public string material_number { get; set; } = "";

    /// <summary>
    ///     产品型号
    /// </summary>
    public string material_name { get; set; } = "";

    public string material_mnemonic_code { get; set; } = "";

    /// <summary>
    ///     国标型号
    /// </summary>
    public string material_execution_standard { get; set; } = "";

    public string material_ns_model { get; set; } = "";
    public int jsbz_id { get; set; } 
    public string jsbz_number { get; set; } = "";
    public string jsbz_name { get; set; } = "";
    public int xpzl_id { get; set; } = 0;
    public string xpzl_number { get; set; } = "";
    public string xpzl_name { get; set; } = "";
    public string xpzl_spec { get; set; } = "";
    public string xpzl_weight { get; set; } = "";
    public int machine_id { get; set; } = 0;
    public string machine_number { get; set; } = "";
    public string machine_nose { get; set; } = "";
    public string machine_name { get; set; } = "";
    public int stock_id { get; set; } 
    public string stock_number { get; set; } = "";
    public string stock_name { get; set; } = "";
    public PackageInfo package_info { get; set; } = new();

    public string operator_code { get; set; } = "";
    public string operator_name { get; set; } = "";
    public string customer_material_number { get; set; } = null;
    public string customer_material_name { get; set; } = null;
    public string customer_material_spec { get; set; } = null;

    public string material_thermal_grade { get; set; } = null;

    public string material_spec { get; set; } = null;

    public string jsbz_short_name { get; set; } = null;

    public string GetSpec => $"{customer_number},{material_number},{xpzl_spec},{jsbz_number}";

    public bool NeedChange(ProductInfo other)
    {
        return customer_number.Equals(other.customer_number) || material_number.Equals(other.material_number) ||
               xpzl_spec.Equals(other.xpzl_spec) ||
               jsbz_number.Equals(other.jsbz_number);
    }

    protected bool Equals(ProductInfo other)
    {
        return product_date == other.product_date && product_org_number == other.product_org_number &&
               product_order_no == other.product_order_no && customer_id == other.customer_id &&
               customer_number == other.customer_number && customer_name == other.customer_name &&
               material_id == other.material_id && material_number == other.material_number &&
               material_name == other.material_name && material_mnemonic_code == other.material_mnemonic_code &&
               material_execution_standard == other.material_execution_standard &&
               material_ns_model == other.material_ns_model && jsbz_id == other.jsbz_id &&
               jsbz_number == other.jsbz_number && jsbz_name == other.jsbz_name && xpzl_id == other.xpzl_id &&
               xpzl_number == other.xpzl_number && xpzl_name == other.xpzl_name && xpzl_spec == other.xpzl_spec &&
               xpzl_weight == other.xpzl_weight && machine_id == other.machine_id &&
               machine_number == other.machine_number && machine_name == other.machine_name &&
               stock_id == other.stock_id && stock_number == other.stock_number && stock_name == other.stock_name &&
               package_info.Equals(other.package_info) && operator_code == other.operator_code &&
               operator_name == other.operator_name && customer_material_number == other.customer_material_number &&
               customer_material_name == other.customer_material_name &&
               customer_material_spec == other.customer_material_spec &&
               material_thermal_grade == other.material_thermal_grade && material_spec == other.material_spec &&
               jsbz_short_name == other.jsbz_short_name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ProductInfo)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(product_date);
        hashCode.Add(product_org_number);
        hashCode.Add(product_order_no);
        hashCode.Add(customer_id);
        hashCode.Add(customer_number);
        hashCode.Add(customer_name);
        hashCode.Add(material_id);
        hashCode.Add(material_number);
        hashCode.Add(material_name);
        hashCode.Add(material_mnemonic_code);
        hashCode.Add(material_execution_standard);
        hashCode.Add(material_ns_model);
        hashCode.Add(jsbz_id);
        hashCode.Add(jsbz_number);
        hashCode.Add(jsbz_name);
        hashCode.Add(xpzl_id);
        hashCode.Add(xpzl_number);
        hashCode.Add(xpzl_name);
        hashCode.Add(xpzl_spec);
        hashCode.Add(xpzl_weight);
        hashCode.Add(machine_id);
        hashCode.Add(machine_number);
        hashCode.Add(machine_name);
        hashCode.Add(stock_id);
        hashCode.Add(stock_number);
        hashCode.Add(stock_name);
        hashCode.Add(package_info);
        hashCode.Add(operator_code);
        hashCode.Add(operator_name);
        hashCode.Add(customer_material_number);
        hashCode.Add(customer_material_name);
        hashCode.Add(customer_material_spec);
        hashCode.Add(material_thermal_grade);
        hashCode.Add(material_spec);
        hashCode.Add(jsbz_short_name);
        return hashCode.ToHashCode();
    }
}

/// <summary>
///     包装信息
/// </summary>
public class PackageInfo
{
    /// <summary>
    ///     包装代码
    /// </summary>
    public string? code { get; set; } 

    /// <summary>
    ///     包装名称
    /// </summary>
    public string? name { get; set; }

    /// <summary>
    ///     包装皮重
    /// </summary>
    public string? tare_weight { get; set; } 

    /// <summary>
    ///     铜线满盘重量
    /// </summary>
    public string? cu_full_coil_weight { get; set; }

    /// <summary>
    ///     铜线最小重量
    /// </summary>
    public string? cu_min_weight { get; set; }

    /// <summary>
    ///     铜线最大重量
    /// </summary>
    public string? cu_max_weight { get; set; } 

    /// <summary>
    ///     线盘内包装名称
    /// </summary>
    public string? wire_reel_inside_package_name { get; set; } = null;

    /// <summary>
    ///     线盘外包装名称
    /// </summary>
    public string? wire_reel_external_package_name { get; set; } = null;

    /// <summary>
    ///     发货子托盘编码
    /// </summary>
    public string? delivery_sub_tray_number { get; set; } = null;


    /// <summary>
    ///     发货子托盘名称
    /// </summary>
    public string? delivery_sub_tray_name { get; set; } = null;


    /// <summary>
    ///     发货子托盘规格
    /// </summary>
    public string? delivery_sub_tray_spec { get; set; } = null;


    /// <summary>
    ///     装箱件数
    /// </summary>
    public int? packing_quantity { get; set; } = 0;

    /// <summary>
    ///     每层堆垛数量
    /// </summary>
    public int? stacking_per_layer { get; set; } = 0;

    /// <summary>
    ///     堆垛层数
    /// </summary>
    public int? stacking_layers { get; set; } = 0;


    /// <summary>
    ///     是否裸装
    /// </summary>
    public bool is_naked { get; set; } = false;
}