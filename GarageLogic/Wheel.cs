using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private const float k_MinAmountToAdd = 0;

        private readonly float r_MaxAirPressure;
        private string m_NameOfManufacturer;
        private float m_CurrentAirPressure;

        public Wheel(float i_MaxAirPressure)
        {
            r_MaxAirPressure = i_MaxAirPressure;
        }

        internal string  Manufacturer
        {
            get
            {
                return m_NameOfManufacturer;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value == " ")
                {
                    throw new Exception("Manufacturer name can't be empty!");
                }
                m_NameOfManufacturer = value;
            }
        }

        internal float CurrentPressure
        {
            get
            {
                return m_CurrentAirPressure;
            }
            set
            {
                if (value > r_MaxAirPressure || value < k_MinAmountToAdd)
                {
                    throw new ValueOutOfRangeException("Current Pressure", k_MinAmountToAdd, r_MaxAirPressure);
                }
                m_CurrentAirPressure = value;
            }
        }

        internal float MaxPressure
        {
            get
            {
                return r_MaxAirPressure;
            }
        }

        // Not used in this program, but implemented for general cases 
        internal void InflateWheel (float i_AirToAddToCurrentPressure)
        {
            float newPressureIfAdded = i_AirToAddToCurrentPressure + m_CurrentAirPressure;

            if (newPressureIfAdded > r_MaxAirPressure || newPressureIfAdded < 0)
            {
                throw new ValueOutOfRangeException("Air Pressure", k_MinAmountToAdd, r_MaxAirPressure);
            }
            else
            {
                m_CurrentAirPressure += i_AirToAddToCurrentPressure;
            }
        }

        internal void InflateWheelToMax()
        {
            m_CurrentAirPressure = r_MaxAirPressure;
        }
    }
}
