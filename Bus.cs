
using System;
namespace MyGarage
{
    [Serializable]
    class Bus : Vehicle
    {
        public int NumerOfSeats { get; private set; }
        public bool IsDoubleDecker { get; private set; }

        public Bus(string regNum, string color, int wheelCount, int numerOfSeats, bool isDoubleDecker)
            : base(regNum, color, wheelCount)
        {
            NumerOfSeats = numerOfSeats;
            IsDoubleDecker = isDoubleDecker;
        }

        public Bus(Vehicle v, int numerOfSeats, bool isDoubleDecker)
            : base(v.RegNum, v.Color, v.WheelCount)
        {
            NumerOfSeats = numerOfSeats;
            IsDoubleDecker = isDoubleDecker;
        }

        public override string ToString()
        {
            return string.Format("Buss: {0}, Antal platser: {1}, Dubbeldeckare: {2}",
                base.ToString(), NumerOfSeats, IsDoubleDecker ? "Ja" : "Nej");
        }

        public override bool Matches(string keyword)
        {
            return base.Matches(keyword) ||
                NumerOfSeats.ToString() == keyword ||
                (IsDoubleDecker && (keyword.ToLower() == "dubbeldeckare"));
        }
    }
}
