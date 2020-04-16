using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    /// <summary>
    /// to store the file name and class name which support an interface
    /// </summary>
    public struct SessionInput
    {
        public string File { get; set; }
        public string Class { get; set; }

        public SessionInput(string FileName, string ClassName)
        {
            this.File = FileName;
            this.Class = ClassName;
        }
    }

    public interface ISessionManager
    {
        object CreateSession(string SessionName, int Position = 1);
        object CreateSession(string SessionName, string FileName, string ClassName, int Position = 1);
        object GetSessionByName(string SessionName, int Position);
        void Remove(string SessionName, int Position);
    }

    interface IArea:ICommonComponent
    {
        void Circle(double Radius);
        void Quadrate(double SideLength);
        double RegularTriangle(int sidelength);
        //bool IsInitialized { get; }
    }
}

