using System;
using System.Text;
using System.Runtime.InteropServices;

namespace InstrumentSystems.CAS4
{
    public class CAS4DLL
    {
        public const string ModuleName = "CAS4.DLL";
        
		public const int ErrorNoError             = 0;
		public const int ErrorUnknown             = -1;
		public const int ErrorTimeoutRWSNoData    = -2;
		public const int ErrorInvalidDeviceType   = -3;
		public const int ErrorAcquisition         = -4;
		public const int ErrorAccuDataStream      = -5;
		public const int ErrorPrivilege           = -6;
		public const int ErrorFIFOOverflow        = -7;
		public const int ErrorTimeoutEOSScan      = -8; //ISA only
		public const int ErrorTimeoutEOSDummyScan = -9;
		public const int ErrorFifoFull            = -10; //USB
		public const int ErrorPixel1FinalCheck    = -11; //test for pixel1 on n+1. failed
		public const int ErrorCCDTemperatureFail  = -13;
		public const int ErrorAdrControl          = -14;
		public const int ErrorFloat               = -15; //floating point error
		public const int ErrorTriggerTimeout      = -16;
		public const int ErrorAbortWaitTrigger    = -17;
		public const int ErrorDarkArray           = -18;
		public const int ErrorNoCalibration       = -19;
		public const int ErrorInterfaceVersion    = -20;
		public const int ErrorCRI                 = -21;
		public const int ErrorNoMultiTrack        = -25;
		public const int ErrorInvalidTrack        = -26;
		public const int ErrorDetectPixel         = -31;
		public const int ErrorSelectParamSet      = -32;
		public const int ErrorI2CInit             = -35;
		public const int ErrorI2CBusy             = -36;
		public const int ErrorI2CNotAck           = -37;
		public const int ErrorI2CRelease          = -38;
		public const int ErrorI2CTimeOut          = -39;
		public const int ErrorI2CTransmission     = -40;
		public const int ErrorI2CController       = -41; 
		public const int ErrorDataNotAck          = -42;
		public const int ErrorNoExternalADC       = -52;
		public const int ErrorShutterPos          = -53;
		public const int ErrorFilterPos           = -54;
		public const int ErrorConfigSerialMismatch = -55;
		public const int ErrorCalibSerialMismatch = -56;
		public const int ErrorInvalidParameter    = -57;
		public const int ErrorGetFilterPos        = -58;
		public const int ErrorParamOutOfRange     = -59;
		public const int ErrorDeviceFileChecksum  = -60;
		public const int ErrorInvalidEEPromType   = -61;
		public const int ErrorDeviceFileTooLarge  = -62;
		public const int ErrorNoCommunication     = -63;
		public const int ErrorNoFilesOnIdentKey   = -64;
		
		public const int errCASOK                 = ErrorNoError;
		
		public const int errCASError              = -1000;
		public const int errCasNoConfig           = errCASError-3;
		public const int errCASDriverMissing      = errCASError-6; //driver problem
		public const int errCasDeviceNotFound     = errCASError-10; //invalid ADevice param

        //ErrorHandling Commands
        [DllImport(ModuleName)] 
        public static extern int casGetError(int ADevice);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casGetErrorMessage(int AError, StringBuilder ADest, int AMaxLen);
        
		//Device Handles and Interfaces
		public const int InterfaceISA         = 0;
		public const int InterfacePCI         = 1;
		public const int InterfaceTest        = 3;
		public const int InterfaceUSB         = 5;
		
        [DllImport(ModuleName)] 
        public static extern int casCreateDevice();
        [DllImport(ModuleName)] 
		public static extern int casCreateDeviceEx(int AInterfaceType, int AInterfaceOption);
        [DllImport(ModuleName)] 
		public static extern int casChangeDevice(int ADevice, int AInterfaceType, int AInterfaceOption);
        [DllImport(ModuleName)] 
		public static extern int casDoneDevice(int ADevice);
		
