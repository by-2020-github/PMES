using System.Diagnostics;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using PMES.UC.reports;
using System.Linq;

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

            object p = pa;
            Debug.WriteLine(p is Person1);
            Debug.WriteLine(p is Person2);
        }       
        [TestMethod]
        public void TestMethod2()
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            var printDocument = new PrintDocument();
            //ָ����ӡ��
            printDocument.PrinterSettings.PrinterName = "����ΪWPS PDF";// ��ӡ����������             
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            try
            {
                printDocument.Print();
            }
            catch (InvalidPrinterException)
            {
                // ��ӡ����
            }
            finally
            {
                printDocument.Dispose();
            }
           
        }

        [TestMethod]
        public void TestMethod3()
        {
            var templateBox = new TemplateBox();
            templateBox.Print("����ΪWPS PDF");
        }

        // �ļ���ʽ����
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            e.PageSettings.PaperSize = new PaperSize
            {
                Height = 400,
                PaperName = "MyOwn",
                RawKind = 0,
                Width = 300
            };
            Font titleFont = new Font("����", 28, FontStyle.Bold);// ��������
            Font fntTxt = new Font("����", 20, FontStyle.Regular);// ��������
            Brush brush = new SolidBrush(Color.Black);// ��ˢ
            Pen pen = new Pen(Color.Black); // ������ɫ
            Point po = new Point(10, 10);  // (���ұ߾�, �����м��)
            try
            {
                e.Graphics.DrawString("   �շ�Ʊ��", titleFont, brush, new Point(10, 30));  //��ӡ��������
                e.Graphics.DrawString(GetPrintSW().ToString(), fntTxt, brush, po);  //��ӡ����
            }

            catch (Exception ex)
            {
               
            }

        }

        // ��ӡ���ݣ�ʹ�� StringBuilder�� AppendLineΪ��ռһ��
        public StringBuilder GetPrintSW()
        {
            StringBuilder printTxt = new StringBuilder();

            printTxt.AppendLine(" \n");
            printTxt.AppendLine(" \n");

            printTxt.AppendLine("����: " + "00008476\n");

            printTxt.AppendLine("����: " + "�����˹���� \n");

            printTxt.AppendLine("���֤��: \n");
            printTxt.AppendLine("350781196663078325 \n");

            printTxt.AppendLine("���ɱ�: " + "10 \n");

            printTxt.AppendLine("֧��ʱ�䣺 \n");
            printTxt.AppendLine(DateTime.Now.ToString() + " \n");

            printTxt.AppendLine("֧����ʽ��" + "΢��֧�� \n");

            printTxt.AppendLine("Ʊ�ݴ�ӡʱ�䣺 \n");
            printTxt.AppendLine(DateTime.Now.ToString() + " \n");

            return printTxt;
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