using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private const int k_IsDangerousMaterialLocation = 6;
        private const int k_TrunkVolumeLocation = 7;
        private const int k_NumberOfWheels = 16;
        private const float k_MaxAirPressure = 26;

        private static readonly List<string> sr_TruckQuestions;

        private bool m_IsContainingDangerousMaterial;
        private float m_TrunkVolumeInLiter;



        static Truck()
        {
            sr_TruckQuestions = new List<string>
            {
                "7. Please enter if the Truck is loaded Dangerous Material 1 for true or 0 for false: ",
                "8. Please enter the Truck's Container Volume in liters: "
            };

        }

        public Truck (string i_LicenseNumber, Engine i_Engine) : base(i_LicenseNumber, i_Engine)
        {
            m_WheelCollection = new Wheel[k_NumberOfWheels];

            for (int i = 0; i < k_NumberOfWheels; i++)
            {
                m_WheelCollection[i] = new Wheel(k_MaxAirPressure);
            }

            (m_Engine as FuelEngine).FuelType = FuelEngine.eFuleType.Soler;
            (m_Engine as FuelEngine).MaxCapacity = 120;
            m_VehicleType = VehicleCreator.eVehicleType.Truck;
        }

        internal override List<string> Questions
        {
            get
            {
                List<string> o_AllQuestions = new List<string>();

                foreach (string question in base.Questions)
                {
                    o_AllQuestions.Add(question);
                }

                foreach (string question in sr_TruckQuestions)
                {
                    o_AllQuestions.Add(question);
                }

                return o_AllQuestions;
            }
        }

        internal override void SetData(List<string> i_DataFromUser)
        {
            base.SetData(i_DataFromUser);

            for (int i = 0; i < k_NumberOfWheels; i++)
            {
                m_WheelCollection[i].CurrentPressure = float.Parse(i_DataFromUser[k_WheelAirPressureLocation]);
                m_WheelCollection[i].Manufacturer = i_DataFromUser[k_WheelManufacturerLocation];
            }

            m_TrunkVolumeInLiter = float.Parse(i_DataFromUser[k_TrunkVolumeLocation]);
            int answerAsInt =  int.Parse(i_DataFromUser[k_IsDangerousMaterialLocation]);
            m_IsContainingDangerousMaterial = Convert.ToBoolean(answerAsInt);
        }

        internal override bool CheckValidity(int i_QuestionToCheck, string i_AnswerToCheck, out string o_ErrorMessage)
        {
            VehicleCreator.eQuestionNumber questionNumber = (VehicleCreator.eQuestionNumber)i_QuestionToCheck;
            bool o_IsValid = true;
            o_ErrorMessage = "None";
            o_IsValid = base.CheckValidity(i_QuestionToCheck, i_AnswerToCheck, out o_ErrorMessage);

            switch (questionNumber)
            {
                case VehicleCreator.eQuestionNumber.Question7:
                    {
                        o_IsValid = validateDangerousMaterial(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question8:
                    {
                        o_IsValid = validateTrunkVolume(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return o_IsValid;
        }

        private bool validateDangerousMaterial(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool o_IsValid = int.TryParse(i_AnswerToCheck, out int answerAsInt);

            if (o_IsValid == false || (answerAsInt != 0 && answerAsInt != 1))
            {
                o_ErrorMessage = "Error: your answer should be 1 or 0";
                o_IsValid = false;
            }
            else
            {
                o_ErrorMessage = "None";
            }

            return o_IsValid;
        }

        private bool validateTrunkVolume(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool o_IsValid = float.TryParse(i_AnswerToCheck, out float answerAsInt);

            if (o_IsValid == false || answerAsInt <= 0 || answerAsInt > 50000)
            {
                o_ErrorMessage = "Error: Trunk Volume must be a positive Integer Value not more than 50000";
                o_IsValid = false;
            }
            else
            {
                o_ErrorMessage = "None";
            }

            return o_IsValid;
        }

        internal override string GatVehicleData()
        {
            string o_VehicleData = base.GatVehicleData();

            string TrucklData = string.Format(@"
Dangerous Materials -   {0}
TrunkVolume         -   {1}
",
                m_IsContainingDangerousMaterial.ToString(), m_TrunkVolumeInLiter);

            o_VehicleData += TrucklData;

            return o_VehicleData;

        }
    }
}
