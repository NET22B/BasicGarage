
using System;
namespace MyGarage
{
    [Serializable]
    class Boat : Vehicle
    {
        public bool PassengerShip { get; private set; }
        public bool SailBoat { get; private set; }

        public Boat(string regNum, string color, int wheelCount, bool passengerShip, bool sailBoat)
            : base(regNum, color, wheelCount)
        {
            PassengerShip = passengerShip;
            SailBoat = sailBoat;
        }

        public Boat(Vehicle v, bool passengerShip, bool sailBoat)
            : base(v.RegNum, v.Color, v.WheelCount)
        {
            PassengerShip = passengerShip;
            SailBoat = sailBoat;
        }

        public override string ToString()
        {
            return string.Format("Båt: {0}, Passagerarfartyg: {1}, Segelbåt: {2}",
                base.ToString(), PassengerShip ? "Ja" : "Nej", SailBoat ? "Ja" : "Nej");
        }

        public override bool Matches(string keyword)
        {
            return base.Matches(keyword) ||
                (PassengerShip && (keyword.ToLower() == "passagerarfartyg")) ||
                (SailBoat && (keyword.ToLower() == "segelfartyg"));
        }
    }
}
