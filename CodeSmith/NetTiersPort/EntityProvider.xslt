<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:se="http://NetTiers/2.2/SchemaExplorer"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:exsl="http://exslt.org/common"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:csc="urn:nettiers:CommonSqlCode"
	xmlns:tmp="urn:temporary"
	extension-element-prefixes="exsl msxsl csc"
	exclude-result-prefixes="se xsl tmp">
	<!-- DEBUG: indent="yes" is for debugging only, has sideeffects on docComment output. -->
	<xsl:output method="xml" indent="yes"/>
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:param name="settingsFragment">
		<setting name="DataAccessLayerNamespace">Data</setting>
		<setting name="BusinessLogicLayerNamespace">EntityLayer</setting>
		<setting name="IncludeCustoms"/>
		<setting name="IncludeDelete"/>
		<setting name="IncludeDrop"/>
		<setting name="IncludeFind"/>
		<setting name="IncludeGet"/>
		<setting name="IncludeGetList"/>
		<setting name="IncludeGetListByFK"/>
		<setting name="IncludeGetListByIX"/>
		<setting name="IncludeInsert"/>
		<setting name="IncludeManyToMany"/>
		<setting name="IncludeRelations"/>
		<setting name="IncludeSave"/>
		<setting name="IncludeUpdate"/>
		<setting name="ChangeUnderscoreToPascalCase"/>
		<setting name="UsePascalCasing">Style2</setting>
		<setting name="MethodNames">
			<methodName name="BulkInsert">BulkInsert</methodName>
			<methodName name="DeepLoad">DeepLoad</methodName>
			<methodName name="DeepSave">DeepSave</methodName>
			<methodName name="Delete">Delete</methodName>
			<methodName name="Find">Find</methodName>
			<methodName name="Get">Get</methodName>
			<methodName name="GetAll">GetAll</methodName>
			<methodName name="GetPaged">GetPaged</methodName>
			<methodName name="GetTotalItems">GetTotalItems</methodName>
			<methodName name="Insert">Insert</methodName>
			<methodName name="Save">Save</methodName>
			<methodName name="Update">Update</methodName>
		</setting>
	</xsl:param>
	<xsl:param name="settings" select="exsl:node-set($settingsFragment)/child::*"/>
	<xsl:variable name="DALNamespace" select="concat($CustomToolNamespace,'.',$settings[@name='DataAccessLayerNamespace'])"/>
	<xsl:variable name="BLLNamespace" select="concat($CustomToolNamespace,'.',$settings[@name='BusinessLogicLayerNamespace'])"/>
	<xsl:template match="/">
		<xsl:variable name="initializeSettings">
			<xsl:value-of select="csc:SetChangeUnderscoreToPascalCase(boolean($settings[@name='ChangeUnderscoreToPascalCase']))"/>
			<xsl:value-of select="csc:SetUsePascalCasing($settings[@name='UsePascalCasing'])"/>
			<xsl:value-of select="csc:SetManyToManyFormat($settings[@name='ManyToManyFormat'])"/>
		</xsl:variable>
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Data"/>
			<plx:namespaceImport name="System.Data.Common"/>
			<plx:namespaceImport name="System.Collections"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.Diagnostics"/>
			<plx:namespaceImport name="{$CustomToolNamespace}"/>
			<plx:namespaceImport name="{$DALNamespace}"/>
			<plx:namespaceImport name="{$BLLNamespace}"/>
			<plx:namespace name="{$DALNamespace}.Bases">
				<xsl:apply-templates select="child::*"/>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="se:databaseSchema">
		<xsl:apply-templates select="se:tables/se:table"/>
	</xsl:template>
	<xsl:template match="se:table">
		<xsl:variable name="allTables" select="../se:table"/>
		<xsl:variable name="currentTable" select="."/>
		<xsl:variable name="currentTableIndexes" select="se:indexes/se:index"/>
		<xsl:variable name="currentTablePrimaryKey" select="$currentTableIndexes[@isPrimary='true' or @isPrimary='1']"/>
		<plx:pragma type="region" data="Classes for {@name}"/>
		<!-- *************************************************************
		Corresponds to DataAccessLayer\Bases\EntityProviderBase.cst
		************************************************************** -->
		<xsl:variable name="baseProviderName" select="csc:GetClassNameForTable(@owner, @name, 'ProviderBase')"/>
		<xsl:variable name="entityClassName" select="csc:GetClassNameForTable(@owner, @name, '')"/>
		<xsl:variable name="keyClassName" select="csc:GetClassNameForTable(@owner, @name, 'Key')"/>
		<xsl:variable name="returnsEntityClassCollectionFragment">
			<plx:returns dataTypeName="TList" dataTypeQualifier="{$BLLNamespace}">
				<plx:passTypeParam dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}"/>
			</plx:returns>
		</xsl:variable>
		<plx:class name="{$baseProviderName}" visibility="public" modifier="abstract" partial="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary>
This class is the base class for any <see cref="{$baseProviderName}" /> implementation.
It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:derivesFromClass dataTypeName="{$baseProviderName}Core"/>
		</plx:class>
		<!-- *************************************************************
		Corresponds to DataAccessLayer\Bases\EntityProviderBaseCore.generated.cst
		************************************************************** -->
		<plx:class name="AddressProviderBaseCore" visibility="public" modifier="abstract">
			<plx:leadingInfo>
				<plx:docComment>
					<summary>
This class is the base class for any <see cref="{$baseProviderName}" /> implementation.
It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:derivesFromClass dataTypeName="EntityProviderBase">
				<plx:passTypeParam dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}"/>
				<plx:passTypeParam dataTypeName="{$keyClassName}" dataTypeQualifier="{$BLLNamespace}"/>
			</plx:derivesFromClass>
			<xsl:if test="$settings['IncludeManyToMany']">
				<plx:pragma type="region" data="Get from Many To Many Relationship Functions"/>
				<!-- SourceTable.PrimaryKeys -->
				<xsl:for-each select="$allTables/se:keys/se:key[@targetTable=current()/@name and @targetOwner=current()/@owner]">
					<xsl:variable name="junctionTable" select="../.."/>
					<!-- IsJunctionTable -->
					<xsl:variable name="junctionTablePrimaryKeyColumns" select="$junctionTable/se:indexes/se:index[@isPrimary='true' or @isPrimary='1'][count(se:column)>1]/se:column"/>
					<xsl:variable name="identifiyingForeignKeysFragment">
						<xsl:for-each select="$junctionTable/se:keys/se:key">
							<xsl:variable name="targetTablePrimaryKeyColumns" select="$allTables[@owner=current()/@targetOwner and @name=current()/@targetTable]/se:indexes/se:index[@isPrimary='true' or @isPrimary='1']/se:column"/>
							<!-- Check IsIdentifyingRelationship (all foreign key target columns are in the primary key of the target table)-->
							<xsl:if test="$targetTablePrimaryKeyColumns and count($targetTablePrimaryKeyColumns)=count($targetTablePrimaryKeyColumns[@ref=current()/se:columnReference/@targetColumn])">
								<xsl:copy-of select="."/>
							</xsl:if>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="identifyingForeignKeys" select="exsl:node-set($identifiyingForeignKeysFragment)/child::*"/>
					<xsl:if test="count($identifyingForeignKeys)>1 and count($junctionTablePrimaryKeyColumns)=count($junctionTablePrimaryKeyColumns[@ref=$identifyingForeignKeys/se:columnReference/@column])">
						<!-- IsJunctionKey -->
						<xsl:if test="count(se:columnReference[@column=$junctionTablePrimaryKeyColumns/@ref])=count(se:columnReference)">
							<!-- Iterate on opposite keys (not the current key, but is a junction table) -->
							<xsl:for-each select="$junctionTable/se:keys/se:key[@name!=current()/@name][count(se:columnReference[@column=$junctionTablePrimaryKeyColumns/@ref])=count(se:columnReference)]">
								<xsl:variable name="secondaryTable" select="$allTables[@owner=current()/@targetOwner and @name=current()/@targetTable]"/>
								<xsl:variable name="combinedColumnNamesFragment">
									<xsl:for-each select="se:columnReference">
										<xsl:value-of select="csc:GetPropertyNameForColumn($junctionTable/@owner,$junctionTable/@name,@column)"/>
									</xsl:for-each>
								</xsl:variable>
								<xsl:variable name="functionName" select="csc:GetManyToManyName(string($combinedColumnNamesFragment),$junctionTable/@owner,$junctionTable/@name)"/>
								<xsl:variable name="docCommentSummaryFragment">
									<summary>
