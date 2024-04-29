using FreeSql.DataAnnotations;

namespace PMES.Core.Managers;

[Table(Name = "tb_ModuleParameters")]
public class ModuleParam
{
    public ModuleParam(string name, string parameter)
    {
        Name = name;
        Parameter = parameter;
    }

    [Column(IsIdentity = true)] public int Id { get; set; }

    public string Name { get; set; }
    [Column(StringLength = -2)] public string Parameter { get; set; }
}