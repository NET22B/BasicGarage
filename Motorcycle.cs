
using System;
namespace MyGarage
{
    [Serializable]
    class Motorcycle : Vehicle
    {
        public int TopSpeed { get; private set; }
        public bool HasSideCar { get; private set; }

        public Motorcycle(string regNum, string color, int wheelCount, int topSpeed, bool hasSideCar)
            : base(regNum, color, wheelCount)
        {
            TopSpeed = topSpeed;
            HasSideCar = hasSideCar;
        }

        public Motorcycle(Vehicle v, int topSpeed, bool hasSideCar)
            : base(v.RegNum, v.Color, v.WheelCount)
        {
            TopSpeed = topSpeed;
            HasSideCar = hasSideCar;
        }

        public override string ToString()
        {
            return string.Format("Motorcykel: {0}, Toppfart: {1}, Sidovagn: {2}",
                base.ToString(), TopSpeed, HasSideCar ? "Ja" : "Nej");
        }

        public override bool Matches(string keyword)
        {
            return base.Matches(keyword) ||
                TopSpeed.ToString() == keyword ||
                (HasSideCar && (keyword.ToLower() == "sidovagn" || keyword.ToLower() == "side car"));
        }
    }
}