Gets <xsl:value-of select="$entityClassName"/> objects from the datasource by <xsl:value-of select="se:columnReference/@column"/> in the
<xsl:value-of select="$junctionTable/@name"/> table. Table <xsl:value-of select="$currentTable/@name"/> is related to table <xsl:value-of select="$secondaryTable/@name"/>
through the (M:N) relationship defined in the <xsl:value-of select="$junctionTable/@name"/> table.
</summary>
								</xsl:variable>
								<xsl:variable name="docCommentReturnsFragment">
									<returns>Returns a typed collection of <xsl:value-of select="$entityClassName"/> objects.</returns>
								</xsl:variable>
								<xsl:variable name="fullColumnParamsFragment">
									<xsl:call-template name="GetColumnParams">
										<xsl:with-param name="table" select="$junctionTable"/>
										<xsl:with-param name="columnNames" select="se:columnReference/@column"/>
									</xsl:call-template>
								</xsl:variable>
								<xsl:variable name="fullColumnParams" select="exsl:node-set($fullColumnParamsFragment)/child::*"/>
								<xsl:variable name="columnParamsFragment">
									<xsl:for-each select="$fullColumnParams">
										<xsl:copy>
											<xsl:copy-of select="@*[not(namespace-uri())]|*"/>
										</xsl:copy>
									</xsl:for-each>
								</xsl:variable>
								<xsl:variable name="docCommentColumnParamsFragment">
									<xsl:for-each select="$fullColumnParams">
										<param name="{@name}">
											<xsl:value-of select="child::comment()"/>
										</param>
									</xsl:for-each>
								</xsl:variable>
								<xsl:variable name="forwardColumnParamsFragment">
									<xsl:for-each select="$fullColumnParams">
										<plx:passParam>
											<plx:nameRef name="{@name}" type="parameter"/>
										</plx:passParam>
									</xsl:for-each>
								</xsl:variable>
								<plx:pragma type="region" data="GetBy{$functionName}"/>
								<plx:function name="GetBy{$functionName}" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:copy-of select="$docCommentSummaryFragment"/>
											<xsl:copy-of select="$docCommentColumnParamsFragment"/>
											<xsl:copy-of select="$docCommentReturnsFragment"/>
										</plx:docComment>
									</plx:leadingInfo>
									<xsl:copy-of select="$columnParamsFragment"/>
									<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
									<plx:local name="count" dataTypeName=".i4">
										<plx:initialize>
											<plx:value data="-1" type="i4"/>
										</plx:initialize>
									</plx:local>
									<plx:return>
										<plx:callThis name="GetBy{$functionName}">
											<plx:passParam>
												<plx:nullKeyword/>
											</plx:passParam>
											<xsl:copy-of select="$forwardColumnParamsFragment"/>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:callStatic name="MaxValue" dataTypeName=".i4" type="property"/>
											</plx:passParam>
											<plx:passParam type="out">
												<plx:nameRef name="count"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:function>
								<plx:function name="GetBy{$functionName}" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:copy-of select="$docCommentSummaryFragment"/>
											<xsl:copy-of select="$docCommentColumnParamsFragment"/>
											<param name="start">Row number at which to start reading, the first row is 0.</param>
											<param name="pageLength">Number of rows to return.</param>
											<xsl:copy-of select="$docCommentReturnsFragment"/>
										</plx:docComment>
									</plx:leadingInfo>
									<xsl:copy-of select="$columnParamsFragment"/>
									<plx:param name="start" dataTypeName=".i4"/>
									<plx:param name="pageLength" dataTypeName=".i4"/>
									<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
									<plx:local name="count" dataTypeName=".i4">
										<plx:initialize>
											<plx:value data="-1" type="i4"/>
										</plx:initialize>
									</plx:local>
									<plx:return>
										<plx:callThis name="GetBy{$functionName}">
											<plx:passParam>
												<plx:nullKeyword/>
											</plx:passParam>
											<xsl:copy-of select="$forwardColumnParamsFragment"/>
											<plx:passParam>
												<plx:nameRef name="start" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="pageLength" type="parameter"/>
											</plx:passParam>
											<plx:passParam type="out">
												<plx:nameRef name="count"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:function>
								<plx:function name="GetBy{$functionName}" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:copy-of select="$docCommentSummaryFragment"/>
											<param name="transactionManager"><see cref="TransactionManager"/> object</param>
											<xsl:copy-of select="$docCommentColumnParamsFragment"/>
											<xsl:copy-of select="$docCommentReturnsFragment"/>
										</plx:docComment>
									</plx:leadingInfo>
									<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
									<xsl:copy-of select="$columnParamsFragment"/>
									<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
									<plx:local name="count" dataTypeName=".i4">
										<plx:initialize>
											<plx:value data="-1" type="i4"/>
										</plx:initialize>
									</plx:local>
									<plx:return>
										<plx:callThis name="GetBy{$functionName}">
											<plx:passParam>
												<plx:nameRef name="transactionManager" type="parameter"/>
											</plx:passParam>
											<xsl:copy-of select="$forwardColumnParamsFragment"/>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:callStatic name="MaxValue" dataTypeName=".i4" type="property"/>
											</plx:passParam>
											<plx:passParam type="out">
												<plx:nameRef name="count"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:function>
								<plx:function name="GetBy{$functionName}" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:copy-of select="$docCommentSummaryFragment"/>
											<param name="transactionManager"><see cref="TransactionManager"/> object</param>
											<xsl:copy-of select="$docCommentColumnParamsFragment"/>
											<param name="start">Row number at which to start reading, the first row is 0.</param>
											<param name="pageLength">Number of rows to return.</param>
											<xsl:copy-of select="$docCommentReturnsFragment"/>
										</plx:docComment>
									</plx:leadingInfo>
									<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
									<xsl:copy-of select="$columnParamsFragment"/>
									<plx:param name="start" dataTypeName=".i4"/>
									<plx:param name="pageLength" dataTypeName=".i4"/>
									<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
									<plx:local name="count" dataTypeName=".i4">
										<plx:initialize>
											<plx:value data="-1" type="i4"/>
										</plx:initialize>
									</plx:local>
									<plx:return>
										<plx:callThis name="GetBy{$functionName}">
											<plx:passParam>
												<plx:nameRef name="transactionManager" type="parameter"/>
											</plx:passParam>
											<xsl:copy-of select="$forwardColumnParamsFragment"/>
											<plx:passParam>
												<plx:nameRef name="start" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="pageLength" type="parameter"/>
											</plx:passParam>
											<plx:passParam type="out">
												<plx:nameRef name="count"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:function>
								<plx:function name="GetBy{$functionName}" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:copy-of select="$docCommentSummaryFragment"/>
											<xsl:copy-of select="$docCommentColumnParamsFragment"/>
											<param name="start">Row number at which to start reading, the first row is 0.</param>
											<param name="pageLength">Number of rows to return.</param>
											<param name="count">out parameter to get total records for query</param>
											<xsl:copy-of select="$docCommentReturnsFragment"/>
										</plx:docComment>
									</plx:leadingInfo>
									<xsl:copy-of select="$columnParamsFragment"/>
									<plx:param name="start" dataTypeName=".i4"/>
									<plx:param name="pageLength" dataTypeName=".i4"/>
									<plx:param name="count" type="out" dataTypeName=".i4"/>
									<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
									<plx:return>
										<plx:callThis name="GetBy{$functionName}">
											<plx:passParam>
												<plx:nullKeyword/>
											</plx:passParam>
											<xsl:copy-of select="$forwardColumnParamsFragment"/>
											<plx:passParam>
												<plx:nameRef name="start" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="pageLength" type="parameter"/>
											</plx:passParam>
											<plx:passParam type="out">
												<plx:nameRef name="count" type="parameter"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:function>
								<plx:function name="GetBy{$functionName}" modifier="abstract"  visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:copy-of select="$docCommentSummaryFragment"/>
											<param name="transactionManager"><see cref="TransactionManager"/> object</param>
											<xsl:copy-of select="$docCommentColumnParamsFragment"/>
											<param name="start">Row number at which to start reading, the first row is 0.</param>
											<param name="pageLength">Number of rows to return.</param>
											<param name="count">out parameter to get total records for query</param>
											<xsl:copy-of select="$docCommentReturnsFragment"/>
										</plx:docComment>
									</plx:leadingInfo>
									<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
									<xsl:copy-of select="$columnParamsFragment"/>
									<plx:param name="start" dataTypeName=".i4"/>
									<plx:param name="pageLength" dataTypeName=".i4"/>
									<plx:param name="count" type="out" dataTypeName=".i4"/>
									<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
								</plx:function>
								<plx:pragma type="closeRegion" data="GetBy{$functionName}"/>
							</xsl:for-each>
						</xsl:if>
					</xsl:if>
				</xsl:for-each>
				<plx:pragma type="closeRegion" data="Get from Many To Many Relationship Functions"/>
			</xsl:if>
			<xsl:if test="$settings['IncludeDelete'] and $currentTablePrimaryKey">
				<xsl:variable name="deleteMethodName" select="$settings[@name='MethodNames']/*[@name='Delete']"/>
				<plx:pragma type="region" data="{$deleteMethodName} Methods"/>
				<xsl:variable name="rowVersionColumn" select="$currentTable/se:columns/se:column[@nativeType='timestamp'][1]"/>
				<plx:function name="{$deleteMethodName}" visibility="public" modifier="override">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>
