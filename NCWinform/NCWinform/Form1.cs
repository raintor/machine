using NCWinform.FANUC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FanucNCCode fnc = new FanucNCCode();
        ushort Flibhndl = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string ip_port = textIP.Text;
            string ip = ip_port.Split(':')[0];
            ushort usart = Convert.ToUInt16(ip_port.Split(':')[1]);
            labConState.Text = fnc.PrepareBFormeforeConnet(ip, usart, out Flibhndl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fnc.Closeconnect(Flibhndl);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textMX.Text = fnc.GetMachineCor1(Flibhndl).ToString();
            textMY.Text = fnc.GetMachineCor2(Flibhndl).ToString();
            textMZ.Text = fnc.GetMachineCor3(Flibhndl).ToString();
            textXX.Text = fnc.GetRelativeCorX(Flibhndl).ToString();
            textXY.Text = fnc.GetRelativeCorY(Flibhndl).ToString();
            textXZ.Text = fnc.GetRelativeCorZ(Flibhndl).ToString();
            textSp.Text = fnc.GetMainSpeed(Flibhndl);
            textFS.Text = fnc.GetFeedingSpeed(Flibhndl);
           
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择NC程序";
            ofd.InitialDirectory = "C:\\Users\\Administrator\\Desktop";
            //ofd.Filter = "*.*";
            ofd.ShowDialog();
            string path = ofd.FileName;
            textPath.Text = path;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textNCName.Text == "" || textPath.Text == "")
            {
                MessageBox.Show("填入不能为空");
                return;

            }
            int ret;
            ret = Focas1.cnc_pdf_del(Flibhndl, "//CNC_MEM/USER/LIBRARY/" + textNCName.Text);
            string line = "";

            StreamReader sw = new StreamReader(textPath.Text);

            line = sw.ReadToEnd();
            sw.Close();
            short rets = Focas1.cnc_dwnstart3(Flibhndl, 0);


            int len = line.Length;
            int n = len;
            while (len > 0)
            {
                n = len;


                ret = Focas1.cnc_download3(Flibhndl, ref n, line);
                if (ret == 10)//EW_BUFFER  
                {
                    continue;
                }
                if (ret == Focas1.EW_OK)
                {
                    len = len - n;
                    line = line.Substring(n, len);
                }

            }
            ret = Focas1.cnc_dwnend3(Flibhndl);
            if (ret == Focas1.EW_OK)
            {
                MessageBox.Show("上传成功");
            }
            else
            {

                MessageBox.Show(ret.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textNCName.Text == "")
            {
                MessageBox.Show("请填入要删除的NC文件名");
                return;

            }
           int ret= Focas1.cnc_pdf_del(Flibhndl, "//CNC_MEM/USER/LIBRARY/" + textNCName.Text);
            //int ret;
            //ret = Focas1.cnc_pdf_del(Flibhndl, "//CNC_MEM/USER/PATH1/" + textNCName.Text);
           if (ret == Focas1.EW_OK)
           {
               MessageBox.Show("成功");
           }
           else
           {

               MessageBox.Show(ret.ToString());
           }
           
           // fnc.UpLoadNCCode(Flibhndl, "O0004", textPath.Text);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textNCName.Text == ""||textPath.Text=="")
            {
                MessageBox.Show("填入不能为空");
                return;

            }
            int ret;
            ret = Focas1.cnc_pdf_del(Flibhndl, "//CNC_MEM/USER/LIBRARY/" + textNCName.Text);
            string line = "";

            StreamReader sw = new StreamReader(textPath.Text);

            line = sw.ReadToEnd();
            sw.Close();
            short rets = Focas1.cnc_dwnstart3(Flibhndl, 0);


            int len = line.Length;
            int n = len;
            while (len > 0)
            {
                n = len;


                ret = Focas1.cnc_download3(Flibhndl, ref n, line);
                if (ret == 10)//EW_BUFFER  
                {
                    continue;
                }
                if (ret == Focas1.EW_OK)
                {
                    len = len - n;
                    line = line.Substring(n, len);
                }

            }
            ret = Focas1.cnc_dwnend3(Flibhndl);
            if (ret == Focas1.EW_OK)
            {
                MessageBox.Show("成功");
            }
            else if(ret==5)
            {
                MessageBox.Show("1:改程序可能打开，请在后台关闭；\n 2：NC代码格式错误，请按照提示格式修改");
                
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

      
    }
}
 