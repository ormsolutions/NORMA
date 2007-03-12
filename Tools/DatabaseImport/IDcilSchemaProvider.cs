#region zlib/libpng Copyright Notice
/**************************************************************************\
* Database Intermediate Language                                           *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* This software is provided 'as-is', without any express or implied        *
* warranty. In no event will the authors be held liable for any damages    *
* arising from the use of this software.                                   *
*                                                                          *
* Permission is granted to anyone to use this software for any purpose,    *
* including commercial applications, and to alter it and redistribute it   *
* freely, subject to the following restrictions:                           *
*                                                                          *
* 1. The origin of this software must not be misrepresented; you must not  *
*    claim that you wrote the original software. If you use this software  *
*    in a product, an acknowledgment in the product documentation would be *
*    appreciated but is not required.                                      *
*                                                                          *
* 2. Altered source versions must be plainly marked as such, and must not  *
*    be misrepresented as being the original software.                     *
*                                                                          *
* 3. This notice may not be removed or altered from any source             *
*    distribution.                                                         *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Neumont.Tools.ORM.DatabaseImport
{
    /// <summary>
    /// Represents required methods for DBMS adapters
    /// </summary>
    public interface IDcilSchemaProvider
    {
        /// <summary>
        /// When implemented in a child class, retrieves a list of available schema names for the given <see cref="System.Data.IDbConnection"/>
        /// </summary>
        /// <param name="dbConn"><see cref="System.Data.IDbConnection"/> object to connect with</param>
        /// <returns>List of available schema names</returns>
        IList<string> GetAvailableSchemaNames(System.Data.IDbConnection dbConn);
        /// <summary>
        /// When implemented in a child class, loads the specified Schema into a <see cref="Neumont.Tools.ORM.DatabaseImport.DcilSchema"/> object
        /// </summary>
        /// <param name="schemaName">Name of the Schema to load</param>
        /// <returns><see cref="Neumont.Tools.ORM.DatabaseImport.DcilSchema"/> object</returns>
        DcilSchema LoadSchema(string schemaName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilTable"/> objects for the specified Schema
        /// </summary>
        /// <param name="schemaName">Name of the Schema from which to load the Tables</param>
        /// <returns>Generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilTable"/> objects for the specified Schema</returns>
        IList<DcilTable> LoadTables(string schemaName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilColumn"/> objects for the specified Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Columns</param>
        /// <returns>Generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilColumn"/> objects for the specified Schema and Table</returns>
        IList<DcilColumn> LoadColumns(string schemaName, string tableName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Indexes</param>
        /// <returns>Generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified Schema and Table</returns>
        IList<DcilUniquenessConstraint> LoadIndexes(string schemaName, string tableName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Indexes</param>
        /// <returns>Generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified Schema and Table</returns>
        IList<DcilReferenceConstraint> LoadForeignKeys(string schemaName, string tableName);
        /// <summary>
        /// When implemented in a child class, loads a generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilProcedure"/> objects for the specified Schema
        /// </summary>
        /// <param name="schemaName">Name of the Schema from which to laod the Stored Procedures</param>
        /// <returns>Generic list of <see cref="Neumont.Tools.ORM.DatabaseImport.DcilProcedure"/> objects for the specified Schema</returns>
        IList<DcilProcedure> LoadProcedures(string schemaName);
    }
}
