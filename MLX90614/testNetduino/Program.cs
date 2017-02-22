//#define Debug

using System.IO;
using System.Threading;

using testMicroToolsKit.Hardware.IO;
using Microtoolskit.Hardware.Displays;
using Microtoolskit.Hardware.IO;

namespace testNetduino
{
    public class Program
    {
        public static void Main()
        {
            MLX90614 tempIR = new MLX90614();
#if Debug
            while (true)
            {
                try
                {
                    Debug.Print("Température Air = " + tempIR.Read_TA_AsCelcius().ToString("F1") + " °C");
                    Debug.Print("Température IR = " + tempIR.Read_Tobj_AsCelcius().ToString("F1") + " °C");
                    Debug.Print("---------------------");
                }
                catch (IOException ioEx)
                {
                    Debug.Print(ioEx.Message);
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
#else
            // Test with PCF8574 and ELCD162
            PCF8574 Leds = new PCF8574(0x38, 100);
            ELCD162 lcd = new ELCD162();
            lcd.Init(); lcd.ClearScreen();

            while (true)
            {
                try
                {
                    Leds.Write(0xf0);
                    Thread.Sleep(200);
                    Leds.Write(0x0f);
                }
                catch (System.Exception ioEx)
                {
                    lcd.ClearScreen();
                    lcd.PutString(ioEx.Message);
                    Thread.Sleep(1000);
                }
                try
                {
                    double TA = tempIR.Read_TA_AsCelcius();
                    double TO = tempIR.Read_Tobj_AsCelcius();
                    lcd.ClearScreen();
                    lcd.PutString("TA=" + TA.ToString("F1") + "C");
                    lcd.SetCursor(0, 1);
                    lcd.PutString("TO=" + TO.ToString("F1") + "C");
                }
                catch (IOException ioEx)
                {
                    lcd.ClearScreen();
                    lcd.PutString(ioEx.Message);
                }
                finally
                {
                    Thread.Sleep(200);
                }
            }
#endif
        }
    }
}
