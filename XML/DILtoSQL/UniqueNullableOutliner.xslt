<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Kevin M. Owen, Robert Moore -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	xmlns:dpp="urn:schemas-orm-net:DIL:Preprocessor"
	extension-element-prefixes="exsl dsf">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:param name="UniqueNullableOutliner.UniquenessOption" select="'backingTable'"/>
	<xsl:param name="UniqueNullableOutliner.GenerateIndexedViews" select="true()"/>
	<!-- If true, turns off the rest of the nullable uniqueness processing (which is still a work in progress). -->
	<xsl:param name="UniqueNullableOutliner.GenerateIndexedViewsOnly" select="$UniqueNullableOutliner.GenerateIndexedViews"/>
	<xsl:param name="UniqueNullableOutliner.ForeignKeyOption" select="'triggers'"/>
	<!-- Controls whether uniqueness constraints over a single column are outlined. -->
	<xsl:param name="UniqueNullableOutliner.OutlineSingleColumnUniquenesses" select="true()"/>

	<xsl:template match="*" mode="UniqueNullableOutliner">
		<xsl:param name="nullableColumnNames"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates mode="UniqueNullableOutliner">
				<xsl:with-param name="nullableColumnNames" select="$nullableColumnNames"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<!-- Filter out any UNIQUE constraint on a COLUMN that doesn't also contain a NOT NULL constraint. -->
	<xsl:template match="ddl:columnConstraintDefinition[ddl:uniqueKeyword and not(parent::ddl:columnDefinition/ddl:columnConstraintDefinition/ddl:notNullKeyword)]" mode="UniqueNullableOutliner">
		<xsl:param name="nullableColumnNames"/>
		<!-- We only want to filter these out if we are outlining them. -->
		<xsl:if test="not($UniqueNullableOutliner.OutlineSingleColumnUniquenesses)">
			<xsl:copy>
				<xsl:copy-of select="@*"/>
				<xsl:apply-templates mode="UniqueNullableOutliner">
					<xsl:with-param name="nullableColumnNames" select="$nullableColumnNames"/>
				</xsl:apply-templates>
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- Filter out any UNIQUE constraint on a TABLE that includes one or more nullable COLUMNs. -->
	<xsl:template match="ddl:tableConstraintDefinition" mode="UniqueNullableOutliner">
		<xsl:param name="nullableColumnNames"/>
		<xsl:if test="not(ddl:uniqueConstraintDefinition[dep:simpleColumnReference[dsf:getInformationSchemaForm(@name) = $nullableColumnNames] and ($UniqueNullableOutliner.OutlineSingleColumnUniquenesses or count(dep:simpleColumnReference) > 1)])">
			<xsl:copy>
				<xsl:copy-of select="@*"/>
				<xsl:apply-templates mode="UniqueNullableOutliner">
					<xsl:with-param name="nullableColumnNames" select="$nullableColumnNames"/>
				</xsl:apply-templates>
			</xsl:copy>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="ddl:tableDefinition" mode="UniqueNullableOutliner">
		<xsl:variable name="catalogAndSchema" select="@catalog | @schema"/>
		<xsl:variable name="table" select="."/>
		<xsl:variable name="tableName" select="string(@name)"/>

		<xsl:variable name="nullableColumnNamesFragment">
			<xsl:for-each select="ddl:columnDefinition[not(ddl:columnConstraintDefinition/ddl:notNullKeyword)]">
				<NullableColumnName name="{dsf:getInformationSchemaForm(@name)}"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="nullableColumnNames" select="exsl:node-set($nullableColumnNamesFragment)/child::*/@name"/>
		
		<!-- Copy the table excluding the uniquenesses that span nullable columns. -->
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates mode="UniqueNullableOutliner">
				<xsl:with-param name="nullableColumnNames" select="$nullableColumnNames"/>
			</xsl:apply-templates>
		</xsl:copy>

		<!-- Create a temporary fragment to hold the parameters passed to the templates for each uniqueness constraint. -->
		<xsl:variable name="uniquenessesFragment">
			<xsl:if test="$UniqueNullableOutliner.OutlineSingleColumnUniquenesses">
				<!-- For each for handling a uniqueness that spans a single nullable COLUMN. -->
				<xsl:for-each select="ddl:columnDefinition[ddl:columnConstraintDefinition/ddl:uniqueKeyword and not(ddl:columnConstraintDefinition/ddl:notNullKeyword)]">
					<uniqueness>
						<xsl:attribute name="name">
							<xsl:variable name="uniquenessConstraintName" select="string(ddl:columnConstraintDefinition[ddl:uniqueKeyword]/@name)"/>
							<xsl:choose>
								<xsl:when test="$uniquenessConstraintName">
									<xsl:value-of select="$uniquenessConstraintName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="dsf:makeValidIdentifier(concat(dsf:unescapeIdentifier($tableName), '_', dsf:unescapeIdentifier(@name), '_Uniqueness'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<column name="{@name}"/>
					</uniqueness>
				</xsl:for-each>
			</xsl:if>
			<xsl:for-each select="ddl:tableConstraintDefinition[ddl:uniqueConstraintDefinition[@type = 'UNIQUE' and dep:simpleColumnReference[dsf:getInformationSchemaForm(@name) = $nullableColumnNames] and ($UniqueNullableOutliner.OutlineSingleColumnUniquenesses or count(dep:simpleColumnReference) > 1)]]">
				<uniqueness name="{@name}">
					<xsl:for-each select="ddl:uniqueConstraintDefinition/dep:simpleColumnReference">
						<column name="{@name}"/>
					</xsl:for-each>
				</uniqueness>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="uniquenesses" select="exsl:node-set($uniquenessesFragment)/child::*"/>

		<xsl:for-each select="$uniquenesses">
			<xsl:if test="$UniqueNullableOutliner.GenerateIndexedViews">
				<xsl:apply-templates select="$table" mode="UniqueNullableOutliner.ForViewDefinition">
					<xsl:with-param name="nullableColumnNames" select="$nullableColumnNames"/>
					<xsl:with-param name="uniquenessName" select="@name"/>
					<xsl:with-param name="uniquenessColumnNames" select="column/@name"/>
				</xsl:apply-templates>
			</xsl:if>
			<xsl:if test="not($UniqueNullableOutliner.GenerateIndexedViewsOnly)">
				<xsl:choose>
					<xsl:when test="$UniqueNullableOutliner.UniquenessOption = 'backingTable'">
						<xsl:apply-templates select="$table" mode="UniqueNullableOutliner.ForBackingTable">
							<xsl:with-param name="nullableColumnNames" select="$nullableColumnNames"/>
							<xsl:with-param name="uniquenessName" select="@name"/>
							<xsl:with-param name="uniquenessColumnNames" select="column/@name"/>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:when test ="$UniqueNullableOutliner.UniquenessOption = 'Trigger'">
						<xsl:apply-templates select="$table" mode="UniqueNullableOutliner.ForTrigger">
							<xsl:with-param name="nullableColumnNames" select="$nullableColumnNames"/>
							<xsl:with-param name="uniquenessName" select="@name"/>
							<xsl:with-param name="uniquenessColumnNames" select="column/@name"/>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">
							<xsl:text>ERROR: UniqueNullableOutliner.UniquenessOption value '</xsl:text>
							<xsl:value-of select="$UniqueNullableOutliner.UniquenessOption"/>
							<xsl:text>' is not recognized.</xsl:text>
						</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:for-each>

	</xsl:template>

	<!-- Templates for Outlining to Views with Unique Indexes -->
	<xsl:template match="ddl:tableDefinition" mode="UniqueNullableOutliner.ForViewDefinition">
		<xsl:param name="nullableColumnNames"/>
		<xsl:param name="uniquenessName"/>
		<xsl:param name="uniquenessColumnNames"/>

		<ddl:viewDefinition name="{$uniquenessName}" dpp:isUniqueNullableView="true">
			<xsl:copy-of select="@catalog | @schema"/>
			<xsl:for-each select="$uniquenessColumnNames">
				<dep:columnNameDefinition name="{.}"/>
			</xsl:for-each>
			<dml:querySpecification>
				<dml:selectList>
					<xsl:for-each select="$uniquenessColumnNames">
						<dep:columnReference name="{.}"/>
					</xsl:for-each>
				</dml:selectList>
				<dml:fromClause>
					<dml:tableName name="{@name}">
						<xsl:copy-of select="@catalog | @schema"/>
					</dml:tableName>
				</dml:fromClause>
				<dml:whereClause>
					<xsl:variable name="nullableColumns" select="$uniquenessColumnNames[dsf:getInformationSchemaForm(.) = $nullableColumnNames]"/>
					<xsl:choose>
						<xsl:when test="count($nullableColumns) = 1">
							<xsl:for-each select="$nullableColumns">
								<dep:nullPredicate type="NOT NULL">
									<dep:columnReference name="{.}"/>
								</dep:nullPredicate>
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise>
							<dep:and>
								<xsl:for-each select="$nullableColumns">
									<dep:nullPredicate type="NOT NULL">
										<dep:columnReference name="{.}"/>
									</dep:nullPredicate>
								</xsl:for-each>
							</dep:and>
						</xsl:otherwise>
					</xsl:choose>
				</dml:whereClause>
			</dml:querySpecification>
		</ddl:viewDefinition>
		
	</xsl:template>
	<!-- End of View Specific Templates -->
	
	<!-- Templates for Outlining to Backing Tables -->
	<xsl:template match="ddl:tableDefinition" mode="UniqueNullableOutliner.ForBackingTable">
		<xsl:param name="nullableColumnNames"/>
		<xsl:param name="uniquenessName"/>
		<xsl:param name="uniquenessColumnNames"/>

		<xsl:variable name="table" select="."/>
		
		<ddl:tableDefinition name="{$uniquenessName}" dpp:isUniqueBackingTable="true">
			<xsl:copy-of select="@catalog | @schema"/>
			<xsl:variable name="isSingleColumn" select="boolean(count($uniquenessColumnNames) = 1)"/>
			<xsl:for-each select="$uniquenessColumnNames">
				<!-- Do a nested for-each here to keep the columns in the order that they appear in the uniqueness constraint. -->
				<xsl:for-each select="$table/ddl:columnDefinition[dsf:getInformationSchemaForm(@name) = dsf:getInformationSchemaForm(current())]">
					<xsl:copy>
						<xsl:copy-of select="@*"/>
						<!-- Copy the datatype. -->
						<xsl:copy-of select="*[1]"/>
						<ddl:columnConstraintDefinition>
							<ddl:notNullKeyword/>
						</ddl:columnConstraintDefinition>
						<xsl:if test="$isSingleColumn">
							<ddl:columnConstraintDefinition>
								<ddl:primaryKeyKeyword/>
							</ddl:columnConstraintDefinition>
						</xsl:if>
					</xsl:copy>
				</xsl:for-each>
			</xsl:for-each>
			<xsl:if test="not($isSingleColumn)">
				<ddl:tableConstraintDefinition name="{$uniquenessName}">
					<ddl:uniqueConstraintDefinition type="PRIMARY KEY">
						<xsl:for-each select="$uniquenessColumnNames">
							<dep:simpleColumnReference name="{.}"/>
						</xsl:for-each>
					</ddl:uniqueConstraintDefinition>
				</ddl:tableConstraintDefinition>
			</xsl:if>
		</ddl:tableDefinition>

		<ddl:triggerDefinition name="{dsf:makeValidIdentifier(concat(dsf:unescapeIdentifier($uniquenessName),'_Insert'))}" actionTime="BEFORE" forEach="ROW">
			<xsl:copy-of select="@catalog | @schema"/>
			<ddl:event type="INSERT"/>
			<ddl:table>
				<xsl:copy-of select="@catalog | @schema | @name"/>
			</ddl:table>
			<ddl:referencing>
				<ddl:newRow name="NewRow"/>
			</ddl:referencing>
			<ddl:atomicBlock>
				<dml:insertStatement name="{$uniquenessName}">
					<xsl:copy-of select="@catalog | @schema"/>
					<xsl:for-each select="$uniquenessColumnNames">
						<dep:simpleColumnRefernence name="{.}"/>
					</xsl:for-each>
					<dml:tableValueConstructor>
						<xsl:for-each select="$uniquenessColumnNames">
							<dep:columnReference name="NewRow.{.}"/>
						</xsl:for-each>
					</dml:tableValueConstructor>
				</dml:insertStatement>
			</ddl:atomicBlock>
		</ddl:triggerDefinition>

		<ddl:triggerDefinition name="{dsf:makeValidIdentifier(concat(dsf:unescapeIdentifier($uniquenessName),'_Delete'))}" actionTime="AFTER" forEach="STATEMENT">
			<xsl:copy-of select="@catalog | @schema"/>
			<ddl:event type="DELETE"/>
			<ddl:table>
				<xsl:copy-of select="@catalog | @schema | @name"/>
			</ddl:table>
			<ddl:referencing>
				<ddl:oldTable name="OldTable"/>
			</ddl:referencing>
			<ddl:atomicBlock>
				<dml:deleteStatement name="{$uniquenessName}">
					<xsl:copy-of select="@catalog | @schema"/>
					<dml:whereClause>
						<dep:inPredicate type="IN">
							<xsl:for-each select="$uniquenessColumnNames">
								<dep:columnReference name="{.}"/>
							</xsl:for-each>
						</dep:inPredicate>
					</dml:whereClause>
					<dml:querySpecification>
						<dml:selectList>
							<xsl:for-each select="$uniquenessColumnNames">
								<dep:columnReference name="{.}"/>
							</xsl:for-each>
						</dml:selectList>
						<dml:fromClause>
							<dml:tableName name="NewTable"/>
						</dml:fromClause>
					</dml:querySpecification>
				</dml:deleteStatement>
			</ddl:atomicBlock>
		</ddl:triggerDefinition>

		<ddl:triggerDefinition name="{dsf:makeValidIdentifier(concat(dsf:unescapeIdentifier($uniquenessName),'_Update'))}" actionTime="BEFORE" forEach="STATEMENT">
			<xsl:copy-of select="@catalog | @schema"/>
			<ddl:event type="UPDATE">
				<xsl:for-each select="$uniquenessColumnNames">
					<dep:simpleColumnReference name="{.}"/>
				</xsl:for-each>
			</ddl:event>
			<ddl:table>
				<xsl:copy-of select="@catalog | @schema | @name"/>
			</ddl:table>
			<ddl:referencing>
				<ddl:oldTable name="OldTable"/>
			</ddl:referencing>
			<ddl:atomicBlock>
				<dml:deleteStatement name="{$uniquenessName}">
					<xsl:copy-of select="@catalog | @schema"/>
					<dml:whereClause>
						<dep:inPredicate type="IN">
							<xsl:for-each select="$uniquenessColumnNames">
								<dep:columnReference name="{.}"/>
							</xsl:for-each>
						</dep:inPredicate>
					</dml:whereClause>
					<dml:querySpecification>
						<dml:selectList>
							<xsl:for-each select="$uniquenessColumnNames">
								<dep:columnReference name="{.}"/>
							</xsl:for-each>
						</dml:selectList>
						<dml:fromClause>
							<dml:tableName name="NewTable"/>
						</dml:fromClause>
					</dml:querySpecification>
				</dml:deleteStatement>
			</ddl:atomicBlock>
		</ddl:triggerDefinition>

		<!-- TODO: Create link from uniqueness to Backing Tables -->
		
		<!-- TODO: Ridirect foreign keys that span all COLUMNS of the backing table to the backing table. -->
		
		
	</xsl:template>
	<!-- End of Backing Table Specific Templates-->
	
	<!-- Templates for Outlining to Triggers-->
	<xsl:template match="ddl:tableDefinition" mode="UniqueNullableOutliner.ForTrigger">
		<xsl:param name="nullableColumnNames"/>
		<xsl:param name="uniquenessName"/>
		<xsl:param name="uniquenessColumnNames"/>

		<!-- For Update -->
		<ddl:triggerDefinition name="{dsf:makeValidIdentifier(concat(dsf:unescapeIdentifier($uniquenessName),'_Update'))}" actionTime="BEFORE" forEach="STATEMENT">
			<xsl:copy-of select="@catalog | @schema"/>
			<ddl:event type="UPDATE">
				<xsl:for-each select="$uniquenessColumnNames">
					<dep:simpleColumnReference name="{.}"/>
				</xsl:for-each>
			</ddl:event>
			<ddl:table>
				<xsl:copy-of select="@catalog | @schema | @name"/>
			</ddl:table>
			<ddl:referencing>
				<ddl:oldTable name="OldTable"/>
			</ddl:referencing>
			<ddl:atomicBlock>
				<dml:deleteStatement name="{$uniquenessName}">
					<xsl:copy-of select="@catalog | @schema"/>
					<dml:whereClause>
						<dep:inPredicate type="IN">
							<xsl:for-each select="$uniquenessColumnNames">
								<dep:columnReference name="{.}"/>
							</xsl:for-each>
						</dep:inPredicate>
					</dml:whereClause>
					<dml:querySpecification>
						<dml:selectList>
							<xsl:for-each select="$uniquenessColumnNames">
								<dep:columnReference name="{.}"/>
							</xsl:for-each>
						</dml:selectList>
						<dml:fromClause>
							<dml:tableName name="NewTable"/>
						</dml:fromClause>
					</dml:querySpecification>
				</dml:deleteStatement>
			</ddl:atomicBlock>
		</ddl:triggerDefinition>


		<!-- ???
		CREATE TRIGGER Table3NullsTrigger ON Table3 FOR INSERT, UPDATE
		AS
		IF	(SELECT count( pk ) FROM inserted
		WHERE exists (SELECT  pk  FROM Table3 WHERE Table3.Col1 = inserted.Col1 AND Table3.pk!= inserted.pk)) > 0
		BEGIN
		ROLLBACK TRANSACTION
		END
		GO-->

		<!-- For Insert -->
		<ddl:triggerDefinition name="InsertUnqiuenessCheck" actionTime="BEFORE">
			<xsl:copy-of select="@catalog | @schema"/>
			<ddl:event type="INSERT">
				<xsl:for-each select="$uniquenessColumnNames">
					<dep:simpleColumnReference name="{.}"/>
				</xsl:for-each>
			</ddl:event>
			<ddl:table/>
			<ddl:atomicBlock>
				<ddl:sqlParameterDeclaration name="">
					<ddt:userDefinedType name="concat('@', $)"/>
				</ddl:sqlParameterDeclaration>
	
				
			</ddl:atomicBlock>	
		</ddl:triggerDefinition>
		
		
	</xsl:template>
	<!-- End of Uniqueness Trigger Specific Templates-->

	<!-- Templates for attaching backing tables to actual tables. -->
	<!-- Remove alter table statements that contain nullable columns, and replace them with triggers. -->
	<!--<xsl:template match="ddl:alterTableStatement/ddl:addTableConstraintDefinition/ddl:referencesSpecification/dep:simpleColumnReference[@name = $nullableColumnNames">
		<ddl:triggerDefinition name ="">
			<ddl:event type="UPDATE">
				<xsl:for-each select="">
					<dep:simpleColumnReference name=""/>
				</xsl:for-each>
			</ddl:event>
			<ddl:table name="" catalog="" schema="">
			</ddl:table>
		</ddl:triggerDefinition>
	</xsl:template>-->
	
	<!--
	Uniqueness TRIGGER:
	After Insert on TableWithUniqueness
	
	
	After Update on TableWithUniqueness
	-->
	<!--<ddl:triggerDefinition actionTime="AFTER" name="">
		<ddl:event type="INSERT">
			
		</ddl:event>
	</ddl:triggerDefinition>

	<ddl:triggerDefinition actionTime="AFTER" name="">
		<ddl:event type="UPDATE">

		</ddl:event>
	</ddl:triggerDefinition>-->	
	<!--
	Foreign Key TRIGGER:
	After Update on TableWithUniqueness
	
	
	After Delete on TableWithUniqueness
	
	
	After Insert on RefrenceTable
	
	
	After Update on RefrenceTable
	
	Backing Table TRIGGER:
	Before Insert on UniquenessTable
	
	
	Before Update on UniquenessTable
	
	
	After Delete on UniquenessTable
	
	-->
	
	<!-- Backing tables require a trigger to auto populate the backing table -->
	<!-- Trigger to lock the table? -->
	
	<!-- End of Backing Table Specific Templates-->

	<!-- Templates for Outlining to Triggers -->

	<!-- End of Trigger Specific Templates-->
	
	<!-- Template for Enforcing Foreign keys through triggers -->
	<!--<xsl:template match="">
		<ddl:triggerDefinition actionTime="BEFORE">
			<xsl:copy-of select="@name"/>
			<ddl:event type="INSERT">
				<dep:simpleColumnReference></dep:simpleColumnReference>
				<dep:simpleColumnReference/>
			</ddl:event>
			<ddl:table/>
			<ddl:atomicBlock>
				
			</ddl:atomicBlock>
			
		</ddl:triggerDefinition>
	</xsl:template>-->
	
	<!-- Need Insert / Update / Delete triggers for table being keyed to -->
	
	<!-- End of Templates for Foreign Keys-->
</xsl:stylesheet>