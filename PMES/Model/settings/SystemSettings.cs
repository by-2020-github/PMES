using FreeSql.DataAnnotations;

namespace PMES.Model.settings;
[Table(Name = "tb_settings")]
public class SystemSettings
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    public string SerialPort { get; set; } = "";
    public int BaudRate { get; set; }
}