using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPTCG.Motion
{
    public interface BaseMotion
    {
        int OpenCard();                                               //打开运动控制卡函数
        void VReSetAlm(ushort CardNo, ushort NodeID);                  //清除报警信息
        void VCloseCard();                                              //关闭运动控制卡函数
        void VSetCard(ushort CardNo, ushort NodeID,ushort svonON_OFF);   //设置卡、轴参数
        void VSetAxisposition(ushort CardNo, ushort NodeID);             //设置轴位置
        void VWriteIOout(ushort CardNo, ushort port, bool sts);          //设置某个IO为真或者假函数
        void VWriteOutPut(ushort port1, ushort port2);                   //设置气缸函数
         bool BReadIOOutPut(ushort CardNo, ushort port);                 //读取输出IO状态
        bool BReadIOInPut(ushort CardNo, ushort port);                                 //读取输入IO状态
        void VGohome(ushort CardNo, ushort nodeid, ushort home_mode,ushort speed);                                     //轴回原点函数
        void VGohomeLimiter(ushort axis, ushort dir);                   //以限位为原点的轴回原点函数
        void VWaitAxisStop(ushort CardNo, ushort nodeid);                                //等待轴停止运动函数
        void VMoveTo(ushort CardNo, ushort NodeID, double dis, int speed); //为绝对运动函数
        void VStepMove(ushort CardNo, ushort NodeID, double dis, int speed);   //单步运动（相对运动）函数
        void VStopAxis(ushort CardNo, ushort NodeID, int swichstop);    //轴立即停止函数,switchstop为选择停止的方式，0为急停，1为减数停止
        double DReadcurrentpulsePos(ushort CardNo, ushort NodeID);                        //读取轴当前脉冲位置函数
        double DReadcurrentencodePos(ushort CardNo, ushort NodeID);                       //读取轴当前编码器反馈位置函数
        void VContinuMove(ushort axis, ushort dir);                      //轴连续运动函数
        void VContinuMove(ushort axis, ushort dir, double v, double acc);//轴连续运动函数 
        bool BReadServoDI(ushort CardNo, ushort NodeID, int idnum);                                  //获取驱动器的原点、正负限位信号

    }
}
