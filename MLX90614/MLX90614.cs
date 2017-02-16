using System;
using Microsoft.SPOT.Hardware;

namespace MicroToolsKit
{
    /// <summary>
    /// MLX90614 IR Temperature sensor class
    /// </summary>
    /// <remarks>
    /// You may have some additional information about this class on https://webge.github.io/MLX90614/
    /// </remarks>
    public class MLX90614
    {
        const byte RAM_TA_REGISTER = 0x06;
        const byte RAM_TOBJ1_REGISTER = 0x07;

        /// <summary>
        /// MLX90614 configuration
        /// </summary>
        I2CDevice.Configuration config;

        I2CDevice i2cBus;

        /// <summary>
        /// MLX90614 IR Temperature sensor
        /// </summary>
        /// <param name="SLA">7 bits Slave Address (0x00 to 0x7F) 0x5A by default</param>
        /// <param name="Frequency">10kHz to 100kHz (50kHz by default)</param>
        public MLX90614(ushort Slave=0x5A, Int16 Frequency=50)
        {
            config = new I2CDevice.Configuration(Slave, Frequency);
        }

        /// <summary>
        /// Returns the air temperature in degrees Celsius
        /// </summary>
        public double Read_TA_AsCelcius()
        {
            double dataAsKelvin = getRegister(RAM_TA_REGISTER) * 0.02;
            return dataAsKelvin - 273.15;
        }

        /// <summary>
        /// Returns the object temperature in degrees Celsius
        /// </summary>
        public double Read_Tobj_AsCelcius()
        {
            double dataAsKelvin = getRegister(RAM_TOBJ1_REGISTER) * 0.02;
            return dataAsKelvin - 273.15; 
        }
        /// <summary>
        /// Returns the air temperature in degrees Fahrenheit
        /// </summary>
        public double Read_TA_AsFahrenheit()
        {
            double dataAsKelvin = getRegister(RAM_TA_REGISTER) * 0.02;
            return ((dataAsKelvin - 273.15) * 1.8) + 32;
        }
        /// <summary>
        /// Returns the object temperature in degrees Fahrenheit
        /// </summary>
        public double Read_Tobj_AsFahrenheit()
        {
            double dataAsKelvin = getRegister(RAM_TOBJ1_REGISTER) * 0.02;
            return ((dataAsKelvin - 273.15) * 1.8) + 32;
        }

        /// <summary>
        /// Get 16 bits data in RAM Register
        /// </summary>
        /// <param name="register">Air temperature TA = 0x06, Object temperature Tobj1 = 0x07</param>
        /// <returns>Temperature in Kelvin divided by 0.02</returns>
        UInt16 getRegister(byte register)
        {
            byte[] outbuffer = new byte[] { register };
            byte[] inbuffer = new byte[2];
            I2CDevice.I2CTransaction[] XAction = new I2CDevice.I2CTransaction[] {
                    I2CDevice.CreateWriteTransaction(outbuffer),
                    I2CDevice.CreateReadTransaction(inbuffer)
                };
            i2cBus = new I2CDevice(config);
            int transferred = i2cBus.Execute(XAction, 1000);
            i2cBus.Dispose();
            if (transferred < (outbuffer.Length + inbuffer.Length))
                throw new System.IO.IOException();
            else
                return (UInt16)(((inbuffer[1] & 0x007F) << 8) + inbuffer[0]);
        }
    }
}
