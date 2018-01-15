using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPTCG.Motion
{                
        public enum Axislist
        {
            [DescriptionAttribute("Module 1 X Axis")]
            Mod1XAxis =10,
            [DescriptionAttribute("Module 1 Y Axis")]
            Mod1YAxis =3,
            [DescriptionAttribute("Module 2 X Axis")]
            Mod2XAxis =9,
            [DescriptionAttribute("Module 2 Y Axis")]
            Mod2YAxis =1,
            [DescriptionAttribute("Module 1 Z Axis")]
            Mod1ZAxis = 11,
            [DescriptionAttribute("Module 2 Z Axis")]
            Mod2ZAxis = 8,
        }
                
        public enum InputIOlist
        {
            [DescriptionAttribute("Input Button Left")]
            BtnLeft = 0,
            [DescriptionAttribute("Input Button Right")]
            BtnRight = 1,

            [DescriptionAttribute("Stop Button")]
            BtnStop = 2,
            [DescriptionAttribute("Home Button")]
            BtnHome = 3,
            [DescriptionAttribute("Emergency Button")]
            BtnEMO = 4,
            [DescriptionAttribute("Safety Sensor")]
            SafetySensor = 5,
            [DescriptionAttribute("Door Sensor")]
            DoorSensor = 6,
            [DescriptionAttribute("Air Pressure")]
            AirPressure = 7,

            //Rotary
            [DescriptionAttribute("Rotary Origin")]
            RotaryOrigin = 8,
            [DescriptionAttribute("Rotary Height")]
            RotaryHeight = 9,
            [DescriptionAttribute("Rotary Motion Complete")]
            RotaryMotionDone = 10,
            [DescriptionAttribute("Rotary Error")]
            RotaryError = 11,

            // 4 module input
            [DescriptionAttribute("Input Button LeftFor4")]
            BtnLeftFor4 = 13,
            [DescriptionAttribute("Input Button RightFor4")]
            BtnRightFor4 = 14,
            //[DescriptionAttribute("Rotary Index 1 Vacuum 1 Sensor")]
            //RI1Vac1=0,
            //[DescriptionAttribute("Rotary Index 1 Vacuum 2 Sensor")]
            //RI1Vac2 = 1,
            //[DescriptionAttribute("Rotary Index 2 Vacuum 1 Sensor")]
            //RI2Vac1 = 2,
            //[DescriptionAttribute("Rotary Index 2 Vacuum 2 Sensor")]
            //RI2Vac2 = 3,
            //[DescriptionAttribute("Rotary Index 3 Vacuum 1 Sensor")]
            //RI3Vac1 = 4,
            //[DescriptionAttribute("Rotary Index 3 Vacuum 2 Sensor")]
            //RI3Vac2 = 5,
            //[DescriptionAttribute("Rotary Index 4 Vacuum 1 Sensor")]
            //RI4Vac1 = 6,
            //[DescriptionAttribute("Rotary Index 4 Vacuum 2 Sensor")]
            //RI4Vac2 = 7,
        }

        public enum OutputIOlist
        {
            [DescriptionAttribute("Green Light")]
            LampGreen = 0, //运行绿色指示灯
            [DescriptionAttribute("Amber Light")]
            LampAmber = 1,//运行黄色指示灯 
            [DescriptionAttribute("Red Light")]
            LampRed = 2,   //运行红色指示灯 
            [DescriptionAttribute("Buzzer")]
            Buzzer = 3,       //蜂鸣器

            [DescriptionAttribute("Rotary Index 1 Vacuum 1")]
            RI1Vac1 = 4,
            [DescriptionAttribute("Rotary Index 1 Vacuum 2")]
            RI1Vac2 = 5,//21,
            [DescriptionAttribute("Rotary Index 2 Vacuum 1")]
            RI2Vac1 = 6,//5,
            [DescriptionAttribute("Rotary Index 2 Vacuum 2")]
            RI2Vac2 = 7,//22,
            [DescriptionAttribute("Rotary Index 3 Vacuum 1")]
            RI3Vac1 = 8,//6,
            [DescriptionAttribute("Rotary Index 3 Vacuum 2")]
            RI3Vac2 = 9,
            [DescriptionAttribute("Rotary Index 4 Vacuum 1")]
            RI4Vac1 = 10,//7,
            [DescriptionAttribute("Rotary Index 4 Vacuum 2")]
            RI4Vac2 = 11,
            [DescriptionAttribute("Camera Light 1")]
            Cam1Light = 13,
            [DescriptionAttribute("Camera Light 2")]
            Cam2Light = 14,
            [DescriptionAttribute("Camera Light 3")]
            Cam3Light = 12,//14,
            [DescriptionAttribute("Camera Light 4")]
            Cam4Light = 15,
            [DescriptionAttribute("Spectrum Light Source")]
            SpectrumLS = 16,
            [DescriptionAttribute("Rotary Enabled")]
            RotaryEnabled = 18,
            [DescriptionAttribute("Rotary Index CW")]
            RotaryIndexCW = 19,
            [DescriptionAttribute("Rotary Home")]
            RotaryHome = 20,
            [DescriptionAttribute("Rotary Stop")]
            RotaryStop = 21,
            [DescriptionAttribute("Rotary Index CCW")]
            RotaryIndexCCW = 22,
            [DescriptionAttribute("Machine Light")]
            MchLight = 23,
        }

        public enum JogDir
        {
            LeftRight = 0,
            UpDown = 1,
        }
}
