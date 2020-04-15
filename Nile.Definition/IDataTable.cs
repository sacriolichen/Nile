using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    public interface IDataTable : ICommonComponent
    {
        #region AddValue
        /// <summary>
        /// This method adds the value <paramref name="Value"/> to the single value
        /// table under the tag <paramref name="Name"/>.  If it finds an existing entry
        /// with the same name, it will be replaced.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Position">Position number</param>
        /// <remarks>
        /// The method should first check if the entry already exists, and if so write over it.
        /// </remarks>
        void AddValue(string Name, object Value, int Position);
        #endregion

        #region GetValue
        /// <summary>
        /// This method retrieves the value stored under <paramref name="Name"/>.
        /// </summary>
        /// <param name="Name">The tag used to identify the value.</param>
        /// <param name="Position">Position number</param>
        /// <remarks>
        object GetValue(string Name, int Position);
        #endregion

        #region RemoveValueFromTable
        /// <summary>
        /// This method removes the single value with <paramref name="Name"/> from the table
        /// if it exists.
        /// </summary>
        /// <param name="Name">The name of the single value to remove fromt he table.</param>
        /// <param name="Position">Position number</param>
        /// <returns>True if a matching entry was removed, false otherwise.</returns>
        bool RemoveValueFromTable(string Name, int Position);
        #endregion

        #region ValueExists
        /// <summary>
        /// This method checks to see if a single value with <paramref name="Name"/> exists in the table.
        /// </summary>
        /// <param name="Name">The tag to look for in the table.</param>
        /// <param name="Position">Position number</param>
        /// <returns>True if a single value with that tag is inthe table, false otherwise.</returns>
        bool ValueExists(string Name, int Position);
        #endregion

        #region ValueExists
        /// <summary>
        /// This method checks to see if a single value with <paramref name="Name"/> exists in the table.
        /// </summary>
        /// <param name="Name">The tag to look for in the table.</param>
        /// <param name="Position">Position numbers which contains the name</param>
        /// <returns>True if a single value with that tag(s) is(are) inthe table, false otherwise.</returns>
        bool ValueExists(string Name, out int[] Position);
        #endregion

        #region GetAllNames
        /// <summary>
        /// This method gets the names of all single values in the data table.
        /// </summary>
        /// <returns>String array containing the single value names.</returns>
        string[] GetAllNames(string Name, int Position);
        #endregion
    }
}
