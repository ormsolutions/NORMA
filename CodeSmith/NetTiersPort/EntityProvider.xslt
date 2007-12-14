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
	<xsl:include href="EntityScript.xslt"/>
	<!-- DEBUG: indent="yes" is for debugging only, has sideeffects on docComment output. -->
	<xsl:output method="xml" indent="no"/>
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:param name="NetTiersSettings" select="document('NETTiersSettings.xml')/child::*"/>
	<xsl:variable name="settings" select="$NetTiersSettings/child::*"/>
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
		<xsl:variable name="currentTableForeignKeys" select="se:keys/se:key"/>
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
		<plx:class name="{$baseProviderName}Core" visibility="public" modifier="abstract">
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
					<plx:param name="transactionManager" dataTypeName="TransactionManager" dataTypeQualifier="{$DALNamespace}"/>
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

					<!-- Method starting on line 16 -->
					<plx:function name="GetBy{$keysName}" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
Gets rows from the datasource based on the primary key <xsl:value-of select="@name" /> index.
</summary>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; class.</returns>
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
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; class.</returns>
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
								<returns>
									Returns an instance of the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; class.
								</returns>
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
								<param name="transactionManager">
									A <see cref="TransactionManager"/> object.
								</param>
								<xsl:copy-of select="$docCommentColumnParamsFragment"/>
								<param name="start">Row number at which to start reading, the first row is 0.</param>
								<param name="pageLength">Number of rows to return.</param>
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; class.</returns>
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
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; class.</returns>
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
								<returns>Returns an instance of the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; class.</returns>
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

			<plx:pragma type="region" data="Helper Functions"/>

			<xsl:variable name="fullColumnParamsFragment">
				<xsl:call-template name="GetColumnParams">
					<xsl:with-param name="table" select="$currentTable"/>
					<xsl:with-param name="columnNames" select="se:indexes/se:index/se:column/@ref"/>
					<xsl:with-param name="getPropertyNames" select="true()" />
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="fullColumnParams" select="exsl:node-set($fullColumnParamsFragment)/child::*"/>

			<plx:function name="Fill" visibility="public" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							Fill a &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; from a DataReader.
						</summary>
						<param name="reader">DataReader</param>
						<param name="rows">The collection to fill.</param>
						<param name="start">row number at which to start reading, the first row is 0.</param>
						<param name="pageLength">Number of rows.</param>
						<returns>
							a &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.TList&lt;<xsl:value-of select="$entityClassName"/>&gt;"/&gt;
						</returns>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="reader" dataTypeName="IDataReader" />
				<plx:param name="start" dataTypeName=".i4" />
				<plx:param name="pageLength" dataTypeName=".i4" />
				<xsl:copy-of select="$returnsEntityClassCollectionFragment"/>

				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="i" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="0" type="i4"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef name="i"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="start"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:branch>
						<plx:condition>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="Read">
									<plx:callObject>
										<plx:nameRef name="reader"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:condition>
						<plx:return>
							<plx:nameRef name="rows"/>
						</plx:return>
					</plx:branch>
				</plx:loop>

				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="i" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="0" type="i4"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef name="i"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="pageLength"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:branch>
						<plx:condition>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="Read">
									<plx:callObject>
										<plx:nameRef name="reader"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:condition>
						<plx:break />
					</plx:branch>
					<plx:local name="key" dataTypeName=".string">
						<plx:initialize>
							<plx:nullKeyword />
						</plx:initialize>
					</plx:local>
					<plx:local name="c" dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}">
						<plx:initialize>
							<plx:nullKeyword />
						</plx:initialize>
					</plx:local>

					<plx:branch>
						<plx:condition>
							<plx:callInstance name="UseEntityFactory" type="property">
								<plx:callObject>
									<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" />
								</plx:callObject>
							</plx:callInstance>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="key"/>
							</plx:left>
							<plx:right>
								<plx:callInstance name="ToString">
									<plx:callObject>
										<xsl:call-template name="KeyStringBuilder">
											<xsl:with-param name="Columns" select="$fullColumnParams"/>
											<xsl:with-param name="CurrentPosition" select="1"/>
											<xsl:with-param name="ItemCount" select="count($fullColumnParams)"/>
											<xsl:with-param name="EntityClassName" select="$entityClassName"/>
											<xsl:with-param name="BLLNamespace" select="$BLLNamespace"/>
										</xsl:call-template>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="c"/>
							</plx:left>
							<plx:right>
								<plx:callStatic name="LocateOrCreate" dataTypeName="EntityManager">
									<plx:passMemberTypeParam dataTypeName="{$entityClassName}" />
									<plx:passParam>
										<plx:callInstance name="ToString">
											<plx:callObject>
												<plx:nameRef name="key"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
									<plx:passParam>
										<plx:string>
											<xsl:copy-of select="$entityClassName"/>
										</plx:string>
									</plx:passParam>
									<plx:passParam>
										<plx:callInstance name="EntityCreationalFactoryType" type="property">
											<plx:callObject>
												<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" />
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
									<plx:passParam>
										<plx:callInstance name="EnableEntityTracking" type="property">
											<plx:callObject>
												<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" />
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callStatic>
							</plx:right>
						</plx:assign>
					</plx:branch>
					<plx:fallbackBranch>
						<!-- c = new EmployeePayHistory(); -->
						<plx:assign>
							<plx:left>
								<plx:nameRef name="c"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}"/>
							</plx:right>
						</plx:assign>
					</plx:fallbackBranch>
					<!-- if ((!DataRepository.Provider.EnableEntityTracking || (c.EntityState == EntityState.Added)) || 
					(DataRepository.Provider.EnableEntityTracking && (((DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.PreserveChanges) 
					&& (c.EntityState == EntityState.Unchanged)) || ((DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.DiscardChanges) 
					&& ((c.EntityState == EntityState.Unchanged) || (c.EntityState == EntityState.Changed)))))) {} -->
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:binaryOperator type="booleanOr">
										<plx:left>
											<plx:unaryOperator type="booleanNot">
												<plx:callInstance name="EnableEntityTracking" type="property">
													<plx:callObject>
														<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" />
													</plx:callObject>
												</plx:callInstance>
											</plx:unaryOperator>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="equality">
												<plx:left>
													<plx:callInstance name="EntityState" type="property">
														<plx:callObject>
															<plx:nameRef name="c"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:callStatic name="Added" type="field" dataTypeName="EntityState" />
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="booleanAnd">
										<plx:left>
											<plx:callInstance name="EnableEntityTracking" type="property">
												<plx:callObject>
													<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" />
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="booleanOr">
												<plx:left>
													<plx:binaryOperator type="booleanAnd">
														<plx:left>
															<plx:binaryOperator type="equality">
																<plx:left>
																	<plx:callInstance name="CurrentLoadPolicy" type="property">
																		<plx:callObject>
																			<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" />
																		</plx:callObject>
																	</plx:callInstance>
																</plx:left>
																<plx:right>
																	<plx:callStatic name="PreserveChanges" type="field" dataTypeName="LoadPolicy"/>
																</plx:right>
															</plx:binaryOperator>
														</plx:left>
														<plx:right>
															<plx:binaryOperator type="equality">
																<plx:left>
																	<plx:callInstance name="EntityState" type="property">
																		<plx:callObject>
																			<plx:nameRef name="c"/>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:left>
																<plx:right>
																	<plx:callStatic name="Unchanged" type="field" dataTypeName="EntityState" />
																</plx:right>
															</plx:binaryOperator>
														</plx:right>
													</plx:binaryOperator>
												</plx:left>
												<plx:right>
													<plx:binaryOperator type="booleanAnd">
														<plx:left>
															<plx:binaryOperator type="equality">
																<plx:left>
																	<plx:callInstance name="CurrentLoadPolicy" type="property">
																		<plx:callObject>
																			<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" />
																		</plx:callObject>
																	</plx:callInstance>
																</plx:left>
																<plx:right>
																	<plx:callStatic name="DiscardChanges" type="field" dataTypeName="LoadPolicy"/>
																</plx:right>
															</plx:binaryOperator>
														</plx:left>
														<plx:right>
															<plx:binaryOperator type="booleanOr">
																<plx:left>
																	<plx:binaryOperator type="equality">
																		<plx:left>
																			<plx:callInstance name="EntityState" type="property">
																				<plx:callObject>
																					<plx:nameRef name="c"/>
																				</plx:callObject>
																			</plx:callInstance>
																		</plx:left>
																		<plx:right>
																			<plx:callStatic name="Unchanged" type="field" dataTypeName="EntityState" />
																		</plx:right>
																	</plx:binaryOperator>
																</plx:left>
																<plx:right>
																	<plx:binaryOperator type="equality">
																		<plx:left>
																			<plx:callInstance name="EntityState" type="property">
																				<plx:callObject>
																					<plx:nameRef name="c"/>
																				</plx:callObject>
																			</plx:callInstance>
																		</plx:left>
																		<plx:right>
																			<plx:callStatic name="Changed" type="field" dataTypeName="EntityState" />
																		</plx:right>
																	</plx:binaryOperator>
																</plx:right>
															</plx:binaryOperator>
														</plx:right>
													</plx:binaryOperator>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<!-- c.SuppressEntityEvents = true; -->
						<plx:assign>
							<plx:left>
								<plx:callInstance name="SuppressEntityEvents" type="property">
									<plx:callObject>
										<plx:nameRef name="c"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:trueKeyword/>
							</plx:right>
						</plx:assign>
						<!-- c.EmployeeId = (int) reader["EmployeeID"]; -->
						<xsl:for-each select="$fullColumnParams">
							<plx:assign>
								<plx:left>
									<plx:callInstance name="{@tmp:propertyName}" type="property">
										<plx:callObject>
											<plx:nameRef name="c"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:cast dataTypeName="{@dataTypeName}">
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:nameRef name="reader" type="parameter"/>
											</plx:callObject>
											<plx:passParam>
												<plx:string>
													<xsl:value-of select="@tmp:propertyName" />
												</plx:string>
											</plx:passParam>
										</plx:callInstance>
									</plx:cast>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:callInstance name="Original{@tmp:propertyName}" type="property">
										<plx:callObject>
											<plx:nameRef name="c"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callInstance name="{@tmp:propertyName}" type="property">
										<plx:callObject>
											<plx:nameRef name="c"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
						</xsl:for-each>
						<!-- c.EntityTrackingKey = key; -->
						<plx:assign>
							<plx:left>
								<plx:callInstance name="EntityTrackingKey" type="property">
									<plx:callObject>
										<plx:nameRef name="c"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nameRef name="key"/>
							</plx:right>
						</plx:assign>
						<!-- c.AcceptChanges(); -->
						<plx:callInstance name="AcceptChanges">
							<plx:callObject>
								<plx:nameRef name="c"/>
							</plx:callObject>
						</plx:callInstance>
						<!-- c.SuppressEntityEvents = false; -->
						<plx:assign>
							<plx:left>
								<plx:callInstance name="SuppressEntityEvents" type="property">
									<plx:callObject>
										<plx:nameRef name="c"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:falseKeyword/>
							</plx:right>
						</plx:assign>
					</plx:branch>
					<!-- rows.Add(c); -->
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:nameRef name="rows" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="c"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:loop>
				<!-- return rows; -->
				<plx:return>
					<plx:nameRef name="rows" type="parameter"/>
				</plx:return>
			</plx:function>

			<!-- RefreshEntity methods-->
			<plx:function name="RefreshEntity" visibility="public" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							Refreshes the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; object from the &lt;see cref="IDataReader"/&gt;.
						</summary>
						<param name="reader">The &lt;see cref="IDataReader"/&gt; to read from.</param>
						<param name="entity">
							The &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; object to refresh.
						</param>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="reader" dataTypeName="IDataReader" />
				<plx:param name="entity" dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
				<plx:branch>
					<plx:condition>
						<plx:unaryOperator type="booleanNot">
							<plx:callInstance name="Read">
								<plx:callObject>
									<plx:nameRef name="reader"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:unaryOperator>
					</plx:condition>
					<plx:return />
				</plx:branch>
				<xsl:for-each select="$fullColumnParams">
					<plx:assign>
						<plx:left>
							<plx:callInstance name="{@tmp:propertyName}" type="property">
								<plx:callObject>
									<plx:nameRef name="entity"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:cast dataTypeName="{@dataTypeName}">
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="reader" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:string>
											<xsl:value-of select="@tmp:propertyName" />
										</plx:string>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:callInstance name="Original{@tmp:propertyName}" type="property">
								<plx:callObject>
									<plx:nameRef name="entity"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:cast dataTypeName="{@dataTypeName}">
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="reader" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:string>
											<xsl:value-of select="@tmp:propertyName" />
										</plx:string>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
				<!-- entity.AcceptChanges()-->
				<plx:callInstance name="AcceptChanges">
					<plx:callObject>
						<plx:nameRef name="entity"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:function>
			<plx:function name="RefreshEntity" visibility="public" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							Refreshes the &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; object from the &lt;see cref="IDataReader"/&gt;.
						</summary>
						<param name="dataSet">The &lt;see cref="DataSet"/&gt; to read from.</param>
						<param name="entity">
							The &lt;see cref="<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>"/&gt; object to refresh.
						</param>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="dataSet" dataTypeName="DataSet" dataTypeQualifier="System.Data" />
				<plx:param name="entity" dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}" />
				<plx:local name="dataRow" dataTypeName="DataRow" dataTypeQualifier="System.Data">
					<plx:initialize>
						<plx:callInstance name=".implied" type="indexerCall">
							<plx:callObject>
								<plx:callInstance name="Rows" type="property">
									<plx:callObject>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:callInstance name="Tables" type="property">
													<plx:callObject>
														<plx:nameRef name="dataSet" type="parameter"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:callObject>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:value data="0" type="i4"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>

				<xsl:for-each select="$fullColumnParams">
					<plx:assign>
						<plx:left>
							<plx:callInstance name="{@tmp:propertyName}" type="property">
								<plx:callObject>
									<plx:nameRef name="entity"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:cast dataTypeName="{@dataTypeName}">
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="dataRow" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:string>
											<xsl:value-of select="@tmp:propertyName" />
										</plx:string>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:callInstance name="Original{@tmp:propertyName}" type="property">
								<plx:callObject>
									<plx:nameRef name="entity"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:cast dataTypeName="{@dataTypeName}">
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="dataRow" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:string>
											<xsl:value-of select="@tmp:propertyName" />
										</plx:string>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
				<!-- entity.AcceptChanges()-->
				<plx:callInstance name="AcceptChanges">
					<plx:callObject>
						<plx:nameRef name="entity"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:function>
			<plx:pragma type="closeRegion" data="Helper Functions"/>

			<xsl:if test="$settings[@name='IncludeRelations'] and $settings[@name='IncludeGetListByFK']">
				<plx:pragma type="region" data="{$settings[@name='MethodNames']/methodName[@name='DeepLoad']} Methods"/>
				<plx:function name="{$settings[@name='MethodNames']/methodName[@name='DeepLoad']}" visibility="internal" modifier="override">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>
							Deep Loads the &lt;see cref="IEntity" /&gt; object with criteria based of the child property collections only N Levels Deep based on the &lt;see cref="DeepLoadType" /&gt;.
						</summary>
							<remarks>Use this method with caution as it is possible to DeepLoad with Recursion and traverse an entire object graph.</remarks>
							<param name="transactionManager">&lt;see cref="TransactionManager" /&gt; object</param>
							<param name="entity">
							The &lt;see cref="<xsl:copy-of select="$BLLNamespace"/>.<xsl:copy-of select="$entityClassName"/>" /&gt; object to load.
						</param>
							<param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
							<param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
							<param name="childTypes">
							<xsl:copy-of select="$BLLNamespace"/>.<xsl:copy-of select="$entityClassName"/> Property Collection Type Array To Include or Exclude from Load
						</param>
							<param name="innerList">A collection of child types for easy access.</param>
							<exception cref="ArgumentNullException">entity or childTypes is null.</exception>
							<exception cref="ArgumentException">deepLoadType has invalid value.</exception>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param name="transactionManager" dataTypeName="TransactionManager" />
					<plx:param name="entity" dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}"/>
					<plx:param name="deep" dataTypeName=".boolean"/>
					<plx:param name="deepLoadType" dataTypeName="DeepLoadType"/>
					<plx:param name="childTypes" dataTypeIsSimpleArray="true" dataTypeName="Type" dataTypeQualifier="System"/>
					<plx:param name="innerList" dataTypeName="DeepSession"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef name="entity" type="parameter"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return/>
					</plx:branch>

					<plx:local name="pkItems" dataTypeIsSimpleArray="true" dataTypeName=".object"/>

					<xsl:for-each select="$currentTableForeignKeys">
						<xsl:variable name="targetTable" select="@targetTable"/>
						<xsl:variable name="targetColumn" select="se:columnReference/@targetColumn" />
						<plx:pragma type="region" data="{$targetColumn}Source"/>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:callThis name="CanDeepLoad">
											<plx:passParam>
												<plx:nameRef name="entity" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string><xsl:value-of select="$targetTable"/>|<xsl:value-of select="$targetColumn"/>Source</plx:string>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="deepLoadType" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="innerList" type="parameter"/>
											</plx:passParam>
										</plx:callThis>
									</plx:left>
									<plx:right>
										<plx:binaryOperator type="identityEquality">
											<plx:left>
												<plx:callInstance name="{$targetColumn}Source" type="property">
													<plx:callObject>
														<plx:nameRef name="entity" type="parameter"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="pkItems"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeIsSimpleArray="true" dataTypeName=".object">
										<plx:arrayInitializer>
											<plx:callInstance name="{$targetColumn}" type="property">
												<plx:callObject>
													<plx:nameRef name="entity" type="parameter"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:arrayInitializer>
									</plx:callNew>
								</plx:right>
							</plx:assign>
							<plx:local name="tmpEntity" dataTypeName="{$targetTable}" dataTypeQualifier="{$BLLNamespace}">
								<plx:initialize>
									<plx:callStatic name="LocateEntity" dataTypeName="EntityManager" dataTypeQualifier="{$BLLNamespace}">
										<plx:passMemberTypeParam dataTypeName="{$targetTable}" dataTypeQualifier="{$BLLNamespace}"/>
										<plx:passParam>
											<plx:callStatic name="ConstructKeyFromPkItems" dataTypeName="EntityLocator" dataTypeQualifier="{$BLLNamespace}">
												<plx:passParam>
													<plx:typeOf dataTypeName="{$targetTable}" dataTypeQualifier="{$BLLNamespace}"/>
												</plx:passParam>
												<plx:passParam>
													<plx:nameRef name="pkItems"/>
												</plx:passParam>
											</plx:callStatic>
										</plx:passParam>
										<plx:passParam>
											<plx:callInstance name="EnableEntityTracking" type="property">
												<plx:callObject>
													<plx:callStatic name="Provider" type="property" dataTypeName="DataRepository" dataTypeQualifier="{$DALNamespace}"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callStatic>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:nameRef name="tmpEntity"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:callInstance name="{$targetColumn}Source" type="property">
											<plx:callObject>
												<plx:nameRef name="entity" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:nameRef name="tmpEntity"/>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:fallbackBranch>
								<plx:assign>
									<plx:left>
										<plx:callInstance name="{$targetColumn}Source" type="property">
											<plx:callObject>
												<plx:nameRef name="entity" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callInstance name="GetBy{$targetColumn}">
											<plx:callObject>
												<plx:callStatic name="{$targetTable}Provider" type="property" dataTypeName="DataRepository" dataTypeQualifier="{$DALNamespace}"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="transactionManager" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:callInstance name="{$targetColumn}" type="property">
													<plx:callObject>
														<plx:nameRef name="entity" type="parameter"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
										</plx:callInstance>
									</plx:right>
								</plx:assign>
							</plx:fallbackBranch>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="booleanAnd">
										<plx:left>
											<plx:nameRef name="deep" type="parameter"/>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:callInstance name="{$targetColumn}Source" type="property">
														<plx:callObject>
															<plx:nameRef name="entity" type="parameter"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:callInstance name="SkipChildren" type="property">
											<plx:callObject>
												<plx:nameRef name="innerList" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:assign>
								<plx:callInstance name="DeepLoad">
									<plx:callObject>
										<plx:callStatic name="{$targetTable}Provider" type="property" dataTypeName="DataRepository" dataTypeQualifier="{$DALNamespace}"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="transactionManager" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callInstance name="{$targetColumn}Source" type="property">
											<plx:callObject>
												<plx:nameRef name="entity" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="deep" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="deepLoadType" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="childTypes" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="innerList" type="parameter"/>
									</plx:passParam>
								</plx:callInstance>
								<!-- innerList.SkipChildren = false; -->
								<plx:assign>
									<plx:left>
										<plx:callInstance name="SkipChildren" type="property">
											<plx:callObject>
												<plx:nameRef name="innerList" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:falseKeyword/>
									</plx:right>
								</plx:assign>
							</plx:branch>
						</plx:branch>
						<plx:pragma type="closeRegion" data="{$targetColumn}Source"/>
					</xsl:for-each>
					<plx:local name="deepHandles" dataTypeName="Dictionary" dataTypeQualifier="System.Collections.Generic">
						<plx:passTypeParam dataTypeName=".string"/>
						<plx:passTypeParam dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
							<plx:passTypeParam dataTypeName="Delegate" dataTypeQualifier="System"/>
							<plx:passTypeParam dataTypeName=".object"/>
						</plx:passTypeParam>
						<plx:initialize>
							<plx:callNew dataTypeName="Dictionary" dataTypeQualifier="System.Collections.Generic">
								<plx:passTypeParam dataTypeName=".string"/>
								<plx:passTypeParam dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
									<plx:passTypeParam dataTypeName="Delegate" dataTypeQualifier="System"/>
									<plx:passTypeParam dataTypeName=".object"/>
								</plx:passTypeParam>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
					<plx:iterator localName="pair" dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
						<plx:passTypeParam dataTypeName="Delegate" dataTypeQualifier="System"/>
						<plx:passTypeParam dataTypeName=".object"/>
						<plx:initialize>
							<plx:callInstance name="Values" type="property">
								<plx:callObject>
									<plx:nameRef name="deepHandles"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
						<plx:callInstance name="DynamicInvoke">
							<plx:callObject>
								<plx:callInstance name="Key" type="property">
									<plx:callObject>
										<plx:nameRef name="pair"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:cast dataTypeIsSimpleArray="true" dataTypeName=".object">
									<plx:callInstance name="Value" type="property">
										<plx:callObject>
											<plx:nameRef name="pair"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:cast>
							</plx:passParam>
						</plx:callInstance>
					</plx:iterator>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="deepHandles"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:assign>

				</plx:function>
				<plx:pragma type="closeRegion" data="{$settings[@name='MethodNames']/methodName[@name='DeepLoad']} Methods"/>

				<xsl:if test="$settings[@name='IncludeSave']">
					<plx:pragma type="region" data="{$settings[@name='MethodNames']/methodName[@name='DeepSave']} Methods"/>
					<plx:function name="{$settings[@name='MethodNames']/methodName[@name='DeepSave']}" visibility="internal" modifier="override">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
							Deep Save the entire object graph of the [CLASS NAME] object with criteria based off the child type property array and DeepSaveType.
						</summary>
								<param name="transactionManager">&lt;see cref="TransactionManager" /&gt; object</param>
								<param name="entity">
							The &lt;see cref="<xsl:copy-of select="$BLLNamespace"/>.<xsl:copy-of select="$entityClassName"/>" /&gt; instance.
						</param>
								<param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
								<param name="childTypes">
							<xsl:copy-of select="$BLLNamespace"/>.<xsl:copy-of select="$entityClassName"/> Property Collection Type Array To Include or Exclude from Save
						</param>
								<param name="innerList">A collection of child types for easy access.</param>
							</plx:docComment>
						</plx:leadingInfo>

						<plx:param name="transactionManager" dataTypeName="TransactionManager" dataTypeQualifier="{$DALNamespace}"/>
						<plx:param name="entity" dataTypeName="{$entityClassName}" dataTypeQualifier="{$BLLNamespace}"/>
						<plx:param name="deepSaveType" dataTypeName="DeepSaveType" dataTypeQualifier="{$DALNamespace}"/>
						<plx:param name="childTypes" dataTypeIsSimpleArray="true" dataTypeName="Type" dataTypeQualifier="System"/>
						<plx:param name="innerList" dataTypeName="DeepSession"/>
						<plx:returns dataTypeName=".boolean"/>

						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:nameRef name="entity" type="parameter"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:return>
								<plx:falseKeyword/>
							</plx:return>
						</plx:branch>

						<plx:pragma type="region" data="Composite Parent Properties"/>
						<plx:comment>Save Source Composite Properties, however, don't call deep save on them.</plx:comment>
						<plx:comment>So they only get saved a single level deep.</plx:comment>
						<xsl:for-each select="$currentTableForeignKeys">
							<xsl:variable name="targetTable" select="@targetTable"/>
							<xsl:variable name="targetColumn" select="se:columnReference/@targetColumn" />
							<plx:pragma type="region" data="{$targetColumn}Source"/>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="booleanAnd">
										<plx:left>
											<plx:callThis name="CanDeepSave" accessor="base">
												<plx:passParam>
													<plx:nameRef name="entity" type="parameter"/>
												</plx:passParam>
												<plx:passParam>
													<plx:string><xsl:value-of select="$targetTable"/>|<xsl:value-of select="$targetColumn"/>Source</plx:string>
												</plx:passParam>
												<plx:passParam>
													<plx:nameRef name="deepSaveType" type="parameter"/>
												</plx:passParam>
												<plx:passParam>
													<plx:nameRef name="innerList" type="parameter"/>
												</plx:passParam>
											</plx:callThis>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:callInstance name="{$targetColumn}Source" type="property">
														<plx:callObject>
															<plx:nameRef name="entity" type="parameter"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:callInstance name="Save">
									<plx:callObject>
										<plx:callStatic name="{$targetTable}Provider" type="property" dataTypeName="DataRepository" dataTypeQualifier="{$DALNamespace}"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="transactionManager" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callInstance name="{$targetColumn}Source" type="property">
											<plx:callObject>
												<plx:nameRef name="entity" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
								<plx:assign>
									<plx:left>
										<plx:callInstance name="{$targetColumn}" type="property">
											<plx:callObject>
												<plx:nameRef name="entity" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callInstance name="{$targetColumn}" type="property">
											<plx:callObject>
												<plx:callInstance name="{$targetColumn}Source" type="property">
													<plx:callObject>
														<plx:nameRef name="entity" type="parameter"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:pragma type="closeRegion" data="{$targetColumn}Source"/>
						</xsl:for-each>

						<plx:local name="deepHandles" dataTypeName="Dictionary" dataTypeQualifier="System.Collections.Generic">
							<plx:passTypeParam dataTypeName="Delegate" dataTypeQualifier="System"/>
							<plx:passTypeParam dataTypeName=".object"/>
							<plx:initialize>
								<plx:callNew dataTypeName="Dictionary" dataTypeQualifier="System.Collections.Generic">
									<plx:passTypeParam dataTypeName="Delegate" dataTypeQualifier="System"/>
									<plx:passTypeParam dataTypeName=".object"/>
								</plx:callNew>
							</plx:initialize>
						</plx:local>
						<plx:iterator localName="pair" dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
							<plx:passTypeParam dataTypeName="Delegate" dataTypeQualifier="System"/>
							<plx:passTypeParam dataTypeName=".object"/>
							<plx:initialize>
								<plx:nameRef name="deepHandles"/>
							</plx:initialize>
							<plx:callInstance name="DynamicInvoke">
								<plx:callObject>
									<plx:callInstance name="Key" type="property">
										<plx:callObject>
											<plx:nameRef name="pair"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:cast dataTypeIsSimpleArray="true" dataTypeName=".object">
										<plx:callInstance name="Value" type="property">
											<plx:callObject>
												<plx:nameRef name="pair"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:cast>
								</plx:passParam>
							</plx:callInstance>
						</plx:iterator>
						<plx:branch>
							<plx:condition>
								<plx:callInstance name="IsDeleted" type="property">
									<plx:callObject>
										<plx:nameRef name="entity" type="parameter"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:condition>
							<plx:callThis name="Save">
								<plx:passParam>
									<plx:nameRef name="transactionManager" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="entity" type="parameter"/>
								</plx:passParam>
							</plx:callThis>
						</plx:branch>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="deepHandles"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:assign>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>

						<plx:pragma type="closeRegion" data="Composite Parent Properties"/>

					</plx:function>
					<plx:pragma type="closeRegion" data="{$settings[@name='MethodNames']/methodName[@name='DeepSave']} Methods"/>
				</xsl:if>
				<!-- end of IncludeSave -->
			</xsl:if>
			<!-- end of IncludeRelations -->
		</plx:class>
		<plx:pragma type="closeRegion" data="Classes for {@name}"/>

		<plx:pragma type="region" data="{$entityClassName}ChildEntityTypes"/>
		<plx:enum name="{$entityClassName}ChildEntityTypes" visibility="public">
			<plx:leadingInfo>
				<plx:docComment>
					<summary>
             Enumeration used to expose the different child entity types 
             for child properties in &lt;c&gt;<xsl:value-of select="$BLLNamespace"/>.<xsl:value-of select="$entityClassName"/>&lt;/c&gt;
            </summary>
				</plx:docComment>
			</plx:leadingInfo>
			<xsl:if test="$settings[@name='IncludeRelations'] and $settings[@name='IncludeGetListByFK']">
				<xsl:for-each select="$currentTableForeignKeys">
					<xsl:variable name="targetTable" select="@targetTable"/>
					<xsl:variable name="targetColumn" select="se:columnReference/@targetColumn" />
					<plx:enumItem name="{$targetTable}">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
             Composite Property for &lt;c&gt;<xsl:value-of select="$targetTable"/>&lt;/c&gt; at <xsl:value-of select="$targetColumn"/>Source
            </summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:attribute dataTypeName="ChildEntityTypeAttribute">
							<plx:passParam>
								<plx:typeOf dataTypeName="{$targetTable}" dataTypeQualifier="{$BLLNamespace}"/>
							</plx:passParam>
						</plx:attribute>
						<!--<plx:initialize>
						<plx:value data="1" type="i4"/>
					</plx:initialize>-->
					</plx:enumItem>
				</xsl:for-each>
			</xsl:if>
		</plx:enum>
		<plx:pragma type="closeRegion" data="{$entityClassName}ChildEntityTypes"/>









		<!-- End Mike/Tommy code -->
	</xsl:template>

	<xsl:template name="KeyStringBuilder">
		<xsl:param name="Columns"/>
		<xsl:param name="CurrentPosition" select="1"/>
		<xsl:param name="ItemCount" select="count($Columns)"/>
		<xsl:param name="EntityClassName"/>
		<xsl:variable name="currentExpression">
			<plx:callNew dataTypeName="StringBuilder" dataTypeQualifier="System.Text">
				<plx:passParam>
					<plx:string>
						<xsl:copy-of select="$EntityClassName"/>
					</plx:string>
				</plx:passParam>
			</plx:callNew>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$CurrentPosition=$ItemCount + 1">
				<xsl:copy-of select="$currentExpression"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:callInstance name="Append">
					<plx:callObject>
						<xsl:call-template name="KeyStringBuilder">
							<xsl:with-param name="Columns" select="$Columns"/>
							<xsl:with-param name="CurrentPosition" select="$CurrentPosition + 1"/>
							<xsl:with-param name="ItemCount" select="$ItemCount"/>
							<xsl:with-param name="EntityClassName" select="$EntityClassName"/>
							<xsl:with-param name="BLLNamespace" select="$BLLNamespace"/>
						</xsl:call-template>
					</plx:callObject>
					<plx:passParam>
						<plx:inlineStatement dataTypeName=".i4">
							<plx:conditionalOperator>
								<plx:condition>
									<plx:unaryOperator type="booleanNot">
										<plx:callInstance name="IsDBNull">
											<plx:callObject>
												<plx:nameRef name="reader" type="parameter"/>
											</plx:callObject>
											<plx:passParam>
												<plx:binaryOperator type="subtract">
													<plx:left>
														<plx:cast dataTypeName=".i4">
															<plx:callStatic name="{$Columns[position()=$CurrentPosition]/@tmp:propertyName}" dataTypeName="{$EntityClassName}Column" dataTypeQualifier="{$BLLNamespace}"  type="field"/>
														</plx:cast>
													</plx:left>
													<plx:right>
														<plx:value data="1" type="i4"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:passParam>
										</plx:callInstance>
									</plx:unaryOperator>
								</plx:condition>
								<plx:left>
									<plx:value data="0" type="i4"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name=".implied" type="indexerCall">
										<plx:callObject>
											<plx:nameRef name="reader" type="parameter"/>
										</plx:callObject>
										<plx:passParam>
											<plx:binaryOperator type="subtract">
												<plx:left>
													<plx:cast dataTypeName=".i4">
														<plx:callStatic name="{$Columns[position()=$CurrentPosition]/@tmp:propertyName}" dataTypeName="{$EntityClassName}Column" dataTypeQualifier="{$BLLNamespace}"  type="field"/>
													</plx:cast>
												</plx:left>
												<plx:right>
													<plx:value data="1" type="i4"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:passParam>
									</plx:callInstance>
								</plx:right>
							</plx:conditionalOperator>
						</plx:inlineStatement>
					</plx:passParam>
				</plx:callInstance>
			</xsl:otherwise>
		</xsl:choose>
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
</xsl:stylesheet>