		public const int aoAssignDevice     = 0;
		public const int aoAssignParameters = 1;
		public const int aoAssignComplete   = 2;
		
        [DllImport(ModuleName)] 
        public static extern int casAssignDeviceEx(int ASourceDevice, int ADestDevice, int AOption);
		
        [DllImport(ModuleName)] 
        public static extern int casGetDeviceTypes();
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casGetDeviceTypeName(int AInterfaceType, StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName)] 
        public static extern int casGetDeviceTypeOptions(int AInterfaceType);
        [DllImport(ModuleName)] 
        public static extern int casGetDeviceTypeOption(int AInterfaceType, int AIndex);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casGetDeviceTypeOptionName(int AInterfaceType, int AInterfaceOptionIndex, StringBuilder Dest, int AMaxLen);

		//Initialization
		public const int InitOnce       = 0;
		public const int InitForced     = 1;
		public const int InitNoHardware = 2;
		
        [DllImport(ModuleName)] 
        public static extern int casInitialize(int ADevice, int Perform);
		
		//Instrument properties
		
		//AWhat parameter constants for DeviceParameter methods below
		public const int dpidIntTimeMin              = 101;
		public const int dpidIntTimeMax              = 102;
		public const int dpidDeadPixels              = 103;
		public const int dpidVisiblePixels           = 104;
		public const int dpidPixels                  = 105;
		public const int dpidParamSets               = 106;
		public const int dpidCurrentParamSet         = 107;
		public const int dpidADCRange                = 108;
		public const int dpidADCBits                 = 109;
		public const int dpidSerialNo                = 110;
		public const int dpidTOPSerial               = 111;
		public const int dpidTransmissionFileName    = 112;
		public const int dpidConfigFileName          = 113;
		public const int dpidCalibFileName           = 114;
		public const int dpidCalibrationUnit         = 115;
		public const int dpidAccessorySerial         = 116;
		public const int dpidTriggerCapabilities     = 118;
		public const int dpidAveragesMax             = 119;
		public const int dpidFilterType              = 120;
		public const int dpidRelSaturationMin        = 123;
		public const int dpidRelSaturationMax        = 124;
		public const int dpidInterfaceVersion        = 125;
		public const int dpidTriggerDelayTimeMax     = 126;
		public const int dpidSpectrometerName        = 127;
		public const int dpidNeedDarkCurrent         = 130;
		public const int dpidNeedDensityFilterChange = 131;
		public const int dpidSpectrometerModel       = 132;
		public const int dpidLine1FlipFlop           = 133;
		public const int dpidTimer                   = 134;
		public const int dpidInterfaceType           = 135;
		public const int dpidInterfaceOption         = 136;
		public const int dpidInitialized             = 137;
		public const int dpidDCRemeasureReasons      = 138;
		public const int dpidAbortWaitForTrigger     = 140;
		public const int dpidGetFilesFromDevice      = 142;
		public const int dpidTOPType                 = 143;
		public const int dpidTOPSerialEx             = 144;
		public const int dpidAutoRangeFilterMin      = 145;
		public const int dpidAutoRangeFilterMax      = 146;
		public const int dpidMultiTrackMaxCount      = 147;

		//dpidTriggerCapabilities TriggerCapabilities constants
		public const int tcoCanTrigger           = 0x0001;
		public const int tcoTriggerDelay         = 0x0002;
		public const int tcoTriggerOnlyWhenReady = 0x0004;
		public const int tcoAutoRangeTriggering  = 0x0008;
		public const int tcoShowBusyState        = 0x0010;
		public const int tcoShowACQState         = 0x0020;
		public const int tcoFlashOutput          = 0x0040;
		public const int tcoFlashHardware        = 0x0080;
		public const int tcoFlashForEachAverage  = 0x0100;
		public const int tcoFlashDelay           = 0x0200;
		public const int tcoFlashDelayNegative   = 0x0400;
		public const int tcoFlashSoftware        = 0x0800;
		public const int tcoGetFlipFlopState     = 0x1000;
		public const int tcoQueryHasData         = 0x2000;
		
