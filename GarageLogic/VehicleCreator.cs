using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    /// <summary>
    /// Vehicle Creator is the only Class in Ex03.GarageLogic that will be modified if a
    /// new vehicle will be supported by the garage. 
    /// </summary>
    public class VehicleCreator
    {
        private static readonly List<string> sr_VehicleOptions = new List<string>();

        static VehicleCreator()
        {
            sr_VehicleOptions.Add("Fuel Car            press 1");
            sr_VehicleOptions.Add("Electric Car        press 2");
            sr_VehicleOptions.Add("Fuel Motorcycle      press 3");
            sr_VehicleOptions.Add("Electric Motorcycle  press 4");
            sr_VehicleOptions.Add("Truck               press 5");
        }

        public enum eVehicleType
        {
            FuelCar = 1,
            ElectricCar,
            FuelMotorcycle,
            ElectricMotorcycle,
            Truck,
        }

        public enum eQuestionNumber
        {
            Question1,
            Question2,
            Question3,
            Question4,
            Question5,
            Question6,
            Question7,
            Question8
        }

        public static List<string> VehicleOptions
        {
            get
            {
                return sr_VehicleOptions;
            }
        }

        internal static Vehicle CreateVehicle (string i_LicenseNumber, eVehicleType i_VehicleType)
        {
            Vehicle o_NewVehicleToReturn;

            switch (i_VehicleType)
            {
                case eVehicleType.FuelCar:
                    {
                        o_NewVehicleToReturn = new Car(i_LicenseNumber, new FuelEngine());
                        break;
                    }
                case eVehicleType.ElectricCar:
                    {
                        o_NewVehicleToReturn = new Car(i_LicenseNumber, new ElectricEngine());
                        break;
                    }
                case eVehicleType.FuelMotorcycle:
                    {
                        o_NewVehicleToReturn = new Motorcycle(i_LicenseNumber, new FuelEngine());
                        break;
                    }
                case eVehicleType.ElectricMotorcycle:
                    {
                        o_NewVehicleToReturn = new Motorcycle(i_LicenseNumber, new ElectricEngine());
                        break;
                    }
                case eVehicleType.Truck:
                    {
                        o_NewVehicleToReturn = new Truck(i_LicenseNumber, new FuelEngine());
                        break;
                    }
                default:
                    {
                        o_NewVehicleToReturn = null;
                        break;
                    }
            }

            return o_NewVehicleToReturn;
        }
    }
}
