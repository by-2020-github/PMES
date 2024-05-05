using System.Diagnostics;
using Newtonsoft.Json;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var pa = new Person1
            {
                Id = 1,
                Name = "zs"
            };
            var p2 = JsonConvert.DeserializeObject<Person2>(JsonConvert.SerializeObject(pa));
            Trace.WriteLine(JsonConvert.SerializeObject(p2));
        }
    }


    public class Person1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Person2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Des { get; set; } = "des";
    }
}