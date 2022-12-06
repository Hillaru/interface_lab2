using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace interface_lab2
{
    public struct EffData
    {
        public int RepeatCount;
        public double ShutdownAvg;
        public double RequestAvg;
        public double P1;
        public double P2;
    }

    internal class FlowEff
    {
        Time_calc Calculator = new Time_calc();
        Random rand = new Random();

        public double Calc_eff(CalcData CData, EffData Data)
        {
            double Shutdown_time;
            double tmp;
            List<double> Requests;
            double CurTime;
            double EndTime;
            int ReqQ;
            int ii;
            double StackTime;

            Calculator.Set_vars(CData);
            int iteration = 0;
            double Effectiveness = 0;
            rand = new Random(DateTime.Now.Millisecond);
            while (iteration < Data.RepeatCount)
            {
                Shutdown_time = -Data.ShutdownAvg * Math.Log(rand.NextDouble());

                Requests = new List<double>();
                tmp = 0;

                while (true)
                {
                    tmp += -Data.RequestAvg * Math.Log(rand.NextDouble());
                    if (tmp >= Shutdown_time)
                        break;
                    Requests.Add(tmp);
                }

                CurTime = 0;
                EndTime = Double.MaxValue;
                ReqQ = 0;
                ii = 0;
                StackTime = 0;

                while (ii < Requests.Count)
                {
                    if (Requests[ii] < EndTime)
                    {
                        CurTime = Requests[ii];

                        if (ReqQ == 1)
                            StackTime = CurTime;
                        else if (ReqQ == 0)
                            EndTime = CurTime + Calculator.Calc_way();

                        ReqQ++;
                        ii++;
                    }
                    else
                    {
                        ReqQ--;
                        CurTime = EndTime;

                        if (ReqQ == 1)
                        {
                            Effectiveness += (CurTime - StackTime) * Data.P1;
                            EndTime = CurTime + Calculator.Calc_way();
                        }
                        else if (ReqQ > 0)
                        {
                            EndTime = CurTime + Calculator.Calc_way();
                        }
                        else if (ReqQ == 0)
                        {
                            EndTime = Double.MaxValue;
                        }
                    }

                    if (EndTime >= Shutdown_time && EndTime != Double.MaxValue)
                    {
                        if (ReqQ > 1)
                            Effectiveness += (Shutdown_time - StackTime) * Data.P1;
                        Effectiveness += Calculator.Calc_way() * Data.P2;
                        break;
                    }
                }   
                
                if (EndTime < Shutdown_time)
                {
                    Effectiveness += Calculator.Calc_way() * Data.P2;
                }

                iteration++;
            }
            return Effectiveness / Data.RepeatCount;
        }
    }
}