Deletes a row from the DataSource.
</summary>
							<param name="transactionManager">A <see cref="TransactionManager" /> object.</param>
							<param name="key">The unique identifier of the row to delete.</param>
							<returns>Returns true if operation suceeded.</returns>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
					<plx:param name="key" dataTypeName="{$keyClassName}" dataTypeQualifier="{$BLLNamespace}"/>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:callThis name="{$deleteMethodName}">
							<plx:passParam>
								<plx:nameRef name="transactionManager" type="parameter"/>
							</plx:passParam>
							<xsl:for-each select="$currentTablePrimaryKey/se:column">
								<plx:passParam>
									<plx:callInstance name="{csc:GetPropertyNameForColumn($currentTable/@owner, $currentTable/@name, @ref)}" type="property">
										<plx:callObject>
											<plx:nameRef name="key" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</xsl:for-each>
							<xsl:if test="$rowVersionColumn">
								<plx:passParam>
									<plx:inlineStatement dataTypeName=".u1" dataTypeIsSimpleArray="true">
										<plx:conditionalOperator>
											<plx:condition>
												<plx:binaryOperator type="identityInequality">
													<plx:left>
														<plx:callInstance name="Entity" type="property">
															<plx:callObject>
																<plx:nameRef name="key" type="parameter"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:left>
													<plx:right>
														<plx:nullKeyword/>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<plx:left>
												<plx:callInstance name="{csc:GetPropertyNameForColumn($currentTable/@owner, $currentTable/@name, $rowVersionColumn/@name)}" type="property">
													<plx:callObject>
														<plx:callInstance name="Entity" type="property">
															<plx:callObject>
																<plx:nameRef name="key" type="parameter"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:callNew dataTypeName=".u1" dataTypeIsSimpleArray="true">
													<plx:passParam>
														<plx:value data="0" type="i4"/>
													</plx:passParam>
												</plx:callNew>
											</plx:right>
										</plx:conditionalOperator>
									</plx:inlineStatement>
								</plx:passParam>
							</xsl:if>
						</plx:callThis>
					</plx:return>
				</plx:function>
				<xsl:variable name="fullColumnParamsFragment">
					<xsl:call-template name="GetColumnParams">
						<xsl:with-param name="table" select="$currentTable"/>
						<xsl:with-param name="columnNames" select="$currentTablePrimaryKey/se:column/@ref"/>
					</xsl:call-template>
					<xsl:for-each select="$rowVersionColumn">
						<plx:param name="{csc:GetFieldNameForColumn($currentTable/@owner, $currentTable/@name, @name)}">
							<xsl:call-template name="ApplyPLiXDataType">
								<xsl:with-param name="nativeType" select="@nativeType"/>
								<xsl:with-param name="nullable" select="@nullable='true' or @nullable='1'"/>
							</xsl:call-template>
							<xsl:comment>The timestamp field used for concurrency check.</xsl:comment>
						</plx:param>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="fullColumnParams" select="exsl:node-set($fullColumnParamsFragment)/child::*"/>
				<xsl:variable name="columnParamsFragment">
					<xsl:for-each select="$fullColumnParams">
						<xsl:copy>
							<xsl:copy-of select="@*[not(namespace-uri())]|*"/>
						</xsl:copy>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="docCommentColumnParamsFragment">
					<xsl:for-each select="$fullColumnParams">
						<param name="{@name}">
							<xsl:value-of select="child::comment()"/>
						</param>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="forwardColumnParamsFragment">
					<xsl:for-each select="$fullColumnParams">
						<plx:passParam>
							<plx:nameRef name="{@name}" type="parameter"/>
						</plx:passParam>
					</xsl:for-each>
				</xsl:variable>
				<plx:function name="{$deleteMethodName}" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>
Deletes a row from the DataSource.
</summary>
							<xsl:copy-of select="$docCommentColumnParamsFragment"/>
							<remarks>Deletes based on primary key(s).</remarks>
							<returns>Returns true if operation suceeded.</returns>
						</plx:docComment>
					</plx:leadingInfo>
					<xsl:copy-of select="$columnParamsFragment"/>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:callThis name="{$deleteMethodName}">
							<plx:passParam>
								<plx:nullKeyword/>
							</plx:passParam>
							<xsl:copy-of select="$forwardColumnParamsFragment"/>
						</plx:callThis>
					</plx:return>
				</plx:function>
				<plx:function name="{$deleteMethodName}" visibility="public" modifier="abstract">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>
Deletes a row from the DataSource.
</summary>
							<param name="transactionManager"><see cref="TransactionManager" /> object</param>
							<xsl:copy-of select="$docCommentColumnParamsFragment"/>
							<remarks>Deletes based on primary key(s).</remarks>
							<returns>Returns true if operation suceeded.</returns>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param name="transactionManager" dataTypeName="TransactionManager" dataTypeQualifier="Tiers.AdventureWorks.Data"/>
					<xsl:copy-of select="$columnParamsFragment"/>
					<plx:returns dataTypeName=".boolean"/>
				</plx:function>
				<plx:pragma type="closeRegion" data="{$deleteMethodName} Methods"/>
			</xsl:if>
			<xsl:variable name="includeGetListByIX" select="$settings['IncludeGetListByIX']"/>
			<xsl:if test="$settings['IncludeGetListByFK']">
				<plx:pragma type="region" data="Get By Foreign Key Functions"/>
				<xsl:for-each select="se:keys/se:key">
					<xsl:if test="not($includeGetListByIX) or not($currentTableIndexes[count(se:column)=count(se:column[@ref=current()/@column])])">
						<xsl:variable name="keysNameFragment">
							<xsl:for-each select="se:columnReference">
								<xsl:value-of select="csc:GetPropertyNameForColumn($currentTable/@owner, $currentTable/@name, @column)"/>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="keysName" select="string($keysNameFragment)"/>
						<xsl:variable name="fullColumnParamsFragment">
							<xsl:call-template name="GetColumnParams">
								<xsl:with-param name="table" select="$currentTable"/>
								<xsl:with-param name="columnNames" select="se:columnReference/@column"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="fullColumnParams" select="exsl:node-set($fullColumnParamsFragment)/child::*"/>
						<xsl:variable name="columnParamsFragment">
							<xsl:for-each select="$fullColumnParams">
								<xsl:copy>
									<xsl:copy-of select="@*[not(namespace-uri())]|*"/>
								</xsl:copy>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="docCommentColumnParamsFragment">
							<xsl:for-each select="$fullColumnParams">
								<param name="{@name}">
									<xsl:value-of select="child::comment()"/>
								</param>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="forwardColumnParamsFragment">
							<xsl:for-each select="$fullColumnParams">
								<plx:passParam>
									<plx:nameRef name="{@name}" type="parameter"/>
								</plx:passParam>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="keyDescriptionFragment">
							<xsl:choose>
								<xsl:when test="string(se:description)">
									<xsl:value-of select="se:description"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>Foreign key constraint referencing </xsl:text>
									<xsl:value-of select="@targetTable"/>
									<xsl:text>.</xsl:text>
									<xsl:for-each select="se:columnReference">
										<xsl:if test="position()=1 and last()!=1">
											<xsl:text>(</xsl:text>
										</xsl:if>
										<xsl:if test="position()!=1">
											<xsl:text>,</xsl:text>
										</xsl:if>
										<xsl:value-of select="@targetColumn"/>
										<xsl:if test="position()=last() and last()!=1">
											<xsl:text>)</xsl:text>
										</xsl:if>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="keyDescription" select="string($keyDescriptionFragment)"/>
						<plx:function name="GetBy{$keysName}" visibility="public">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>
