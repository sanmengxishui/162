using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCI_DMC;
using PCI_DMC_ERR;
using System.Windows.Forms;
using System.Threading;

namespace JPTCG.Motion
{
    public class MotionManager:BaseMotion
    {
        public short existcard = 0, rc;
        public ushort gCardNo;
        ushort[] gCardNoList = new ushort[16];
        public int [] ScaleAxis=new int[5];
        uint[] SlaveTable = new uint[4];
        ushort[] NodeID = new ushort[32];
        byte[] value = new byte[10];
        List<Int32> MotorSpeed = new List<int>();
        List<Int32> MotorScale = new List<int>();

        private static MotionManager mCmotion;

        public delegate void NotifySaveWarning(string strwarm);//用于显示报警信息
        public event NotifySaveWarning OnNotifySaveWarning;

        public delegate void DisplaywriteLoginfor(string Loginfor);//用于显示log的委托
        public event DisplaywriteLoginfor OnDisplaywriteLoginfor;

        public static MotionManager CreateMotion()
        {
            if (mCmotion == null) mCmotion = new MotionManager();            
            return mCmotion;
        }

        public MotionManager()
        {
            initialization();
        }
        //用于显示报警信息的委托
        private void SendMsgToDisplayWarning(string strwarning)
        {
            if (OnNotifySaveWarning != null)
                OnNotifySaveWarning(strwarning);
        }

        //用于显示LOG信息的委托
        private void SendMsgToSaveLoginfor(string Loginfor)
        {
            if (OnDisplaywriteLoginfor != null)
                OnDisplaywriteLoginfor(Loginfor);
        }
        private void initialization()//初始化轴的细分
        {
            ScaleAxis[0] = 128000;
            ScaleAxis[1] = 128000;
            ScaleAxis[2] = 128000;
            ScaleAxis[3] = 128000;
            ScaleAxis[4] = 128000;           
        }

        public int OpenCard()  //打开控制卡并初始化
        {
            ushort i, card_no = 0, cardnum = 0, DeviceInfo = 0;
            ushort  lMask = 0x1;
            ushort gNodeNum = 0;
            uint DeviceType=0, IdentityObject=0;
             try 
             {
                 rc = CPCI_DMC.CS_DMC_01_open(ref existcard);

                 if (existcard <= 0)
                 {
                   //  throw new Exception("没有找到运动控制卡！" + e.Message);
                     SendMsgToDisplayWarning("No DMC-NET card can be found!");
                     MessageBox.Show("No DMC-NET card can be found!");
                     return -1;
                 }
                 else
                 {
                     for (i = 0; i < existcard; i++)
                     {
                         rc = CPCI_DMC.CS_DMC_01_get_CardNo_seq(i, ref card_no);
                         gCardNoList[i] = card_no;
                     }
                     gCardNo = gCardNoList[0];

                     for (i = 0; i < existcard; i++)
                     {
                         rc = CPCI_DMC.CS_DMC_01_pci_initial(gCardNoList[i]);
                         if (rc != 0)
                         {
                             // throw new Exception("运动控制卡无法进行初始化！" + e.Message);
                             SendMsgToDisplayWarning("Can't boot PCI_DMC Master Card！");
                             return -1;
                         }
                         else
                         {
                             rc = CPCI_DMC.CS_DMC_01_initial_bus(gCardNoList[i]);
                             if (rc != 0)
                             {
                                 SendMsgToDisplayWarning("The PCI_DMC Initial Failed！");
                                 return -1;
                             }
                             else
                             {
                                 for (i = 0; i < 4; i++)
                                     SlaveTable[i] = 0;

                                 rc = CPCI_DMC.CS_DMC_01_start_ring(gCardNo, 0);                      //Start communication
                                 rc = CPCI_DMC.CS_DMC_01_get_device_table(gCardNo, ref DeviceInfo);   //Get Slave Node ID   
                                 rc = CPCI_DMC.CS_DMC_01_get_node_table(gCardNo, ref SlaveTable[0]);
                                
                                    
                                 if (SlaveTable[0] == 0)
                                 {
                                   SendMsgToDisplayWarning("The node table is empty！");
                                 }
                                 else
                                 {
                                     for (i = 0; i < 32; i++)
                                     {
                                         NodeID[i] = 0;
                                         if ((SlaveTable[0] & lMask) != 0)
                                         {
                                             NodeID[gNodeNum] = (ushort)(i + 1);
                                             rc = CPCI_DMC.CS_DMC_01_get_devicetype(gCardNo, NodeID[i], 0, ref DeviceType, ref IdentityObject);
                                        
                                             gNodeNum++;
                                         }
                                         lMask <<= 1;
                                     }

                                    
                                 }
                             
                             }
                         }
                       
                     }

                     if (existcard != 0)
                     {

                         gCardNo = gCardNoList[0];
                     }
                     
                 }
             }
             catch (Exception e)
             {
               //  throw (new Exception("运动控制卡打开失败或者初始化失败！"));
                 SendMsgToDisplayWarning("PCI_DMC Master Card failed OR PCI_DMC Initial Failed！");
                 return -1;
             }
        
            return gCardNo;
        }
        