		//DCRemeasureReasons constants; seedpidDCRemeasureReasons 
		public const int todcrrNeedDarkCurrent   = 0x0001;
		public const int todcrrCCDTemperature    = 0x0002;

                //TOPType constants; see dpidTOPType
		public const int ttNone       = 0;
		public const int ttTOP100     = 1;
		public const int ttTOP200     = 2;
		public const int ttTOP150     = 3;

        [DllImport(ModuleName)] 
        public static extern double casGetDeviceParameter(int ADevice, int AWhat);
        [DllImport(ModuleName)] 
        public static extern int casSetDeviceParameter(int ADevice, int AWhat, double AValue);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
		public static extern int casGetDeviceParameterString(int ADevice, int AWhat, StringBuilder ADest, int ADestSize);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
		public static extern int casSetDeviceParameterString(int ADevice, int AWhat, string AValue);
		
		public const int casSerialComplete  = 0;
		public const int casSerialAccessory = 1;
		public const int casSerialExtInfo   = 2;
		public const int casSerialDevice    = 3;
		public const int casSerialTOP       = 4;
		
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern int casGetSerialNumberEx(int ADevice, int AWhat, StringBuilder Dest, int AMaxLen);
		
		public const int coShutter                 = 0x00000001;
		public const int coFilter                  = 0x00000002;
		public const int coGetShutter              = 0x00000004;
		public const int coGetFilter               = 0x00000008;
		public const int coGetAccessories          = 0x00000010;
		public const int coGetTemperature          = 0x00000020;
		public const int coUseDarkcurrentArray     = 0x00000040;
		public const int coUseTransmission         = 0x00000080;
		public const int coAutorangeMeasurement    = 0x00000100;
		public const int coAutorangeFilter         = 0x00000200;
		public const int coCheckCalibConfigSerials = 0x00000400;
		public const int coTOPHasFieldOfViewConfig = 0x00000800;
		public const int coAutoRemeasureDC         = 0x00001000;
		public const int coCanMultiTrack           = 0x00008000;
		public const int coCanSwitchLEDOff         = 0x00010000;
		public const int coLEDOffWhileMeasuring    = 0x00020000;
		
        [DllImport(ModuleName)]
        public static extern int casGetOptions(int ADevice);
        [DllImport(ModuleName)]
		public static extern void casSetOptionsOnOff(int ADevice, int AOptions, int AOnOff);
        [DllImport(ModuleName)]
		public static extern void casSetOptions(int ADevice, int AOptions);
		
		// Measurement commands
        [DllImport(ModuleName)]
        public static extern int casMeasure(int ADevice);
		
        [DllImport(ModuleName)]
        public static extern int casStart(int ADevice);
        [DllImport(ModuleName)]
        public static extern int casFIFOHasData(int ADevice);
        [DllImport(ModuleName)]
		public static extern int casGetFIFOData(int ADevice);
		
        [DllImport(ModuleName)]
        public static extern int casMeasureDarkCurrent(int ADevice);
		
		public const int paPrepareMeasurement = 1;
		public const int paLoadCalibration    = 3;
		public const int paCheckAccessories   = 4;
		public const int paMultiTrackStart    = 5;
		
        [DllImport(ModuleName)]
        public static extern int casPerformAction(int ADevice, int AID);
		
		//Measurement Parameter
		
