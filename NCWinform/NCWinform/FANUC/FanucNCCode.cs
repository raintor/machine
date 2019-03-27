using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCWinform.FANUC
{
    class FanucNCCode
    {
          
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="ip">设备ip</param>
        /// <usart>设备串口号
        /// <returns>连接状态</returns>
        public string  PrepareBFormeforeConnet(string ip,ushort usart,out ushort Flibhndl )
        {
            String _ip = ip;
            short ret = Focas1.cnc_allclibhndl3(_ip, usart, 10, out Flibhndl);
            if (ret == Focas1.EW_OK)
            {
                return "连接成功";
               
            }
            else
            {
                return "连接失败";
            }
        }
        /// <summary>
        /// 断开连接设备
        /// </summary>
        /// <param name="ip">设备ip</param>
        /// <returns></returns>
        public void Closeconnect(ushort Flibhndl)
        {
           
            short ret;
            ret = Focas1.cnc_freelibhndl(Flibhndl);
            if (ret == Focas1.EW_OK)
            {
                Console.WriteLine("释放库句柄成功！");
            }
            else
            {
                Console.WriteLine("释放库句柄成功失败，请检查！");
               
            }
        }

        /// <summary>
        /// 获得机床机械坐标
        /// </summary>
        public double GetMachineCor1(ushort Flibhndl)      //X轴
        {


            double cor1;
            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();

            short ret = Focas1.cnc_machine(Flibhndl, (short)(1), 8, odbaxis);
            cor1 = odbaxis.data[0] * Math.Pow(10, -4);
            return cor1 * 10;

        }
        public double GetMachineCor2(ushort Flibhndl)     //Y轴
        {


            double cor2;
            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();

            short ret = Focas1.cnc_machine(Flibhndl, (short)(2), 8, odbaxis);
            cor2 = odbaxis.data[0] * Math.Pow(10, -4);
            return cor2 * 10;

        }
        public double GetMachineCor3(ushort Flibhndl)     //Z轴
        {


            double cor3;
            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();

            short ret = Focas1.cnc_machine(Flibhndl, (short)(3), 8, odbaxis);
            cor3 = odbaxis.data[0] * Math.Pow(10, -3);
            return cor3;

        }
        /// <summary>
        /// 获得机床相对坐标
        /// </summary>
        public double GetRelativeCorX(ushort Flibhndl)                  //获得相对X轴
        {
            Focas1.ODBST odbst = new Focas1.ODBST();
            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();

            short ret2 = Focas1.cnc_relative2(Flibhndl, 1, 16, odbaxis);
            string relative2X = odbaxis.data[0].ToString();
            double x = Convert.ToDouble(relative2X) / 1000;
            return x;
        }

        public double GetRelativeCorY(ushort Flibhndl)                   //获得相对Z轴
        {
            Focas1.ODBST odbst = new Focas1.ODBST();
            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();

            short ret2 = Focas1.cnc_relative2(Flibhndl, 2, 16, odbaxis);
            string relative2Z = odbaxis.data[0].ToString();
            double z = Convert.ToDouble(relative2Z) / 1000;
            return z;
        }
        public double GetRelativeCorZ(ushort Flibhndl)                   //获得相对Z轴
        {
            Focas1.ODBST odbst = new Focas1.ODBST();
            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();

            short ret2 = Focas1.cnc_relative2(Flibhndl, 3, 16, odbaxis);
            string relative2Z = odbaxis.data[0].ToString();
            double z = Convert.ToDouble(relative2Z) / 1000;
            return z;
        }

        /// <summary>
        /// 获得主轴转速
        /// </summary>
        public string GetMainSpeed(ushort Flibhndl)
        {


            Focas1.ODBACT odbaxis = new Focas1.ODBACT();
            short ret = Focas1.cnc_acts(Flibhndl, odbaxis);
            return odbaxis.data.ToString();
        }
        /// <summary>
        /// 获得进给速度
        /// </summary>
        public string GetFeedingSpeed(ushort Flibhndl)
        {
            Focas1.ODBACT odbaxis = new Focas1.ODBACT();
            short ret = Focas1.cnc_actf(Flibhndl, odbaxis);
            return odbaxis.data.ToString();
        }
        //当前执行程序号
        public string SupervisorCurrentProgramNum(ushort Flibhndl)
        {
            string state = "";
            Focas1.ODBPRO pnum = new Focas1.ODBPRO();
            short ret = Focas1.cnc_rdprgnum(Flibhndl, pnum);
            state = String.Format("O{0:0000}", pnum.data);
            return state;
        }
        /// <summary>
        /// 程序上传
        /// </summary>
        /// <param name="filename">所需要覆盖的NC代码</param>
        /// <param name="path">PC端NC代码的路径，代码格式为.prg</param>
        /// 该函数要求上传代码与覆盖代码的文件名一样，且打开的代码不能覆盖
        /// 如 filename为O0002时，PC端名称为2.prg
        public bool UpLoadNCCode(ushort Flibhndl, string filename, string path)
        {
            int ret;
            ret = Focas1.cnc_pdf_del(Flibhndl, "//CNC_MEM/USER/LIBRARY/" + filename);
            string line = "";

            StreamReader sw = new StreamReader(path);

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
                return true;
            else
                return false;
        }
        /// <summary>
        /// 删除机床内的指定NC程序
        /// </summary>
        public void DeletNCCode(ushort Flibhndl, string filename)
        {
            int ret;
            ret = Focas1.cnc_pdf_del(Flibhndl, "//CNC_MEM/USER/PATH1/" + filename);
        }
        /// <summary>
        /// 读取NC目录
        /// </summary>
        public Focas1.PRGDIR2 getNCDir(string _top_prog, string _num_prog ,ushort Flibhndl)
        {
          
        
            short type = 2;
            short top_prog = short.Parse(_top_prog);
            short num_prog = short.Parse(_num_prog); 
             
            Focas1.PRGDIR2 buf = new Focas1.PRGDIR2();
            Focas1.cnc_rdprogdir2(Flibhndl, type, ref top_prog, ref num_prog, buf);
            return buf;
        }
    }
}