Gets rows from the datasource based on the <xsl:value-of select="@name"/> key.
<xsl:value-of select="@name"/> Description: <xsl:value-of select="$keyDescription"/>
</summary>
									<xsl:copy-of select="$docCommentColumnParamsFragment"/>
									<returns>Returns a typed collection of <xsl:value-of select="$entityClassName"/> objects.</returns>
								</plx:docComment>
							</plx:leadingInfo>
							<xsl:copy-of select="$columnParamsFragment"/>
							<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
							<plx:local name="count" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="-1" type="i4"/>
								</plx:initialize>
							</plx:local>
							<plx:return>
								<plx:callThis name="GetBy{$keysName}">
									<xsl:copy-of select="$forwardColumnParamsFragment"/>
									<plx:passParam>
										<plx:value data="0" type="i4"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callStatic name="MaxValue" dataTypeName=".i4" type="property"/>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="count"/>
									</plx:passParam>
								</plx:callThis>
							</plx:return>
						</plx:function>
						<plx:function name="GetBy{$keysName}" visibility="public">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>
Gets rows from the datasource based on the <xsl:value-of select="@name"/> key.
<xsl:value-of select="@name"/> Description: <xsl:value-of select="$keyDescription"/>
</summary>
									<param name="transactionManager"><see cref="TransactionManager"/> object</param>
									<xsl:copy-of select="$docCommentColumnParamsFragment"/>
									<returns>Returns a typed collection of <xsl:value-of select="$entityClassName"/> objects.</returns>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
							<xsl:copy-of select="$columnParamsFragment"/>
							<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
							<plx:local name="count" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="-1" type="i4"/>
								</plx:initialize>
							</plx:local>
							<plx:return>
								<plx:callThis name="GetBy{$keysName}">
									<plx:passParam>
										<plx:nameRef name="transactionManager" type="parameter"/>
									</plx:passParam>
									<xsl:copy-of select="$forwardColumnParamsFragment"/>
									<plx:passParam>
										<plx:value data="0" type="i4"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callStatic name="MaxValue" dataTypeName=".i4" type="property"/>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="count"/>
									</plx:passParam>
								</plx:callThis>
							</plx:return>
						</plx:function>
						<plx:function name="GetBy{$keysName}" visibility="public">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>
Gets rows from the datasource based on the <xsl:value-of select="@name"/> key.
<xsl:value-of select="@name"/> Description: <xsl:value-of select="$keyDescription"/>
</summary>
									<param name="transactionManager"><see cref="TransactionManager"/> object</param>
									<xsl:copy-of select="$docCommentColumnParamsFragment"/>
									<param name="start">Row number at which to start reading, the first row is 0.</param>
									<param name="pageLength">Number of rows to return.</param>
									<returns>Returns a typed collection of <xsl:value-of select="$entityClassName"/> objects.</returns>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
							<xsl:copy-of select="$columnParamsFragment"/>
							<plx:param name="start" dataTypeName=".i4"/>
							<plx:param name="pageLength" dataTypeName=".i4"/>
							<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
							<plx:local name="count" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="-1" type="i4"/>
								</plx:initialize>
							</plx:local>
							<plx:return>
								<plx:callThis name="GetBy{$keysName}">
									<plx:passParam>
										<plx:nameRef name="transactionManager" type="parameter"/>
									</plx:passParam>
									<xsl:copy-of select="$forwardColumnParamsFragment"/>
									<plx:passParam>
										<plx:nameRef name="start" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="pageLength" type="parameter"/>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="count"/>
									</plx:passParam>
								</plx:callThis>
							</plx:return>
						</plx:function>
						<plx:function name="GetBy{$keysName}" visibility="public">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>
Gets rows from the datasource based on the <xsl:value-of select="@name"/> key.
<xsl:value-of select="@name"/> Description: <xsl:value-of select="$keyDescription"/>
</summary>
									<xsl:copy-of select="$docCommentColumnParamsFragment"/>
									<param name="start">Row number at which to start reading, the first row is 0.</param>
									<param name="pageLength">Number of rows to return.</param>
									<returns>Returns a typed collection of <xsl:value-of select="$entityClassName"/> objects.</returns>
								</plx:docComment>
							</plx:leadingInfo>
							<xsl:copy-of select="$columnParamsFragment"/>
							<plx:param name="start" dataTypeName=".i4"/>
							<plx:param name="pageLength" dataTypeName=".i4"/>
							<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
							<plx:local name="count" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="-1" type="i4"/>
								</plx:initialize>
							</plx:local>
							<plx:return>
								<plx:callThis name="GetBy{$keysName}">
									<plx:passParam>
										<plx:nullKeyword/>
									</plx:passParam>
									<xsl:copy-of select="$forwardColumnParamsFragment"/>
									<plx:passParam>
										<plx:nameRef name="start" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="pageLength" type="parameter"/>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="count"/>
									</plx:passParam>
								</plx:callThis>
							</plx:return>
						</plx:function>
						<plx:function name="GetBy{$keysName}" visibility="public">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>
Gets rows from the datasource based on the <xsl:value-of select="@name"/> key.
<xsl:value-of select="@name"/> Description: <xsl:value-of select="$keyDescription"/>
</summary>
									<xsl:copy-of select="$docCommentColumnParamsFragment"/>
									<param name="start">Row number at which to start reading, the first row is 0.</param>
									<param name="pageLength">Number of rows to return.</param>
									<param name="count">out parameter to get total records for query</param>
									<returns>Returns a typed collection of <xsl:value-of select="$entityClassName"/> objects.</returns>
								</plx:docComment>
							</plx:leadingInfo>
							<xsl:copy-of select="$columnParamsFragment"/>
							<plx:param name="start" dataTypeName=".i4"/>
							<plx:param name="pageLength" dataTypeName=".i4"/>
							<plx:param name="count" dataTypeName=".i4" type="out"/>
							<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
							<plx:return>
								<plx:callThis name="GetBy{$keysName}">
									<plx:passParam>
										<plx:nullKeyword/>
									</plx:passParam>
									<xsl:copy-of select="$forwardColumnParamsFragment"/>
									<plx:passParam>
										<plx:nameRef name="start" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="pageLength" type="parameter"/>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="count" type="parameter"/>
									</plx:passParam>
								</plx:callThis>
							</plx:return>
						</plx:function>
						<plx:function name="GetBy{$keysName}" visibility="public" modifier="abstract">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>
