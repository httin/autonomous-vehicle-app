using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ThesisInterface
{
    public partial class Form1 : Form
    {
        public class Vehicle
        {
            public double M1RefVelocity, M2RefVelocity, M1Velocity, M2Velocity, RefAngle, Angle, Test;

            public Vehicle(string[] ArrayInfo)
            {
                try
                {
                    M1RefVelocity = double.Parse(ArrayInfo[2], System.Globalization.CultureInfo.InvariantCulture);
                    M2RefVelocity = double.Parse(ArrayInfo[3], System.Globalization.CultureInfo.InvariantCulture);
                    M1Velocity = double.Parse(ArrayInfo[4], System.Globalization.CultureInfo.InvariantCulture);
                    M2Velocity = double.Parse(ArrayInfo[5], System.Globalization.CultureInfo.InvariantCulture);
                    RefAngle = double.Parse(ArrayInfo[6], System.Globalization.CultureInfo.InvariantCulture);
                    Angle = double.Parse(ArrayInfo[7], System.Globalization.CultureInfo.InvariantCulture);
                    Test = double.Parse(ArrayInfo[8], System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Vehicle::Vehicle(string[] )", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public string GetVehicleStatus()
            {
                return "M1 Velocity Ref: " + M1RefVelocity.ToString() + "\r\n"
                        + "M1 Velocity: " + M1Velocity.ToString() + "\r\n"
                        + "M2 Velocity Ref: " + M2RefVelocity.ToString() + "\r\n"
                        + "M2 Velocity: " + M2Velocity.ToString() + "\r\n"
                        + "Set Angle: " + RefAngle.ToString() + "\r\n"
                        + "Current Angle: " + Angle.ToString() + "\r\n"
                        + "Obstacle angle: " + Test.ToString() + "\r\n";
            }
        }

        public class GPS
        {
            public string GPS_Available, GPS_Mode;
            public double GPS_Lat, GPS_Lng;
            public GPS(string[] ArrayInfo)
            {
                try
                {
                    GPS_Available = ArrayInfo[2];
                    GPS_Lat = double.Parse(ArrayInfo[4], System.Globalization.CultureInfo.InvariantCulture);
                    GPS_Lng = double.Parse(ArrayInfo[5], System.Globalization.CultureInfo.InvariantCulture);
                    switch (ArrayInfo[3])
                    {
                        case "0":
                            GPS_Mode = "GPS Not Available";
                            break;
                        case "1":
                            GPS_Mode = "GPS Available";
                            break;
                        case "2":
                            GPS_Mode = "DGNSS";
                            break;
                        case "4":
                            GPS_Mode = "Fixed";
                            break;
                        case "5":
                            GPS_Mode = "Float";
                            break;
                        default:
                            GPS_Mode = "Parsed Error";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public string GetGPSStatus()
            {
                return (GPS_Available.Contains("Y")) ?
                    "GPS Mode: " + GPS_Mode + "\r\nPosition: " + GPS_Lat.ToString() + ", " + GPS_Lng.ToString() + "\r\n" :
                    "GPS is not available\r\n"; 
            }
        }

        public class StanleyControl
        {
            public double thetaE, thetaD, Delta, ErrorDistance, Efa;
            
            public StanleyControl(string []ArrayInfo)
            {
                try
                {
                    thetaE = double.Parse(ArrayInfo[2], System.Globalization.CultureInfo.InvariantCulture);
                    thetaD = double.Parse(ArrayInfo[3], System.Globalization.CultureInfo.InvariantCulture);
                    Delta = double.Parse(ArrayInfo[4], System.Globalization.CultureInfo.InvariantCulture);
                    ErrorDistance = double.Parse(ArrayInfo[5], System.Globalization.CultureInfo.InvariantCulture);
                    Efa = double.Parse(ArrayInfo[6], System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "StanleyControl::StanleyControl(string[] )", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public string GetStanleyControlStatus()
            {
                return "Theta E: " + thetaE.ToString() + "\r\nTheta D: "
                                + thetaD.ToString() + "\r\nDelta Angle: "
                                + Delta.ToString() + "\r\nDistance Error: "
                                + ErrorDistance.ToString() + "\r\n"; ;
            }
        }

        public class UserControlStatus
        {
            public bool KCTRL_START, KCTRL_STOP;
            public bool VEHCF_DATA_ON, VEHCF_DATA_OFF;
            public bool AUCON_START, AUCON_STOP;
            public bool AUCON_RUN, AUCON_PAUSE;
            public bool AUCON_DATA;
            public bool SFRST;
            public bool VPLAN_FLAG; // true if START sending map, otherwise false
            public UserControlStatus() { }

            public void ResetAllBits()
            {
                KCTRL_START = KCTRL_STOP = VEHCF_DATA_ON = VEHCF_DATA_OFF = AUCON_START = AUCON_STOP =
                    AUCON_RUN = AUCON_PAUSE = AUCON_DATA = SFRST = VPLAN_FLAG = false; 
            }
        }

        Vehicle MyVehicle;
        GPS MyGPS;
        StanleyControl MyStanleyControl;
        UserControlStatus MyStatus = new UserControlStatus();

        public class PlannedCoordinate
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class ActualCoordinate
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class CoordinatesInfo
        {
            public List<PlannedCoordinate> plannedCoordinates { get; set; }
            public List<ActualCoordinate> actualCoordinates { get; set; }
            public List<double> ErrorDistances { get; set; }
            public List<double> Efa { get; set; }
        }

        public class ProcessedMap
        {
            public List<double> lat { get; set; }
            public List<double> lng { get; set; }
            public List<double> x { get; set; }
            public List<double> y { get; set; }
        }

        // SET DEFAULT IMAGE FOR VELOCITY OF VEHICLE, CONSIST OF: RED, ORANGE, YELLOW, GREEN
        Image ZeroVelocity = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\ZERO.png");
        Image LowVelocity = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\LOW.png");
        Image MediumVelocity = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\MEDIUM.png");
        Image HighVelocity = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\HIGH.png");

        public string Vehicle_Information = "", GPS_Information = "", StanleyControl_Information = "";

        //---------------------------------------------------------------------------------------------------------------
        private bool AutoSetting = false;
        private bool OnHelperPanel = false; //flag to on/off helper panel

        public int setting_config_timeout = 40; // for setting config timer
        public int IMU_config_timeout = 40; // for imu config timer

        public string WaitKey = "|";  // This is used for creating waiting messages
        public string OldMess = "";

        private string PrevMode;
        private string serial_command;

        private List<PointLatLng> DistanceCalculate = new List<PointLatLng>();

        private int numOfCalculatePoints = 0;

        GMapOverlay DistanceOverlay = new GMapOverlay("Distance");

        GMapOverlay DistanceMarkers = new GMapOverlay("DSM");

        public bool PlanMapEnable = false, online = false;

        public List<PointLatLng> PlanCoordinatesList = new List<PointLatLng>(); 
        public List<PointLatLng> ActualCoordinatesList = new List<PointLatLng>();

        public List<double> DistanceErrors = new List<double>();
        public List<double> Efa = new List<double>();

        public GMapOverlay PlanLines = new GMapOverlay("PlanLines");
        public GMapOverlay ActualLines = new GMapOverlay("ActualLines");
        
        private int kctrl_timeout = 20;
        private bool KctrlEnabled = false;
        private bool ManualEnabled = false;
        private bool AutoEnable = true;
        private bool SerialPortEnable = false;

        private bool WRONG_CKSUM_FLAG = false;
        private bool VEHICLE_RECEIVED_DATA_FLAG = false; // show that our vehicle has been received "correct data" already
        private bool VEHICLE_RECEIVED_ERROR_FLAG = false;

        BackgroundWorker TransferMapBackGroundWorker;
        BackgroundWorker PreprocessBackGroundWorker;
        // This will get the exactly current directort of executing file (i.e. \bin\Debug)
        string workingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        public Form1()
        {
            InitializeComponent();

            SidePanel.Width = 0;
            InitAutoUC();       // Initialize 'Auto'
            InitManualUC();     // Initialize 'Manual'
            InitSettingUC();    // Initialize 'Setting'
            this.vehicleSetting1.BringToFront();
            LinkToUCEvents();   // Link Event Handlers to function
            MyStatus.ResetAllBits();
            DisableAllTimers();
            /*-----------------*/
            KeyPreview = true;
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            KeyUp += new KeyEventHandler(Form1_KeyUp);

            TransferMapBackGroundWorker = new BackgroundWorker(); // New thread handler for transferring map
            TransferMapBackGroundWorker.WorkerReportsProgress = true;
            TransferMapBackGroundWorker.WorkerSupportsCancellation = true;
            TransferMapBackGroundWorker.DoWork += TransferMapBackGroundWorker_DoWork; // Function to handle
            TransferMapBackGroundWorker.ProgressChanged += TransferMapBackGroundWorker_ProgressChanged; // Update progress
            TransferMapBackGroundWorker.RunWorkerCompleted += TransferMapBackGroundWorker_RunWorkerCompleted; // Function when thread complete

            PreprocessBackGroundWorker = new BackgroundWorker(); // New thread handler for pre-processing map
            PreprocessBackGroundWorker.WorkerReportsProgress = true;
            PreprocessBackGroundWorker.WorkerSupportsCancellation = true;
            PreprocessBackGroundWorker.DoWork += PreprocessBackGroundWorker_DoWork; // Function to handle
            PreprocessBackGroundWorker.ProgressChanged += PreprocessBackGroundWorker_ProgressChanged;
            PreprocessBackGroundWorker.RunWorkerCompleted += PreprocessBackGroundWorker_RunWorkerCompleted; // Function when thread complete
        }

        private void PreprocessBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (PlanCoordinatesList.Count > 0)
                {
                    ProcessedMap processedMap = new ProcessedMap();
                    List<double> listLat = new List<double>();
                    List<double> listLng = new List<double>();

                    for (int i = 0; i < PlanCoordinatesList.Count; ++i)
                    {

                        listLat.Add(PlanCoordinatesList[i].Lat);
                        listLng.Add(PlanCoordinatesList[i].Lng);
                    }
                    processedMap.lat = listLat;
                    processedMap.lng = listLng;

                    File.WriteAllText(workingDirectory + "\\map\\pre_map.txt", JsonConvert.SerializeObject(processedMap, Formatting.Indented));
                    MessageBox.Show(workingDirectory + "\\map\\pre_map.txt is created", "Map pre-processing",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    goto handle_premap;
                }
                else if (File.Exists(workingDirectory + @"\map\pre_map.txt"))
                {
                    goto handle_premap;
                }

                MessageBox.Show("Can't find " + workingDirectory + "\\map\\pre_map.txt");
                e.Cancel = true;
                return;

            handle_premap:
                int exit_code;
                ProcessStartInfo _processInfo;

                _processInfo = new ProcessStartInfo(workingDirectory + @"\map\process_map.bat");
                    //@"C:\Users\ThanhHien\Desktop\Tin\ThesisInterface\ThesisInterface\bin\Debug\map\process_map.bat");

                _processInfo.WorkingDirectory = string.Format(workingDirectory + @"\map\");
                    //@"C:\Users\ThanhHien\Desktop\Tin\ThesisInterface\ThesisInterface\bin\Debug\map\");

                _processInfo.CreateNoWindow = false; //Process started creating a new window
                _processInfo.UseShellExecute = false; //Do not use shell execution
                //Redirects error and output of the process (command prompt).
                _processInfo.RedirectStandardError = true;
                _processInfo.RedirectStandardOutput = true;

                //start a new process
                using (Process process = Process.Start(_processInfo))
                {
                    process.WaitForExit(); //wait indefinitely for the associated process to exit
                    //reads output and error of command prompt to string.
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    exit_code = process.ExitCode;
#if DEBUG
                    Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
                    Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
                    Console.WriteLine("ExitCode: " + exit_code.ToString(), "ExecuteCommand");
                    MessageBox.Show(workingDirectory + @"\map\map_out.txt is created", "Map pre-processing", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
                    process.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Map pre-processing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreprocessBackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) { }

        private void PreprocessBackGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                Console.WriteLine("pre-processing: task cancelled");
            else if (e.Error != null)
                Console.WriteLine("pre-processing: error while performing background operation");
            else // Everything completed normally
                Console.WriteLine("close PreprocessBackGroundWorker_DoWork thread successfully"); 
        }

        private void TransferMapBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TransferMapBackGroundWorker.ReportProgress(0, "START"); 
            string text = File.ReadAllText(workingDirectory + "\\map\\map_out.txt");

            TransferMapBackGroundWorker.ReportProgress(0, "PARSE");
            ProcessedMap processedMap = JsonConvert.DeserializeObject<ProcessedMap>(text); 

            ClearPLannedData(); // clear the PlanCoordinates List and the corresponding Plan line
            //ClearActualData();
            for (int i = 0; i < processedMap.lat.Count(); i++)
                PlanCoordinatesList.Add(new PointLatLng(processedMap.lat[i], processedMap.lng[i]));
            
            DisplayRouteOnMap(
                autoUC1.gmap, 
                new GMapRoute(PlanCoordinatesList, "single_line") { Stroke = new Pen(Color.LightGoldenrodYellow, 3) },
                "Planned");

            TransferMapBackGroundWorker.ReportProgress(0, "SEND");
            if (AutoEnable)
            {
                int report_value = 0, repeat;
                WRONG_CKSUM_FLAG = VEHICLE_RECEIVED_ERROR_FLAG = VEHICLE_RECEIVED_DATA_FLAG = false;
                
            VPLAN_START:
                repeat = 300000000;
                serialPort1.Write(MessagesDocker("VPLAN,SPLINE," + processedMap.x.Count().ToString()));
                while (!MyStatus.VPLAN_FLAG) // waiting for vehicle ready for receiving data
                {
                    if (WRONG_CKSUM_FLAG || VEHICLE_RECEIVED_ERROR_FLAG || (--repeat == 0))
                    {
                        WRONG_CKSUM_FLAG = VEHICLE_RECEIVED_ERROR_FLAG = false;
                        goto VPLAN_START;
                    }
                }

                for (int i = 0; (i < processedMap.x.Count) && !TransferMapBackGroundWorker.CancellationPending; i++)
                {
                    if (report_value - (int)(100 * i / processedMap.x.Count) != 0)
                    {
                        report_value = (int)(100 * i / processedMap.x.Count);
                        TransferMapBackGroundWorker.ReportProgress(report_value);
                    }
            VPLAN_DATA:
                    repeat = 300000000;
                    serialPort1.Write(
                        MessagesDocker("VPLAN," + i.ToString() + "," + processedMap.x[i].ToString() + "," + processedMap.y[i].ToString()));
                    while (!VEHICLE_RECEIVED_DATA_FLAG && MyStatus.VPLAN_FLAG)
                    {
                        if (WRONG_CKSUM_FLAG || VEHICLE_RECEIVED_ERROR_FLAG || (--repeat == 0))
                        {
                            WRONG_CKSUM_FLAG = VEHICLE_RECEIVED_ERROR_FLAG = false;
                            goto VPLAN_DATA;
                        }
                    }
                    VEHICLE_RECEIVED_DATA_FLAG = false;
                }
            VPLAN_STOP:
                repeat = 300000000;
                serialPort1.Write(MessagesDocker("VPLAN,STOP"));
                while (MyStatus.VPLAN_FLAG) // wating for vehicle finishing receive data
                {
                    if (WRONG_CKSUM_FLAG || VEHICLE_RECEIVED_ERROR_FLAG || (--repeat == 0))
                    {
                        WRONG_CKSUM_FLAG = VEHICLE_RECEIVED_ERROR_FLAG = false;
                        goto VPLAN_STOP;
                    }
                }
                TransferMapBackGroundWorker.ReportProgress(100);
            }
            else
            {
                TransferMapBackGroundWorker.ReportProgress(0, "FAIL");
                MessageBox.Show(AutoEnable ? "AUTO ON":"AUTO OFF", "TransferMapBackGroundWorker_DoWork", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TransferMapBackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!TransferMapBackGroundWorker.CancellationPending) 
            {
                if (e.ProgressPercentage > 0) {
                    progressUC1.ProgressBar.Value = e.ProgressPercentage;
                    progressUC1.ProgressBar.Update();
                    progressUC1.ProgressBar.PerformLayout();
                    autoUC1.SentTb.Text += e.ProgressPercentage.ToString() + "%... \r\n";
                } else if ((string)e.UserState == "START") {
                    autoUC1.SentTb.Text += "Reading map_out.txt...\n";
                } else if ((string)e.UserState == "PARSE") {
                    autoUC1.SentTb.Text += "Parsing text to map...\n";
                } else if ((string)e.UserState == "SEND") {
                    progressUC1.BringToFront();
                    autoUC1.SentTb.Text += "Ready for sending data... Total " + PlanCoordinatesList.Count.ToString() + "\r\n";
                } else if ((string)e.UserState == "FAIL") {
                    autoUC1.SentTb.Text += "OOPS! Auto mode is off, stop sending!\n";
                }
            }
        }

        /*  When DoWork() has completed, has been canceled, or has raised an exception */
        private void TransferMapBackGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Console.WriteLine("Transfer map has been canceled!");
            }
            else if (e.Error != null)
            {
                Console.WriteLine("Transfer map caught error: " + e.Error.Message);
            }
            else
            {
                if (progressUC1.ProgressBar.Value < 100)
                    MessageBox.Show("Transferring map is uncompleted");
                else
                    MessageBox.Show("Transferring map is completed");
            }
        }

        //-----------------------------------progressUC1 buttons------------------------------------//
        private void HideProgressBarBtClickHandler(object sender, EventArgs e) // HIDE button
        {
            if (TransferMapBackGroundWorker.IsBusy != true)
                this.progressUC1.SendToBack();
        }

        private void CancelProgressBarBtClickHandler(object sender, EventArgs e) // CANCEL button
        { 
            if (TransferMapBackGroundWorker.IsBusy)
            {
                Console.WriteLine("Cancel transferring map thread...");
                TransferMapBackGroundWorker.CancelAsync();
                progressUC1.ProgressBar.Value = 0;
                progressUC1.ProgressBar.Update();
            }
        }
        //--------------------------------------------------------------------------------------------//
        //TODO: Set temporarily, fix later
        char KeyW = '!', KeyS = '!', KeyA = '!', KeyD = '!';
        int level = 4, defaultLevel = 4;

        private void SendKCTRLCommand()
        {
            if (serialPort1.IsOpen)
            {
                string mess = MessagesDocker("KCTRL," + KeyW.ToString() + "," +
                    KeyS.ToString() + "," + KeyA.ToString() + "," + KeyD.ToString() + "," + level.ToString());
                serialPort1.Write(mess);
#if DEBUG
                Console.Write("[SENT] " + mess);
#endif
            }
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 112: //Key = F1
                    vehicleSetting1.BringToFront();
                    break;
                case 113: // Key = F2
                    imuSetting1.BringToFront();
                    break;
                case 114: // Key = F3
                    manualUC1.BringToFront();
                    break;
                case 115: // Key = F4
                    autoUC1.BringToFront();
                    break;
                case 116: // Key = F5
                    KctrlChangeMode(0);
                    break;
                case 117: // Key = F6
                    KctrlChangeMode(1);
                    break;
                case 118: // Key = F7, ON Send data from vehicle
                    ControlSendDataFromVehicle(0);
                    break;
                case 119: // Key = F8, OFF Send data from vehicle
                    ControlSendDataFromVehicle(1);
                    break;
                case 121: // Key = F10, Reset
                    serialPort1.Write(MessagesDocker("SFRST"));
                    MyStatus.SFRST = true;
                    KctrlTimer.Enabled = true;
                    SetPreviousMode();
                    break;
                case 123: // Key = F12, Get all help controls
                    if(OnHelperPanel)
                    {
                        OnHelperPanel = false;
                        helperControls1.SendToBack();
                    }
                    else
                    {
                        OnHelperPanel = true;
                        helperControls1.BringToFront();
                    }
                    break;
                case 27: // Key = Escape
                    helperControls1.SendToBack(); 
                    break;
            }
            if(KctrlEnabled)
            {
                if (e.KeyCode == Keys.W)
                {
                    if (KeyW != 'W')
                    {
                        KeyW = 'W';
                        level = defaultLevel;
                        SendKCTRLCommand();
                    }
                }
                else if (e.KeyCode == Keys.S)
                {
                    if (KeyS != 'S')
                    {
                        KeyS = 'S';
                        level = defaultLevel;
                        SendKCTRLCommand();
                    }
                }
                else if (e.KeyCode == Keys.A)
                {
                    if (KeyA != 'A')
                    {
                        KeyA = 'A';
                        level = Convert.ToInt16(0.5 * defaultLevel);
                        SendKCTRLCommand();
                    }
                }
                else if (e.KeyCode == Keys.D)
                {
                    if (KeyD != 'D')
                    {
                        KeyD = 'D';
                        level = Convert.ToInt16(0.5 * defaultLevel);
                        SendKCTRLCommand();
                    }
                }
                else if (e.KeyCode == Keys.E)
                {
                    defaultLevel += (defaultLevel >= 9) ? 0 : 1; // max='9' because "10" is 2 characters
                }
                else if (e.KeyCode == Keys.Q)
                {
                    defaultLevel -= (defaultLevel <= 0) ? 0 : 1;
                }
            }
        }

        void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(KctrlEnabled)
            {
                if (e.KeyCode == Keys.W)
                {
                    KeyW = '!';
                }
                else if (e.KeyCode == Keys.S)
                {
                    KeyS = '!';
                }
                else if (e.KeyCode == Keys.A)
                {
                    KeyA = '!';
                    level = defaultLevel;
                }
                else if (e.KeyCode == Keys.D)
                {
                    KeyD = '!';
                    level = defaultLevel;
                }
            }
        }

        // Main form buttons click events ----------------------------------------//
        private void menubt_Click(object sender, EventArgs e)
        {
            if (SidePanel.Width == 0)
            {
                SidePanel.Width = 156;
            }
            else
                SidePanel.Width = 0;
        }

        private void minbt_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void maxbt_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void closebt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            this.vehicleSetting1.BringToFront();
            SidePanel.Width = 0;
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            this.manualUC1.BringToFront();
            SidePanel.Width = 0;
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            imuSetting1.BringToFront();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            autoUC1.BringToFront();
            SidePanel.Width = 0;
        }

        private void TopPanel_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void ManualBtClickHandler(object sender, EventArgs e)
        {
            // TODO: Add disable for this mode later
            try
            {
                serialPort1.Write(MessagesDocker("KCTRL,1"));
                manualUC1.SentBox.Text += DateTime.Now.ToString("h:mm:ss tt") + ": Started to control manually\r\n";
                manualUC1.FormStatus.Text = "STARTED";
                AutoEnable = true;
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //-------------------------------------------------------------------------//
        //------------------- Init Functions & Linking to events ------------------//
        private void InitSettingUC()
        {
            ConfigTextBox(false);

            vehicleSetting1.ConnectedImage.Visible = false;
            vehicleSetting1.ConnectedLabel.Visible = false;
            // Add items to combo box
            string[] Ports = SerialPort.GetPortNames();
            vehicleSetting1.PortNameBox.Items.AddRange(Ports);
            vehicleSetting1.PortNameBox.Text = "COM4";
            vehicleSetting1.BaudrateBox.Items.AddRange(new string[] { "9600", "19200", "38400", "57600", "115200", "921000"});
            vehicleSetting1.BaudrateBox.Text = "115200";
            // Read txt file
            StreamReader sr = new StreamReader(Application.StartupPath + @"\Config.txt");

            string[] a;
            a = sr.ReadLine().Split(',');
            vehicleSetting1.Velocity.Text = a[1];
            vehicleSetting1.Angle.Text = a[2];
            vehicleSetting1.Kp1.Text = a[3];
            vehicleSetting1.Ki1.Text = a[4];
            vehicleSetting1.Kd1.Text = a[5];
            vehicleSetting1.Kp2.Text = a[6];
            vehicleSetting1.Ki2.Text = a[7];
            vehicleSetting1.Kd2.Text = a[8];
            vehicleSetting1.Mode.Text = "0";
            // Exit stream reader
            sr.Close();
            sr.Dispose();
        }

        private void InitManualUC()
        {
            manualUC1.gmap.DragButton = MouseButtons.Left;
            manualUC1.gmap.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            manualUC1.gmap.SetPositionByKeywords("Vietnam, Ho Chi Minh");
            manualUC1.gmap.Position = new PointLatLng(10.772801, 106.659273);
            manualUC1.gmap.Zoom = 19;
        }

        private void InitAutoUC()
        {
            autoUC1.gmap.DragButton = MouseButtons.Left;
            autoUC1.gmap.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            autoUC1.gmap.SetPositionByKeywords("Vietnam, Ho Chi Minh");
            autoUC1.gmap.Position = new PointLatLng(10.772801, 106.659273);
            autoUC1.gmap.Zoom = 19;
        }

        private void LinkToUCEvents()
        {
            /* vehicleSetting buttons event handler */
            this.vehicleSetting1.OpenSPBtClickHandler(new EventHandler(OpenSPBtSettingUCClickHandler));
            this.vehicleSetting1.SendBtClickHandler(new EventHandler(SendBtSettingUCClickHandler));
            this.vehicleSetting1.SaveBtClickHandler(new EventHandler(SaveBtSettingUCClickHandler));
            this.vehicleSetting1.UpdateSPBtClickHandler(new EventHandler(UpdateSPBtSettingUCClickHandler));

            /* manualUC buttons event handler */
            this.manualUC1.StartBtClickHandler(new EventHandler(StartBtManualUCClickHandler));
            this.manualUC1.StopBtClickHandler(new EventHandler(StopBtManualUCClickHandler));
            this.manualUC1.ImportBtClickHandler(new EventHandler(ImportBtManualUCClickHandler));
            this.manualUC1.ExportBtClickHandler(new EventHandler(ExportBtManualUCClickHandler));
            this.manualUC1.ChangeModeBtClickHandler(new EventHandler(ChangeModeBtManualUCClickHandler)); // modeBt

            /* IMU Setting mode */
            this.imuSetting1.SendMessConfigBtClickHandler(new EventHandler(SendMessConfigIMUClickHandler));
            this.imuSetting1.SendBaudrateConfigBtClickHandler(new EventHandler(SendBaudrateConfigClickHandler));
            this.imuSetting1.SendFreqBtClickHandler(new EventHandler(SendFreqIMUClickHandler));
            this.imuSetting1.CalibBtClickHandler(new EventHandler(CalibIMUBtClickHandler));
            this.imuSetting1.ReadConfigBtClickHandler(new EventHandler(ReadIMUConfigClickHandler));
            this.imuSetting1.StartBtClickHandler(new EventHandler(StartIMUBtClickHandler));

            /* Auto mode */
            this.autoUC1.OnBtClickHandler(new EventHandler(OnBtAutoUCClickHandler)); // ON button
            this.autoUC1.StartBtClickHandler(new EventHandler(StartBtAutoUCClickHandler)); // START button
            this.autoUC1.OffBtClickHandler(new EventHandler(OffBtAutoUCClickHandler)); // OFF button
            this.autoUC1.StopVehicleBtClickHandler(new EventHandler(StopVehicleBtAutoUCClickHandler)); // STOP VEHICLE button
            this.autoUC1.OpenBtClickHandler(new EventHandler(OpenBtAutoUCClickHandler)); // OPEN button
            this.autoUC1.SaveBtClickHandler(new EventHandler(SaveBtAutoUCClickHandler)); // SAVE button
            this.autoUC1.PlanRoutesBtClickHandler(new EventHandler(PlanRoutesBtAutoUCClickHandler)); // PlanMap button
            this.autoUC1.SendRoutesBtClickHandler(new EventHandler(SendRoutesBtAutoUCClickHandler)); // SENDROUTES button
            this.autoUC1.ClearPlannedMapBtClickHandler(new EventHandler(ClearPlannedMapBtAutoUCClickHandler)); // 'Clear planned map' button
            this.autoUC1.ClearActualMapBtClickHandler(new EventHandler(ClearActualMapBtAutoUCClickHandler)); // 'Clear actual map' button
            this.autoUC1.SettingBtClickHandler(new EventHandler(SettingBtAutoUCClickHandler)); // SETTING button
            this.autoUC1.CreatePreProcessingBtClickHandler(new EventHandler(CreatePreProcessingMapHandler)); // CREATE PRE-PROCESSING button
            this.autoUC1.ImportProcessedMapBtClickHandler(new EventHandler(ImportProcessedMapHandler)); // IMPORT PROCESSED MAP button
            this.autoSetting1.SendButtonClickHandler(new EventHandler(SendButton_ClickHandler));
            this.autoSetting1.OffSelfUpdateBtClickHandler(new EventHandler(OffSelfUpdate_ClickHandler));
            this.autoSetting1.OnSelfUpdateBtClickHandler(new EventHandler(OnSelfUpdate_ClickHandler));
            this.autoSetting1.AutoEnableClickHandler(new EventHandler(AutoEnable_ClickHandler));

            /* Control Panel */
            this.helperControls1.CloseBtClickHandler(new EventHandler(CloseBtHelperUCClickHandler));
            this.helperControls1.SettingVehicleBtClickHandler(new EventHandler(SettingVehicleHelperUCClickHandler));
            this.helperControls1.IMUSettingBtClickHandler(new EventHandler(IMUSettingHelperUCClickHandler));
            this.helperControls1.ManualBtClickHandler(new EventHandler(ManualBtHelperUCClickHandler));
            this.helperControls1.AutoBtClickHandler(new EventHandler(AutoBtHelperUCClickHandler));
            this.helperControls1.KctrlOnClickHandler(new EventHandler(KctrlOnHelperUCClickHandler));
            this.helperControls1.KctrlOffClickHandler(new EventHandler(KctrlOffHelperUCClickHandler));
            this.helperControls1.OnSendDataBtClickHandler(new EventHandler(OnSendDataBtHelperUCClickHandler));
            this.helperControls1.OffSendDataBtClickHandler(new EventHandler(OffSendDataBtHelperUCClickHandler));

            /* Progress bar for importing map */
            this.progressUC1.HideBtClickHandler(new EventHandler(HideProgressBarBtClickHandler));
            this.progressUC1.CancelBtClickHandler(new EventHandler(CancelProgressBarBtClickHandler));
        }

        /* ------------------------------------------------------------------------------ */
        /* ---------------------- Handler for SETTING User Control ---------------------- */
        private void ConfigTextBox(bool b) // @b = 'true' if ENABLE, otherwise 'false'
        {
            vehicleSetting1.Kp1.Enabled = vehicleSetting1.Ki1.Enabled = vehicleSetting1.Kd1.Enabled = b;
            vehicleSetting1.Kp2.Enabled = vehicleSetting1.Ki2.Enabled = vehicleSetting1.Kd2.Enabled = b;
            vehicleSetting1.Velocity.Enabled = b;
            vehicleSetting1.Angle.Enabled = b;
            vehicleSetting1.Mode.Enabled = b;
        }

        private void ConfigSerialPort(bool b) // @b = 'true' if Open, 'false' if Close
        {
            vehicleSetting1.ConnectedImage.Visible = b;
            vehicleSetting1.ConnectedLabel.Visible = b;
            vehicleSetting1.DisonnectedImage.Visible = !b;
            vehicleSetting1.DisconnectedLabel.Visible = !b;
            vehicleSetting1.PortNameBox.Enabled = !b;
            vehicleSetting1.BaudrateBox.Enabled = !b;
        }

        private void UpdateSPBtSettingUCClickHandler(object sender, EventArgs e)
        {
            string[] Ports = SerialPort.GetPortNames();
            vehicleSetting1.PortNameBox.Items.Clear();
            vehicleSetting1.PortNameBox.Items.AddRange(Ports);
        }

        private void OpenSPBtSettingUCClickHandler(object sender, EventArgs e)
        {
            try
            {
                if(SerialPortEnable)
                {
                    /* Close */
                    serialPort1.Close();
                    ConfigSerialPort(false);
                    ConfigTextBox(false);
                    SerialPortEnable = false;
                    vehicleSetting1.OpenSPBt.Image = global::ThesisInterface.Properties.Resources.icons8_toggle_off_80;
                }
                else 
                {
                    /* Open */
                    serialPort1.PortName = vehicleSetting1.PortNameBox.Text;
                    serialPort1.BaudRate = Convert.ToInt32(vehicleSetting1.BaudrateBox.Text);
                    serialPort1.ReadBufferSize = 1000;
                    serialPort1.Open(); /* err: The port 'COMx' does not exist */
                    ConfigSerialPort(true);
                    ConfigTextBox(true);
                    SerialPortEnable = true;
                    vehicleSetting1.OpenSPBt.Image = global::ThesisInterface.Properties.Resources.icons8_toggle_on_80;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ON-OFF SerialPort Button", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendBtSettingUCClickHandler(object sender, EventArgs e)
        {
            try
            {
                string mess = "VEHCF," + vehicleSetting1.Velocity.Text + ","
                    + vehicleSetting1.Kp1.Text + "," + vehicleSetting1.Ki1.Text + ","
                    + vehicleSetting1.Kd1.Text + "," + vehicleSetting1.Kp2.Text + ","
                    + vehicleSetting1.Ki2.Text + "," + vehicleSetting1.Kd2.Text;
                mess = MessagesDocker(mess);
                serialPort1.Write(mess); /* err: The port is closed */
                OldMess = vehicleSetting1.ReceiveMessTextBox.Text;
                vehicleSetting1.SentMessTextBox.Text += 
                    DateTime.Now.ToString("h:mm:ss tt") + " SENDING MESSAGE:\r\n" + mess;
                ConfigWaitForRespond.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Send Button Vehicle-Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveBtSettingUCClickHandler(object sender, EventArgs e)
        {
            try
            {
                string mess = "$VEHCF," + vehicleSetting1.Velocity.Text + "," + vehicleSetting1.Angle.Text + ","
                    + vehicleSetting1.Kp1.Text + "," + vehicleSetting1.Ki1.Text + ","
                    + vehicleSetting1.Kd1.Text + "," + vehicleSetting1.Kp2.Text + ","
                    + vehicleSetting1.Ki2.Text + "," + vehicleSetting1.Kd2.Text;

                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\Config.txt");
                sw.WriteLine(mess);
                sw.Close();
                sw.Dispose();
                MessageBox.Show("Saved Setting");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //--------------------------------------------------------------------------//
        //--------------------- Handler for IMU User Control -----------------------//
        private void SendMessConfigIMUClickHandler(object sender, EventArgs e)
        {
            try
            {
                int[] indexes = imuSetting1.MessConfigCheckBox.CheckedIndices.Cast<int>().ToArray();
                string Mess = "";
                for (int i = 0; i < imuSetting1.MessConfigCheckBox.Items.Count; i++ )
                {
                    if (indexes.Contains(i))
                        Mess += "1";
                    else
                        Mess += "0";
                }
                Mess = "IMUCF,DATOP," + Mess;
                Mess = MessagesDocker(Mess);
                serialPort1.Write(Mess);
                imuSetting1.SentTextBox.Text += "Sending Messages Configuration \r\nMessages: "+ Mess;
                OldMess = imuSetting1.ReceivedTextBox.Text;
                IMUConfigWaitForRespond.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendBaudrateConfigClickHandler(object sender, EventArgs e)
        {
            try
            {
                int[] indexes = imuSetting1.BaudrateCheckBox.CheckedIndices.Cast<int>().ToArray();
                if(indexes.Count() != 0)
                {
                    string Mess = "IMUCF,DATOP," + (indexes[0] + 1).ToString();
                    Mess = MessagesDocker(Mess);
                    serialPort1.Write(Mess);
                    imuSetting1.SentTextBox.Text += "Sending Baudrate Configuration... \r\nMessages: " + Mess;
                    OldMess = imuSetting1.ReceivedTextBox.Text;
                    IMUConfigWaitForRespond.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please choose the suitable baudrate", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendFreqIMUClickHandler(object sender, EventArgs e)
        {
            try
            {
                string Mess = "IMUCF,TSAMP," + imuSetting1.FreqTextBox.Text;
                Mess = MessagesDocker(Mess);
                serialPort1.Write(Mess);
                imuSetting1.SentTextBox.Text += "Sending Update Frequency Configuration... \r\nMessages: " + Mess;
                OldMess = imuSetting1.ReceivedTextBox.Text;
                IMUConfigWaitForRespond.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalibIMUBtClickHandler(object sender, EventArgs e)
        {
            try
            {
                if (imuSetting1.CalibBt.Text == "Start")
                {
                    string Mess = "IMUCF,MAG2D";
                    Mess = MessagesDocker(Mess);
                    serialPort1.Write(Mess);
                    imuSetting1.SentTextBox.Text += "Sending Calibration Command... \r\nMessages: " + Mess;
                    OldMess = imuSetting1.ReceivedTextBox.Text;
                    imuSetting1.CalibBt.Text = "Stop";
                    IMUConfigWaitForRespond.Enabled = true;
                }
                else
                {
                    serialPort1.Write("$VSTOP");
                    imuSetting1.CalibBt.Text = "Start";
                    IMUConfigWaitForRespond.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CalibIMUBtClickHandler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadIMUConfigClickHandler(object sender, EventArgs e)
        {
            try
            {
                string Mess = "IMUCF,GPARA";
                Mess = MessagesDocker(Mess);
                serialPort1.Write(Mess);
                imuSetting1.SentTextBox.Text += "Sending Read Config Command... \r\nMessages: " + Mess;
                OldMess = imuSetting1.ReceivedTextBox.Text;
                IMUConfigWaitForRespond.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartIMUBtClickHandler(object sender, EventArgs e)
        {
            try
            {
                string Mess = "IMUCF,START";
                Mess = MessagesDocker(Mess);
                serialPort1.Write(Mess);
                imuSetting1.SentTextBox.Text += "Sending Start Command... \r\nMessages: " + Mess;
                OldMess = imuSetting1.ReceivedTextBox.Text;
                IMUConfigWaitForRespond.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //--------------------------------------------------------------------------//
        //-------------------- Handler for MANUAL User Control ---------------------//
        private void StartBtManualUCClickHandler(object sender, EventArgs e) // Start button from Mannual Setting
        {
            try // Send 'start command' to MCU
            {
                serialPort1.Write(MessagesDocker("MACON,START"));
                manualUC1.SentBox.Text += DateTime.Now.ToString("h:mm:ss tt") + ": Started to control manually\r\n";
                manualUC1.FormStatus.Text = "STARTED";
                DisableAllTimers();
                ManualEnabled = true;
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Start", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopBtManualUCClickHandler(object sender, EventArgs e)
        {
            try
            {
                ManualEnabled = false;
                serialPort1.Write(MessagesDocker("MACON,STOP"));
                DisableAllTimers();
                manualUC1.FormStatus.Text = "STOPPED";
                manualUC1.SentBox.Text += DateTime.Now.ToString("h:mm:ss tt") + ": Stop controlling manually\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Stop", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportBtManualUCClickHandler(object sender, EventArgs e)
        {
            /* TODO */
        }

        private void ExportBtManualUCClickHandler(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text File|*.txt";
                sfd.FileName = "Map";
                sfd.Title = "Save Text File";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = sfd.FileName;
                    File.WriteAllText(path, manualUC1.ReceiveBox.Text);
                    MessageBox.Show("Map Saved");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExportBtManualUCClickHandler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void manualUC1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                serialPort1.Write(MessagesDocker("MACON,W"));
            }
            else if (e.KeyCode == Keys.S)
            {
                serialPort1.Write(MessagesDocker("MACON,S"));
            }
            else if (e.KeyCode == Keys.A)
            {
                serialPort1.Write(MessagesDocker("MACON,A"));
            }
            else if (e.KeyCode == Keys.D)
            {
                serialPort1.Write(MessagesDocker("MACON,D"));
            }
            else if (e.KeyCode == Keys.F)
            {
                serialPort1.Write(MessagesDocker("MACON,F"));
            }
        }

        private void ChangeModeBtManualUCClickHandler(object sender, EventArgs e)
        {
            if(online)
            {
                manualUC1.modeBt.ButtonText = "Offline";
                manualUC1.gmap.SendToBack();
                online = false;
            }
            else
            {
                manualUC1.modeBt.ButtonText = "Online";
                manualUC1.chart1.SendToBack();
                online = true;
            }
        }

        //--------------------------------------------------------------------------//
        //---------------------- Handler for AUTO User Control ---------------------//
        private void autoUC_send(string mess)
        {
            DisableAllTimers();
            serialPort1.Write(mess);
            autoUC1.SentTb.Text += DateTime.Now.ToString("hh:mm:ss") + " >> " + mess;
            KctrlTimer.Enabled = true;
        }

        private void OnBtAutoUCClickHandler(object sender, EventArgs e)  
        {
            try
            {
                MyStatus.AUCON_START = true;
                string mess = MessagesDocker("AUCON,START");
                autoUC_send(mess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ON", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OffBtAutoUCClickHandler(object sender, EventArgs e)
        {
            try
            {
                MyStatus.AUCON_STOP = true;
                string mess = MessagesDocker("AUCON,STOP");
                autoUC_send(mess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OFF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartBtAutoUCClickHandler(object sender, EventArgs e)  // *TODO: Consider to remove this
        {
            try
            {
                MyStatus.AUCON_RUN = true;
                string mess = MessagesDocker("AUCON,RUN");
                autoUC_send(mess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "START", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        private void StopVehicleBtAutoUCClickHandler(object sender, EventArgs e) // *TODO: Consider to remove this
        {
            try
            {
                MyStatus.AUCON_PAUSE = true;
                string mess = MessagesDocker("AUCON,PAUSE");
                autoUC_send(mess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 

        private string ReadJsonFile()
        {
            string text = "";
            try
            {
                using (var sfd = new OpenFileDialog())
                {
                    sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FilterIndex = 2;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        text = File.ReadAllText(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return text;
        }

        private void OpenBtAutoUCClickHandler(object sender, EventArgs e)
        {
            try
            {
                string jsonfile = ReadJsonFile();
                Console.WriteLine("jsonfile:\n" + jsonfile);

                CoordinatesInfo coordinatesInformation = JsonConvert.DeserializeObject<CoordinatesInfo>(jsonfile);
                Console.WriteLine(coordinatesInformation.plannedCoordinates.Count.ToString());

                ClearPLannedData();
                ClearActualData();
                
                for(int i = 0; i < coordinatesInformation.plannedCoordinates.Count(); i++)
                {
                    PlanCoordinatesList.Add(
                        new PointLatLng(coordinatesInformation.plannedCoordinates[i].Lat, 
                        coordinatesInformation.plannedCoordinates[i].Lng));
                }
                DisplayRouteOnMap(
                    autoUC1.gmap, 
                    new GMapRoute(PlanCoordinatesList, "single_line") { Stroke = new Pen(Color.LightGoldenrodYellow, 3) }, 
                    "Planned");

                for (int i = 0; i < coordinatesInformation.actualCoordinates.Count; i++)
                {
                    if (i>0)
                    {
                        double [] utm1 = LatLonToUTM(
                            coordinatesInformation.actualCoordinates[i - 1].Lat, 
                            coordinatesInformation.actualCoordinates[i - 1].Lng);
                        double [] utm2 = LatLonToUTM(
                            coordinatesInformation.actualCoordinates[i].Lat, 
                            coordinatesInformation.actualCoordinates[i].Lng);
                        double deltaError = Math.Abs(utm1[0] - utm2[0]);
                        if (deltaError > 5)
                        {
                            coordinatesInformation.actualCoordinates[i].Lat = coordinatesInformation.actualCoordinates[i - 1].Lat;
                            coordinatesInformation.actualCoordinates[i].Lng = coordinatesInformation.actualCoordinates[i - 1].Lng;
                        }
                        ActualCoordinatesList.Add(
                            new PointLatLng(coordinatesInformation.actualCoordinates[i].Lat, 
                            coordinatesInformation.actualCoordinates[i].Lng));
                    }
                    else
                    {
                        ActualCoordinatesList.Add(
                            new PointLatLng(coordinatesInformation.actualCoordinates[i].Lat, 
                            coordinatesInformation.actualCoordinates[i].Lng));
                    }
                    DisplayRouteOnMap(
                        autoUC1.gmap, 
                        new GMapRoute(ActualCoordinatesList, "single_line") { Stroke = new Pen(Color.Red, 3) }, 
                        "Actual");
                }

                DistanceErrors.Clear();
                //DistanceErrors = coordinatesInformation.ErrorDistances;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private CoordinatesInfo CreateCoordinatesInformation()
        {
            List<PlannedCoordinate> listPlanned = new List<PlannedCoordinate>();
            List<ActualCoordinate> listActual = new List<ActualCoordinate>();
            
            CoordinatesInfo CoordinatesInformation = new CoordinatesInfo();

            for (int i = 0; i < PlanCoordinatesList.Count; i++)
            {
                PlannedCoordinate plannedCoordinates = new PlannedCoordinate();
                plannedCoordinates.Lat = PlanCoordinatesList[i].Lat;
                plannedCoordinates.Lng = PlanCoordinatesList[i].Lng;
                listPlanned.Add(plannedCoordinates);
            }

            for (int i = 0; i < ActualCoordinatesList.Count; i++)
            {
                ActualCoordinate actualCoordinates = new ActualCoordinate();
                actualCoordinates.Lat = Convert.ToDouble(ActualCoordinatesList[i].Lat);
                actualCoordinates.Lng = Convert.ToDouble(ActualCoordinatesList[i].Lng); 
                listActual.Add(actualCoordinates);
            }

            CoordinatesInformation.plannedCoordinates = listPlanned;
            CoordinatesInformation.actualCoordinates = listActual;
            CoordinatesInformation.ErrorDistances = DistanceErrors;
            CoordinatesInformation.Efa = Efa;

            return CoordinatesInformation;
        }

        private void SaveBtAutoUCClickHandler(object sender, EventArgs e)
        {            
            try
            {
                CoordinatesInfo coordinatesInformation = CreateCoordinatesInformation();
                WriteJsonFile(coordinatesInformation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void autoUC1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if(numOfCalculatePoints < 1)
                {
                    numOfCalculatePoints++;
                    DistanceCalculate.Add(autoUC1.gmap.FromLocalToLatLng(e.X, e.Y));
                }
                else if (numOfCalculatePoints == 1)
                {
                    DistanceCalculate.Add(autoUC1.gmap.FromLocalToLatLng(e.X, e.Y));
                    GMapMarker distanceMarker = new GMarkerGoogle(
                        DistanceCalculate[0],
                        GMarkerGoogleType.red_small
                        );
                    GMapMarker distanceMarker2 = new GMarkerGoogle(
                        DistanceCalculate[1],
                        GMarkerGoogleType.red_small
                        );
                    double[] UTM1 = LatLonToUTM(DistanceCalculate[0].Lat, DistanceCalculate[0].Lng);
                    double[] UTM2 = LatLonToUTM(DistanceCalculate[1].Lat, DistanceCalculate[1].Lng);
                    double distance = Math.Sqrt(Math.Pow(UTM1[0] - UTM2[0], 2) + Math.Pow(UTM1[1] - UTM2[1], 2))*100;

                    distanceMarker2.ToolTipMode = MarkerTooltipMode.Always;
                    distanceMarker2.ToolTipText = distance.ToString() + "cm";

                    autoUC1.gmap.Overlays.Add(DistanceMarkers);
                    autoUC1.gmap.Overlays.Add(DistanceOverlay);

                    DistanceOverlay.Routes.Add(new GMapRoute(DistanceCalculate, "single-line") { Stroke = new Pen(Color.Chartreuse, 1) });
                    DistanceMarkers.Markers.Add(distanceMarker);
                    DistanceMarkers.Markers.Add(distanceMarker2);
                    numOfCalculatePoints++;
                }
                else
                {
                    numOfCalculatePoints = 0;
                    DistanceCalculate.Clear();
                    DistanceOverlay.Clear();
                    DistanceMarkers.Clear();
                }
            }
            else if (PlanMapEnable && (e.Button == MouseButtons.Right))
            {
                autoUC1.gmap.Overlays.Clear();
                PlanCoordinatesList.Add(autoUC1.gmap.FromLocalToLatLng(e.X, e.Y)); // mouse-coordinate to Lat, Lon
                GMapMarker marker = new GMarkerGoogle(
                    PlanCoordinatesList[PlanCoordinatesList.Count - 1],
                    GMarkerGoogleType.red_big_stop);

                DisplayRouteOnMap(
                    autoUC1.gmap, 
                    new GMapRoute(PlanCoordinatesList, "single_line") { Stroke = new Pen(Color.LightGoldenrodYellow, 3) }, 
                    "Planned", 
                    marker);
            }
        }

        private void PlanRoutesBtAutoUCClickHandler(object sender, EventArgs e)
        {
            if(PlanMapEnable == true)
            {
                PlanMapEnable = false;
                autoUC1.PlanMapBt.Text = "Enable Plan Map";
            }
            else
            {
                PlanMapEnable = true;
                autoUC1.PlanMapBt.Text = "Disable Plan Map";
            }
        }

        private void SendRoutesBtAutoUCClickHandler(object sender, EventArgs e)
        {
            MessageBox.Show("This button is unused", "SendRoutes Button", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ClearPlannedMapBtAutoUCClickHandler(object sender, EventArgs e)
        {
            ClearPLannedData();
        }

        private void ClearActualMapBtAutoUCClickHandler(object sender, EventArgs e)
        {
            ClearActualData();
        }

        private void SettingBtAutoUCClickHandler(object sender, EventArgs e)
        {
            if(AutoSetting)
            {
                this.autoSetting1.SendToBack();
                AutoSetting = false;
            }
            else
            {
                this.autoSetting1.BringToFront();
                AutoSetting = true;
            }
        }

        private void SendButton_ClickHandler(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                MyStatus.AUCON_DATA = true;
                string mess = MessagesDocker("AUCON,DATA," + autoSetting1.VelocityTb.Text + "," + autoSetting1.KGainTb.Text + ",0.5");
                autoUC_send(mess);
            }
        }

        private void OffSelfUpdate_ClickHandler(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                string mess = MessagesDocker("AUCON,SUPDT,0");
                serialPort1.Write(mess);
                autoUC1.SentTb.Text += DateTime.Now.ToString("h:mm:ss tt") + " SEND MESSAGE: " + mess;
            }
        }

        private void OnSelfUpdate_ClickHandler(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                string mess = MessagesDocker("AUCON,SUPDT,1");
                serialPort1.Write(mess);
                autoUC1.SentTb.Text += DateTime.Now.ToString("h:mm:ss tt") + " SEND MESSAGE: " + mess;
            }
        }

        private void AutoEnable_ClickHandler(object sender, EventArgs e)
        {
            if (AutoEnable)
            {
                AutoEnable = false;
                this.autoSetting1.AutoEnable.Image = global::ThesisInterface.Properties.Resources.OFF;
            } else
            {
                AutoEnable = true;
                this.autoSetting1.AutoEnable.Image = global::ThesisInterface.Properties.Resources.ON;
            }
        }

        private void CreatePreProcessingMapHandler(object sender, EventArgs e) // Click CreatePreProcessingBt
        {
            if(PreprocessBackGroundWorker.IsBusy)
            {
                Console.WriteLine("cancel a map pre-processing thread...");
                PreprocessBackGroundWorker.CancelAsync(); // Cancel map pre-processing thread
            }
            else
            {
                Console.WriteLine("Create map pre-processing thread...");
                PreprocessBackGroundWorker.RunWorkerAsync(); // Start thread 
            }
        }

        private void ImportProcessedMapHandler(object sender, EventArgs e) // Click ImportProcessedMapBt
        {
            if(TransferMapBackGroundWorker.IsBusy != true)
            {
                progressUC1.ProgressBar.Value = 0;
                progressUC1.ProgressBar.Update();
                Console.WriteLine("Create transferring-map thread...");
                TransferMapBackGroundWorker.RunWorkerAsync(); // Start thread
            }
        }
        
        //---------------------------------------------------------------------------//
        // Helper Control

        private void CloseBtHelperUCClickHandler(object sender, EventArgs e) // CLOSE
        {
            helperControls1.SendToBack();
        }

        private void SettingVehicleHelperUCClickHandler(object sender, EventArgs e) // Vehicle Setting
        {
            vehicleSetting1.BringToFront();
        }

        private void IMUSettingHelperUCClickHandler(object sender, EventArgs e) // IMU Setting
        {
            imuSetting1.BringToFront();
        }

        private void ManualBtHelperUCClickHandler(object sender, EventArgs e) // Manual Control
        {
            manualUC1.BringToFront();
        }

        private void AutoBtHelperUCClickHandler(object sender, EventArgs e) // Auto Control
        {
            autoUC1.BringToFront();
        }

        private void KctrlOnHelperUCClickHandler(object sender, EventArgs e) // KCTRL - ON
        {
            KctrlChangeMode(0);
        }

        private void KctrlOffHelperUCClickHandler(object sender, EventArgs e) // KCTRL - OFF
        {
            KctrlChangeMode(1);
        }

        private void OnSendDataBtHelperUCClickHandler(object sender, EventArgs e) // ON
        {
            ControlSendDataFromVehicle(0);
        }

        private void OffSendDataBtHelperUCClickHandler(object sender, EventArgs e) // OFF
        {
            ControlSendDataFromVehicle(1);
        }
        
        //--------------------------------------------------------------------------//
        // TIMERS ------------------------------------------------------------------//
        private void timer1_Tick(object sender, EventArgs e) // Main timer for displaying data of vehicle
        {
            if (ManualEnabled)
            {
                if (serialPort1.IsOpen && (serialPort1.BytesToRead != 0))
                {
                    try
                    {
                        string temp;
                        string mess = serialPort1.ReadLine();
                        string[] value;
                        value = mess.Split(',');
                        temp = mess;
                        if(mess.Length >= 5)
                        {
                            temp = temp.Remove(temp.Length - 3, 3);
                            temp = temp.Remove(0, 1);
                        }
                        string CC = checksum(temp);

                        // Update Vehicle Information
                        if (mess.Contains("$VINFO,0") && mess.Contains(CC))
                        {
                            MyVehicle = new Vehicle(value);
                            Vehicle_Information = MyVehicle.GetVehicleStatus();
                        }

                        // Update GPS Information
                        if (mess.Contains("$VINFO,1") && mess.Contains(CC))
                        {
                            MyGPS = new GPS(value);
                            GPS_Information = MyGPS.GetGPSStatus();
                        }

                        // Update Stanley Information
                        if (mess.Contains("$VINFO,2") && mess.Contains(CC))
                        {
                            MyStanleyControl = new StanleyControl(value);
                            StanleyControl_Information = MyStanleyControl.GetStanleyControlStatus();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                
                }
            }
            else if(AutoEnable) //Change mode a little bit here, *TODO: Change it back later
            {
                if (serialPort1.IsOpen && (serialPort1.BytesToRead != 0))
                {
                    try
                    {
                        string temp;
                        string mess = serialPort1.ReadLine();
                        string[] value;
                        if(mess.Contains("$"))
                        {
                            value = mess.Split(',');
                            temp = mess;
                            if (mess.Length >= 5)
                            {
                                temp = temp.Remove(temp.Length - 3, 3);
                                temp = temp.Remove(0, 1);
                            }
                            string CC = checksum(temp);

                            // Update Vehicle Information
                            if (mess.Contains("$VINFO,0") && mess.Contains(CC))
                            {
                                MyVehicle = new Vehicle(value);
                                Vehicle_Information = MyVehicle.GetVehicleStatus();

                                /*  Draw turning State of vehicle by subtracting the RefAngle and the ActualAngle
                                (It help users to understand whether the vehicle is turning left or right)      */
                                DrawVehicleTurningStatusOnImage(autoUC1.VehicleStatusImage, -MyVehicle.RefAngle + MyVehicle.Angle, LowVelocity);
                                autoUC1.TurningState.Text = "Turning " + Math.Round(-MyVehicle.RefAngle + MyVehicle.Angle, 4).ToString() + "°";
                                //autoUC1.ReceivedTb.Text += mess;
                            }

                            // Update GPS Information
                            if (mess.Contains("$VINFO,1") && mess.Contains(CC))
                            {
                                MyGPS = new GPS(value);
                                GPS_Information = MyGPS.GetGPSStatus();
                                autoUC1.ReceivedTb.Text += mess;
                                if (MyGPS.GPS_Available.Contains("Y"))
                                {
                                    // Save Position Data & Draw On Map
                                    ActualCoordinatesList.Add(new GMap.NET.PointLatLng(MyGPS.GPS_Lat, MyGPS.GPS_Lng));
                                    GMapMarker marker = new GMarkerGoogle(
                                        new PointLatLng(MyGPS.GPS_Lat, MyGPS.GPS_Lng),
                                        GMarkerGoogleType.orange_dot);
                                    DisplayRouteOnMap(
                                        autoUC1.gmap, 
                                        new GMapRoute(ActualCoordinatesList, "single_line") { Stroke = new Pen(Color.Red, 3) },
                                        "Actual",
                                        marker);
                                }
                            }

                            // Update Stanley Information
                            if (mess.Contains("$VINFO,2") && mess.Contains(CC))
                            {
                                MyStanleyControl = new StanleyControl(value);
                                StanleyControl_Information = MyStanleyControl.GetStanleyControlStatus();
                                DistanceErrors.Add(MyStanleyControl.ErrorDistance);
                                Efa.Add(MyStanleyControl.Efa);
                            }
                                
                            /* If GPS Status is OK, then draw the positions of the vehicle on MAP. Otherwise, skip drawing positions. */

                            autoUC1.DetailInfoTb.Text = Vehicle_Information;
                            autoUC1.PosTb.Text = GPS_Information;
                            autoUC1.StanleyControlTb.Text = StanleyControl_Information;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ConfigWaitForRespond_Tick(object sender, EventArgs e)
        {
            if(setting_config_timeout > 0)
            {
                --setting_config_timeout;
                vehicleSetting1.ReceiveMessTextBox.Text = OldMess + "Waiting for respond..." + WaitKey;
                WaitKey = (WaitKey == "|") ? "-" : "|";

                if (serialPort1.IsOpen)
                {
                    if (VEHICLE_RECEIVED_DATA_FLAG == true)
                    {
                        VEHICLE_RECEIVED_DATA_FLAG = false;
                        ConfigWaitForRespond.Enabled = false;
                        setting_config_timeout = 40;

                        vehicleSetting1.ReceiveMessTextBox.Text =
                            OldMess + DateTime.Now.ToString("h:mm:ss tt") + ": Setting successfully\r\n";
                    }
                    else if (WRONG_CKSUM_FLAG)
                    {
                        WRONG_CKSUM_FLAG = false;
                        ConfigWaitForRespond.Enabled = false;
                        setting_config_timeout = 40;

                        vehicleSetting1.ReceiveMessTextBox.Text =
                            OldMess + DateTime.Now.ToString("h:mm:ss tt") + " Fail config: Wrong checksum\r\n";
                    }
                }
                else
                {
                    ConfigWaitForRespond.Enabled = false;
                    MessageBox.Show("Serial Port is closed!", "ConfigWaitForRespond", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                ConfigWaitForRespond.Enabled = false;
                setting_config_timeout = 40;
                vehicleSetting1.ReceiveMessTextBox.Text = OldMess + DateTime.Now.ToString("h:mm:ss tt") + ": Fail config... Time out\r\n";
            }
        }

        private void IMUConfigWaitForRespond_Tick(object sender, EventArgs e)
        {          
            if(IMU_config_timeout > 0)
            {
                IMU_config_timeout--;
                imuSetting1.ReceivedTextBox.Text = OldMess + "Waiting for respond" + WaitKey;
                WaitKey = (WaitKey == "|") ? "-" : "|";

                if(serialPort1.BytesToRead != 0)
                {
                    string mess = serialPort1.ReadLine();
                    string [] messParsed = mess.Split(',');
                    if(messParsed[0].Contains("$SINFO") && messParsed[1].Contains("1"))
                    {
                        imuSetting1.ReceivedTextBox.Text = OldMess + DateTime.Now.ToString("h:mm:ss tt") + ": IMU config successfully\r\n";
                        IMU_config_timeout = 40;
                        IMUConfigWaitForRespond.Enabled = false;
                    }
                }
            }
            else
            {
                IMU_config_timeout = 40;
                imuSetting1.ReceivedTextBox.Text = OldMess + DateTime.Now.ToString("h:mm:ss tt") + ": Fail config IMU... Time out\r\n";
                IMUConfigWaitForRespond.Enabled = false;
            }
        }

        private void KctrlTimer_Tick(object sender, EventArgs e)
        {
            if (kctrl_timeout > 0)
            {
                kctrl_timeout--;
                if (serialPort1.IsOpen)
                {
                    if (VEHICLE_RECEIVED_DATA_FLAG)
                    {
                        VEHICLE_RECEIVED_DATA_FLAG = false;
                        KctrlTimer.Enabled = false;
                        kctrl_timeout = 20;
                        if (MyStatus.KCTRL_START)
                        {
                            MyStatus.KCTRL_START = false;
                            KctrlEnabled = true;
                            MessageBox.Show("Started KCTRL successfully", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (MyStatus.KCTRL_STOP)
                        {
                            MyStatus.KCTRL_STOP = false;
                            KctrlEnabled = false;
                            MessageBox.Show("Stopped KCTRL successfully", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (MyStatus.VEHCF_DATA_ON)
                        {
                            MyStatus.VEHCF_DATA_ON = false;
                            MessageBox.Show("Sending data is ON", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (MyStatus.VEHCF_DATA_OFF)
                        {
                            MyStatus.VEHCF_DATA_OFF = false;
                            MessageBox.Show("Sending data is OFF", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (MyStatus.AUCON_START)
                        {
                            MyStatus.AUCON_START = false;
                            autoUC1.ReceivedTb.Text += DateTime.Now.ToString("hh:mm:ss") + " << START auto successfully\r\n";
                            PrevMode = "auto";
                            AutoEnable = true;
                            timer1.Enabled = true;
                        }
                        else if (MyStatus.AUCON_STOP)
                        {
                            MyStatus.AUCON_STOP = false;
                            autoUC1.ReceivedTb.Text += DateTime.Now.ToString("hh:mm:ss") + " << STOP auto successfully\r\n";
                            AutoEnable = false;
                            timer1.Enabled = false;
                            SetPreviousMode();
                        }
                        else if (MyStatus.AUCON_RUN)
                        {
                            MyStatus.AUCON_RUN = false;
                            //SetPreviousMode();
                            autoUC1.ReceivedTb.Text += DateTime.Now.ToString("hh:mm:ss") + " << RUN succesfully\r\n";
                        }
                        else if (MyStatus.AUCON_PAUSE)
                        {
                            MyStatus.AUCON_PAUSE = false;
                            //SetPreviousMode();
                            autoUC1.ReceivedTb.Text += DateTime.Now.ToString("hh:mm:ss") + " << PAUSE successfully\r\n";
                        }
                        else if (MyStatus.AUCON_DATA)
                        {
                            MyStatus.AUCON_DATA = false;
                            autoUC1.ReceivedTb.Text += DateTime.Now.ToString("hh:mm:ss") + " << config successfully\r\n";
                        }
                        else if (MyStatus.SFRST)
                        {
                            MyStatus.SFRST = false;
                            MessageBox.Show("Command SFRST sent successfully", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            return;
                    }
                    else if (WRONG_CKSUM_FLAG) /* $WRONG_CKSUM */
                    {
                        WRONG_CKSUM_FLAG = false;
                        KctrlTimer.Enabled = false;
                        kctrl_timeout = 20;
                        MyStatus.ResetAllBits();
                        MessageBox.Show("[vehicle] Wrong checksum", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (VEHICLE_RECEIVED_ERROR_FLAG) /* $SINFO,0 */
                    {
                        VEHICLE_RECEIVED_ERROR_FLAG = false;
                        KctrlTimer.Enabled = false;
                        kctrl_timeout = 20;
                        if (MyStatus.AUCON_RUN)
                        {
                            MyStatus.AUCON_RUN = false;
                            autoUC1.ReceivedTb.Text += DateTime.Now.ToString("hh:mm:ss") + 
                                ": [WARNING] start auto mode when there is no map\r\n";
                            return;
                        }
                        MessageBox.Show("[vehicle] Checksum is correct but wrong message", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else /* case timeout: the packet sent from PC or vehicle was dropped */
            {
                KctrlTimer.Enabled = false;
                kctrl_timeout = 20;
                MyStatus.ResetAllBits();
                MessageBox.Show("timeout: no respone from vehicle", "KctrlTimer_Tick", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //SetPreviousMode();
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;
            string indata = sp.ReadExisting();
            serial_command += indata;
            
            if(serial_command.Contains("\r\n"))
            {
                string[] mess = serial_command.Split(',');
#if DEBUG
                Console.WriteLine("[RECV] " + indata.Length + "new bytes, splitted into " + mess.Length + ", command: " + serial_command);
#endif
                switch (mess[0])
                {
                    case "$SINFO":
                        if (mess[1].Contains("0")) // use contains method because message is "0\r\n"
                        {
                            VEHICLE_RECEIVED_ERROR_FLAG = true;
                        }
                        else if (mess[1].Contains("1"))
                        {
                            VEHICLE_RECEIVED_DATA_FLAG = true;
                        }
                        else if (mess[1] == "VPLAN")
                        {
                            if (mess[2].Contains("1"))
                            {
                                this.MyStatus.VPLAN_FLAG = true;
                            }
                            else if (mess[2].Contains("0"))
                            {
                                this.MyStatus.VPLAN_FLAG = false;
                                autoUC1.ReceivedTb.Text +=
                                    DateTime.Now.ToString("hh:mm:ss") + " << transfer map successfully, ready to go\r\n";
                            }
                            else if (mess[2].Contains("?"))
                            {
                                Console.WriteLine("[vehicle] Data received is correct checksum but wrong header");
                            }
                        }
                        break;
                    case "$KCTRL":
                        {
                            break;
                        }
                    case "$VINFO":
                        if (AutoEnable || ManualEnabled)
                        {
                            /* TODO */
                            if (mess[1] == "0")
                            {

                            }
                            else if (mess[1] == "1")
                            {
                                MyGPS = new GPS(mess);
                                GPS_Information = MyGPS.GetGPSStatus();
                                autoUC1.ReceivedTb.Text += mess;
                                if (MyGPS.GPS_Available.Contains("Y"))
                                {
                                    // Save Position Data & Draw On Map
                                    ActualCoordinatesList.Add(new GMap.NET.PointLatLng(MyGPS.GPS_Lat, MyGPS.GPS_Lng));
                                    GMapMarker marker = new GMarkerGoogle(
                                        new PointLatLng(MyGPS.GPS_Lat, MyGPS.GPS_Lng),
                                        GMarkerGoogleType.orange_dot);
                                    DisplayRouteOnMap(
                                        autoUC1.gmap,
                                        new GMapRoute(ActualCoordinatesList, "single_line") { Stroke = new Pen(Color.Red, 3) },
                                        "Actual",
                                        marker);
                                }
                                autoUC1.PosTb.Text = GPS_Information;
                            }
                            else if (mess[1] == "2")
                            {

                            }
                        }
                        else
                            Console.WriteLine("Auto mode and Manual mode is disable, drop message");
                        break;
                    case "$WRONG_CKSUM":
                        WRONG_CKSUM_FLAG = true;
#if DEBUG
                        Console.WriteLine("Wrong checksum");
#endif
                        break;
                    default:
                        Console.WriteLine("Unknown type: " + mess[0]);
                        break;
                } // end switch

                serial_command = string.Empty;
                //int index = serial_command.IndexOf("\r\n");
                //serial_command = serial_command.Remove(0, index + 1);
            }
        }
        //---------------------------------------------------------------------------//
        // Other functions ----------------------------------------------------------//

        public void DrawVehicleTurningStatusOnImage(PictureBox ImgBox, double angle, Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            PointF offset = new PointF((float)image.Width / 2, (float)image.Height / 2);

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform((float)angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            ImgBox.Image = rotatedBmp;  
        }

        public static string checksum(string message)
        {
            byte[] buffer;
            int sum = 0;
            buffer = Encoding.Unicode.GetBytes(message);
            foreach (var b in buffer)
                sum ^= b;
            if (sum.ToString("X").Count() < 2)
                return "0" + sum.ToString("X");
            return sum.ToString("X");
        }

        private string MessagesDocker(string RawMess)
        {
            string MessWithoutKey = RawMess + ",";
            return "$" + MessWithoutKey + checksum(MessWithoutKey) + "\r\n";
        }

        private void WriteJsonFile(CoordinatesInfo listCoordinates)
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FilterIndex = 2;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(listCoordinates, Formatting.Indented));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearPLannedData()
        {
            //autoUC1.gmap.Overlays.Clear();
            PlanCoordinatesList.Clear(); 
            PlanLines.Clear();
        }

        private void ClearActualData()
        {
            //autoUC1.gmap.Overlays.Clear();
            ActualCoordinatesList.Clear();
            ActualLines.Clear();
            DistanceErrors.Clear();
            Efa.Clear();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            progressUC1.Location = new Point(this.Width / 2 - progressUC1.Width / 2, this.Height/2 - progressUC1.Height/2);
            autoSetting1.Location = new Point(this.Width / 2 - autoSetting1.Width / 2, this.Height / 2 - autoSetting1.Height / 2);
            helperControls1.Location = new Point(this.Width / 2 - helperControls1.Width / 2, this.Height / 2 - helperControls1.Height / 2);
        }

        private void SidePanel_SizeChanged(object sender, EventArgs e)
        {
            if(SidePanel.Width > 100)
            {
                autoUC1.SidePanelAuto.Width = 0;
                manualUC1.SidePanelManual.Width = 0;
            }
            else
            {
                autoUC1.SidePanelAuto.Width = 38;
                manualUC1.SidePanelManual.Width = 172;
            }
        }

        private void DisplayRouteOnMap(GMapControl map, GMapRoute route, string mode, GMapMarker marker=null)
        {
            try
            {
                if (mode.Contains("Plan"))
                {
                    //PlanLines.Routes.Clear();
                    if(marker != null)
                    {
                        GMapOverlay markers = new GMapOverlay("markers");
                        map.Overlays.Add(markers);
                        markers.Markers.Add(marker);
                    }
                    map.Overlays.Add(PlanLines);
                    map.Overlays.Add(ActualLines);
                    PlanLines.Routes.Add(route);
                }
                else
                {
                    map.Overlays.Clear();
                    ActualLines.Routes.Clear();
                    if(marker != null)
                    {
                        GMapOverlay markers = new GMapOverlay("markers");
                        map.Overlays.Add(markers);
                        markers.Markers.Add(marker);
                    }
                    map.Overlays.Add(ActualLines);
                    map.Overlays.Add(PlanLines);
                    ActualLines.Routes.Add(route);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DisplayRouteOnMap", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisableAllTimers()
        {
            timer1.Enabled = false;
            KctrlTimer.Enabled = false;
            ConfigWaitForRespond.Enabled = false;
            IMUConfigWaitForRespond.Enabled = false;
        }

        private void KctrlChangeMode(int mode)
        {
            try
            {
                if (mode == 0)
                {
                    serialPort1.Write(MessagesDocker("KCTRL,START,1"));
                    MyStatus.KCTRL_START = true;
                }
                else
                {
                    serialPort1.Write(MessagesDocker("KCTRL,STOP"));
                    MyStatus.KCTRL_STOP = true;
                }
                DisableAllTimers();
                KctrlTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "KctrlChangeMode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ControlSendDataFromVehicle(int mode)
        {
            try
            {
                if (mode == 0)
                {
                    MyStatus.VEHCF_DATA_ON = true;
                    serialPort1.Write(MessagesDocker("VEHCF,DATA,1"));
                }
                else if (mode == 1)
                {
                    MyStatus.VEHCF_DATA_OFF = true;
                    serialPort1.Write(MessagesDocker("VEHCF,DATA,0"));
                }
                DisableAllTimers();
                KctrlTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ControlSendDataFromVehicle", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetPreviousMode()
        {
            switch(PrevMode)
            {
                case "auto":
                    timer1.Enabled = true;
                    break;
                case "":
                    break;
            }
            AutoEnable = true;
        }

        private double[] LatLonToUTM(double Lat, double Lon)
        {
            double[] result = new double[2];
            double la, lo, lat, lon, sa, sb, e2, e2cuadrada, c, Huso, S, deltaS, a, 
                epsilon, nu, v, ta, a1, a2, j2, j4, j6, alfa, beta, gama, Bm, xx, yy;
            la = Lat;
            lo = Lon;
            sa = 6378137.000000;
            sb = 6356752.314245;
            e2 = Math.Pow((Math.Pow(sa, 2) - Math.Pow(sb, 2)), 0.5) / sb;
            e2cuadrada = Math.Pow(e2, 2);
            c = Math.Pow(sa, 2) / sb;
            lat = la * (Math.PI / 180);
            lon = lo * (Math.PI / 180);
            Huso = Math.Round(lo / 6, 0) + 31;
            S = ((Huso * 6) - 183);
            deltaS = lon - (S * (Math.PI / 180));
            a = Math.Cos(lat) * Math.Sin(deltaS);
            epsilon = 0.5 * Math.Log((1 + a) / (1 - a));
            nu = Math.Atan(Math.Tan(lat) / Math.Cos(deltaS)) - lat;
            v = (c / Math.Pow((1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2))), 0.5d)) * 0.9996;
            ta = (e2cuadrada / 2) * Math.Pow(epsilon, 2) * Math.Pow(Math.Cos(lat), 2);
            a1 = Math.Sin(2 * lat);
            a2 = a1 * Math.Pow(Math.Cos(lat), 2);
            j2 = lat + (a1 / 2);
            j4 = ((3 * j2) + a2) / 4;
            j6 = ((5 * j4) + (a2 * Math.Pow(Math.Cos(lat), 2))) / 3;
            alfa = (3d / 4) * e2cuadrada;
            beta = (5d / 3) * Math.Pow(alfa, 2);
            gama = (35d / 27) * Math.Pow(alfa, 3);
            Bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            xx = epsilon * v * (1 + (ta / 3)) + 500000;
            yy = nu * v * (1 + ta) + Bm;
            if (yy < 0)
            {
                yy = 9999999 + yy;
            }
            result[0] = xx;
            result[1] = yy;
            return result;
        }
    }
}
