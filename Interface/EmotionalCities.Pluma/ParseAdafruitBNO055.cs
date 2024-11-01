using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;

namespace EmotionalCities.Pluma
{
    /// <summary>
    /// Represents an operator that parses a sequence of line text messages sent by Adafruit BNO055.
    /// The firmware is implemented by Adafruit-BNO055.ino firmware.
    /// </summary>
    [Combinator]
    [Description("Parses a sequence of line text messages sent by Adafruit BNO055. The firmware is implemented by Adafruit-BNO055.ino firmware.")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class ParseAdafruitBno055
    {
        /// <summary>
        /// Parses an observable sequence of line text messages sent by Adafruit BNO055.
        /// </summary>
        /// <param name="source">The sequence of raw text messages sent by the firmware.</param>
        /// <returns>A sequence of <see cref="Bno055DataFrame"/> objects containing sensor data.</returns>
        public IObservable<Bno055DataFrame> Process(IObservable<string> source)
        {
            return source.Select(value =>
            {
                // Example message format:
                // Orient:359.94,-3.06,-109.31;Gyro1:-0.00,0.00,-0.00;Linear:0.00,0.00,0.00;Mag:48.19,-47.75,2.06;Accl:-0.52,9.24,-3.24;Gravity:-0.52,9.24,-3.24;Calibration:Sys=0,Gyro=1,Accel=0,Mag=0;Temp:value=37;

                var output = new Bno055DataFrame();
                var data_split = value.Split(';');
                //Orientation
                var Orientation = data_split[0].Split(':')[1].Split(',');
                output.Euler = new Point3f(float.Parse(Orientation[0]), float.Parse(Orientation[1]), float.Parse(Orientation[2]));
                //Gyroscope
                var Gyroscope = data_split[1].Split(':')[1].Split(',');
                output.Gyroscope = new Point3f(float.Parse(Gyroscope[0]), float.Parse(Gyroscope[1]), float.Parse(Gyroscope[2]));
                //LinearAcc
                var LinearAccl = data_split[2].Split(':')[1].Split(',');
                output.LinearAccel = new Point3f(float.Parse(LinearAccl[0]), float.Parse(LinearAccl[1]), float.Parse(LinearAccl[2]));
                //Magnet
                var Magnetometer = data_split[3].Split(':')[1].Split(',');
                output.Magnetometer = new Point3f(float.Parse(Magnetometer[0]), float.Parse(Magnetometer[1]), float.Parse(Magnetometer[2]));
                //Accl
                var Accl = data_split[4].Split(':')[1].Split(',');
                output.Accelerometer = new Point3f(float.Parse(Accl[0]), float.Parse(Accl[1]), float.Parse(Accl[2]));
                //Gravity
                var Gravity = data_split[5].Split(':')[1].Split(',');
                output.Gravity = new Point3f(float.Parse(Gravity[0]), float.Parse(Gravity[1]), float.Parse(Gravity[2]));
                //Calibration
                var Calibration = data_split[6].Split(':')[1].Split(',');
                var sysCalib = int.Parse(Calibration[0].Split('=')[1]);
                var gyroCalib = int.Parse(Calibration[1].Split('=')[1]);
                var accelCalib = int.Parse(Calibration[2].Split('=')[1]);
                var magCalib = int.Parse(Calibration[3].Split('=')[1]);
                output.Calibration = (Bno055CalibrationFlags)(
                    sysCalib << 6 |
                    gyroCalib << 4 |
                    accelCalib << 2 |
                    magCalib);

                //Temperature
                var Temperature = float.Parse(data_split[7].Split('=')[1].Split(';')[0]);
                output.Temperature = Temperature;
                return output;
            });
        }
    }

    /// <summary>
    /// Represents the 3D-orientation data produced by a Bosch Bno55 9-axis inertial measurement unit (IMU).
    /// </summary>
    public struct Bno055DataFrame
    {
        /// <summary>
        /// Gets the 3D orientation in Euler angle format with units of degrees.
        /// </summary>
        public Point3f Euler;

        /// <summary>
        /// Gets the three axis of rotation speed in rad/s.
        /// </summary>
        public Point3f Gyroscope;

        /// <summary>
        /// Gets the three axis of linear acceleration data (acceleration minus gravity) in m/s^2.
        /// </summary>
        public Point3f LinearAccel;

        /// <summary>
        /// Gets the three axis of magnetic field sensing in micro Tesla (uT).
        /// </summary>
        public Point3f Magnetometer;

        /// <summary>
        /// Gets the three axis of acceleration (gravity + linear motion) in m/s^2.
        /// </summary>
        public Point3f Accelerometer;

        /// <summary>
        /// Gets the three axis of gravitational acceleration (minus any movement) in m/s^2.
        /// </summary>
        public Point3f Gravity;

        /// <summary>
        /// Gets the ambient temperature in degrees celsius.
        /// </summary>
        public float Temperature;

        /// <summary>
        /// Gets the sensor fusion calibration status.
        /// </summary>
        public Bno055CalibrationFlags Calibration;
    }

    /// <summary>
    /// Specifies the sensor fusion calibration status.
    /// </summary>
    [Flags]
    public enum Bno055CalibrationFlags : byte
    {
        /// <summary>
        /// Specifies that no sub-system is calibrated.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the magnetometer is poorly calibrated.
        /// </summary>
        MagnetometerLow = 0b0000_0001,

        /// <summary>
        /// Specifies that the magnetometer is partially calibrated.
        /// </summary>
        MagnetometerMed = 0b0000_0010,

        /// <summary>
        /// Specifies that the magnetometer is fully calibrated.
        /// </summary>
        MagnetometerFull = 0b0000_0011,

        /// <summary>
        /// Specifies that the accelerometer is poorly calibrated.
        /// </summary>
        AccelerometerLow = 0b0000_0100,

        /// <summary>
        /// Specifies that the accelerometer is partially calibrated.
        /// </summary>
        AccelerometerMed = 0b0000_1000,

        /// <summary>
        /// Specifies that the accelerometer is fully calibrated.
        /// </summary>
        AccelerometerFull = 0b0000_1100,

        /// <summary>
        /// Specifies that the gyroscope is poorly calibrated.
        /// </summary>
        GyroscopeLow = 0b0001_0000,

        /// <summary>
        /// Specifies that the gyroscope is partially calibrated.
        /// </summary>
        GyroscopeMed = 0b0010_0000,

        /// <summary>
        /// Specifies that the gyroscope is fully calibrated.
        /// </summary>
        GyroscopeFull = 0b0011_0000,

        /// <summary>
        /// Specifies that sensor fusion is poorly calibrated.
        /// </summary>
        SystemLow = 0b0100_0000,

        /// <summary>
        /// Specifies that sensor fusion is partially calibrated.
        /// </summary>
        SystemMed = 0b1000_0000,

        /// <summary>
        /// Specifies that sensor fusion is fully calibrated.
        /// </summary>
        SystemFull = 0b1100_0000,

    }
}
