
using System;
namespace MyGarage
{
    [Serializable]
    class Airplane : Vehicle
    {
        public int WingSpan { get; private set; }
        public bool HasJetEngines { get; private set; }

        public Airplane(string regNum, string color, int wheelCount, int wingSpan, bool hasJetEngines)
            : base(regNum, color, wheelCount)
        {
            WingSpan = wingSpan;
            HasJetEngines = hasJetEngines;
        }

        public Airplane(Vehicle v, int wingSpan, bool hasJetEngines)
            : base(v.RegNum, v.Color, v.WheelCount)
        {
            WingSpan = wingSpan;
            HasJetEngines = hasJetEngines;
        }

        public override string ToString()
        {
            return string.Format("Flygplan: {0}, Vingbredd: {1}, Jetplan: {2}",
                base.ToString(), WingSpan, HasJetEngines ? "Ja" : "Nej");
        }

        public override bool Matches(string keyword)
        {
            return base.Matches(keyword) ||
                WingSpan.ToString() == keyword ||
                (HasJetEngines && keyword.ToLower() == "jet");
        }
    }
}
