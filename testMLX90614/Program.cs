using System.Threading;
using Microsoft.SPOT;

using MicroToolsKit;

namespace testMLX90614
{
    public class Program
    {
        public static void Main()
        {
            MLX90614 tempIR = new MLX90614();

            while (true)
            {
                Debug.Print("Température Air = " + tempIR.Read_TA_AsCelcius().ToString("F1") + " °C");
                Debug.Print("Température IR = " + tempIR.Read_Tobj_AsCelcius().ToString("F1") + " °C");
                Debug.Print("---------------------");
                Thread.Sleep(200);
            }
        }
    }
}
