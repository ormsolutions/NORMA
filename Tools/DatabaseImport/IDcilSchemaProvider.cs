#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace ORMSolutions.ORMArchitect.DatabaseImport
{
    /// <summary>
    /// Represents required methods for DBMS adapters
    /// </summary>
    public interface IDcilSchemaProvider
    {
        /// <summary>
        /// When implemented in a child class, retrieves a list of available schema names
        /// </summary>
        /// <returns>List of available schema names</returns>
        IList<string> GetAvailableSchemaNames();
        /// <summary>
        /// When implemented in a child class, loads the specified Schema into a <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilSchema"/> object
        /// </summary>
        /// <param name="schemaName">Name of the Schema to load</param>
        /// <returns><see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilSchema"/> object</returns>
        DcilSchema LoadSchema(string schemaName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilTable"/> objects for the specified Schema
        /// </summary>
        /// <param name="schemaName">Name of the Schema from which to load the Tables</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilTable"/> objects for the specified Schema</returns>
        IList<DcilTable> LoadTables(string schemaName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilColumn"/> objects for the specified Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Columns</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilColumn"/> objects for the specified Schema and Table</returns>
        IList<DcilColumn> LoadColumns(string schemaName, string tableName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Indexes</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified Schema and Table</returns>
        IList<DcilUniquenessConstraint> LoadIndexes(string schemaName, string tableName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Indexes</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified Schema and Table</returns>
        IList<DcilReferenceConstraint> LoadForeignKeys(string schemaName, string tableName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilProcedure"/> objects for the specified Schema
        /// </summary>
        /// <param name="schemaName">Name of the Schema from which to laod the Stored Procedures</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilProcedure"/> objects for the specified Schema</returns>
        IList<DcilProcedure> LoadProcedures(string schemaName);
    }
}
