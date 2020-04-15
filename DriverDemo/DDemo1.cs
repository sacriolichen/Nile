using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nile.Definition;

namespace Nile.Component
{
    public class PrintArea : ComponentBase, IArea
    {
        private int Size;
        private int Length;
        private int Area;

        #region public member
        public PrintArea()
        {
        }

        public double Circle(double Radius)
        {
            Console.WriteLine(string.Format("The area of the circle (r={0}) is {1}", Radius, Math.PI * Math.Pow(Radius, 2)));
            return Math.PI * Math.Pow(Radius, 2);
        }

        public double Quadrate(double SideLength)
        {
            Console.WriteLine(string.Format("The area of the quadrate (l={0}) is {1}", SideLength, Math.Pow(SideLength, 2)));
            Length = 0 - Length;
            return Length * Math.Pow(SideLength, 2);
        }

        public double RegularTriangle(int sidelength)
        {
            Console.WriteLine(string.Format("The area of the regular triangle (l={0}) is {1}", sidelength, 0.4330127 * Math.Pow(sidelength, 2)));
            return 0.4330127 * Math.Pow(sidelength, 2);
        }
        #endregion

        #region interface member
        public override bool IsInitialized { get; set; }
        public override void Initialize(Dictionary<string, object> Options)
        {
            base.Initialize(Options);
            try
            {
                Size = 0;
                Length = 0;
                Area = 0;
                Size = Convert.ToInt32( GetConfig("Size", true, 0));
                Length = Convert.ToInt32(GetConfig("Length", true, 10));
                Area = Convert.ToInt32(GetConfig("area", false, -1));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.-> {1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
            IsInitialized = true;
        }

        public override void Reset()
        {
            throw new System.NotImplementedException("To implemented once needed.");
        }

        public override void Send(string Command)
        {
            throw new System.NotImplementedException("To implemented once needed.");
        }
        public override string Receive()
        {
            throw new System.NotImplementedException("To implemented once needed.");
        }
        #endregion
    }
}