using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class GarageLogicManager
    {
        private readonly Dictionary<string, Vehicle> r_Vehicles = new Dictionary<string, Vehicle>();
        private int m_NumberOfElectricVehicles;
        private int m_NumberOfFuelVehicles;

        public Dictionary<string, Vehicle> ListOfVehicles
        {
            get
            {
                return r_Vehicles;
            }
        }

        public int NumberOfElectricVehicles
        {
            get
            {
                return m_NumberOfElectricVehicles;
            }
        }

        public int NumberOfFuelVehicles
        {
            get
            {
                return m_NumberOfFuelVehicles;
            }
        }

        public List<string> GetQuestions (string i_LicenseNumber)
        {
            Vehicle vehicle = r_Vehicles[i_LicenseNumber];

            return vehicle.Questions;
        }

        public void SetNewVehicleData(string i_LicenseNumber, List<string> i_VehicleData)
        {
            Vehicle newVehicle = r_Vehicles[i_LicenseNumber];
            newVehicle.SetData(i_VehicleData);
        }

        public void AddNewVehicleToGarage(string i_LicenseNumber, VehicleCreator.eVehicleType i_VehicleType)
        {
            if (r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                r_Vehicles[i_LicenseNumber].VehicleStatus = Vehicle.eVehicleStatus.InRepair;
            }
            else
            {
                Vehicle newVehicle = VehicleCreator.CreateVehicle(i_LicenseNumber, i_VehicleType);
                r_Vehicles.Add(i_LicenseNumber, newVehicle);
                if (newVehicle.Engine is FuelEngine)
                {
                    m_NumberOfFuelVehicles++;
                }
                else
                {
                    m_NumberOfElectricVehicles++;
                }
            }
        }

        public void ChangeVehicleStatusInGarage(string i_LicenseNumber, Vehicle.eVehicleStatus i_NewVehicleStatus)
        {
            IsGarageEmpty();

            if ((r_Vehicles.ContainsKey(i_LicenseNumber)) == true)
            {
                (r_Vehicles[i_LicenseNumber].VehicleStatus) = i_NewVehicleStatus;
            }
            else
            {
                throw new Exception("We don't have this vehicle in our garage, sorry.");
            }
        }
            
        public void IsGarageEmpty()
        {
            if (r_Vehicles.Count == 0)
            {
                throw new Exception("There are no vehicle in the garage yet.");
            }
        }

        public void AddFuelToVehicle(string i_LicenseNumber, FuelEngine.eFuleType i_TypeFuelToFill, float i_AmountFuelToFill)
        {
            IsGarageEmpty();

            if (!(r_Vehicles.ContainsKey(i_LicenseNumber)))
            {
                throw new Exception("We don't have this vehicle in our garage, sorry.");
            }

            Vehicle vehicle = r_Vehicles[i_LicenseNumber];

            if (!(vehicle.Engine is FuelEngine))
            {
                throw new Exception("This is not a Fuel driven Vehicle, please try again");
            }

            (vehicle.Engine as FuelEngine).addFuel(i_AmountFuelToFill, i_TypeFuelToFill);
        }

        public void InflateWheelToMax(string i_LicenseNumber)
        {
            Vehicle vehicle = r_Vehicles[i_LicenseNumber];

            foreach (Wheel wheel in vehicle.Wheels)
            {
                wheel.InflateWheelToMax();
            }
        }

        public string GetVehicleData(string i_LicenseNumber)
        {
            IsGarageEmpty();

            string o_VehicleData;

            if (!r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new Exception("We don't have this vehicle in our garage, sorry.");
            }
            else
            {   
                o_VehicleData = r_Vehicles[i_LicenseNumber].GatVehicleData();
            }

            return o_VehicleData;
        }

        public void AddHoursToElectricEngine(string i_LicenseNumber, float i_AmountHoursToFill)
        { 
            IsGarageEmpty();
            if (!(r_Vehicles.ContainsKey(i_LicenseNumber)))
            {
                throw new Exception("We don't have this vehicle in our garage, sorry.");
            }

            Vehicle vehicle = r_Vehicles[i_LicenseNumber];

            if (!(vehicle.Engine is ElectricEngine))
            {
                throw new Exception("This is not an electric vehicle, please try again");
            }
            else
            {
                (vehicle.Engine as ElectricEngine).ChargeBattery(i_AmountHoursToFill);
            }
        }

        public bool checkValidity(int i_QuestionToCheck, string i_AnswerToCheck, out string io_ErrorMessage, string i_LicenseNumber)
        {
            bool isValid = r_Vehicles[i_LicenseNumber].CheckValidity(i_QuestionToCheck, i_AnswerToCheck, out io_ErrorMessage);
            return isValid;
        }

        public bool CheckIfElectricEngine (Vehicle i_Vehicle)
        {
            return i_Vehicle.Engine is ElectricEngine;  
        }

        public bool CheckIfFuelEngine(Vehicle i_Vehicle)
        {
            return i_Vehicle.Engine is FuelEngine;
        }

        public bool isFuelVehicle (string i_LicenseNumber)
        {
            if (!r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new Exception("No such Vehicle in the Garage!");
            }

            return r_Vehicles[i_LicenseNumber].Engine is FuelEngine;
        }

        public bool isElectricVehicle(string i_LicenseNumber)
        {
            if (!r_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new Exception("No such Vehicle in the Garage!");
            }

            return r_Vehicles[i_LicenseNumber].Engine is ElectricEngine;
        }
    }
}