        public void VCloseCard()  //关闭运动控制卡
        {
            int i;
            for (i = 0; i < existcard; i++)
                rc = CPCI_DMC.CS_DMC_01_reset_card(gCardNoList[i]);

            CPCI_DMC.CS_DMC_01_close();
        }
        public void VSetCard(ushort CardNo, ushort NodeID,ushort svonON_OFF)  //设置脉冲模式及伺服是能
        {
            //rc = CPCI_DMC.CS_DMC_01_set_rm_04pi_ipulser_mode(gCardNo, NodeID, 0, 1);
            //rc = CPCI_DMC.CS_DMC_01_set_rm_04pi_opulser_mode(gCardNo, NodeID, 0, 1);

            rc = CPCI_DMC.CS_DMC_01_ipo_set_svon(gCardNo, NodeID, 0, svonON_OFF);  //1为伺服ON，0为伺服OFF
            //CPCI_DMC.CS_DMC_01_enable_soft_limit(gCardNo, NodeID, 0, 1);           //设置轴的软限位是能，1为碰到限位立即停止，2为减数停止。
        }
        public void VSetAxisServo(ushort CardNo, ushort NodeID, ushort svonON_OFF)
        {
            try
            {
                rc = CPCI_DMC.CS_DMC_01_ipo_set_svon(gCardNo, NodeID, 0, svonON_OFF);//1为伺服ON，0为伺服OFF
               
            }
            catch
            {
                //return false;
            }
          
        }
        
        public void VReSetAlm(ushort CardNo, ushort NodeID)
        {
            rc = CPCI_DMC.CS_DMC_01_set_ralm(gCardNo, NodeID, 0);
            if (rc!=0)
            {
                SendMsgToDisplayWarning("Clear the alm of PCI_DMC Master Card failed！");
            }
        }

