using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace cbar
{
    public partial class Form1 : Form
    {
        public string queryString;
        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateDb_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=192.168.19.35\SQLEXPRESS;Initial Catalog=ManatDbR;Persist Security Info=True;User ID=sa;Password=123456";


            

            var webClient = new WebClient();
            var data = webClient.DownloadData("https://www.cbar.az/currencies/14.12.2018.xml");
            var xmlData = Encoding.UTF8.GetString(data);

            StringReader xmlStreamData = new StringReader(xmlData);

            XmlSerializer xmlser = new XmlSerializer(typeof(XmlRegust.ValCurs));
            var myValCurs = (XmlRegust.ValCurs)xmlser.Deserialize(xmlStreamData);
            //foreach(var el in myValCurs.ValType)
           // {
                foreach(var elm in myValCurs.ValType[1].Valute)
                {
                    queryString += $"INSERT INTO [Table](Code,Nominal,Name,Value,ValueType,ValDateTime)" +
               $"VALUES ('{elm.Code}',{elm.Nominal},N'{elm.Name}',{elm.Value},N'{ myValCurs.ValType[1].Type}','{myValCurs.Date}')";
                }
//}

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

            }
        }   
    }
}