		//AWhat parameter constants for MeasurementParameter methods below
		public const int mpidIntegrationTime        = 01;
		public const int mpidAverages               = 02;
		public const int mpidTriggerDelayTime       = 03;
		public const int mpidTriggerTimeout         = 04;
		public const int mpidCheckStart             = 05;
		public const int mpidCheckStop              = 06;
		public const int mpidColormetricStart       = 07;
		public const int mpidColormetricStop        = 08;
		public const int mpidACQTime                = 10;
		public const int mpidMaxADCValue            = 11;
		public const int mpidMaxADCPixel            = 12;
		public const int mpidTriggerSource          = 14;
		public const int mpidAmpOffset              = 15;
		public const int mpidSkipLevel              = 16;
		public const int mpidSkipLevelEnabled       = 17;
		public const int mpidScanStartTime          = 18;
		public const int mpidAutoRangeMaxIntTime    = 19;
		public const int mpidAutoRangeLevel         = 20; //deprecated; use mpidAutoRangeMinLevel below
		public const int mpidAutoRangeMinLevel      = 20;
		public const int mpidDensityFilter          = 21;
		public const int mpidCurrentDensityFilter   = 22;
		public const int mpidNewDensityFilter       = 23;
		public const int mpidLastDCAge              = 24;
		public const int mpidRelSaturation          = 25;
		public const int mpidPulseWidth             = 27;
		public const int mpidRemeasureDCInterval    = 28;
		public const int mpidFlashDelayTime         = 29;
		public const int mpidTOPAperture            = 30;
		public const int mpidTOPDistance            = 31;
		public const int mpidTOPSpotSize            = 32;
		public const int mpidTriggerOptions         = 33;
		public const int mpidForceFilter            = 34;
		public const int mpidFlashType              = 35;
		public const int mpidFlashOptions           = 36;
		public const int mpidACQStateLine           = 37;
		public const int mpidACQStateLinePolarity   = 38;
		public const int mpidBusyStateLine          = 39;
		public const int mpidBusyStateLinePolarity  = 40;
		public const int mpidAutoFlowTime           = 41;
		public const int mpidCRIMode                = 42;
		public const int mpidObserver               = 43;
		public const int mpidTOPFieldOfView         = 44;
		public const int mpidCurrentCCDTemperature  = 46;
		public const int mpidLastCCDTemperature     = 47;
		public const int mpidDCCCDTemperature       = 48;
		public const int mpidAutoRangeMaxLevel      = 49;
		public const int mpidMultiTrackAcqTime      = 50;
		public const int mpidTimeSinceScanStart     = 51;
		public const int mpidCMTTrackStart          = 52;
		
		//mpidTriggerOptions constants
		public const int toAcceptOnlyWhenReady   =  1;
		public const int toForEachAutoRangeTrial =  2;
		public const int toShowBusyState         =  4;
		public const int toShowACQState          =  8;
		
		//mpidFlashType constants
		public const int ftNone     = 0;
		public const int ftHardware = 1;
		public const int ftSoftware = 2;
		
		//mpidFlashOptions constants
		public const int foEveryAverage     = 1;
		
		//mpidTriggerSource
		public const int trgSoftware = 0;
		public const int trgFlipFlop = 3;
		
		//mpidCRIMode
		public const int criDIN6169    = 0;
		public const int criCIE13_3_95 = 1;
		
		//mpidObserver
		public const int cieObserver1931 = 0;
		public const int cieObserver1964 = 1;
		
        [DllImport(ModuleName)]
        public static extern double casGetMeasurementParameter(int ADevice, int AWhat);
        [DllImport(ModuleName)]
        public static extern int casSetMeasurementParameter(int ADevice, int AWhat, double AValue);
        [DllImport(ModuleName)]
        public static extern int casClearDarkCurrent(int ADevice);
        [DllImport(ModuleName)]
        public static extern int casDeleteParamSet(int ADevice, int AParamSet);
		
		//Filter and Shutter commands
		public const int casShutterInvalid = -1;
		public const int casShutterOpen    = 0;
		public const int casShutterClose   = 1;
		