Gets rows from the datasource based on the <xsl:value-of select="@name"/> key.
<xsl:value-of select="@name"/> Description: <xsl:value-of select="$keyDescription"/>
</summary>
									<param name="transactionManager"><see cref="TransactionManager"/> object</param>
									<xsl:copy-of select="$docCommentColumnParamsFragment"/>
									<param name="start">Row number at which to start reading, the first row is 0.</param>
									<param name="pageLength">Number of rows to return.</param>
									<param name="count">out parameter to get total records for query</param>
									<returns>Returns a typed collection of <xsl:value-of select="$entityClassName"/> objects.</returns>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
							<xsl:copy-of select="$columnParamsFragment"/>
							<plx:param name="start" dataTypeName=".i4"/>
							<plx:param name="pageLength" dataTypeName=".i4"/>
							<plx:param name="count" dataTypeName=".i4" type="out"/>
							<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>
						</plx:function>
					</xsl:if>
				</xsl:for-each>
				<plx:pragma type="closeRegion" data="Get By Foreign Key Functions"/>
			</xsl:if>

			<!-- Start Mike/Tommy code -->
			<xsl:if test="$settings['IncludeGetListByIX']">
				<plx:pragma type="region" data="Get By Index Functions"/>
				<xsl:for-each select="se:indexes/se:index">
					<xsl:variable name="fullColumnParamsFragment">
						<xsl:call-template name="GetColumnParams">
							<xsl:with-param name="table" select="$currentTable"/>
							<xsl:with-param name="columnNames" select="se:column/@ref"/>
							<xsl:with-param name="getPropertyNames" select="true()" />
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="fullColumnParams" select="exsl:node-set($fullColumnParamsFragment)/child::*"/>
					<xsl:variable name="columnParamsFragment">
						<xsl:for-each select="$fullColumnParams">
							<xsl:copy>
								<xsl:copy-of select="@*[not(namespace-uri())]|*"/>
							</xsl:copy>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="docCommentColumnParamsFragment">
						<xsl:for-each select="$fullColumnParams">
							<param name="{@name}">
								<xsl:value-of select="child::comment()"/>
							</param>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="forwardColumnParamsFragment">
						<xsl:for-each select="$fullColumnParams">
							<plx:passParam>
								<plx:nameRef name="{@name}" type="parameter"/>
							</plx:passParam>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="keysNameFragment">
						<xsl:for-each select="$fullColumnParams">
							<xsl:value-of select="@tmp:propertyName"/>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="keysName" select="string($keysNameFragment)"/>
					<xsl:variable name="getMethodName" select="$settings[@name='MethodNames']/*[@name='Get']"/>
					<xsl:if test="@isPrimary='true' or @isPrimary='1'">
						<plx:function name="{$getMethodName}" visibility="public" modifier="override">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>
Gets a row from the DataSource based on its primary key.
</summary>
									<param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
									<param name="key">The unique identifier of the row to retrieve.</param>
									<param name="start">Row number at which to start reading, the first row is 0.</param>
									<param name="pageLength">Number of rows to return.</param>
									<returns>Returns an instance of the Entity class.</returns>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
							<plx:param name="key" dataTypeName="{$keyClassName}"/>
							<plx:param name="start" dataTypeName=".i4"/>
							<plx:param name="pageLength" dataTypeName=".i4"/>
							<plx:returns dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
							<plx:return>
								<plx:callThis name="GetBy{$keysName}">
									<plx:passParam>
										<plx:nameRef name="transactionManager" type="parameter"/>
									</plx:passParam>
									<xsl:for-each select="$fullColumnParams">
										<plx:passParam>
											<plx:callInstance name="{@tmp:propertyName}" type="property">
												<plx:callObject>
													<plx:nameRef name="key" type="parameter"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</xsl:for-each>
									<plx:passParam>
										<plx:nameRef name="start" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="pageLength" type="parameter"/>
									</plx:passParam>
								</plx:callThis>
							</plx:return>
						</plx:function>
					</xsl:if>
					<xsl:variable name="FullReturnType" select="concat($BLLNamespace,'.',$entityClassName)"/>

					<!-- Method starting on line 16 -->
					<plx:function name="GetBy{$keysName}" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
Gets rows from the datasource based on the primary key <xsl:value-of select="@name" /> index.
</summary>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$FullReturnType"/>"/&gt; class.</returns>
							</plx:docComment>
						</plx:leadingInfo>
						<xsl:copy-of select="$columnParamsFragment"/>
						<plx:returns dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
						<plx:local name="count" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="-1" type="i4" />
							</plx:initialize>
						</plx:local>
						<plx:return>
							<plx:callThis name="GetBy{$keysName}">
								<plx:passParam>
									<plx:nullKeyword />
								</plx:passParam>
								<xsl:copy-of select="$forwardColumnParamsFragment"/>
								<plx:passParam>
									<plx:value data="0" type="i4" />
								</plx:passParam>
								<plx:passParam>
									<plx:callStatic name="MaxValue" dataTypeName=".i4"/>
								</plx:passParam>
								<plx:passParam type="out">
									<plx:nameRef name="count"/>
								</plx:passParam>
							</plx:callThis>
						</plx:return>
					</plx:function>

					<!-- Method Starting on Line 28 -->
					<plx:function name="GetBy{$keysName}" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
Gets rows from the datasource based on the primary key <xsl:value-of select="@name" /> index.
</summary>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<param name="start">Row number at which to start reading, the first row is 0.</param>
								<param name="pageLength">Number of rows to return.</param>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$FullReturnType"/>"/&gt; class.</returns>
							</plx:docComment>
						</plx:leadingInfo>
						<xsl:copy-of select="$columnParamsFragment"/>
						<plx:param name="start" dataTypeName=".i4"/>
						<plx:param name="pageLength" dataTypeName=".i4"/>
						<plx:returns dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
						<plx:local name="count" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="-1" type="i4" />
							</plx:initialize>
						</plx:local>
						<plx:return>
							<plx:callThis name="GetBy{$keysName}">
								<plx:passParam>
									<plx:nullKeyword />
								</plx:passParam>
								<xsl:copy-of select="$forwardColumnParamsFragment"/>
								<plx:passParam>
									<plx:nameRef name="start" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="pageLength" type="parameter"/>
								</plx:passParam>
								<plx:passParam type="out">
									<plx:nameRef name="count"/>
								</plx:passParam>
							</plx:callThis>
						</plx:return>
					</plx:function>

					<!-- Method Starting on Line 43 -->
					<plx:function name="GetBy{$keysName}" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
Gets rows from the datasource based on the primary key <xsl:value-of select="@name" /> index.
</summary>
								<param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$FullReturnType"/>"/&gt; class.</returns>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
						<xsl:copy-of select="$columnParamsFragment"/>
						<plx:returns dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
						<plx:local name="count" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="-1" type="i4" />
							</plx:initialize>
						</plx:local>
						<plx:return>
							<plx:callThis name="GetBy{$keysName}">
								<plx:passParam>
									<plx:nameRef name="transactionManager" type="parameter"/>
								</plx:passParam>
								<xsl:copy-of select="$forwardColumnParamsFragment"/>
								<plx:passParam>
									<plx:value data="0" type="i4" />
								</plx:passParam>
								<plx:passParam>
									<plx:callStatic name="MaxValue" dataTypeName=".i4"/>
								</plx:passParam>
								<plx:passParam type="out">
									<plx:nameRef name="count"/>
								</plx:passParam>
							</plx:callThis>
						</plx:return>
					</plx:function>

					<!-- Method starting on line 57 -->
					<plx:function name="GetBy{$keysName}" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
Gets a row from the DataSource based on its primary key.
</summary>
								<param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<param name="start">Row number at which to start reading, the first row is 0.</param>
								<param name="pageLength">Number of rows to return.</param>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$FullReturnType"/>"/&gt; class.</returns>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
						<xsl:copy-of select="$columnParamsFragment"/>
						<plx:param name="start" dataTypeName=".i4"/>
						<plx:param name="pageLength" dataTypeName=".i4"/>
						<plx:returns dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
						<plx:local name="count" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="-1" type="i4" />
							</plx:initialize>
						</plx:local>
						<plx:return>
							<plx:callThis name="GetBy{$keysName}">
								<plx:passParam>
									<plx:nameRef name="transactionManager" type="parameter"/>
								</plx:passParam>
								<xsl:copy-of select="$forwardColumnParamsFragment"/>
								<plx:passParam>
									<plx:nameRef name="start" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="pageLength" type="parameter"/>
								</plx:passParam>
								<plx:passParam type="out">
									<plx:nameRef name="count"/>
								</plx:passParam>
							</plx:callThis>
						</plx:return>
					</plx:function>

					<!-- Method starting on line 74 -->
					<plx:function name="GetBy{$keysName}" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
