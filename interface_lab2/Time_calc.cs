using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interface_lab2
{
    public class Joint
    {
        Random rand = new Random(DateTime.Now.Millisecond);
        string _value;

        public double value
        {
            get
            {
                if (string.IsNullOrEmpty(_value))
                {
                    return -1;
                }

                string[] split_str = _value.Split('-');
                if (split_str.Length == 2)
                {
                    if (Double.TryParse(split_str[0], out double plain_a) && Double.TryParse(split_str[1], out double plain_b))
                        return (rand.NextDouble() * (plain_b - plain_a) + plain_a);
                }

                if (Double.TryParse(_value, out double i) == true)
                {
                    return i;
                }
                else
                    return -1;
            }
        }

        public Joint(string s)
        {
            _value = s;
        }
    }

    public struct CalcData
    {
        public List<string> Joints;
        public List<double> Way_chances;
        public List<List<int>> Ways;
        public double Error_chance;
    }

    internal class Time_calc
    {
        CalcData Data;
        public List<Joint> Joints;
        Random rand = new Random();

        public Time_calc()
        {

        }

        public void Set_vars(CalcData _Data)
        {
            Joints = new List<Joint>();
            foreach (string j in _Data.Joints)
            {
                Joints.Add(new Joint(j));
            }
            Data.Ways = new List<List<int>>(_Data.Ways);
            Data.Way_chances = _Data.Way_chances;
            Data.Error_chance = _Data.Error_chance;
        }

        public double Calc_way()
        {
            rand = new Random(DateTime.Now.Millisecond);
            double r = rand.NextDouble();
            double cr = 0;
            int Way_index = 0;
            while (Way_index < Data.Way_chances.Count)
            {
                cr += Data.Way_chances[Way_index];
                if (cr >= r)
                {
                    break;
                }
                Way_index++;
            }

            double Result_time = 0;
            int i = 0;
            while (i < Data.Ways[Way_index].Count)
            {
                if (Joints[Data.Ways[Way_index][i] - 1].value == -1)
                    return -1;

                Result_time += Joints[Data.Ways[Way_index][i] - 1].value;
                if (rand.NextDouble() >= Data.Error_chance)
                    i++;
            }

            return Result_time;
        }
    }
}
