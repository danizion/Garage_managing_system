using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;

namespace ConsoleUI
{

    public class Menu
    {
        private readonly List<string> r_MenuOptions = new List<string>();
        private readonly List<string> r_AddVehicleOptions;

        public Menu(List<string> i_VehicleOptions)
        {
            r_MenuOptions.Add("=============================================");
            r_MenuOptions.Add("1. Add a new vehicle to the garage");
            r_MenuOptions.Add("2. Show the list of all the vehicles");
            r_MenuOptions.Add("3. Change status for a vehicle");
            r_MenuOptions.Add("4. Add air pressure");
            r_MenuOptions.Add("5. Add fuel (only for patrol vehicle)");
            r_MenuOptions.Add("6. Charge battery (only for electric vehicle");
            r_MenuOptions.Add("7. Show a vehicle's data");
            r_MenuOptions.Add("8. Exit Program");
            r_MenuOptions.Add("=============================================");
            r_AddVehicleOptions = i_VehicleOptions;
        }

        public List<string> MenuOptions
        {
            get
            {
                return r_MenuOptions;
            }
        }

        public List<string> AddVehicleOptionsMenu
        {
            get
            {
                return r_AddVehicleOptions;
            }
        }
    }
}