        public uint VGetAxisAlm(ushort CardNo, ushort NodeID)
        { 
             uint alm_code=0;
             CPCI_DMC.CS_DMC_01_get_alm_code(CardNo, NodeID, 0, ref alm_code);
            return alm_code;
        }
        public void VSetAxisposition(ushort CardNo, ushort NodeID)
        {
            CPCI_DMC.CS_DMC_01_set_position(gCardNo, NodeID, 0, 0);
            CPCI_DMC.CS_DMC_01_set_command(gCardNo, NodeID, 0, 0);
        }
        public void VWriteIOout(ushort CardNo, ushort port, bool sts)
        {
            uint IOStatus = 0;

            CPCI_DMC.CS_DMC_01_get_dio_output_DW(CardNo, ref IOStatus);
            ushort IOOutStatus = 0;
            if (sts)
            {
                IOOutStatus = (ushort)(IOStatus | (ushort)Math.Pow(2, port));
            }
            else
            {
                IOOutStatus = (ushort)(IOStatus & (0xFFFF - (ushort)Math.Pow(2, port)));
            }

            CPCI_DMC.CS_DMC_01_set_dio_output_DW(CardNo, IOOutStatus);
        }
        public void VWriteOutPut(ushort port1, ushort port2)
        {
            VWriteIOout(gCardNo, port1, true);
            VWriteIOout(gCardNo, port2, false);
        }
        public bool BReadIOOutPut(ushort CardNo, ushort port)
        {
            uint IOStatus = 0;
            CPCI_DMC.CS_DMC_01_get_dio_output_DW(CardNo, ref IOStatus);
            //if ((IOStatus & (0x01 << port)) != 0)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return ((IOStatus & (0x01 << port)) != 0 ? true : false);
        }
        public bool BReadIOInPut(ushort CardNo, ushort port)
        {
            uint IOStatus = 0;
            try
            {
                CPCI_DMC.CS_DMC_01_get_dio_input_DW(CardNo, ref IOStatus);
                if ((IOStatus & (0x01 << port)) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            

        }

        public bool BReadServoDI(ushort CardNo, ushort NodeID, int idnum)  //idnum为4则对应原点位，5、6分别对应负限位和正限位
        { 
            ushort servo_DI=0;
            try
            {
                CPCI_DMC.CS_DMC_01_get_servo_DI(CardNo, NodeID, 0, ref servo_DI);
                if ((servo_DI & (0x01 << idnum)) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
           

            
        }
        public void VGohome(ushort CardNo, ushort nodeid, ushort home_mode, ushort speed)  //home_mode=1,以负限位为原点，2,为正限为原点，7以原点遇正限位反转
        {
            double acc = 0.1;
            //double Tdec =0.1;
            ushort StrVel =100;
            short rc;
            CPCI_DMC.CS_DMC_01_set_home_config(CardNo, nodeid, 0, home_mode, 0, StrVel, speed, acc);
            
            rc = CPCI_DMC.CS_DMC_01_set_home_move(CardNo, nodeid, 0);

        }

        public void VSetpotionZero(ushort CardNo, ushort nodeid)
        { 
            CPCI_DMC.CS_DMC_01_set_command( CardNo,  nodeid, 0, 0);
            CPCI_DMC.CS_DMC_01_set_position(CardNo, nodeid, 0, 0);
        }
        public void VGohomeLimiter(ushort axis, ushort dir)
        { 

        }
        public void VWaitAxisGohome(ushort CardNo, ushort nodeid)
        {
            ushort MC_done = 0;
            uint MC_status = 0;
            short rc;
            uint status1 = 0, status2 = 0, status3 = 0, status4=0;
            CPCI_DMC.CS_DMC_01_motion_done(CardNo, nodeid, 0, ref MC_done);

            rc = CPCI_DMC.CS_DMC_01_motion_status(CardNo, nodeid, 0, ref MC_status);
            status1 = ((MC_status >> 1) & 0X0001);
            status2 = ((MC_status >> 2) & 0X0001);
            status3 = ((MC_status >> 10) & 0X0001);
            status4 = ((MC_status >> 12) & 0X0001);
            while ((status1 != 1) || (status2 != 1) || (status3 != 1) || (status4 != 1) || (MC_done!=0))
            {
                Application.DoEvents();
                Thread.Sleep(10);
                CPCI_DMC.CS_DMC_01_motion_status(CardNo, nodeid, 0, ref MC_status);
                status1 = ((MC_status >> 1) & 0X0001);
                status2 = ((MC_status >> 2) & 0X0001);
                status3 = ((MC_status >> 10) & 0X0001);
                status4 = ((MC_status >> 12) & 0X0001);
                CPCI_DMC.CS_DMC_01_motion_done(CardNo, nodeid, 0, ref MC_done);
            }

        }
        public void VWaitAxisStop(ushort CardNo, ushort nodeid)
        {
            ushort MC_done = 0;
            short rc;

            rc=CPCI_DMC.CS_DMC_01_motion_done(CardNo, nodeid, 0, ref MC_done);
            while(MC_done!=0)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                CPCI_DMC.CS_DMC_01_motion_done(CardNo, nodeid, 0, ref MC_done);
            }

        }
        public void VMoveTo(ushort CardNo, ushort NodeID, double Dist, int speed)
        {
            double acc = 0.2;
            double Tdec =0.2;
            int StrVel = speed / 10;
            int dist;
            dist = (Int32)(ScaleAxis[NodeID] * Dist);

            CPCI_DMC.CS_DMC_01_start_ta_move(CardNo, NodeID, 0, dist, StrVel, speed, acc, Tdec);
           
           
        }
        public void VStepMove(ushort CardNo, ushort NodeID, double Dist, int speed)
        {
            double acc = 0.2;
            double Tdec = 0.2;
            int StrVel = speed / 10;
            Int32 dist;
            dist = (Int32)(ScaleAxis[NodeID] * Dist);
            //rc = CPCI_DMC.CS_DMC_01_start_tr_move(0, 2, 0, -1280000, 0, 1280000, 0.3, 0.3);
            rc = CPCI_DMC.CS_DMC_01_start_tr_move(CardNo, NodeID, 0, dist, StrVel, speed, acc, Tdec);
            if (rc != 0)
            {
                MessageBox.Show("not ok");
            }
        }
        public void VStopAxis(ushort CardNo, ushort NodeID, int swichstop)//0为急停，1为减数停止
        { 
           if(swichstop==0)
           {
               CPCI_DMC.CS_DMC_01_emg_stop(CardNo, NodeID, 0);   
               
           }
           else
           {
               CPCI_DMC.CS_DMC_01_sd_stop(gCardNo, NodeID, 0, 0.1);
                 
           }
            
        }
        public double DReadcurrentpulsePos(ushort CardNo, ushort NodeID)
        {
             int cmd = 0;
             double commdpos = 0.0;
             CPCI_DMC.CS_DMC_01_get_command(gCardNo, NodeID, 0, ref cmd);
             commdpos = cmd/ScaleAxis[NodeID];
            return commdpos;
        }
        public double DReadcurrentencodePos(ushort CardNo, ushort NodeID)
        {
             int pos = 0;
             double codepos=0.0;
             CPCI_DMC.CS_DMC_01_get_position(gCardNo, NodeID, 0, ref pos);
              codepos = (double)pos/ScaleAxis[NodeID];
             return codepos;
        }
        public void VContinuMove(ushort axis, ushort dir)
        { 
        }
        public void VContinuMove(ushort axis, ushort dir, double v, double acc)
        { 

        }
    }
}