        [DllImport(ModuleName)]
        public static extern int casGetShutter(int ADevice);
        [DllImport(ModuleName)]
        public static extern void casSetShutter(int ADevice, int OnOff);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casGetFilterName(int ADevice, int AFilter, StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName)]
        public static extern int casGetDigitalOut(int ADevice, int APort);
        [DllImport(ModuleName)]
        public static extern void casSetDigitalOut(int ADevice, int APort, int OnOff);
        [DllImport(ModuleName)]
        public static extern int casGetDigitalIn(int ADevice, int APort);
		
		//Calibration and Configuration Commands
        [DllImport(ModuleName)]
        public static extern void casCalculateCorrectedData(int ADevice);
        [DllImport(ModuleName)]
        public static extern void casConvoluteTransmission(int ADevice);
		
		public const int gcfDensityFunction       = 0;
		public const int gcfSensitivityFunction   = 1;
		public const int gcfTransmissionFunction  = 2;
		public const int gcfDensityFactor         = 3;
		public const int gcfTOPApertureFactor     = 4;
		public const int gcfTOPDistanceFactor     = 5;
			public const int gcfTDCount         = -1;
			public const int gcfTDExtraDistance = 1;
			public const int gcfTDExtraFactor   = 2;
		public const int gcfWLCalibrationChannel  = 6;
			public const int gcfWLCalibPointCount           = -1;
			public const int gcfWLExtraCalibrationDelete    = 1;
			public const int gcfWLExtraCalibrationDeleteAll = 2;
		public const int gcfWLCalibrationAlias    = 7;
		public const int gcfWLCalibrationSave     = 8; 
		public const int gcfDarkArrayValues       = 9;
			public const int gcfDarkArrayDepth   = -1;  //Extra
			public const int gcfDarkArrayIntTime = -2;  //Extra
		public const int gcfTOPParameter          = 11;
		public const int gcfTOPApertureSize         = 0; //Extra
		public const int gcfTOPSpotSizeDenominator  = 1;
		public const int gcfTOPSpotSizeOffset       = 2;
		public const int gcfLinearityFunction     = 12;
			public const int gcfLinearityCounts = 0;
			public const int gcfLinearityFactor = 1;
		public const int  gcfRawData              = 14;
		
        [DllImport(ModuleName)]
        public static extern double casGetCalibrationFactors(int ADevice, int What, int Index, int Extra);
        [DllImport(ModuleName)]
        public static extern void casSetCalibrationFactors(int ADevice, int What, int Index, int Extra, double Value);
        [DllImport(ModuleName)]
        public static extern void casUpdateCalibrations(int ADevice);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern void casSaveCalibration(int ADevice, string AFileName);
        [DllImport(ModuleName)]
        public static extern void casClearCalibration(int ADevice, int What);
		
		//Measurement Results
        [DllImport(ModuleName)]
        public static extern double casGetData(int ADevice, int AIndex);
        [DllImport(ModuleName)]
        public static extern double casGetXArray(int ADevice, int AIndex);
        [DllImport(ModuleName)]
        public static extern double casGetDarkCurrent(int ADevice, int AIndex);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern void casGetPhotInt(int ADevice, out double APhotInt, StringBuilder AUnit, int AUnitLen);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern void casGetRadInt(int ADevice, out double ARadInt, StringBuilder AUnit, int AUnitLen);
        [DllImport(ModuleName)]
        public static extern double casGetCentroid(int ADevice);
        [DllImport(ModuleName)]
        public static extern void casGetPeak(int ADevice, out double x, out double y);
        [DllImport(ModuleName)]
        public static extern double casGetWidth(int ADevice);
		
		public const int cLambdaWidth       = 0;
		public const int cLambdaLow         = 1;
		public const int cLambdaMiddle      = 2;
		public const int cLambdaHigh        = 3;
		public const int cLambdaOuterWidth  = 4;
		public const int cLambdaOuterLow    = 5;
		public const int cLambdaOuterMiddle = 6;
		public const int cLambdaOuterHigh   = 7;
		
        [DllImport(ModuleName)]
        public static extern double casGetWidthEx(int ADevice, int What);
        [DllImport(ModuleName)]
        public static extern void casGetColorCoordinates(int ADevice, ref double x, ref double y, ref double z, ref double u, ref double v1976, ref double v1960);
        [DllImport(ModuleName)]
        public static extern double casGetCCT(int ADevice);
        [DllImport(ModuleName)]
        public static extern double casGetCRI(int ADevice, int Index);
        [DllImport(ModuleName)]
        public static extern void casGetTriStimulus(int ADevice, ref double X, ref double Y, ref double Z);
		
		public const int ecvVisualEffect = 2;
		public const int ecvUVA          = 3;
		public const int ecvUVB          = 4;
		public const int ecvUVC          = 5;
		public const int ecvVIS          = 6;
		public const int ecvCRICCT       = 7;
		public const int ecvCDI          = 8;
		public const int ecvDistance     = 9;
		public const int ecvCalibMin     = 10;
		public const int ecvCalibMax     = 11;
		public const int ecvScotopicInt  = 12;

		public const int ecvCRIFirst          = 100;
		public const int ecvCRILast           = 116;
		public const int ecvCRITriKXFirst     = 120;
		public const int ecvCRITriKXLast      = 136;
		public const int ecvCRITriKYFirst     = 140;
		public const int ecvCRITriKYLast      = 156;
		public const int ecvCRITriKZFirst     = 160;
		public const int ecvCRITriKZLast      = 176;
		public const int ecvCRITriRXordUFirst = 180;
		public const int ecvCRITriRXordULast  = 196;
		public const int ecvCRITriRYordVFirst = 200;
		public const int ecvCRITriRYordVLast  = 216;
		public const int ecvCRITriRZordWFirst = 220;
		public const int ecvCRITriRZordWLast  = 236;
		
        [DllImport(ModuleName)]
        public static extern double casGetExtendedColorValues(int ADevice, int What);
		
		//Colormetric Calculation
        [DllImport(ModuleName)]
        public static extern int casColorMetric(int ADevice);
        [DllImport(ModuleName)]
        public static extern int casCalculateCRI(int ADevice);
        [DllImport(ModuleName)]
        public static extern int cmXYToDominantWavelength(double x, double y, double IllX, double IllY, ref double LambdaDom, ref double Purity);
		
		//Utilities
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casGetDLLFileName(StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casGetDLLVersionNumber(StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casSaveSpectrum(int ADevice, string AFileName);
        [DllImport(ModuleName)]
        public static extern double casGetExternalADCValue(int ADevice, int AIndex);
		
		public const int extNoError       = 0;
		public const int extExternalError = 1;
		public const int extFilterBlink   = 2;
		public const int extShutterBlink  = 4;
		
        [DllImport(ModuleName)]
        public static extern void casSetStatusLED(int ADevice, int AWhat);
        [DllImport(ModuleName)]
        public static extern int casNmToPixel(int ADevice, double nm);
        [DllImport(ModuleName)]
        public static extern double casPixelToNm(int ADevice, int APixel);
        [DllImport(ModuleName)]
        public static extern int casCalculateTOPParameter(int ADevice, int AAperture, double ADistance, ref double ASpotSize, ref double AFieldOfView);
		
		//MultiTrack
        [DllImport(ModuleName)]
        public static extern int casMultiTrackInit(int ADevice, int ATracks);
        [DllImport(ModuleName)]
        public static extern int casMultiTrackDone(int ADevice);
        [DllImport(ModuleName)]
        public static extern int casMultiTrackCount(int ADevice);
        [DllImport(ModuleName)]
        public static extern int casMultiTrackCopyData(int ADevice, int ATrack);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casMultiTrackSaveData(int ADevice, string AFileName);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern int casMultiTrackLoadData(int ADevice, string AFileName);
		
		//Spectrum Manipulation
        [DllImport(ModuleName)]
        public static extern void casSetData(int ADevice, int AIndex, double Value);
        [DllImport(ModuleName)]
        public static extern void casSetXArray(int ADevice, int AIndex, double Value);
        [DllImport(ModuleName)]
        public static extern void casSetDarkCurrent(int ADevice, int AIndex, double Value);
        [DllImport(ModuleName)]
        public static extern IntPtr casGetDataPtr(int ADevice);
        [DllImport(ModuleName)]
        public static extern IntPtr casGetXPtr(int ADevice);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true)] 
        public static extern void casLoadTestData(int ADevice, string AFileName);

        //deprecated methods!!
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetInitialized(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetDeviceType(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetDeviceOption(int ADevice );
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetAdcBits(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetAdcRange(int ADevice);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetSerialNumber(int ADevice, StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetDeadPixels(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetVisiblePixels(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetPixels(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetModel(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern double casGetAmpOffset(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetIntTimeMin(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetIntTimeMax(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casBackgroundMeasure(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetIntegrationTime(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetIntegrationTime(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetAccumulations(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetAccumulations(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern double casGetAutoIntegrationLevel(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetAutoIntegrationLevel(int ADevice, double ALevel);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetAutoIntegrationTimeMax(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetAutoIntegrationTimeMax(int ADevice, int AMaxTime);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casClearBackground(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetNeedBackground(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetNeedBackground(int ADevice, int AValue);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetTop100(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetTop100(int ADevice, int AIndex);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern double casGetTop100Distance(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetTop100Distance(int ADevice, double ADistance);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetFilter(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetFilter(int ADevice, int AFilter);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetActualFilter(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetNewDensityFilter(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetNewDensityFilter(int ADevice, int AFilter);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetForceFilter(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetForceFilter(int ADevice, int AForce);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetParamSets(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetParamSets(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetParamSet(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetParamSet(int ADevice, int Value);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetCalibrationFileName(int ADevice, StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetCalibrationFileName(int ADevice, string Value);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetConfigFileName(int ADevice, StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetConfigFileName(int ADevice, string Value);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetTransmissionFileName(int ADevice, StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetTransmissionFileName(int ADevice, string Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casValidateConfigAndCalibFile(int ADevice);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetCalibrationUnit(int ADevice, StringBuilder Dest, int AMaxLen);
        [DllImport(ModuleName, CharSet = CharSet.Ansi, ExactSpelling = true), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetCalibrationUnit(int ADevice, string Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern double casGetBackground(int ADevice, int AIndex);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetBackground(int ADevice, int AIndex, double Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetMaxAdcValue(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetCheckStart(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetCheckStart(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetCheckStop(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetCheckStop(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern double casGetColormetricStart(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetColormetricStart(int ADevice, double Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern double casGetColormetricStop(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetColormetricStop(int ADevice, double Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
        public static extern int casGetObserver();
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetObserver(int AObserver);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern double casGetSkipLevel(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetSkipLevel(int ADevice, double ASkipLevel);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetSkipLevelEnabled(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetSkipLevelEnabled(int ADevice, int ASkipLevel);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetTriggerSource(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetTriggerSource(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetLine1FlipFlop(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetLine1FlipFlop(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetTimeout(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetTimeout(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetFlash(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetFlash(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetFlashDelayTime(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetFlashDelayTime(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetFlashOptions(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetFlashOptions(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetDelayTime(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern void casSetDelayTime(int ADevice, int Value);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetStartTime(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casGetACQTime(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
		public static extern int casReadWatch(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
        public static extern int casStopTime(int ADevice, int ARefTime);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
        public static extern void casMultiTrackCopySet(int ADevice);
        [DllImport(ModuleName), ObsoleteAttribute("method obsolete!")]
        public static extern int casMultiTrackReadData(int ADevice, int ATrack);
    }
}
