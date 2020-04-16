////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// Class, DutInfo.
//
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    public class DutInfo
    {
        /// <summary>
        /// The number used in factory for tracking
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// The number get from cetification authority, such as IMEI or others
        /// </summary>
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// MAC address, wifi
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// BlueTooth
        /// </summary>
        public string IeeeAddress { get; set; }

        /// <summary>
        /// The number of defined modle or other
        /// </summary>
        public string ProductNumber { get; set; }

        /// <summary>
        /// revision number of product
        /// </summary>
        public string ProductRevision { get; set; }

        /// <summary>
        /// The number of position in fixture
        /// </summary>
        public int Position { get; set; }
    }
}
