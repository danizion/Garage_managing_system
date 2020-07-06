using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        private const int k_LicenseTypeLocation = 6;
        private const int k_EngineVolumeLocation = 7;
        private const int k_NumberOfWheels = 2;
        private const float k_MaxAirPressure = 30;

        private static readonly List<string> sr_MotorcycleQuestions;

        private eLicenseType m_LicenseType;
        private int m_EngineVolume;

        static Motorcycle()
        {
            sr_MotorcycleQuestions = new List<string>
            {
                "7. Please Enter the License Type: 1 for A, 2 for A1, 3 for AA or 4 for B: ",
                "8. Please enter the Engine volume: (Should be an Integer)"
            };

        }

        public Motorcycle(string i_LicenseNumber, Engine i_Engine) : base(i_LicenseNumber, i_Engine)
        {
            m_WheelCollection = new Wheel[k_NumberOfWheels];

            for (int i = 0; i < k_NumberOfWheels; i++)
            {
                m_WheelCollection[i] = new Wheel(k_MaxAirPressure);
            }

            if (m_Engine is FuelEngine)
            {
                (m_Engine as FuelEngine).FuelType = FuelEngine.eFuleType.Octan95;
                (m_Engine as FuelEngine).MaxCapacity = 5.5f;
                m_VehicleType = VehicleCreator.eVehicleType.FuelMotorcycle;
            }
            else
            {
                (m_Engine as ElectricEngine).MaxCapacity = 1.6f;
                m_VehicleType = VehicleCreator.eVehicleType.ElectricMotorcycle;
            }
        }

        enum eLicenseType
        {
            A = 1,
            A1,
            AA,
            B
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

                foreach (string question in sr_MotorcycleQuestions)
                {
                    o_AllQuestions.Add(question);
                }

                return o_AllQuestions;
            }
        }

        internal int Enginevolume
        {
            get
            {
                return m_EngineVolume;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Engine Volume must be a Positive integer!");
                }
                else
                {
                    m_EngineVolume = value;
                }
            }
        }

        internal override void SetData(List<string> i_DataFromUser)
        {
            base.SetData(i_DataFromUser);
            int licenseTypeAsInt = int.Parse(i_DataFromUser[k_LicenseTypeLocation]);

            for (int i = 0; i < k_NumberOfWheels; i++)
            {
                m_WheelCollection[i].CurrentPressure = float.Parse(i_DataFromUser[k_WheelAirPressureLocation]);
                m_WheelCollection[i].Manufacturer = i_DataFromUser[k_WheelManufacturerLocation];
            }

            if (!Enum.IsDefined(typeof(eLicenseType), licenseTypeAsInt))
            {
                throw new ArgumentException("Car color must be Red, White, Black Or Silver only");
            }

            m_LicenseType = (eLicenseType)licenseTypeAsInt;
            Enginevolume = int.Parse(i_DataFromUser[k_EngineVolumeLocation]);
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
                        o_IsValid = validateLicenseType(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                case VehicleCreator.eQuestionNumber.Question8:
                    {
                        o_IsValid = validateEngineVolume(i_AnswerToCheck, out o_ErrorMessage);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return o_IsValid;
        }

        private bool validateEngineVolume(string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool o_IsValid = int.TryParse(i_AnswerToCheck, out int answerAsInt);

            if (o_IsValid == false || answerAsInt <= 0 || answerAsInt > 2500)
            {
                o_ErrorMessage = "Error: Engine Volume must be a positive Integer Value not more than 2500";
                o_IsValid = false;
            }
            else
            {
                o_ErrorMessage = "None";
            }

            return o_IsValid;
        }

        private bool validateLicenseType (string i_AnswerToCheck, out string o_ErrorMessage)
        {
            bool o_IsValid = int.TryParse(i_AnswerToCheck, out int answerAsInt);

            if (o_IsValid == false || Enum.IsDefined(typeof(eLicenseType), answerAsInt) == false)
            {
                o_ErrorMessage = "Error: Your choice for License Type should be 1, 2 , 3 or 4";
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

            string motorcycleData = string.Format(@"
License Type        -   {0}
Engine Volume       -   {1}
",
                m_LicenseType, m_EngineVolume);

            o_VehicleData += motorcycleData;

            return o_VehicleData;

        }
    }
}