Gets a row from the DataSource based on its primary key.
</summary>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<param name="start">Row number at which to start reading, the first row is 0.</param>
								<param name="pageLength">Number of rows to return.</param>
								<param name="count">out parameter to get total records for query.</param>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$FullReturnType"/>"/&gt; class.</returns>
							</plx:docComment>
						</plx:leadingInfo>
						<xsl:copy-of select="$columnParamsFragment"/>
						<plx:param name="start" dataTypeName=".i4"/>
						<plx:param name="pageLength" dataTypeName=".i4"/>
						<plx:param name="count" dataTypeName=".i4" type="out" />
						<plx:returns dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
						<plx:return>
							<plx:callThis name="GetBy{$keysName}">
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<xsl:copy-of select="$forwardColumnParamsFragment"/>
								<plx:passParam>
									<plx:nameRef name="start" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="pageLength" type="parameter"/>
								</plx:passParam>
								<plx:passParam type="out">
									<plx:nameRef name="count"/>
								</plx:passParam>
							</plx:callThis>
						</plx:return>
					</plx:function>

					<!-- Method starting on line 89 -->
					<plx:function name="GetBy{$keysName}" visibility="public" modifier="abstract">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
Gets a row from the DataSource based on its primary key.
</summary>
								<param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<param name="start">Row number at which to start reading, the first row is 0.</param>
								<param name="pageLength">Number of rows to return.</param>
								<param name="count">The total number of records.</param>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$FullReturnType"/>"/&gt; class.</returns>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:param name="transactionManager" dataTypeName="TransactionManager"/>
						<xsl:copy-of select="$columnParamsFragment"/>
						<plx:param name="start" dataTypeName=".i4"/>
						<plx:param name="pageLength" dataTypeName=".i4"/>
						<plx:param name="count" dataTypeName=".i4" type="out" />
						<plx:returns dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
					</plx:function>

				</xsl:for-each>
				<plx:pragma type="closeRegion" data="Get By Index Functions"/>
			</xsl:if>
			<!-- End Mike/Tommy code -->

		</plx:class>
		<plx:pragma type="closeRegion" data="Classes for {@name}"/>
	</xsl:template>
	<!-- Build a set of plx:param elements from a set of column names and a table.
	Includes a comment with the parameter description, which can be stripped when
	using the generated fragment to spit plx:param elements. -->
	<xsl:template name="GetColumnParams">
		<xsl:param name="table"/>
		<xsl:param name="columnNames"/>
		<xsl:param name="getPropertyNames" select="false()"/>
		<!-- Corresponds to GetFunctionHeaderParameters in .NETTiers source -->
		<xsl:variable name="tableColumns" select="$table/se:columns/se:column"/>
		<xsl:for-each select="$columnNames">
			<xsl:for-each select="$tableColumns[@name=current()]">
				<plx:param name="{csc:GetFieldNameForColumn($table/@owner, $table/@name, @name)}">
					<xsl:if test="$getPropertyNames='true' or $getPropertyNames='1'">
						<xsl:attribute name="tmp:propertyName">
							<xsl:value-of select="csc:GetPropertyNameForColumn($table/@owner, $table/@name, @name)"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:call-template name="ApplyPLiXDataType">
						<xsl:with-param name="nativeType" select="@nativeType"/>
						<xsl:with-param name="nullable" select="@nullable='true' or @nullable='1'"/>
					</xsl:call-template>
					<xsl:comment>
						<xsl:value-of select="se:description"/>
					</xsl:comment>
				</plx:param>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="ApplyPLiXDataType">
		<xsl:param name="nativeType"/>
		<xsl:param name="nullable" select="false()"/>
		<xsl:choose>
			<xsl:when test="$nullable">
				<xsl:variable name="passTypeParamFragment">
					<plx:passTypeParam>
						<xsl:call-template name="ApplyPLiXDataType">
							<xsl:with-param name="nativeType" select="$nativeType"/>
						</xsl:call-template>
					</plx:passTypeParam>
				</xsl:variable>
				<xsl:for-each select="exsl:node-set($passTypeParamFragment)/child::*">
					<xsl:choose>
						<xsl:when test="@dataTypeIsSimpleArray='1' or @dataTypeIsSimpleArray='true' or plx:arrayDescriptor or @dataTypeName='.string' or @dataTypeName='.object'">
							<xsl:copy-of select="@*"/>
							<xsl:copy-of select="child::*"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="dataTypeName">
								<xsl:text>Nullable</xsl:text>
							</xsl:attribute>
							<xsl:attribute name="dataTypeQualifier">
								<xsl:text>System</xsl:text>
							</xsl:attribute>
							<xsl:copy-of select="."/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="testType" select="string(@nativeType)"/>
				<xsl:choose>
					<xsl:when test="$testType='bigint'">
						<xsl:attribute name="dataTypeName">.i8</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='binary'">
						<xsl:attribute name="dataTypeName">.object</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='bit'">
						<xsl:attribute name="dataTypeName">.boolean</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='char'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='character varying'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='datetime'">
						<xsl:attribute name="dataTypeName">.date</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='decimal'">
						<xsl:attribute name="dataTypeName">.decimal</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='double precision'">
						<xsl:attribute name="dataTypeName">.r8</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='float'">
						<xsl:attribute name="dataTypeName">.r8</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='image'">
						<xsl:attribute name="dataTypeName">.u1</xsl:attribute>
						<xsl:attribute name="dataTypeIsSimpleArray">true</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='int'">
						<xsl:attribute name="dataTypeName">.i4</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='integer'">
						<xsl:attribute name="dataTypeName">.i4</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='interval'">
						<xsl:attribute name="dataTypeName">.date</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='money'">
						<xsl:attribute name="dataTypeName">.decimal</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='nchar'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='ntext'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='numeric'">
						<xsl:attribute name="dataTypeName">.decimal</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='nvarchar'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='real'">
						<xsl:attribute name="dataTypeName">.r4</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='smalldatetime'">
						<xsl:attribute name="dataTypeName">.date</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='smallint'">
						<xsl:attribute name="dataTypeName">.i2</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='smallmoney'">
						<xsl:attribute name="dataTypeName">.decimal</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='text'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='timestamp'">
						<xsl:attribute name="dataTypeName">.u1</xsl:attribute>
						<xsl:attribute name="dataTypeIsSimpleArray">true</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='tinyint'">
						<xsl:attribute name="dataTypeName">.i1</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='uniqueidentifier'">
						<xsl:attribute name="dataTypeName">Guid</xsl:attribute>
						<xsl:attribute name="dataTypeQualifier">System</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='varbinary'">
						<xsl:attribute name="dataTypeName">.u1</xsl:attribute>
						<xsl:attribute name="dataTypeIsSimpleArray">true</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='varchar'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='xml'">
						<xsl:attribute name="dataTypeName">.string</xsl:attribute>
					</xsl:when>
					<xsl:when test="$testType='sql_variant'">
						<xsl:attribute name="dataTypeName">.object</xsl:attribute>
					</xsl:when>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<msxsl:script implements-prefix="csc" language="CSharp">
		// Enum definitions
		<![CDATA[
		/// <summary>
		/// Indicates the style of Pascal casing to be used
		/// </summary>
		private enum PascalCasingStyle
		{
			/// <summary>
			/// No pascal casing is applied
			/// </summary>
			None,
			
			/// <summary>
			/// Original .NetTiers styling (pre SVN553)
			/// </summary>
			Style1,
			
			/// <summary>
			/// New styling that handles uppercase (post SVN552)
			/// </summary>
			Style2,
		}
		private enum ReturnFields
		{
			EntityName,
			PropertyName,
			FieldName,
			Id,
			CSType,
			FriendlyName
		}

		private enum ClassNameFormat
		{
			None,
			Base,
			Abstract,
			Interface,
			Key,
			Column,
			Comparer,
			EventHandler,
			EventArgs,
			Partial,
			PartialAbstract,
			PartialAbstractService,
			PartialCollection,
			PartialProviderBase,
			PartialUnitTest,
			Service,
			AbstractService,
			Proxy,
			Enum,
			Struct,
			Collection,
			AbstractCollection,
			CollectionProperty,
			ViewCollection,
			Provider,
			ProviderInterface,
			ProviderBase,
			UnitTest,
			Repository,
			AbstractRepository
		}
		]]>
		// Initial settings
		<![CDATA[
		private bool _changeUnderscoreToPascalCase = true;
		public void SetChangeUnderscoreToPascalCase(bool value)
		{
			_changeUnderscoreToPascalCase = value;
		}
		private PascalCasingStyle _usePascalCasing = PascalCasingStyle.Style2;
		public void SetUsePascalCasing(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				_usePascalCasing = (PascalCasingStyle)Enum.Parse(typeof(PascalCasingStyle), value);
			}
		}
		private string _entityFormat = "{0}";
		public void SetEntityFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1)
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EntityFormat");
				}
				_entityFormat = value;
			}
		}
		private string _entityKeyFormat = "{0}Key";
		public void SetEntityKeyFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EntityKeyFormat");
				}
				_entityKeyFormat = value;
			}
		}
		private string _entityDataFormat 	= "{0}EntityData";
		public void SetEntityDataFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EntityDataFormat");
				}
				_entityDataFormat = value;
			}
		}
		private string _collectionFormat = "{0}Collection";
		public void SetCollectionFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "CollectionFormat");
				}
				_collectionFormat = value;
			}
		}
		private string _providerFormat = "{0}Provider";
		public void SetProviderFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "ProviderFormat");
				}
				_providerFormat = value;
			}
		}
		private string _interfaceFormat = "I{0}";
		public void SetInterfaceFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "InterfaceFormat");
				}
				_interfaceFormat = value;
			}
		}
		private string _baseClassFormat = "{0}Base";
		public void SetBaseClassFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "BaseClassFormat");
				}
				_baseClassFormat = value;
			}
		}
		private string _enumFormat = "{0}List";
		public void SetEnumFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EnumFormat");
				}
				_enumFormat = value;
			}
		}
		private string _manyToManyFormat = "{0}From{1}";
		public void SetManyToManyFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1 || value.IndexOf("{1}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the patterns {0} and {1} to be valid.", "ManyToManyFormat");
				}
				_manyToManyFormat = value;
			}
		}
		private string _serviceClassNameFormat = "{0}Service";
		public void SetServiceClassNameFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "ServiceClassNameFormat");
				}
				_serviceClassNameFormat = value;
			}
		}
		private string _safeNamePrefix = "SafeName_";
		public void SetSafeNamePrefix(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				_serviceClassNameFormat = value;
			}
		}
		private string _genericListFormat = "TList<{0}>";
		private string _genericViewFormat = "VList<{0}>";
		private string _unitTestFormat = "{0}Test";
		private string _strippedTablePrefixes = "tbl;tbl_";
		private string _strippedTableSuffixes= "_t";
		]]>
		// Specific casing queries
		<![CDATA[
		public string GetFieldNameForColumn(string ownerName, string tableName, string columnName)
		{
			return GetAliasName(ownerName, tableName, columnName, ReturnFields.FieldName);
		}
		public string GetClassNameForTable(string ownerName, string tableName, string classNameFormat)
		{
			ClassNameFormat format = ClassNameFormat.None;
			if (!string.IsNullOrEmpty(classNameFormat))
			{
				format = (ClassNameFormat)Enum.Parse(typeof(ClassNameFormat), classNameFormat);
			}
			return GetFormattedClassName(GetAliasName(ownerName, tableName, null, ReturnFields.EntityName), format);
		}
		public string GetPropertyNameForColumn(string ownerName, string tableName, string columnName)
		{
			return GetAliasName(ownerName, tableName, columnName, ReturnFields.PropertyName);
		}
		public string GetManyToManyName(string combinedColumnNames, string ownerName, string tableName)
		{
			return string.Format(_manyToManyFormat, combinedColumnNames, GetClassNameForTable(ownerName, tableName, ""));
		}
		]]>
		// Casing routines
		<![CDATA[
		/// <summary>
		/// Get the camel cased version of a name.  
		/// If the name is all upper case, change it to all lower case
		/// </summary>
		/// <param name="name">Name to be changed</param>
		/// <returns>CamelCased version of the name</returns>
		private string GetCamelCaseName(string name)
		{
			if (name == null)
				return string.Empty;
			// first get the PascalCase version of the name
			string pascalName = GetPascalCaseName(name);
			// now lowercase the first character to transform it to camelCase
			return pascalName.Substring(0, 1).ToLower() + pascalName.Substring(1);
		}

		/// <summary>
		/// Get the Pascal cased version of a name.  
		/// </summary>
		/// <param name="name">Name to be changed</param>
		/// <returns>PascalCased version of the name</returns>
		private string GetPascalCaseName(string name)
		{
			string result = name;
			switch (_usePascalCasing)
			{
				case PascalCasingStyle.Style1 :
					result = GetPascalCaseNameStyle1(name);
					break;
				case PascalCasingStyle.Style2 :
					result = GetPascalCaseNameStyle2(name);
					break;
			}
			return result;
		}
		/// <summary>
		/// Get the Pascal cased version of a name.  
		/// </summary>
		private string GetPascalCaseNameStyle1(string name)
		{
			string[] splitNames;
			name = name.Trim();
			if (_changeUnderscoreToPascalCase)
			{
				char[] splitter = {'_', ' '};
				splitNames = name.Split(splitter);
			}
			else
			{
				char[] splitter =  {' '};
				splitNames = name.Split(splitter);
			}
			
			string pascalName = "";
			foreach (string s in splitNames)
			{
				if (s.Length > 0)
				{
					pascalName += s.Substring(0, 1).ToUpper() + s.Substring(1);
				}
			}

			return pascalName;
		}
		/// <summary>
		/// Gets the pascal case name of a string.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private string GetPascalCaseNameStyle2(string name)
		{
			string pascalName = string.Empty;
			// UNDONE: a-zA-Z is too restrictive here
			string notStartingAlpha = Regex.Replace(name, "^[^a-zA-Z]+", string.Empty);
			string workingString = ToLowerExceptCamelCase(notStartingAlpha);
			pascalName = RemoveSeparatorAndCapNext(workingString);

			return pascalName;
		}
		/// <summary>
		/// Converts a pascal string to a spaced string
		/// </summary>
		private static string PascalToSpaced(string name)
		{
			// ignore missing text
			if (string.IsNullOrEmpty(name))
				return string.Empty;
			// split the words
			Regex regex = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
			name = regex.Replace(name, " ${x}");
			// get rid of any underscores or dashes
			name = name.Replace("_", string.Empty);
			return name.Replace("-", string.Empty);
		}
		private static string ToLowerExceptCamelCase(string input)
		{
			char[] chars = input.ToCharArray();
			char[] origChars = input.ToCharArray();

			for (int i = 0; i < chars.Length; i++)
			{
				int left = (i > 0 ? i - 1 : i);
				int right = (i < chars.Length - 1 ? i + 1 : i);

				if (i != left &&
						i != right)
				{
					if (Char.IsUpper(chars[i]) &&
							Char.IsLetter(chars[left]) &&
							Char.IsUpper(chars[left]))
					{
						chars[i] = Char.ToLower(chars[i], System.Globalization.CultureInfo.InvariantCulture);
					}
					else if (Char.IsUpper(chars[i]) &&
							Char.IsLetter(chars[right]) &&
							Char.IsUpper(chars[right]) &&
							Char.IsUpper(origChars[left]))
					{
						chars[i] = Char.ToLower(chars[i], System.Globalization.CultureInfo.InvariantCulture);
					}
					else if (Char.IsUpper(chars[i]) &&
							!Char.IsLetter(chars[right]))
					{
						chars[i] = Char.ToLower(chars[i], System.Globalization.CultureInfo.InvariantCulture);
					}
				}

				string x = new string(chars);
			}

			if (chars.Length > 0)
			{
				chars[chars.Length - 1] = Char.ToLower(chars[chars.Length - 1], System.Globalization.CultureInfo.InvariantCulture);
			}

			return new string(chars);
		}
		/// <summary>
		/// Removes the separator and capitalises next character.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		private string RemoveSeparatorAndCapNext(string input)
		{
			char[] splitter = new char[] { '-', '_', ' ' }; // potential chars to split on
			string workingString = input.TrimEnd(splitter);
			char[] chars = workingString.ToCharArray();

			if (chars.Length > 0)
			{
				int under = workingString.IndexOfAny(splitter);
				while (under > -1)
				{
					chars[under + 1] = Char.ToUpper(chars[under + 1], System.Globalization.CultureInfo.InvariantCulture);
					workingString = new String(chars);
					under = workingString.IndexOfAny(splitter, under + 1);
				}

				chars[0] = Char.ToUpper(chars[0], System.Globalization.CultureInfo.InvariantCulture);

				workingString = new string(chars);
			}
			string regexReplacer = "[" + new string(_changeUnderscoreToPascalCase ? new char[] { '-', '_', ' ' } : new char[] { ' ' }) + "]";

			return Regex.Replace(workingString, regexReplacer, string.Empty);
		}
	]]>
		// Class name helpers
		<![CDATA[
		private string GetFormattedClassName(string name, ClassNameFormat format)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			switch (format)
			{
				case ClassNameFormat.None:
					return name;

				case ClassNameFormat.Base:
				case ClassNameFormat.Abstract:
					return string.Format(_baseClassFormat, name);

				case ClassNameFormat.Interface:
					return string.Format("I{0}", name);

				case ClassNameFormat.Key:
					return string.Format(_entityKeyFormat, name);

				case ClassNameFormat.Column:
					return string.Format("{0}Column", name);

				case ClassNameFormat.Comparer:
					return string.Format("{0}Comparer", name);

				case ClassNameFormat.EventHandler:
					return string.Format("{0}EventHandler", name);

				case ClassNameFormat.EventArgs:
					return string.Format("{0}EventArgs", name);

				case ClassNameFormat.Partial:
					return string.Format("{0}.generated", name);

				case ClassNameFormat.PartialAbstract:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Abstract), ClassNameFormat.Partial);

				case ClassNameFormat.PartialCollection:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Collection), ClassNameFormat.Partial);

				case ClassNameFormat.PartialProviderBase:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.ProviderBase), ClassNameFormat.Partial);

				case ClassNameFormat.PartialUnitTest:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.UnitTest), ClassNameFormat.Partial);

				case ClassNameFormat.PartialAbstractService:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.AbstractService), ClassNameFormat.Partial);

				case ClassNameFormat.Service:
					return string.Format(_serviceClassNameFormat, name);

				case ClassNameFormat.AbstractService:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Service), ClassNameFormat.Abstract);

				case ClassNameFormat.Proxy:
					return string.Format("{0}Services", name);

				case ClassNameFormat.Enum:
					return string.Format(_enumFormat, name);

				case ClassNameFormat.Struct:
					return string.Format(_entityDataFormat, name);

				case ClassNameFormat.Collection:
					return string.Format(_genericListFormat, name);

				case ClassNameFormat.AbstractCollection:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Collection), ClassNameFormat.Abstract);

				case ClassNameFormat.CollectionProperty:
					return string.Format(_collectionFormat, name);

				case ClassNameFormat.ViewCollection:
					return string.Format(_genericViewFormat, name);

				case ClassNameFormat.Provider:
				case ClassNameFormat.Repository:
					return string.Format(_providerFormat, name);

				case ClassNameFormat.AbstractRepository:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Repository), ClassNameFormat.Abstract);

				case ClassNameFormat.ProviderInterface:
					return string.Format(_interfaceFormat, GetFormattedClassName(name, ClassNameFormat.Provider));

				case ClassNameFormat.ProviderBase:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Provider), ClassNameFormat.Base);

				case ClassNameFormat.UnitTest:
					return string.Format(_unitTestFormat, name);

				default:
					throw new ArgumentOutOfRangeException("format");
			}
		}
		/// <summary>
		/// This function get the alias name for this object name.
		/// </summary>
		/// <remark>This function should not be called directly, but via the GetClassName.</remark>
		private string GetAliasName(string owner, string obj, string item, ReturnFields returnType)
		{
			// UNDONE: Note that this is skipping the NameConversionType values in .NETTiers. This
			// is simply NameConversionType.None
			string name = string.Empty;
			// get the name
			if (!string.IsNullOrEmpty(obj) && string.IsNullOrEmpty(item)) // table/view names
			{
				name = obj;
				char[] delims = new char[] {',', ';'};
				// strip the prefix
				string[] strips = _strippedTablePrefixes.ToLower().Split(delims);
				foreach(string strip in strips)
					if (name.ToLower().StartsWith(strip))
						{
							name = name.Remove(0, strip.Length);
							continue;
						}
				// strip the suffix
				strips = _strippedTableSuffixes.Split(delims);
				foreach(string strip in strips)
				{
					if (name.ToLower().EndsWith(strip))
					{
						name = name.Remove(name.Length - strip.Length, strip.Length);
						continue;
					}
				}
			}
			else if (!string.IsNullOrEmpty(obj) && !string.IsNullOrEmpty(item)) // column names
			{
				name = item;
			}
			else
			{
				throw new ArgumentNullException();
			}

			// return the formatted name
			switch (returnType)
			{
				case ReturnFields.EntityName:
				case ReturnFields.PropertyName:
					name = GetCSharpSafeName(name);
					return GetPascalCaseName(name); // class and property names are pascal-cased
				case ReturnFields.FieldName:
					name = GetCSharpSafeName(name);
					return GetCamelCaseName(name); // fields (private member variables) are camel-cased
				case ReturnFields.FriendlyName:
					return PascalToSpaced(GetPascalCaseName(name)); // just return the pascal name with spaces
				case ReturnFields.Id:
				case ReturnFields.CSType:
				default:
					return string.Empty; // what should happen here, exactly?
			}
		}
		/// <summary>
		/// Gets a C Sharp safe version of the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private string GetCSharpSafeName( string name )
		{
			string result = name;

			// we must have something to start with!
			if (!IsValidCSharpName( result ))
			{
				result = _safeNamePrefix + result;

				// replace any non valid char with an underscore
				// UNDONE: a-zA-Z0-9 is too restrictive here
				result = Regex.Replace( result, "[^a-zA-Z0-9_]", "_" );
			}

			return result;
		}
		private static string[] _csharpKeywords = PopulateCSharpKeywords();
		private static string[] PopulateCSharpKeywords()
		{
			string[] names = new string[] 
			{
					"abstract","event", "new", "struct", 
					"as", "explicit", "null", "switch",
					"base", "extern", "object", "this",
					"bool", "false", "operator", "throw",
					"break", "finally", "out", "true",
					"byte", "fixed", "override", "try",
					"case", "float", "params", "typeof",
					"catch", "for", "private", "uint",
					"char", "foreach", "protected", "ulong",
					"checked", "goto", "public", "unchecked",
					"class", "if", "readonly", "unsafe",
					"const", "implicit", "ref", "ushort",
					"continue","in","return","using",
					"decimal","int","sbyte","virtual",
					"default","interface","sealed","volatile",
					"delegate","internal","short","void",
					"do","is","sizeof","while",
					"double","lock","stackalloc",
					"else","long","static",
					"enum","namespace", "string"
			};
			Array.Sort(names, CaseInsensitiveComparer.DefaultInvariant);
			return names;
		}
		/// <summary>
		/// Determines whether specified name is valid in C#.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		/// 	<c>true</c> if the name is valid; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValidCSharpName( string name )
		{
			// we assume that the name is invalid
			bool result = false;

			// we must have something to start with!
			if (!string.IsNullOrEmpty(name))
			{
				// the first char must not be a digit
				if (!char.IsDigit(name, 0))
				{
					// check if its a reserved C# keyword
					// Note this is changed from the .nettiers codebase. There is no need to use IndexOf here.
					if (Array.BinarySearch(_csharpKeywords, name, CaseInsensitiveComparer.DefaultInvariant) < 0)
					{
						// only letters, digits and underscores are allowed
						// we're also allowing spaces and dashes as the 
						// user has the option of suppressing those
						// UNDONE: a-zA-Z0-9 is too restrictive here
						Regex validChars = new Regex(@"[^a-zA-Z0-9_\s-]");
						result = !validChars.IsMatch(name);
					}
				}
			}
			return result;
		}
		]]>
	</msxsl:script>
</xsl:stylesheet>