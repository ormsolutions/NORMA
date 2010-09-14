<?xml version="1.0" encoding="utf-8"?>
<!--
	Natural Object-Role Modeling Architect for Visual Studio

	Copyright © ORM Solutions, LLC. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<xsl:stylesheet
	version="1.0"
	xmlns:cmc="http://schemas.ormsolutions.com/ORM/SDK/CopyMergeClosureGenerator"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:dsl="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:template match="cmc:CopyMergeClosure">
		<xsl:variable name="closureModel" select="cmc:Model"/>
		<xsl:variable name="primaryDslModel" select="document($closureModel/@modelFile)/dsl:Dsl"/>
		<xsl:variable name="referencedDslModelsFragment">
			<xsl:for-each select="$closureModel/cmc:ModelReferences/cmc:ModelReference">
				<xsl:copy-of select="document(@modelFile)/dsl:Dsl"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="referencedDslModels" select="exsl:node-set($referencedDslModelsFragment)/child::*"/>
		<plx:root xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="Microsoft.VisualStudio.Modeling"/>
			<plx:namespaceImport name="ORMSolutions.ORMArchitect.Framework"/>
			<plx:namespace name="{$primaryDslModel/@Namespace}">
				<xsl:variable name="copyright" select="cmc:Copyright"/>
				<xsl:if test="$copyright">
					<plx:leadingInfo>
						<plx:comment blankLine="true"/>
						<plx:comment>
							<xsl:value-of select="$copyright/@name"/>
						</plx:comment>
						<xsl:for-each select="$copyright/cmc:CopyrightLine">
							<plx:comment>
								<xsl:value-of select="."/>
							</plx:comment>
						</xsl:for-each>
						<plx:comment blankLine="true"/>
					</plx:leadingInfo>
				</xsl:if>
				<xsl:variable name="rootElementNames" select="$closureModel/cmc:RootElements/cmc:RootElement/@class"/>
				<xsl:variable name="domainClasses" select="$primaryDslModel/dsl:Classes/dsl:DomainClass"/>
				<xsl:variable name="domainRelationships" select="$primaryDslModel/dsl:Relationships/dsl:DomainRelationship"/>
				<xsl:variable name="rootDomainClassNames" select="$domainClasses/@Name[.=$rootElementNames]"/>
				<xsl:variable name="closureRoles" select="$closureModel/cmc:ClosureRoles/cmc:ClosureRole"/>
				<xsl:variable name="allEmbeddingRelationships" select="$domainRelationships[@IsEmbedding='true' or @IsEmbedding='1']"/>
				<xsl:variable name="rootEmbeddings" select="$domainRelationships[dsl:Source/dsl:DomainRole/dsl:RolePlayer/dsl:DomainClassMoniker/@Name=$rootElementNames]"/>
				<xsl:variable name="embeddingDirectives" select="$closureModel/cmc:EmbeddingDirectives/cmc:EmbeddingDirective"/>
				<!-- Verify referenced classes exist in model file. -->
				<xsl:if test="count($rootElementNames)!=count($rootDomainClassNames)">
					<xsl:message terminate="yes">
						<xsl:text>RootElements not found in domain model: </xsl:text>
						<xsl:for-each select="$rootElementNames[not(.=$rootDomainClassNames)]">
							<xsl:if test="position()!=1">
								<xsl:text>, </xsl:text>
							</xsl:if>
							<xsl:value-of select="."/>
						</xsl:for-each>
					</xsl:message>
				</xsl:if>
				<xsl:variable name="unboundEmbeddingDirectives" select="$embeddingDirectives[not(@relationship=$allEmbeddingRelationships/@Name)]"/>
				<xsl:if test="$unboundEmbeddingDirectives">
					<xsl:message terminate="yes">
						<xsl:text>EmbeddingDirectives not found in domain model: </xsl:text>
						<xsl:for-each select="$unboundEmbeddingDirectives">
							<xsl:if test="position()!=1">
								<xsl:text>, </xsl:text>
							</xsl:if>
							<xsl:value-of select="@relationship"/>
						</xsl:for-each>
					</xsl:message>
				</xsl:if>
				<xsl:variable name="embeddingRelationships" select="$allEmbeddingRelationships[not(@Name=$embeddingDirectives[@ignore[.='true' or .='1']]/@relationship)]"/>
				<plx:class name="{$primaryDslModel/@Name}DomainModel" visibility="deferToPartial" partial="true">
					<plx:leadingInfo>
						<plx:pragma type="region" data="Customize Copy Closure for {$primaryDslModel/@Name}DomainModel"/>
					</plx:leadingInfo>
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="Customize Copy Closure for {$primaryDslModel/@Name}DomainModel"/>
					</plx:trailingInfo>
					<plx:implementsInterface dataTypeName="ICopyClosureProvider"/>
					<plx:function visibility="private" name="AddCopyClosureDirectives">
						<plx:interfaceMember dataTypeName="ICopyClosureProvider" memberName="AddCopyClosureDirectives"/>
						<plx:param name="closureManager" dataTypeName="ICopyClosureManager"/>

						<!-- Automatically add embedding information for root elements -->
						<xsl:if test="$rootEmbeddings">
							<plx:pragma type="region" data="Automatic top-level embedding relationships"/>
							<xsl:for-each select="$rootEmbeddings">
								<plx:callInstance name="AddRootEmbeddingRelationship">
									<plx:callObject>
										<plx:nameRef name="closureManager" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic dataTypeName="{@Name}" name="DomainClassId" type="field"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
							<plx:pragma type="closeRegion" data="Automatic top-level embedding relationships"/>
						</xsl:if>
						<!-- Automatically add closure information for embedding relationships that
						do not come from root elements. -->
						<xsl:variable name="standardClosureEmbeddings" select="$embeddingRelationships[not(@Name=$closureRoles/@relationship)][not(dsl:Source/dsl:DomainRole/dsl:RolePlayer/dsl:DomainClassMoniker/@Name=$rootElementNames)]"/>
						<xsl:if test="$standardClosureEmbeddings">
							<plx:pragma type="region" data="Closures for standard embedding relationships"/>
							<xsl:for-each select="$standardClosureEmbeddings">
								<xsl:sort select="@Name"/>
								<xsl:variable name="relationshipName" select="string(@Name)"/>
								<xsl:variable name="fromRole">
									<plx:callNew dataTypeName="DomainRoleClosureRestriction">
										<plx:passParam>
											<plx:callStatic name="{dsl:Source/dsl:DomainRole/@Name}DomainRoleId" dataTypeName="{$relationshipName}" type="field"/>
										</plx:passParam>
									</plx:callNew>
								</xsl:variable>
								<xsl:variable name="toRole">
									<plx:callNew dataTypeName="DomainRoleClosureRestriction">
										<plx:passParam>
											<plx:callStatic name="{dsl:Target/dsl:DomainRole/@Name}DomainRoleId" dataTypeName="{$relationshipName}" type="field"/>
										</plx:passParam>
									</plx:callNew>
								</xsl:variable>
								<plx:callInstance name="AddCopyClosureDirective">
									<plx:callObject>
										<plx:nameRef name="closureManager" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<xsl:copy-of select="$toRole"/>
									</plx:passParam>
									<plx:passParam>
										<xsl:copy-of select="$fromRole"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callStatic name="None" dataTypeName="CopyClosureDirectiveOptions" type="field"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callStatic name="Container" dataTypeName="CopyClosureBehavior" type="field"/>
									</plx:passParam>
								</plx:callInstance>
								<plx:callInstance name="AddCopyClosureDirective">
									<plx:callObject>
										<plx:nameRef name="closureManager" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<xsl:copy-of select="$fromRole"/>
									</plx:passParam>
									<plx:passParam>
										<xsl:copy-of select="$toRole"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callStatic name="None" dataTypeName="CopyClosureDirectiveOptions" type="field"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callStatic name="ContainedPart" dataTypeName="CopyClosureBehavior" type="field"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
							<plx:pragma type="closeRegion" data="Closures for standard embedding relationships"/>
						</xsl:if>
						
						<!-- Orderings for embedded relationships -->
						<xsl:variable name="orderedEmbeddings" select="$embeddingRelationships[dsl:Source[dsl:DomainRole/@Multiplicity[contains(.,'Many')]]][not(@Name=$embeddingDirectives[@order='Unordered']/@relationship)]"/>
						<xsl:if test="$orderedEmbeddings">
							<plx:pragma type="region" data="Embedded relationship ordering"/>
							<xsl:for-each select="$orderedEmbeddings">
								<xsl:sort select="@Name"/>
								<plx:callInstance name="AddOrderedRole">
									<plx:callObject>
										<plx:nameRef name="closureManager" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic dataTypeName="{@Name}" name="{dsl:Source/dsl:DomainRole/@Name}DomainRoleId" type="field"/>
									</plx:passParam>
									<plx:passParam>
										<plx:callStatic dataTypeName="MergeIntegrationOrder" name="AfterLeading" type="field">
											<xsl:variable name="directiveOrder" select="string($embeddingDirectives[@relationship=current()/@Name]/@order)"/>
											<xsl:if test="$directiveOrder">
												<xsl:attribute name="name">
													<xsl:value-of select="$directiveOrder"/>
												</xsl:attribute>
											</xsl:if>
										</plx:callStatic>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
							<plx:pragma type="closeRegion" data="Embedded relationship ordering"/>
						</xsl:if>
						<xsl:if test="$closureRoles">
							<plx:pragma type="region" data="Closures for explicit relationships"/>
							<xsl:variable name="sortedClosureRolesFragment">
								<xsl:for-each select="$closureRoles">
									<xsl:sort select="@relationshipNamespace"/>
									<xsl:sort select="@relationship"/>
									<xsl:sort select="@role"/>
									<xsl:copy-of select="."/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="sortedClosureRoles" select="exsl:node-set($sortedClosureRolesFragment)/child::*"/>
							<xsl:for-each select="$sortedClosureRoles">
								<xsl:variable name="behavior" select="string(@closureBehavior)"/>
								<xsl:variable name="relationship" select="string(@relationship)"/>
								<xsl:variable name="relationshipNamespace" select="string(@relationshipNamespace)"/>
								<xsl:variable name="role" select="string(@role)"/>
								<xsl:if test="$behavior!='Ignored' or preceding-sibling::cmc:*[@relationship=$relationship and @role=$role and string(@relationshipNamespace)=$relationshipNamespace] or following-sibling::cmc:*[@relationship=$relationship and @role=$role and string(@relationshipNamespace)=$relationshipNamespace]">
									<xsl:variable name="dslRelationshipFragment">
										<xsl:choose>
											<xsl:when test="$relationshipNamespace">
												<xsl:copy-of select="$referencedDslModels[@Namespace=$relationshipNamespace]/dsl:Relationships/dsl:DomainRelationship[@Name=$relationship]"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:copy-of select="$domainRelationships[@Name=$relationship]"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:variable name="dslRelationship" select="exsl:node-set($dslRelationshipFragment)/child::*"/>
									<xsl:if test="not($dslRelationship)">
										<xsl:message terminate="yes">
											<xsl:text>The relationship '</xsl:text>
											<xsl:value-of select="$relationship"/>
											<xsl:text>' does not exist in the domain model.</xsl:text>
										</xsl:message>
									</xsl:if>
									<xsl:variable name="dslToRole" select="$dslRelationship/dsl:*/dsl:DomainRole[@Name=$role]"/>
									<xsl:if test="not($dslToRole)">
										<xsl:message terminate="yes">
											<xsl:text>The role '</xsl:text>
											<xsl:value-of select="$role"/>
											<xsl:text>' does not exist in relationship</xsl:text>
											<xsl:value-of select="$relationship"/>
											<xsl:text>.</xsl:text>
										</xsl:message>
									</xsl:if>
									<xsl:variable name="dslFromRole" select="$dslRelationship/dsl:*/dsl:DomainRole[not(@Name=$role)]"/>
									<xsl:call-template name="AddComments"/>
									<plx:callInstance name="AddCopyClosureDirective">
										<plx:callObject>
											<plx:nameRef name="closureManager" type="parameter"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callNew dataTypeName="DomainRoleClosureRestriction">
												<plx:passParam>
													<xsl:for-each select="$dslFromRole">
														<plx:callStatic name="{@Name}DomainRoleId" dataTypeName="{$relationship}" type="field">
															<xsl:if test="$relationshipNamespace">
																<xsl:attribute name="dataTypeQualifier">
																	<xsl:value-of select="$relationshipNamespace"/>
																</xsl:attribute>
															</xsl:if>
														</plx:callStatic>
													</xsl:for-each>
												</plx:passParam>
												<xsl:variable name="restrictedRolePlayer" select="string(@explicitFromRoleClass)"/>
												<xsl:variable name="restrictedRolePlayerNamespace" select="string(@explicitFromRoleNamespace)"/>
												<xsl:if test="$restrictedRolePlayer">
													<xsl:choose>
														<xsl:when test="$restrictedRolePlayerNamespace">
															<xsl:if test="not($referencedDslModels[@Namespace=$restrictedRolePlayerNamespace][dsl:Classes/dsl:DomainClass[@Name=$restrictedRolePlayer] or dsl:Relationships/dsl:DomainRelationship[@Name=$restrictedRolePlayer]])">
																<xsl:message terminate="yes">
																	<xsl:text>'</xsl:text>
																	<xsl:value-of select="$restrictedRolePlayer"/>
																	<xsl:text>' must be a DomainClass or a DomainRelationship in the model file with namespace</xsl:text>
																	<xsl:value-of select="$restrictedRolePlayerNamespace"/>
																	<xsl:text>.</xsl:text>
																</xsl:message>
															</xsl:if>
															<plx:passParam>
																<plx:callStatic name="DomainClassId" dataTypeName="{$restrictedRolePlayer}" dataTypeQualifier="{$restrictedRolePlayerNamespace}" type="field"/>
															</plx:passParam>
														</xsl:when>
														<xsl:otherwise>
															<xsl:if test="not($domainClasses[@Name=$restrictedRolePlayer]) and not($domainRelationships[@Name=$restrictedRolePlayer])">
																<xsl:message terminate="yes">
																	<xsl:text>'</xsl:text>
																	<xsl:value-of select="$restrictedRolePlayer"/>
																	<xsl:text>' must be a DomainClass or a DomainRelationship in the model file.</xsl:text>
																</xsl:message>
															</xsl:if>
															<plx:passParam>
																<plx:callStatic name="DomainClassId" dataTypeName="{$restrictedRolePlayer}" type="field"/>
															</plx:passParam>
														</xsl:otherwise>
													</xsl:choose>
													<plx:passParam>
														<xsl:choose>
															<xsl:when test="@includeFromRoleClassDescendants[.='true' or .='1']">
																<plx:trueKeyword/>
															</xsl:when>
															<xsl:otherwise>
																<plx:falseKeyword/>
															</xsl:otherwise>
														</xsl:choose>
													</plx:passParam>
												</xsl:if>
											</plx:callNew>
										</plx:passParam>
										<plx:passParam>
											<plx:callNew dataTypeName="DomainRoleClosureRestriction">
												<plx:passParam>
													<plx:callStatic name="{$role}DomainRoleId" dataTypeName="{$relationship}" type="field">
														<xsl:if test="$relationshipNamespace">
															<xsl:attribute name="dataTypeQualifier">
																<xsl:value-of select="$relationshipNamespace"/>
															</xsl:attribute>
														</xsl:if>
													</plx:callStatic>
												</plx:passParam>
												<xsl:variable name="restrictedRolePlayer" select="string(@explicitToRoleClass)"/>
												<xsl:variable name="restrictedRolePlayerNamespace" select="string(@explicitToRoleNamespace)"/>
												<xsl:if test="$restrictedRolePlayer">
													<xsl:choose>
														<xsl:when test="$restrictedRolePlayerNamespace">
															<xsl:if test="not($referencedDslModels[@Namespace=$restrictedRolePlayerNamespace][dsl:Classes/dsl:DomainClass[@Name=$restrictedRolePlayer] or dsl:Relationships/dsl:DomainRelationship[@Name=$restrictedRolePlayer]])">
																<xsl:message terminate="yes">
																	<xsl:text>'</xsl:text>
																	<xsl:value-of select="$restrictedRolePlayer"/>
																	<xsl:text>' must be a DomainClass or a DomainRelationship in the model file with namespace</xsl:text>
																	<xsl:value-of select="$restrictedRolePlayerNamespace"/>
																	<xsl:text>.</xsl:text>
																</xsl:message>
															</xsl:if>
															<plx:passParam>
																<plx:callStatic name="DomainClassId" dataTypeName="{$restrictedRolePlayer}" dataTypeQualifier="{$restrictedRolePlayerNamespace}" type="field"/>
															</plx:passParam>
														</xsl:when>
														<xsl:otherwise>
															<xsl:if test="not($domainClasses[@Name=$restrictedRolePlayer]) and not($domainRelationships[@Name=$restrictedRolePlayer])">
																<xsl:message terminate="yes">
																	<xsl:text>'</xsl:text>
																	<xsl:value-of select="$restrictedRolePlayer"/>
																	<xsl:text>' must be a DomainClass or a DomainRelationship in the model file.</xsl:text>
																</xsl:message>
															</xsl:if>
															<plx:passParam>
																<plx:callStatic name="DomainClassId" dataTypeName="{$restrictedRolePlayer}" type="field"/>
															</plx:passParam>
														</xsl:otherwise>
													</xsl:choose>
													<plx:passParam>
														<xsl:choose>
															<xsl:when test="@includeToRoleClassDescendants[.='true' or .='1']">
																<plx:trueKeyword/>
															</xsl:when>
															<xsl:otherwise>
																<plx:falseKeyword/>
															</xsl:otherwise>
														</xsl:choose>
													</plx:passParam>
												</xsl:if>
											</plx:callNew>
										</plx:passParam>
										<plx:passParam>
											<plx:callStatic name="None" dataTypeName="CopyClosureDirectiveOptions" type="field">
												<xsl:if test="@rootElementOnly[.='true' or .='1']">
													<xsl:attribute name="name">
														<xsl:text>RootElementOnly</xsl:text>
													</xsl:attribute>
												</xsl:if>
											</plx:callStatic>
										</plx:passParam>
										<plx:passParam>
											<xsl:choose>
												<xsl:when test="$behavior='Custom'">
													<xsl:variable name="delegateBody" select="cmc:CustomBehavior/plx:*"/>
													<xsl:if test="not($delegateBody)">
														<xsl:message terminate="yes">
															<xsl:text>closureBehavior="Custom" requires a population CustomBehavior element.</xsl:text>
														</xsl:message>
													</xsl:if>
													<plx:anonymousFunction>
														<plx:param name="link" dataTypeName="ElementLink"/>
														<plx:returns dataTypeName="CopyClosureBehavior"/>
														<xsl:copy-of select="$delegateBody"/>
													</plx:anonymousFunction>
												</xsl:when>
												<xsl:otherwise>
													<plx:callStatic name="{$behavior}" dataTypeName="CopyClosureBehavior" type="field"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:passParam>
									</plx:callInstance>
									<xsl:variable name="ordering" select="string(@order)"/>
									<xsl:if test="$ordering and not($ordering='Neither')">
										<xsl:if test="$ordering='From' or $ordering='Both'">
											<xsl:if test="$dslFromRole[@Multiplicity[.='One' or .='ZeroOne']]">
												<xsl:message terminate="yes">
													<xsl:text>The role '</xsl:text>
													<xsl:value-of select="$dslFromRole/@Name"/>
													<xsl:text>' (opposite </xsl:text><xsl:value-of select="$role"/><xsl:text>) in relationship </xsl:text>
													<xsl:value-of select="$relationship"/>
													<xsl:text> does not form a collection and should not be ordered.</xsl:text>
												</xsl:message>
											</xsl:if>
											<plx:callInstance name="AddOrderedRole">
												<plx:callObject>
													<plx:nameRef name="closureManager" type="parameter"/>
												</plx:callObject>
												<plx:passParam>
													<plx:callStatic dataTypeName="{$relationship}" name="{$dslFromRole/@Name}DomainRoleId" type="field">
														<xsl:if test="$relationshipNamespace">
															<xsl:attribute name="dataTypeQualifier">
																<xsl:value-of select="$relationshipNamespace"/>
															</xsl:attribute>
														</xsl:if>
													</plx:callStatic>
												</plx:passParam>
												<plx:passParam>
													<plx:callStatic dataTypeName="MergeIntegrationOrder" name="AfterLeading" type="field">
														<xsl:if test="@fromIntegrationOrder[not(.='Unordered' or .='AfterLeading')]">
															<xsl:attribute name="name">
																<xsl:value-of select="@fromIntegrationOrder"/>
															</xsl:attribute>
														</xsl:if>
													</plx:callStatic>
												</plx:passParam>
											</plx:callInstance>
										</xsl:if>
										<xsl:if test="$ordering='To' or $ordering='Both'">
											<xsl:if test="$dslToRole[@Multiplicity[.='One' or .='ZeroOne']]">
												<xsl:message terminate="yes">
													<xsl:text>The role '</xsl:text>
													<xsl:value-of select="$role"/>
													<xsl:text>' in relationship </xsl:text>
													<xsl:value-of select="$relationship"/>
													<xsl:text> does not form a collection and should not be ordered.</xsl:text>
												</xsl:message>
											</xsl:if>
											<plx:callInstance name="AddOrderedRole">
												<plx:callObject>
													<plx:nameRef name="closureManager" type="parameter"/>
												</plx:callObject>
												<plx:passParam>
													<plx:callStatic dataTypeName="{$relationship}" name="{$role}DomainRoleId" type="field">
														<xsl:if test="$relationshipNamespace">
															<xsl:attribute name="dataTypeQualifier">
																<xsl:value-of select="$relationshipNamespace"/>
															</xsl:attribute>
														</xsl:if>
													</plx:callStatic>
												</plx:passParam>
												<plx:passParam>
													<plx:callStatic dataTypeName="MergeIntegrationOrder" name="AfterLeading" type="field">
														<xsl:if test="@toIntegrationOrder[not(.='Unordered' or .='AfterLeading')]">
															<xsl:attribute name="name">
																<xsl:value-of select="@toIntegrationOrder"/>
															</xsl:attribute>
														</xsl:if>
													</plx:callStatic>
												</plx:passParam>
											</plx:callInstance>
										</xsl:if>
									</xsl:if>
								</xsl:if>
							</xsl:for-each>
							<plx:pragma type="closeRegion" data="Closures for explicit relationships"/>
						</xsl:if>
						
						<!-- Add callbacks for implied references -->
						<xsl:variable name="impliedReferences" select="$closureModel/cmc:ImpliedReferences/cmc:ImpliedReference"/>
						<xsl:if test="$impliedReferences">
							<plx:pragma type="region" data="Implied reference callbacks"/>
							<xsl:for-each select="$impliedReferences">
								<plx:callInstance name="AddImpliedReference">
									<plx:callObject>
										<plx:nameRef name="closureManager" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="DomainClassId" dataTypeName="{@class}" type="field"/>
									</plx:passParam>
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="@includeClassDescendants='true' or @includeClassDescendants='1'">
												<plx:trueKeyword/>
											</xsl:when>
											<xsl:otherwise>
												<plx:falseKeyword/>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
									<plx:passParam>
										<plx:anonymousFunction>
											<plx:param name="element" dataTypeName="ModelElement"/>
											<plx:param name="notifyImpliedReference" dataTypeName="Action">
												<plx:passTypeParam dataTypeName="ModelElement"/>
											</plx:param>
											<xsl:copy-of select="*"/>
										</plx:anonymousFunction>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
							<plx:pragma type="closeRegion" data="Implied reference callbacks"/>
						</xsl:if>

						<!-- Handle properties that should never be copied with an element. -->
						<xsl:variable name="explicitIgnoredProperties" select="$closureModel/cmc:IgnoredProperties/cmc:IgnoredProperty"/>
						<xsl:variable name="conditionalProperties" select="$closureModel/cmc:ConditionalProperties/cmc:ConditionalProperty"/>
						<!-- Verify correct binding for ignored properties -->
						<xsl:for-each select="$explicitIgnoredProperties|$conditionalProperties">
							<xsl:variable name="missingPropertyMessageFragment">
								<xsl:variable name="propertyClass" select="$domainClasses[@Name=current()/@class]"/>
								<xsl:choose>
									<xsl:when test="$propertyClass">
										<xsl:if test="not($propertyClass/dsl:Properties/dsl:DomainProperty[@Name=current()/@property])">
											<xsl:choose>
												<xsl:when test="self::cmc:IgnoredProperty">
													<xsl:text>Ignored property '</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text>Conditional property '</xsl:text>
												</xsl:otherwise>
											</xsl:choose>
											<xsl:value-of select="@property"/>
											<xsl:text>' does not exist in domain class '</xsl:text>
											<xsl:value-of select="@class"/>
											<xsl:text>'.</xsl:text>
										</xsl:if>
									</xsl:when>
									<xsl:otherwise>
										<xsl:variable name="propertyRelationship" select="$domainRelationships[@Name=current()/@class]"/>
										<xsl:choose>
											<xsl:when test="$propertyRelationship">
												<xsl:if test="not($propertyRelationship/dsl:Properties/dsl:DomainProperty[@Name=current()/@property])">
													<xsl:choose>
														<xsl:when test="self::cmc:IgnoredProperty">
															<xsl:text>Ignored property '</xsl:text>
														</xsl:when>
														<xsl:otherwise>
															<xsl:text>Conditional property '</xsl:text>
														</xsl:otherwise>
													</xsl:choose>
													<xsl:value-of select="@property"/>
													<xsl:text>' does not exist in domain relationship '</xsl:text>
													<xsl:value-of select="@class"/>
													<xsl:text>'.</xsl:text>
												</xsl:if>
											</xsl:when>
											<xsl:otherwise>
												<xsl:choose>
													<xsl:when test="self::cmc:IgnoredProperty">
														<xsl:text>Ignored property class'</xsl:text>
													</xsl:when>
													<xsl:otherwise>
														<xsl:text>Conditional property class'</xsl:text>
													</xsl:otherwise>
												</xsl:choose>
												<xsl:value-of select="@class"/>
												<xsl:text>' does not exist in the domain model.</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="missingPropertyMessage" select="string($missingPropertyMessageFragment)"/>
							<xsl:if test="$missingPropertyMessage">
								<xsl:message terminate="yes">
									<xsl:value-of select="$missingPropertyMessage"/>
								</xsl:message>
							</xsl:if>
						</xsl:for-each>
						<xsl:variable name="allIgnoredPropertiesFragment">
							<xsl:copy-of select="$explicitIgnoredProperties"/>
							<xsl:for-each select="$domainClasses/dsl:Properties/dsl:DomainProperty[@Kind='Calculated']|$domainRelationships/dsl:Properties/dsl:DomainProperty[@Kind='Calculated']">
								<xsl:variable name="className" select="string(../../@Name)"/>
								<xsl:variable name="propertyName" select="string(@Name)"/>
								<xsl:if test="not($explicitIgnoredProperties[@property=$propertyName][@class=$className])">
									<cmc:IgnoredProperty class="{$className}" property="{$propertyName}"/>
								</xsl:if>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="allIgnoredProperties" select="exsl:node-set($allIgnoredPropertiesFragment)/child::*"/>
						<xsl:if test="$allIgnoredProperties">
							<plx:pragma type="region" data="Register ignored properties"/>
							<xsl:for-each select="$allIgnoredProperties">
								<xsl:sort select="@class"/>
								<xsl:sort select="@property"/>
								<plx:callInstance name="AddIgnoredProperty">
									<plx:callObject>
										<plx:nameRef name="closureManager" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="{@property}DomainPropertyId" dataTypeName="{@class}" type="field"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
							<plx:pragma type="closeRegion" data="Register ignored properties"/>
						</xsl:if>
						<xsl:if test="$conditionalProperties">
							<plx:pragma type="region" data="Register conditional properties"/>
							<xsl:for-each select="$conditionalProperties">
								<xsl:sort select="@class"/>
								<xsl:sort select="@property"/>
								<plx:callInstance name="AddConditionalProperty">
									<plx:callObject>
										<plx:nameRef name="closureManager" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="{@property}DomainPropertyId" dataTypeName="{@class}" type="field"/>
									</plx:passParam>
									<plx:passParam>
										<plx:anonymousFunction>
											<plx:param name="sourceElement" dataTypeName="ModelElement"/>
											<plx:param name="targetElement" dataTypeName="ModelElement"/>
											<xsl:copy-of select="*"/>
										</plx:anonymousFunction>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
							<plx:pragma type="closeRegion" data="Register conditional properties"/>
						</xsl:if>
					</plx:function>
				</plx:class>
				<!-- Add automatic equivalence implementations based on 1-1 embeddings for target types in this model. -->
				<xsl:variable name="autoEquivalenceOneToOneEmbeddings" select="$embeddingRelationships[dsl:Source[dsl:DomainRole/@Multiplicity[not(contains(.,'Many'))]]][not(@Name=$embeddingDirectives[@automaticEquivalence[.='false' or .='0']]/@relationship)][not(dsl:Target/dsl:DomainRole/dsl:RolePlayer/dsl:*/@Name[starts-with(.,'/')])]"/>
				<xsl:if test="$autoEquivalenceOneToOneEmbeddings">
					<plx:pragma type="region" data="One-to-One Embedded Element Equivalence"/>
					<xsl:variable name="oneToOneEmbeddingsByTargetFragment">
						<xsl:for-each select="$autoEquivalenceOneToOneEmbeddings">
							<xsl:sort select="dsl:Target/dsl:DomainRole/dsl:RolePlayer/dsl:*/@Name"/>
							<xsl:sort select="dsl:Source/dsl:DomainRole/dsl:RolePlayer/dsl:*/@Name"/>
							<xsl:copy-of select="."/>
						</xsl:for-each>
					</xsl:variable>
					<xsl:for-each select="exsl:node-set($oneToOneEmbeddingsByTargetFragment)/child::*">
						<xsl:variable name="firstTargetRolePlayer" select="dsl:Target/dsl:DomainRole/dsl:RolePlayer/dsl:*"/>
						<xsl:variable name="targetRolePlayerType" select="string($firstTargetRolePlayer/@Name)"/>
						<xsl:if test="not(preceding-sibling::*[1]/dsl:Target/dsl:DomainRole/dsl:RolePlayer/dsl:*/@Name=$targetRolePlayerType)">
							<xsl:variable name="isSealedClassFragment">
								<xsl:choose>
									<xsl:when test="$firstTargetRolePlayer[self::dsl:DomainClassMoniker]">
										<xsl:if test="$domainClasses[@Name=$targetRolePlayerType][@InheritanceModifier='Sealed']">
											<xsl:text>x</xsl:text>
										</xsl:if>
									</xsl:when>
									<xsl:when test="$firstTargetRolePlayer[self::dsl:DomainRelationshipMoniker]">
										<xsl:if test="$domainRelationships[@Name=$targetRolePlayerType][@InheritanceModifier='Sealed']">
											<xsl:text>x</xsl:text>
										</xsl:if>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="isSealedClass" select="boolean(string-length($isSealedClassFragment))"/>
							<plx:class name="{$targetRolePlayerType}" partial="true" visibility="deferToPartial">
								<plx:leadingInfo>
									<plx:pragma type="region" data="{$targetRolePlayerType} Element Equivalence"/>
								</plx:leadingInfo>
								<plx:trailingInfo>
									<plx:pragma type="closeRegion" data="{$targetRolePlayerType} Element Equivalence"/>
								</plx:trailingInfo>
								<plx:implementsInterface dataTypeName="IElementEquivalence"/>
								<plx:function name="MapEquivalentElements" visibility="protected">
									<xsl:choose>
										<xsl:when test="$isSealedClass">
											<xsl:attribute name="visibility">
												<xsl:text>privateInterfaceMember</xsl:text>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<plx:leadingInfo>
												<xsl:if test="not($isSealedClass)">
													<plx:docComment>
														<summary>
															<xsl:text>Implements </xsl:text>
															<cref name="IElementEquivalence.MapEquivalentElements"/>
														</summary>
													</plx:docComment>
												</xsl:if>
											</plx:leadingInfo>
										</xsl:otherwise>
									</xsl:choose>
									<plx:interfaceMember dataTypeName="IElementEquivalence" memberName="MapEquivalentElements"/>
									<plx:param name="foreignStore" dataTypeName="Store"/>
									<plx:param name="elementTracker" dataTypeName="IEquivalentElementTracker"/>
									<plx:returns dataTypeName=".boolean"/>
									<xsl:variable name="possibleRelationships" select=".|following-sibling::*[dsl:Target/dsl:DomainRole/dsl:RolePlayer/dsl:*/@Name[.=$targetRolePlayerType]]"/>
									<xsl:variable name="allSourceRolePlayerTypeAttributesFragment">
										<xsl:for-each select="$possibleRelationships">
											<dummy>
												<xsl:call-template name="AddPlixTypeAttributes">
													<xsl:with-param name="Type" select="string(dsl:Source/dsl:DomainRole/dsl:RolePlayer/dsl:*/@Name)"/>
												</xsl:call-template>
											</dummy>
										</xsl:for-each>
									</xsl:variable>
									<xsl:variable name="allSourceRolePlayerTypeAttributes" select="exsl:node-set($allSourceRolePlayerTypeAttributesFragment)/child::*"/>
									<xsl:for-each select="$possibleRelationships">
										<xsl:variable name="currentPosition" select="position()"/>
										<xsl:variable name="sourceRolePlayerTypeAttributes" select="$allSourceRolePlayerTypeAttributes[position()=$currentPosition]"/>
										<plx:local name="parent{$sourceRolePlayerTypeAttributes/@dataTypeName}">
											<xsl:copy-of select="$sourceRolePlayerTypeAttributes/@*"/>
										</plx:local>
									</xsl:for-each>
									<xsl:for-each select="$possibleRelationships">
										<xsl:variable name="currentPosition" select="position()"/>
										<xsl:variable name="sourceRole" select="dsl:Source/dsl:DomainRole"/>
										<xsl:variable name="sourceRolePlayerTypeAttributes" select="$allSourceRolePlayerTypeAttributes[position()=$currentPosition]"/>
										<xsl:variable name="sourceRolePlayerTypeName" select="string($sourceRolePlayerTypeAttributes/@dataTypeName)"/>
										<xsl:variable name="targetRole" select="dsl:Target/dsl:DomainRole"/>
										<xsl:variable name="relationshipType" select="string(@Name)"/>
										<xsl:variable name="branchElementName">
											<xsl:choose>
												<xsl:when test="position()=1">
													<xsl:text>plx:branch</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text>plx:alternateBranch</xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:element name="{$branchElementName}">
											<plx:condition>
												<plx:binaryOperator type="identityInequality">
													<plx:left>
														<plx:nullKeyword/>
													</plx:left>
													<plx:right>
														<plx:inlineStatement>
															<xsl:copy-of select="$sourceRolePlayerTypeAttributes/@*"/>
															<plx:assign>
																<plx:left>
																	<plx:nameRef name="parent{$sourceRolePlayerTypeName}"/>
																</plx:left>
																<plx:right>
																	<xsl:choose>
																		<xsl:when test="$targetRole/@IsPropertyGenerator[.='true' or .='1']">
																			<!-- Just call the property generated on this class -->
																			<plx:callThis name="{$targetRole/@PropertyName}" type="property"/>
																		</xsl:when>
																		<xsl:otherwise>
																			<plx:callStatic dataTypeName="{$relationshipType}" name="Get{$targetRole/@PropertyName}">
																				<xsl:if test="not(string($targetRole/@PropertyName))">
																					<xsl:attribute name="name">
																						<xsl:text>Get</xsl:text>
																						<xsl:value-of select="$sourceRole/@Name"/>
																					</xsl:attribute>
																				</xsl:if>
																				<plx:passParam>
																					<plx:thisKeyword/>
																				</plx:passParam>
																			</plx:callStatic>
																		</xsl:otherwise>
																	</xsl:choose>
																</plx:right>
															</plx:assign>
														</plx:inlineStatement>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<plx:comment>
												<xsl:text>Embedded through the </xsl:text>
												<xsl:value-of select="$relationshipType"/>
												<xsl:text> relationship</xsl:text>
											</plx:comment>
											<plx:local name="otherParent{$sourceRolePlayerTypeName}">
												<xsl:copy-of select="$sourceRolePlayerTypeAttributes/@*"/>
											</plx:local>
											<plx:local name="other{$targetRolePlayerType}" dataTypeName="{$targetRolePlayerType}"/>
											<plx:branch>
												<plx:condition>
													<plx:binaryOperator type="booleanAnd">
														<plx:left>
															<plx:binaryOperator type="identityInequality">
																<plx:left>
																	<plx:nullKeyword/>
																</plx:left>
																<plx:right>
																	<plx:inlineStatement>
																		<xsl:copy-of select="$sourceRolePlayerTypeAttributes/@*"/>
																		<plx:assign>
																			<plx:left>
																				<plx:nameRef name="otherParent{$sourceRolePlayerTypeName}"/>
																			</plx:left>
																			<plx:right>
																				<plx:callStatic dataTypeName="CopyMergeUtility" name="GetEquivalentElement">
																					<plx:passParam>
																						<plx:nameRef name="parent{$sourceRolePlayerTypeName}"/>
																					</plx:passParam>
																					<plx:passParam>
																						<plx:nameRef name="foreignStore" type="parameter"/>
																					</plx:passParam>
																					<plx:passParam>
																						<plx:nameRef name="elementTracker" type="parameter"/>
																					</plx:passParam>
																				</plx:callStatic>
																			</plx:right>
																		</plx:assign>
																	</plx:inlineStatement>
																</plx:right>
															</plx:binaryOperator>
														</plx:left>
														<plx:right>
															<plx:binaryOperator type="identityInequality">
																<plx:left>
																	<plx:nullKeyword/>
																</plx:left>
																<plx:right>
																	<plx:inlineStatement>
																		<xsl:copy-of select="$sourceRolePlayerTypeAttributes/@*"/>
																		<plx:assign>
																			<plx:left>
																				<plx:nameRef name="other{$targetRolePlayerType}"/>
																			</plx:left>
																			<plx:right>
																				<xsl:choose>
																					<xsl:when test="$sourceRole/@IsPropertyGenerator[.='true' or .='1']">
																						<!-- Just call the property generated on this class -->
																						<plx:callInstance name="{$sourceRole/@PropertyName}" type="property">
																							<plx:callObject>
																								<plx:nameRef name="otherParent{$sourceRolePlayerTypeName}"/>
																							</plx:callObject>
																						</plx:callInstance>
																					</xsl:when>
																					<xsl:otherwise>
																						<plx:callStatic dataTypeName="{$relationshipType}" name="Get{$sourceRole/@PropertyName}">
																							<xsl:if test="not(string($sourceRole/@PropertyName))">
																								<xsl:attribute name="name">
																									<xsl:text>Get</xsl:text>
																									<xsl:value-of select="$targetRole/@Name"/>
																								</xsl:attribute>
																							</xsl:if>
																							<plx:passParam>
																								<plx:nameRef name="otherParent{$sourceRolePlayerTypeName}"/>
																							</plx:passParam>
																						</plx:callStatic>
																					</xsl:otherwise>
																				</xsl:choose>
																			</plx:right>
																		</plx:assign>
																	</plx:inlineStatement>
																</plx:right>
															</plx:binaryOperator>
														</plx:right>
													</plx:binaryOperator>
												</plx:condition>
												<plx:callInstance name="AddEquivalentElement">
													<plx:callObject>
														<plx:nameRef name="elementTracker" type="parameter"/>
													</plx:callObject>
													<plx:passParam>
														<plx:thisKeyword/>
													</plx:passParam>
													<plx:passParam>
														<plx:nameRef name="other{$targetRolePlayerType}"/>
													</plx:passParam>
												</plx:callInstance>
												<xsl:apply-templates select="$embeddingDirectives[@relationship=$relationshipType]/cmc:AfterAutomaticEquivalence/child::*" mode="DuplicateChangeOTHER">
													<xsl:with-param name="OTHERName" select="concat('other',$targetRolePlayerType)"/>
												</xsl:apply-templates>
												<plx:return>
													<plx:trueKeyword/>
												</plx:return>
											</plx:branch>
										</xsl:element>
									</xsl:for-each>
									<plx:return>
										<plx:falseKeyword/>
									</plx:return>
								</plx:function>
							</plx:class>
						</xsl:if>
					</xsl:for-each>
					<plx:pragma type="closeRegion" data="One-to-One Embedded Element Equivalence"/>
				</xsl:if>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="*" mode="DuplicateChangeOTHER">
		<xsl:param name="OTHERName"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="*" mode="DuplicateChangeOTHER">
				<xsl:with-param name="OTHERName" select="$OTHERName"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="plx:nameRef[@name='OTHER']" mode="DuplicateChangeOTHER">
		<xsl:param name="OTHERName"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:attribute name="name">
				<xsl:value-of select="$OTHERName"/>
			</xsl:attribute>
			<xsl:copy-of select="*"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template name="AddPlixTypeAttributes">
		<xsl:param name="Type"/>
		<xsl:choose>
			<xsl:when test="starts-with($Type,'/')">
				<!-- This is a namespace-qualified moniker in the form "/NAMESPACE/NAME -->
				<xsl:variable name="strippedLead" select="substring($Type,2)"/>
				<xsl:attribute name="dataTypeName">
					<xsl:value-of select="substring-after($strippedLead,'/')"/>
				</xsl:attribute>
				<xsl:attribute name="dataTypeQualifier">
					<xsl:value-of select="substring-before($strippedLead,'/')"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="dataTypeName">
					<xsl:value-of select="$Type"/>
				</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="AddComments">
		<xsl:for-each select="comment()">
			<plx:comment>
				<xsl:value-of select="normalize-space(translate(.,'&#xd;&#xa;',' '))"/>
			</plx:comment>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
