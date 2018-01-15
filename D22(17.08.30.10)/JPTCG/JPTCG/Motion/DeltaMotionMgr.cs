using Common;
using JPTCG.Common;
using PCI_DMC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Motion
{
    public enum DM_ErrorList
    {
        [DescriptionAttribute("No Error")]
        NoError = 0,
        [DescriptionAttribute("No Delta Motion Card Found.")]
        NoCardFound = -1,
        [DescriptionAttribute("Can't Boot PCI_DMC Card.")]
        BootFail = -2, 
        [DescriptionAttribute("PCI_DMC Card Initial Failed.")]
        CardInitFail = -3,
        [DescriptionAttribute("PCI_DMC Card Slave Not Found.")]
        SlaveIsEmpty = -4,
        [DescriptionAttribute("PCI_DMC Card Reset Alarm Fail.")]
        ResetAlmFail = -5,
        [DescriptionAttribute("PCI_DMC Card Homing Timeout.")]
        HomingTimeout = -6,
        [DescriptionAttribute("PCI_DMC Card Motion Timeout.")]
        MotionTimeout = -7,
        [DescriptionAttribute("PCI_DMC Card Motion Error.")]
        MotionError = -8,
        [DescriptionAttribute("Not Safe to Move.")]
        MotionSafetyError = -9,
    }
    public class DMPara
    {
        public Axislist AxisIdx;
        public int HomeSpeed = 100;
        public int RunSpeed = 100;
        public int MotorScale = 128000;
        public bool IsServoMotor = true;
    }
    public class DeltaMotionMgr
    {
        //Parameters
        int HomingTimeoutMS = 100000;
        int MotionTimeoutMS = 100000;
        public List<DMPara> AxisPara = new List<DMPara>();

        string motionSettingsFilePath = "";
        string motionSetFileName = "motion_Settings.xml";

        int motionCmdDly = 100;
        List<ushort> CardNo = new List<ushort>();
        private static DeltaMotionMgr h_motion;

        public static DeltaMotionMgr CreateMotion()
        {
            if (h_motion == null) h_motion = new DeltaMotionMgr();
            return h_motion;
        }
        public DeltaMotionMgr()
        {
            CreateDefaultParameters();
            Init();

            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            motionSettingsFilePath = exePath + motionSetFileName;

            LoadMotionSettings();
        }
        ~DeltaMotionMgr()
        {
            if (Para.MachineOnline == false)
                return;
            CPCI_DMC.CS_DMC_01_set_dio_output_DW(CardNo[0], 0);
            CloseCard();
        }
        public void ResetIO()
        {
            CPCI_DMC.CS_DMC_01_set_dio_output_DW(CardNo[0], 0);
        }
        public void LoadMotionSettings()
        {
            string strread = "";
            string headerStr = "";
            for (int i = 0; i < AxisPara.Count; i++)
            {
                headerStr = "Axis" + (i + 1).ToString();
                //FileOperation.ReadData(motionSettingsFilePath, headerStr, "AxisID", ref strread);
                //AxisPara[i].AxisIdx = (Axislist)int.Parse(strread);

                FileOperation.ReadData(motionSettingsFilePath, headerStr, "HomeSpeed", ref strread);
                if (strread != "0")
                    AxisPara[i].HomeSpeed = int.Parse(strread);

                FileOperation.ReadData(motionSettingsFilePath, headerStr, "RunSpeed", ref strread);
                if (strread != "0")
                    AxisPara[i].RunSpeed = int.Parse(strread);

                FileOperation.ReadData(motionSettingsFilePath, headerStr, "MotorScale", ref strread);
                if (strread != "0")
                    AxisPara[i].MotorScale = int.Parse(strread);

                FileOperation.ReadData(motionSettingsFilePath, headerStr, "IsServoMotor", ref strread);
                if (strread != "0")
                    AxisPara[i].IsServoMotor = bool.Parse(strread);
            }
        }
        public void SaveMotionSettings()
        {
            string headerStr = "";
            for (int i = 0; i < AxisPara.Count; i++)
            {
                headerStr = "Axis" + (i + 1).ToString();
                FileOperation.SaveData(motionSettingsFilePath, headerStr, "HomeSpeed", AxisPara[i].HomeSpeed.ToString());
                FileOperation.SaveData(motionSettingsFilePath, headerStr, "RunSpeed", AxisPara[i].RunSpeed.ToString());
                FileOperation.SaveData(motionSettingsFilePath, headerStr, "MotorScale", AxisPara[i].MotorScale.ToString());
                FileOperation.SaveData(motionSettingsFilePath, headerStr, "IsServoMotor", AxisPara[i].IsServoMotor.ToString());
            }
        }

        public DMPara GetPara(int AxisId)
        {
            for (int i = 0; i < AxisPara.Count; i++)
            {
                if ((int)AxisPara[i].AxisIdx == AxisId)
                    return AxisPara[i];
            }
            return null;
        }
        public void CreateDefaultParameters()
        {
            AxisPara.Clear();
            foreach(Axislist axis in Enum.GetValues(typeof(Axislist)))
            {            
                DMPara myPara = new DMPara();
                myPara.AxisIdx = axis;
                AxisPara.Add(myPara);
            }
        }
        public int Init() 
        {
            short rc;
            short existcard = 0;                                   
            try
            {
                rc = CPCI_DMC.CS_DMC_01_open(ref existcard);

                if (existcard <= 0)
                {                                      
                    MessageBox.Show("No DMC-NET card can be found!");                    
                    return (int)DM_ErrorList.NoCardFound;  
                }
                else
                {
                    CardNo.Clear();
                    ushort i, card_no = 0, DeviceInfo = 0;;
                    uint[] SlaveTable = new uint[4];

                    for (i = 0; i < existcard; i++)
                    {
                        rc = CPCI_DMC.CS_DMC_01_get_CardNo_seq(i, ref card_no);
                        CardNo.Add(card_no) ;

                        rc = CPCI_DMC.CS_DMC_01_pci_initial(card_no);
                        if (rc != 0)
                            return (int)DM_ErrorList.BootFail;   
                     
                        rc = CPCI_DMC.CS_DMC_01_initial_bus(card_no);
                        if (rc != 0)
                            return (int)DM_ErrorList.CardInitFail;

                        for (i = 0; i < 4; i++)
                            SlaveTable[i] = 0;

                        rc = CPCI_DMC.CS_DMC_01_start_ring(card_no, 0);  
                        rc = CPCI_DMC.CS_DMC_01_get_device_table(card_no, ref DeviceInfo);
                        rc = CPCI_DMC.CS_DMC_01_get_node_table(card_no, ref SlaveTable[0]);

                        if (SlaveTable[0] == 0)
                            return (int)DM_ErrorList.SlaveIsEmpty;
                    }                   
                }
            }
            catch (Exception e)
            {
                return (int)DM_ErrorList.CardInitFail;
            }

            return (int)DM_ErrorList.NoError;
        }
        public void CloseCard()  //关闭运动控制卡
        {
            for (int i = 0; i < CardNo.Count; i++)
                CPCI_DMC.CS_DMC_01_reset_card(CardNo[i]);

            CPCI_DMC.CS_DMC_01_close();
        }

        public void SetServoOn(ushort NodeID, ushort svonON_OFF)
        {
            if (CardNo.Count ==0)
                return;
            CPCI_DMC.CS_DMC_01_ipo_set_svon(CardNo[0], NodeID, 0, svonON_OFF);   
        }
        private bool ReadServoDI(ushort NodeID, int idnum)  //idnum为4则对应原点位，5、6分别对应负限位和正限位
        {
            if (CardNo.Count == 0)
                return false;
            bool res = false;

            ushort servo_DI = 0;
            try
            {
                CPCI_DMC.CS_DMC_01_get_servo_DI(CardNo[0], NodeID, 0, ref servo_DI);
                if ((servo_DI & (0x01 << idnum)) != 0)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
            }
            catch
            {
                res = false;
            }
            if (GetPara(NodeID).IsServoMotor)
                return res;
            else
                return !res;
        }
        public int ResetAlarm(ushort NodeID)
        {
            if (CardNo.Count ==0)
                return (int)DM_ErrorList.NoCardFound; 
            short rc = CPCI_DMC.CS_DMC_01_set_ralm(CardNo[0], NodeID, 0);
            if (rc != 0)
            {
                return (int)DM_ErrorList.ResetAlmFail;
            }
            return (int)DM_ErrorList.NoError;
        }
        public uint GetAlarmStatus(ushort NodeID)
        {
            uint alm_code = 0;
            if (CardNo.Count == 0)
                return alm_code;
            CPCI_DMC.CS_DMC_01_get_alm_code(CardNo[0], NodeID, 0, ref alm_code);
            return alm_code;
        }
        public void SetPosition(ushort NodeID, int Pos)
        {
            if (CardNo.Count == 0)
                return;
            CPCI_DMC.CS_DMC_01_set_position(CardNo[0], NodeID, 0, Pos);
            CPCI_DMC.CS_DMC_01_set_command(CardNo[0], NodeID, 0, Pos);
        }
        object sny_Obj = new object();
        public void WriteIOOut(ushort port, bool sts)
        {
            if (CardNo.Count == 0)
                return;
            uint IOStatus = 0;

            lock (sny_Obj)
            {
                CPCI_DMC.CS_DMC_01_get_dio_output_DW(CardNo[0], ref IOStatus);
                //ushort IOOutStatus = 0;
                //if (sts)
                //{
                //    IOOutStatus = (ushort)(IOStatus | (ushort)Math.Pow(2, port));
                //}
                //else
                //{
                //    IOOutStatus = (ushort)(IOStatus & (0xFFFF - (ushort)Math.Pow(2, port)));
                //}

                BitArray myVal = new BitArray(new int[] { (int)IOStatus });
                myVal[port] = sts;
                Byte[] myB = new Byte[4];
                myVal.CopyTo(myB, 0);

                uint IOOutStatus = BitConverter.ToUInt32(myB, 0);

                CPCI_DMC.CS_DMC_01_set_dio_output_DW(CardNo[0], IOOutStatus);
            }
        }
        public void WriteIOOutRM32(ushort NodeID, ushort port, bool sts) //12, bit 0, 1
        {
            if (CardNo.Count == 0)
                return;
            ushort IOStatus = 0;

            //CPCI_DMC.CS_DMC_01_get_dio_output_DW(CardNo[0], ref IOStatus);
            CPCI_DMC.CS_DMC_01_get_rm_output_value(CardNo[0], NodeID, 0, 0, ref IOStatus);
            BitArray myVal = new BitArray(new int[] { (int)IOStatus });
            myVal[port] = sts;
            Byte[] myB= new Byte[4];
            myVal.CopyTo(myB,0);

            ushort IOOutStatus = BitConverter.ToUInt16(myB, 0);

            //CPCI_DMC.CS_DMC_01_set_dio_output_DW(CardNo[0], IOOutStatus);
            CPCI_DMC.CS_DMC_01_set_rm_output_value(CardNo[0], NodeID, 0, 0, IOOutStatus);
        }
        public bool ReadIOOutRM32(ushort NodeID, ushort port)
        {
            if (CardNo.Count == 0)
                return false;
            ushort IOStatus = 0;
            CPCI_DMC.CS_DMC_01_get_rm_output_value(CardNo[0], NodeID, 0, 0, ref IOStatus);
            return ((IOStatus & (0x01 << port)) != 0 ? true : false);
        }

        public bool ReadIOOut(ushort port)
        {
            if (CardNo.Count == 0)
                return false;
            uint IOStatus = 0;
            CPCI_DMC.CS_DMC_01_get_dio_output_DW(CardNo[0], ref IOStatus);            
            return ((IOStatus & (0x01 << port)) != 0 ? true : false);
        }
        public bool ReadIOIn(ushort port)
        {
            if (CardNo.Count == 0)
                return false;
            uint IOStatus = 0;
            try
            {
                CPCI_DMC.CS_DMC_01_get_dio_input_DW(CardNo[0], ref IOStatus);
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
        public bool NEL(ushort NodeID)
        {
            if (GetPara(NodeID).IsServoMotor)
                return ReadServoDI(NodeID, 5);
            else
                return ReadServoDI(NodeID, 6);
        }
        public bool PEL(ushort NodeID)
        {
            if (GetPara(NodeID).IsServoMotor)
                return ReadServoDI(NodeID, 6);
            else
                return ReadServoDI(NodeID, 5);
        }
        public bool Org(ushort NodeID)
        {
            return ReadServoDI(NodeID, 4);
        }
        public void ManHoming(ushort nodeid, ushort home_mode)
        {            
            ResetAlarm(nodeid);
            CPCI_DMC.CS_DMC_01_speed_continue(CardNo[0], nodeid, 0, (ushort)1);
            int speed = GetPara(nodeid).HomeSpeed;
            int StrVel = speed / 10;
            Int32 dist = (int)(GetPara(nodeid).MotorScale*0.005);//pulse
            if (dist < 5)
                dist = 5;
            //if (home_mode ==2)
            //{
                StepMove(nodeid, -10);
                WaitAxisStop(nodeid);
                CPCI_DMC.CS_DMC_01_speed_continue(CardNo[0], nodeid, 0, (ushort)0);
                do
                {
                    ResetAlarm(nodeid);
                    CPCI_DMC.CS_DMC_01_start_tr_move(CardNo[0], nodeid, 0, dist, StrVel, speed, 0.2, 0.2);
                    WaitAxisStop(nodeid);
                } while (NEL(nodeid));
            //}
            //else
            //{
            //     StepMove(nodeid, 10);
            //     WaitAxisStop(nodeid);
            //     CPCI_DMC.CS_DMC_01_speed_continue(CardNo[0], nodeid, 0, (ushort)1);
            //     do
            //     {
            //         ResetAlarm(nodeid);
            //         CPCI_DMC.CS_DMC_01_start_tr_move(CardNo[0], nodeid, 0, -dist, StrVel, speed, 0.2, 0.2);
            //         WaitAxisStop(nodeid);
            //     } while (PEL(nodeid));
            //}           
        }
        public void Homing(ushort nodeid, ushort home_mode)  //home_mode=1,以负限位为原点，2,为正限为原点，7以原点遇正限位反转
        {
            ResetAlarm(nodeid);
            if (CardNo.Count == 0)
                return;

            if (!GetPara(nodeid).IsServoMotor)
            {
                //ManHoming(nodeid, home_mode);
                double acc = 0.1;
                ushort StrVel = 100;
                CPCI_DMC.CS_DMC_01_set_home_config(CardNo[0], nodeid, 0, 17, 0, StrVel, (ushort)GetPara(nodeid).HomeSpeed, acc);
                CPCI_DMC.CS_DMC_01_set_home_move(CardNo[0], nodeid, 0);
                Thread.Sleep(motionCmdDly);
            }
            else
            {
                double acc = 0.1;
                ushort StrVel = 100;
                CPCI_DMC.CS_DMC_01_set_home_config(CardNo[0], nodeid, 0, home_mode, 0, StrVel, (ushort)GetPara(nodeid).HomeSpeed, acc);
                CPCI_DMC.CS_DMC_01_set_home_move(CardNo[0], nodeid, 0);
                Thread.Sleep(motionCmdDly);
            }
        }
        public int WaitHomeDone(ushort nodeid)
        {
            if (CardNo.Count == 0)
                return (int)DM_ErrorList.NoCardFound;
            ushort MC_done = 0;
            uint MC_status = 0;
            short rc;
            uint status1 = 0, status2 = 0, status3 = 0, status4 = 0;
            CPCI_DMC.CS_DMC_01_motion_done(CardNo[0], nodeid, 0, ref MC_done);

            rc = CPCI_DMC.CS_DMC_01_motion_status(CardNo[0], nodeid, 0, ref MC_status);
            status1 = ((MC_status >> 1) & 0X0001);
            status2 = ((MC_status >> 2) & 0X0001);
            status3 = ((MC_status >> 10) & 0X0001);
            status4 = ((MC_status >> 12) & 0X0001);
            DateTime st_time = DateTime.Now;
            TimeSpan time_span;

            while ((status1 != 1) || (status2 != 1) || (status3 != 1) || (status4 != 1) || (MC_done != 0))
            {
                time_span = DateTime.Now - st_time;
                if (time_span.TotalMilliseconds > HomingTimeoutMS)
                    return (int)DM_ErrorList.HomingTimeout;

                Application.DoEvents();
                Thread.Sleep(10);
                CPCI_DMC.CS_DMC_01_motion_status(CardNo[0], nodeid, 0, ref MC_status);
                status1 = ((MC_status >> 1) & 0X0001);
                status2 = ((MC_status >> 2) & 0X0001);
                status3 = ((MC_status >> 10) & 0X0001);
                status4 = ((MC_status >> 12) & 0X0001);
                CPCI_DMC.CS_DMC_01_motion_done(CardNo[0], nodeid, 0, ref MC_done);
            }
            return (int)DM_ErrorList.NoError;
        }
        public int WaitAxisStop(ushort nodeid)
        {
            if (CardNo.Count == 0)
                return (int)DM_ErrorList.NoCardFound;

            ushort MC_done = 0;
            short rc;

            rc = CPCI_DMC.CS_DMC_01_motion_done(CardNo[0], nodeid, 0, ref MC_done);
            DateTime st_time = DateTime.Now;
            TimeSpan time_span;
            while (MC_done != 0)
            {
                time_span = DateTime.Now - st_time;
                if (time_span.TotalMilliseconds > MotionTimeoutMS)
                    return (int)DM_ErrorList.MotionTimeout;

                Application.DoEvents();
                Thread.Sleep(10);
                CPCI_DMC.CS_DMC_01_motion_done(CardNo[0], nodeid, 0, ref MC_done);
            }
            return (int)DM_ErrorList.NoError;
        }
        public int MoveTo(ushort NodeID, double Position, int Profile)
        {
            ResetAlarm(NodeID);
            if (CardNo.Count == 0)
                return (int)DM_ErrorList.NoCardFound;

            double acc = 0.2;
            double Tdec = 0.2;
            int speed = GetPara(NodeID).RunSpeed;
            if (Profile == 0)
                speed = GetPara(NodeID).HomeSpeed;

            int StrVel = speed / 10;
            int dist;
            dist = (Int32)(GetPara(NodeID).MotorScale * Position);

            short rc = CPCI_DMC.CS_DMC_01_start_ta_move(CardNo[0], NodeID, 0, dist, StrVel, speed, acc, Tdec);
            if (rc != 0)
                return (int)DM_ErrorList.MotionError;

            Thread.Sleep(motionCmdDly);
            return (int)DM_ErrorList.NoError;
        }
        public int MoveTo(ushort NodeID, double Position)
        {
            ResetAlarm(NodeID);
            if (CardNo.Count == 0)
                return (int)DM_ErrorList.NoCardFound;

            double acc = 0.2;
            double Tdec = 0.2;
            int speed = GetPara(NodeID).RunSpeed;
            int StrVel = speed / 10;
            int dist;
            dist = (Int32)(GetPara(NodeID).MotorScale * Position);

            if ((NodeID == (ushort)Axislist.Mod1YAxis) || (NodeID == (ushort)Axislist.Mod2YAxis))
            {
                if (!ReadIOIn((ushort)InputIOlist.RotaryMotionDone))
                {
                    return (int)DM_ErrorList.MotionSafetyError;  
                }
            }

            short rc = CPCI_DMC.CS_DMC_01_start_ta_move(CardNo[0], NodeID, 0, dist, StrVel, speed, acc, Tdec);
            if (rc != 0)
                return (int)DM_ErrorList.MotionError;          

            Thread.Sleep(motionCmdDly);
            return (int)DM_ErrorList.NoError;
        }
        public int StepMove(ushort NodeID, double Dist)
        {
            ResetAlarm(NodeID);
            if (CardNo.Count == 0)
                return (int)DM_ErrorList.NoCardFound;
            double acc = 0.2;
            double Tdec = 0.2;
            int speed = GetPara(NodeID).RunSpeed;
            int StrVel = speed / 10;
            Int32 dist;
            dist = (Int32)(GetPara(NodeID).MotorScale * Dist);            
            short rc = CPCI_DMC.CS_DMC_01_start_tr_move(CardNo[0], NodeID, 0, dist, StrVel, speed, acc, Tdec);
            if (rc != 0)
                return (int)DM_ErrorList.MotionError;

            Thread.Sleep(motionCmdDly);
            return (int)DM_ErrorList.NoError;
        }
        public void StopMotion(ushort NodeID, int swichstop)//0为急停，1为减数停止
        {
            if (CardNo.Count == 0)
                return ;
            if (swichstop == 0)
            {
                CPCI_DMC.CS_DMC_01_emg_stop(CardNo[0], NodeID, 0);
            }
            else
            {
                CPCI_DMC.CS_DMC_01_sd_stop(CardNo[0], NodeID, 0, 0.1);
            }
        }
        public double GetCommandPos(ushort NodeID)
        {
            if (CardNo.Count == 0)
                return 0.0;
            int cmd = 0;
            double commdpos = 0.0;
            CPCI_DMC.CS_DMC_01_get_command(CardNo[0], NodeID, 0, ref cmd);
            commdpos = cmd / GetPara(NodeID).MotorScale;
            return commdpos;
        }
        public double GetPos(ushort NodeID)
        {
            if (CardNo.Count == 0)
                return 0.0;
            int pos = 0;
            double codepos = 0.0;
            if (!GetPara(NodeID).IsServoMotor)
            {
                CPCI_DMC.CS_DMC_01_get_command(CardNo[0], NodeID, 0, ref pos);
            }
            else
            {
                CPCI_DMC.CS_DMC_01_get_position(CardNo[0], NodeID, 0, ref pos);
            }
            codepos = (double)pos / GetPara(NodeID).MotorScale;
            return codepos;
        }

        public void ShowSettings()
        {
            WinMotionSettings myWin = new WinMotionSettings(this);
            myWin.ShowDialog();
        }
        public string GetErrorDescription(int ErrorCode)
        {
            string des = "No Error";

            DM_ErrorList myError = (DM_ErrorList)ErrorCode;
            des = Helper.GetEnumDescription(myError);

            return des;
        }
        public void writeLightIO(int port, bool sts)
        {
            //
            if (CardNo.Count == 0)
                return;
            ushort IOStatus = 0;
            CPCI_DMC.CS_DMC_01_get_rm_output_value(CardNo[0], 12, 0, 0, ref IOStatus);
            BitArray myVal = new BitArray(new int[] { (int)IOStatus });
            myVal[port] = sts;
            Byte[] myB = new Byte[4];
            myVal.CopyTo(myB, 0);
            ushort IOOutStatus = BitConverter.ToUInt16(myB, 0);
            CPCI_DMC.CS_DMC_01_set_rm_output_value(CardNo[0], 12, 0, 0, IOOutStatus);
            //
        }

        // for 4 unloading
        //public void writeLightIO4(int port, bool sts)
        //{
        //    //
        //    if (CardNo.Count == 0)
        //        return;
        //    ushort IOStatus = 0;
        //    CPCI_DMC.CS_DMC_01_get_rm_output_value(CardNo[0], 12, 0, 1, ref IOStatus);
        //    BitArray myVal = new BitArray(new int[] { (int)IOStatus });
        //    myVal[port] = sts;
        //    Byte[] myB = new Byte[4];
        //    myVal.CopyTo(myB, 0);
        //    ushort IOOutStatus = BitConverter.ToUInt16(myB, 0);
        //    CPCI_DMC.CS_DMC_01_set_rm_output_value(CardNo[0], 12, 0, 1, IOOutStatus);
        //    //
        //}

        public void InitRM32NT()
        {
            //
            CPCI_DMC.CS_DMC_01_set_rm_output_active(CardNo[0], 12, 0, 1);
        }

        public void resetRM32NT()
        {
            //
            if (CardNo.Count == 0)
                return;
            ushort IOStatus = 0;
            CPCI_DMC.CS_DMC_01_get_rm_output_value(CardNo[0], 12, 0, 0, ref IOStatus);
            BitArray myVal = new BitArray(new int[] { (int)IOStatus });
            for (int i = 0; i < 16;i++ )
                myVal[i] = false;
            Byte[] myB = new Byte[4];
            myVal.CopyTo(myB, 0);
            ushort IOOutStatus = BitConverter.ToUInt16(myB, 0);
            CPCI_DMC.CS_DMC_01_set_rm_output_value(CardNo[0], 12, 0, 0, IOOutStatus);
            
            //for 4 unloading
            //IOStatus = 0;
            //CPCI_DMC.CS_DMC_01_get_rm_output_value(CardNo[0], 12, 0, 1, ref IOStatus);
            //myVal = new BitArray(new int[] { (int)IOStatus });
            //for (int i = 0; i < 16; i++)
            //    myVal[i] = false;
            //myB = new Byte[4];
            //myVal.CopyTo(myB, 0);
            //IOOutStatus = BitConverter.ToUInt16(myB, 0);
            //CPCI_DMC.CS_DMC_01_set_rm_output_value(CardNo[0], 12, 0, 1, IOOutStatus);
        }
    }
}
