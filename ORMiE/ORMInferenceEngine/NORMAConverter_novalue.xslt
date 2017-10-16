<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="2.0" 	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
								xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore" 
								xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram" 
								xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
>
	<!-- Author : Ferdian Jovan, Affiliation : EMCL, Year : 2012 -->
	<xsl:output method='text' indent='yes'/>
  
	<xsl:template match="/">
	
		<!-- ENTITY TYPES -->
		<xsl:text>ENTITYTYPES:{</xsl:text>
			<xsl:for-each select="//orm:Objects/orm:EntityType">
				<xsl:value-of select="@id"/>
				<xsl:text>#</xsl:text>
				<xsl:value-of select="@Name"/>
				<xsl:if test="position() != last()">
					<xsl:text>, </xsl:text>
				</xsl:if>
			</xsl:for-each>
			<xsl:for-each select="//orm:Objects/orm:ObjectifiedType">
				<xsl:if test="not(orm:NestedPredicate/@IsImplied)">
					<xsl:if test="//orm:Objects/orm:EntityType[1]/@id != ''">
						<xsl:text>, </xsl:text>
					</xsl:if>
					<xsl:value-of select="@id"/>
					<xsl:text>#</xsl:text>
					<xsl:value-of select="@Name"/>
					<xsl:if test="//orm:Objects/orm:EntityType[1]/@id = ''">
						<xsl:if test="position() != last()">
							<xsl:text>, </xsl:text>
						</xsl:if>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		<xsl:text>}</xsl:text>
		
		<xsl:text>&#xa;</xsl:text>
		<xsl:text>&#xa;</xsl:text>
		
		<!-- RELATIONS TYPES -->
		<xsl:text>RELATIONS:{</xsl:text>
			<xsl:for-each select="//orm:Facts/orm:Fact">
				<xsl:value-of select="@id"/>
				<xsl:text>#</xsl:text>
				<xsl:value-of select="@_Name"/>
				<xsl:if test="position() != last()">
					<xsl:text>, </xsl:text>
				</xsl:if>
			</xsl:for-each>
		<xsl:text>}</xsl:text>
		
		<xsl:text>&#xa;</xsl:text>
		<xsl:text>&#xa;</xsl:text>
		
		<!-- TYPE (ATTRIBUTE,OBJECT{ENTITY_TYPE,VALUE_TYPE) -->		
		<xsl:for-each select="//orm:Facts/orm:Fact">
			<xsl:for-each select="orm:FactRoles/orm:Role">
				<xsl:text>LOC-ROLES-INDEX(</xsl:text>	
					<xsl:variable name="entity_name" select="orm:RolePlayer/@ref"></xsl:variable>
					<xsl:value-of select="@id"/>
					<xsl:text>#</xsl:text>
					<xsl:value-of select="../../@_Name"/>
					<xsl:text>.</xsl:text><xsl:value-of select="position()" />
					<xsl:text>, </xsl:text>
					<xsl:value-of select="position()" />
				<xsl:text>) </xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:for-each>
		</xsl:for-each>
		
		<xsl:text>&#xa;</xsl:text>
		
		<!-- TYPE (ATTRIBUTE,OBJECT{ENTITY_TYPE,VALUE_TYPE) -->		
		<xsl:for-each select="//orm:Facts/orm:Fact">
			<xsl:for-each select="orm:FactRoles/orm:Role">
				<xsl:text>TYPE(</xsl:text>	
					<xsl:variable name="entity_name" select="orm:RolePlayer/@ref"></xsl:variable>
					<xsl:value-of select="@id"/>
					<xsl:text>#</xsl:text>
					<xsl:value-of select="../../@_Name"/>
					<xsl:text>.</xsl:text><xsl:value-of select="position()" />
					<xsl:text>, </xsl:text>
					<xsl:value-of select="orm:RolePlayer/@ref"/>
					<xsl:text>#</xsl:text>
					<xsl:value-of select="//orm:Objects/orm:EntityType[@id=$entity_name]/@Name | //orm:Objects/orm:ValueType[@id=$entity_name]/@Name"/>
				<xsl:text>) </xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:for-each>
		</xsl:for-each>
		
		<xsl:text>&#xa;</xsl:text>
		
		<!-- MANDATORY (ATTRIBUTE,ENTITY_TYPE)-->
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:MandatoryConstraint">
			<!-- internal mandatory constraint -->
			<xsl:if test="@IsSimple='true'">
				<xsl:variable name="entity_name" select="orm:RoleSequence/orm:Role/@ref"></xsl:variable>
				<xsl:if test="//orm:Fact/orm:FactRoles/orm:Role[@id= $entity_name]">
					<xsl:text>MAND({</xsl:text>
						<xsl:value-of select="orm:RoleSequence/orm:Role/@ref"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>
						<xsl:text>}, </xsl:text>
						<xsl:value-of select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/orm:RolePlayer/@ref"/>
						<xsl:text>#</xsl:text>
						<xsl:variable name="entity_name" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/orm:RolePlayer/@ref"></xsl:variable>
						<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name]/@Name"/>
					<xsl:text>) </xsl:text>
					<xsl:text>&#xa;</xsl:text>
				</xsl:if>
			</xsl:if>
			<!-- Disjunctive inclusive constraint -->
			<xsl:if test="not(@IsSimple='true') and not(@IsImplied = 'true')">
				<xsl:variable name="entity_name" select="orm:RoleSequence/orm:Role[1]/@ref"></xsl:variable>
				<xsl:if test="//orm:Fact/orm:FactRoles/orm:Role[@id= $entity_name]">
					<xsl:text>MAND({</xsl:text>
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:variable name="entity_name" select="@ref"></xsl:variable>
							<xsl:if test="position() != 1">
								<xsl:text>, </xsl:text>
							</xsl:if>
							<xsl:value-of select="@ref"/>
							<xsl:text>#</xsl:text>
							<xsl:for-each select="//orm:Facts/orm:Fact">
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<xsl:if test="@id = $entity_name">
										<xsl:value-of select="../../@_Name"/>
										<xsl:text>.</xsl:text>
										<xsl:value-of select="position()" />	
									</xsl:if>		
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>		
						<xsl:text>}, </xsl:text>
						<xsl:value-of select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/orm:RolePlayer/@ref"/>
						<xsl:text>#</xsl:text>
						<xsl:variable name="entity_name" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/orm:RolePlayer/@ref"></xsl:variable>
						<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name]/@Name"/>
					<xsl:text>) </xsl:text>
					<xsl:text>&#xa;</xsl:text>
				</xsl:if>
			</xsl:if>
		</xsl:for-each>
		
		<xsl:text>&#xa;</xsl:text>
		
		<!-- FREQ (ATTRIBUTE,(I,J))-->		
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:UniquenessConstraint">
			<!-- Internal uniqueness constraint-->
			<xsl:if test="@IsInternal = 'true'">
				<xsl:variable name="entity_name" select="orm:RoleSequence/orm:Role/@ref"></xsl:variable>
				<xsl:if test="//orm:Fact/orm:FactRoles/orm:Role[@id= $entity_name]">
					<xsl:text>FREQ({</xsl:text>
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:if test="position() != 1">
								<xsl:text>, </xsl:text>
							</xsl:if>
							<xsl:value-of select="@ref"/>
							<xsl:text>#</xsl:text>
							<xsl:variable name="entity_name" select="@ref"></xsl:variable>
							<xsl:for-each select="//orm:Facts/orm:Fact">
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<xsl:if test="@id = $entity_name">
										<xsl:value-of select="../../@_Name"/>
										<xsl:text>.</xsl:text>
										<xsl:value-of select="position()" />	
									</xsl:if>		
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>	
						<xsl:text>}, {}, (1,1)</xsl:text>
					<xsl:text>) </xsl:text>
					<xsl:text>&#xa;</xsl:text>
				</xsl:if>
			</xsl:if>
			
			<!-- External uniqueness constraint-->
			<xsl:if test="not(@IsInternal = 'true')">
				<xsl:variable name="entity_name" select="orm:RoleSequence/orm:Role/@ref"></xsl:variable>
				<xsl:if test="//orm:Fact/orm:FactRoles/orm:Role[@id= $entity_name]">
					<xsl:text>FREQ({</xsl:text>
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:if test="position() != 1">
								<xsl:text>, </xsl:text>
							</xsl:if>
							<xsl:value-of select="@ref"/>
							<xsl:text>#</xsl:text>
							<xsl:variable name="entity_name" select="@ref"></xsl:variable>
							<xsl:for-each select="//orm:Facts/orm:Fact">
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<xsl:if test="@id = $entity_name">
										<xsl:value-of select="../../@_Name"/>
										<xsl:text>.</xsl:text>
										<xsl:value-of select="position()" />	
									</xsl:if>		
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>	
						<xsl:text>}, {</xsl:text>
						<xsl:choose>
							<xsl:when test="orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
								<xsl:for-each select="orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
									<xsl:choose>
										<xsl:when test="position() = 1">
											<xsl:text>(</xsl:text>
											<xsl:value-of select="@ref"/>
											<xsl:text>#</xsl:text>
											<xsl:variable name="entity_name" select="@ref"></xsl:variable>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>
											<xsl:text>, </xsl:text>
										</xsl:when>
										<xsl:when test="position() = last()">
											<xsl:value-of select="@ref"/>
											<xsl:text>#</xsl:text>
											<xsl:variable name="entity_name" select="@ref"></xsl:variable>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>
											<xsl:text>)</xsl:text>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="@ref"/>
											<xsl:text>#</xsl:text>
											<xsl:variable name="entity_name" select="@ref"></xsl:variable>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>
											<xsl:text>), (</xsl:text>
											<xsl:value-of select="@ref"/>
											<xsl:text>#</xsl:text>
											<xsl:variable name="entity_name" select="@ref"></xsl:variable>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>
											<xsl:text>, </xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:when>
							<xsl:when test="orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
								<xsl:for-each select="orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
									<xsl:variable name="entity_name" select="@ref"></xsl:variable>

									<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
									<xsl:variable name="entity_name3" select="position()"></xsl:variable>
									<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
									<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:UniquenessConstraint/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
										<xsl:if test="position() = ($entity_name3 + 1)">
											<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
											<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
												<xsl:text>(</xsl:text>
												<xsl:value-of select="$entity_name"/>
												<xsl:text>#</xsl:text>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>	
												<xsl:text>, </xsl:text>	
												<xsl:value-of select="$entity_name5"/>
												<xsl:text>#</xsl:text>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name5">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>	
												<xsl:text>), </xsl:text>
											</xsl:if>
										</xsl:if>
									</xsl:for-each>

								</xsl:for-each>
								<xsl:text>()</xsl:text>
							</xsl:when>
							<xsl:otherwise>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:text>}, (1,1)</xsl:text>
					<xsl:text>) </xsl:text>
					<xsl:text>&#xa;</xsl:text>
				</xsl:if>
			</xsl:if>				
		</xsl:for-each>
		
		<xsl:text>&#xa;</xsl:text>
		
		<!-- OBJ (ATTRIBUTE,(I,J))-->
		<xsl:for-each select="//orm:Objects/orm:ObjectifiedType">
			<xsl:if test="not(orm:NestedPredicate/@IsImplied)">
				<xsl:text>OBJ(</xsl:text>
				<xsl:variable name="entity_name" select="orm:NestedPredicate/@ref"></xsl:variable>
				<xsl:value-of select="@id"/>
				<xsl:text>#</xsl:text>
				<xsl:value-of select="@Name"/>
				<xsl:text>, </xsl:text>
				<xsl:value-of select="//orm:Facts/orm:Fact[@id = $entity_name]/@id"/>
				<xsl:text>#</xsl:text>
				<xsl:value-of select="//orm:Facts/orm:Fact[@id = $entity_name]/@_Name"/>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
		</xsl:for-each>
		
		<xsl:text>&#xa;</xsl:text>
		
		<!-- V-VAL(ValueType) -->
		<xsl:for-each select="//orm:Facts/orm:Fact">
			<xsl:for-each select="orm:FactRoles/orm:Role">
				<xsl:if test="orm:ValueRestriction">
					<xsl:variable name="entity_name" select="orm:RolePlayer/@ref"></xsl:variable>
					<xsl:if test="//orm:Objects/orm:ValueType[@id = $entity_name]">
						<xsl:text>V-VAL(</xsl:text>	
						<xsl:value-of select="$entity_name"/>
						<xsl:text>#</xsl:text>
						<xsl:value-of select="//orm:Objects/orm:ValueType[@id = $entity_name]/@Name"/>
						<xsl:text>) = {</xsl:text>
							<xsl:for-each select="orm:ValueRestriction/orm:RoleValueConstraint/orm:ValueRanges/orm:ValueRange">
								<xsl:if test="position() != 1">
									<xsl:text>, </xsl:text>
								</xsl:if>
								<xsl:choose>
									<xsl:when test="@MinInclusion='Open'">
										<xsl:text>(</xsl:text>
										<xsl:value-of select="@MinValue"/>
										<xsl:text>..</xsl:text>
									</xsl:when>
									<xsl:when test="@MinInclusion='Closed'">
										<xsl:text>[</xsl:text>
										<xsl:value-of select="@MinValue"/>
										<xsl:text>..</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@MinValue"/>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:choose>
									<xsl:when test="@MaxInclusion='Open'">
										<xsl:value-of select="@MaxValue"/>
										<xsl:text>)</xsl:text>
									</xsl:when>
									<xsl:when test="@MaxInclusion='Closed'">
										<xsl:value-of select="@MaxValue"/>
										<xsl:text>]</xsl:text>
									</xsl:when>
									<xsl:otherwise>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						<xsl:text>}</xsl:text>
						<xsl:text>&#xa;</xsl:text>
					</xsl:if>

					<xsl:if test="//orm:Objects/orm:EntityType[@id = $entity_name]">
						<xsl:text>Type(</xsl:text>
							<xsl:value-of select="@id"/>
							<xsl:text>#</xsl:text>
							<xsl:value-of select="../../@_Name"/>
							<xsl:text>.</xsl:text><xsl:value-of select="position()" />
							<xsl:text>, </xsl:text>
							<xsl:value-of select="orm:ValueRestriction/orm:RoleValueConstraint/@id"/>
							<xsl:text>#</xsl:text>
							<xsl:value-of select="//orm:Objects/orm:EntityType[@id=$entity_name]/@Name | //orm:Objects/orm:ValueType[@id=$entity_name]/@Name"/>
						<xsl:text>_RestrictedValue)</xsl:text>						
						<xsl:text>&#xa;</xsl:text>
						
						<xsl:text>V-VAL(</xsl:text>	
						<xsl:value-of select="orm:ValueRestriction/orm:RoleValueConstraint/@id"/>
						<xsl:text>#</xsl:text>
						<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name]/@Name"/>
						<xsl:text>_RestrictedValue) = {</xsl:text>
							<xsl:for-each select="orm:ValueRestriction/orm:RoleValueConstraint/orm:ValueRanges/orm:ValueRange">
								<xsl:if test="position() != 1">
									<xsl:text>, </xsl:text>
								</xsl:if>
								<xsl:choose>
									<xsl:when test="@MinInclusion='Open'">
										<xsl:text>(</xsl:text>
										<xsl:value-of select="@MinValue"/>
										<xsl:text>..</xsl:text>
									</xsl:when>
									<xsl:when test="@MinInclusion='Closed'">
										<xsl:text>[</xsl:text>
										<xsl:value-of select="@MinValue"/>
										<xsl:text>..</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@MinValue"/>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:choose>
									<xsl:when test="@MaxInclusion='Open'">
										<xsl:value-of select="@MaxValue"/>
										<xsl:text>)</xsl:text>
									</xsl:when>
									<xsl:when test="@MaxInclusion='Closed'">
										<xsl:value-of select="@MaxValue"/>
										<xsl:text>]</xsl:text>
									</xsl:when>
									<xsl:otherwise>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						<xsl:text>}</xsl:text>
						<xsl:text>&#xa;</xsl:text>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
		
		<xsl:text>&#xa;</xsl:text>
		
		<!-- R-SETsub (ATTRIBUTE,(I,J))-->		
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:SubsetConstraint">
			<xsl:text>R-SETsub({</xsl:text>
				<xsl:for-each select="orm:RoleSequences/orm:RoleSequence">
					<xsl:choose>
						<xsl:when test="position() = 1">
							<xsl:for-each select="orm:Role">
								<xsl:if test="position() != 1">
									<xsl:text>, </xsl:text>
								</xsl:if>
								<xsl:variable name="entity_name" select="@ref"></xsl:variable>
								<xsl:value-of select="@ref"/>
								<xsl:text>#</xsl:text>
								<xsl:for-each select="//orm:Facts/orm:Fact">
									<xsl:for-each select="orm:FactRoles/orm:Role">
										<xsl:if test="@id = $entity_name">
											<xsl:value-of select="../../@_Name"/>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="position()" />	
										</xsl:if>		
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
							<xsl:text>}, {</xsl:text>
							<xsl:choose>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
										<xsl:choose>
											<xsl:when test="position() = 1">
												<xsl:text>(</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:when>
											<xsl:when test="position() = last()">
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>)</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>), (</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</xsl:when>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										
										<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
										<xsl:variable name="entity_name3" select="position()"></xsl:variable>
										<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
										<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:SubsetConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
											<xsl:if test="position() = ($entity_name3 + 1)">
												<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
												<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
													<xsl:text>(</xsl:text>
													<xsl:value-of select="$entity_name"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>, </xsl:text>	
													<xsl:value-of select="$entity_name5"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name5">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>), </xsl:text>
												</xsl:if>
											</xsl:if>
										</xsl:for-each>

									</xsl:for-each>
									<xsl:text>()</xsl:text>
								</xsl:when>
								<xsl:otherwise>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:text>}, </xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>{</xsl:text>
							<xsl:for-each select="orm:Role">
								<xsl:if test="position() != 1">
									<xsl:text>, </xsl:text>
								</xsl:if>
								<xsl:variable name="entity_name" select="@ref"></xsl:variable>
								<xsl:value-of select="@ref"/>
								<xsl:text>#</xsl:text>
								<xsl:for-each select="//orm:Facts/orm:Fact">
									<xsl:for-each select="orm:FactRoles/orm:Role">
										<xsl:if test="@id = $entity_name">
											<xsl:value-of select="../../@_Name"/>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="position()" />	
										</xsl:if>		
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
							<xsl:text>}, {</xsl:text>
							<xsl:choose>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
										<xsl:choose>
											<xsl:when test="position() = 1">
												<xsl:text>(</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:when>
											<xsl:when test="position() = last()">
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>)</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>), (</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</xsl:when>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										
										<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
										<xsl:variable name="entity_name3" select="position()"></xsl:variable>
										<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
										<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:SubsetConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
											<xsl:if test="position() = ($entity_name3 + 1)">
												<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
												<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
													<xsl:text>(</xsl:text>
													<xsl:value-of select="$entity_name"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>, </xsl:text>	
													<xsl:value-of select="$entity_name5"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name5">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>), </xsl:text>
												</xsl:if>
											</xsl:if>
										</xsl:for-each>

									</xsl:for-each>
									<xsl:text>()</xsl:text>
								</xsl:when>
								<xsl:otherwise>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:text>}, </xsl:text>
						</xsl:otherwise>
					</xsl:choose>	
				</xsl:for-each>
				<xsl:text>{</xsl:text>
				<xsl:for-each select="orm:RoleSequences">
					<xsl:variable name="entity_name3" select="../@id"></xsl:variable>
					<xsl:for-each select="orm:RoleSequence[1]/orm:Role">
						<xsl:variable name="entity_name" select="@ref"></xsl:variable>
						<xsl:variable name="entity_name1" select="position()"></xsl:variable>
						
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>
						<xsl:text>(</xsl:text>
						
						<xsl:value-of select="$entity_name"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
						
						<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:SubsetConstraint[@id = $entity_name3]/orm:RoleSequences/orm:RoleSequence[2]/orm:Role">
							<xsl:if test="position() = $entity_name1">
								<xsl:text>, </xsl:text>
								<xsl:variable name="entity_name2" select="@ref"></xsl:variable>
								<xsl:value-of select="$entity_name2"/>
								<xsl:text>#</xsl:text>
								<xsl:for-each select="//orm:Facts/orm:Fact">
									<xsl:for-each select="orm:FactRoles/orm:Role">
										<xsl:if test="@id = $entity_name2">
											<xsl:value-of select="../../@_Name"/>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="position()" />	
										</xsl:if>		
									</xsl:for-each>
								</xsl:for-each>	
							</xsl:if>
						</xsl:for-each>
						<xsl:text>)</xsl:text>
					</xsl:for-each>
				</xsl:for-each>
				<xsl:text>}</xsl:text>	
			<xsl:text>)</xsl:text>
			<xsl:text>&#xa;</xsl:text>
		</xsl:for-each>
		
		<!-- Equality Constraints-->
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint">
			<xsl:text>R-SETsub({</xsl:text>
				<xsl:for-each select="orm:RoleSequences/orm:RoleSequence">
					<xsl:choose>
						<xsl:when test="position() = 1">
							<xsl:for-each select="orm:Role">
								<xsl:if test="position() != 1">
									<xsl:text>, </xsl:text>
								</xsl:if>
								<xsl:variable name="entity_name" select="@ref"></xsl:variable>
								<xsl:value-of select="@ref"/>
								<xsl:text>#</xsl:text>
								<xsl:for-each select="//orm:Facts/orm:Fact">
									<xsl:for-each select="orm:FactRoles/orm:Role">
										<xsl:if test="@id = $entity_name">
											<xsl:value-of select="../../@_Name"/>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="position()" />	
										</xsl:if>		
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
							<xsl:text>}, {</xsl:text>
							<xsl:choose>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
										<xsl:choose>
											<xsl:when test="position() = 1">
												<xsl:text>(</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:when>
											<xsl:when test="position() = last()">
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>)</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>), (</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</xsl:when>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>

										<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
										<xsl:variable name="entity_name3" select="position()"></xsl:variable>
										<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
										<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
											<xsl:if test="position() = ($entity_name3 + 1)">
												<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
												<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
													<xsl:text>(</xsl:text>
													<xsl:value-of select="$entity_name"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>, </xsl:text>	
													<xsl:value-of select="$entity_name5"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name5">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>), </xsl:text>
												</xsl:if>
											</xsl:if>
										</xsl:for-each>

									</xsl:for-each>
									<xsl:text>()</xsl:text>
								</xsl:when>
								<xsl:otherwise>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:text>}, </xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>{</xsl:text>
							<xsl:for-each select="orm:Role">
								<xsl:if test="position() != 1">
									<xsl:text>, </xsl:text>
								</xsl:if>
								<xsl:variable name="entity_name" select="@ref"></xsl:variable>
								<xsl:value-of select="@ref"/>
								<xsl:text>#</xsl:text>
								<xsl:for-each select="//orm:Facts/orm:Fact">
									<xsl:for-each select="orm:FactRoles/orm:Role">
										<xsl:if test="@id = $entity_name">
											<xsl:value-of select="../../@_Name"/>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="position()" />	
										</xsl:if>		
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
							<xsl:text>}, {</xsl:text>
							<xsl:choose>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
										<xsl:choose>
											<xsl:when test="position() = 1">
												<xsl:text>(</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:when>
											<xsl:when test="position() = last()">
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>)</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>), (</xsl:text>
												<xsl:value-of select="@ref"/>
												<xsl:text>#</xsl:text>
												<xsl:variable name="entity_name" select="@ref"></xsl:variable>
												<xsl:for-each select="//orm:Facts/orm:Fact">
													<xsl:for-each select="orm:FactRoles/orm:Role">
														<xsl:if test="@id = $entity_name">
															<xsl:value-of select="../../@_Name"/>
															<xsl:text>.</xsl:text>
															<xsl:value-of select="position()" />	
														</xsl:if>		
													</xsl:for-each>
												</xsl:for-each>
												<xsl:text>, </xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</xsl:when>
								<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
									<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>

										<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
										<xsl:variable name="entity_name3" select="position()"></xsl:variable>
										<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
										<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
											<xsl:if test="position() = ($entity_name3 + 1)">
												<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
												<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
													<xsl:text>(</xsl:text>
													<xsl:value-of select="$entity_name"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>, </xsl:text>	
													<xsl:value-of select="$entity_name5"/>
													<xsl:text>#</xsl:text>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name5">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>	
													<xsl:text>), </xsl:text>
												</xsl:if>
											</xsl:if>
										</xsl:for-each>

									</xsl:for-each>
									<xsl:text>()</xsl:text>
								</xsl:when>
								<xsl:otherwise>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:text>}, </xsl:text>
						</xsl:otherwise>
					</xsl:choose>	
				</xsl:for-each>
				<xsl:text>{</xsl:text>
				<xsl:for-each select="orm:RoleSequences">
					<xsl:variable name="entity_name3" select="../@id"></xsl:variable>
					<xsl:for-each select="orm:RoleSequence[1]/orm:Role">
						<xsl:variable name="entity_name" select="@ref"></xsl:variable>
						<xsl:variable name="entity_name1" select="position()"></xsl:variable>
						
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>
						<xsl:text>(</xsl:text>
						
						<xsl:value-of select="$entity_name"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
						
						<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint[@id = $entity_name3]/orm:RoleSequences/orm:RoleSequence[2]/orm:Role">
							<xsl:if test="position() = $entity_name1">
								<xsl:text>, </xsl:text>
								<xsl:variable name="entity_name2" select="@ref"></xsl:variable>
								<xsl:value-of select="$entity_name2"/>
								<xsl:text>#</xsl:text>
								<xsl:for-each select="//orm:Facts/orm:Fact">
									<xsl:for-each select="orm:FactRoles/orm:Role">
										<xsl:if test="@id = $entity_name2">
											<xsl:value-of select="../../@_Name"/>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="position()" />	
										</xsl:if>		
									</xsl:for-each>
								</xsl:for-each>	
							</xsl:if>
						</xsl:for-each>
						<xsl:text>)</xsl:text>
					</xsl:for-each>
				</xsl:for-each>
				<xsl:text>}</xsl:text>	
			<xsl:text>)</xsl:text>
			<xsl:text>&#xa;</xsl:text>
		</xsl:for-each>
		
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint">
			<xsl:text>R-SETsub({</xsl:text>
				<xsl:for-each select="orm:RoleSequences/orm:RoleSequence[2]">
					<xsl:for-each select="orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>
						<xsl:variable name="entity_name" select="@ref"></xsl:variable>
						<xsl:value-of select="@ref"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:text>}, {</xsl:text>
					<xsl:choose>
						<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
							<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
								<xsl:choose>
									<xsl:when test="position() = 1">
										<xsl:text>(</xsl:text>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>, </xsl:text>
									</xsl:when>
									<xsl:when test="position() = last()">
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>)</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>), (</xsl:text>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>, </xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:when>
						<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
							<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
								<xsl:variable name="entity_name" select="@ref"></xsl:variable>

								<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
								<xsl:variable name="entity_name3" select="position()"></xsl:variable>
								<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
								<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
									<xsl:if test="position() = ($entity_name3 + 1)">
										<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
										<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
											<xsl:text>(</xsl:text>
											<xsl:value-of select="$entity_name"/>
											<xsl:text>#</xsl:text>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>	
											<xsl:text>, </xsl:text>	
											<xsl:value-of select="$entity_name5"/>
											<xsl:text>#</xsl:text>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name5">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>	
											<xsl:text>), </xsl:text>
										</xsl:if>
									</xsl:if>
								</xsl:for-each>

							</xsl:for-each>
							<xsl:text>()</xsl:text>
						</xsl:when>
						<xsl:otherwise>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:text>}, </xsl:text>
				</xsl:for-each>	
				<xsl:for-each select="orm:RoleSequences/orm:RoleSequence[1]">			
					<xsl:text>{</xsl:text>
					<xsl:for-each select="orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>
						<xsl:variable name="entity_name" select="@ref"></xsl:variable>
						<xsl:value-of select="@ref"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:text>}, {</xsl:text>
					<xsl:choose>
						<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
							<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
								<xsl:choose>
									<xsl:when test="position() = 1">
										<xsl:text>(</xsl:text>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>, </xsl:text>
									</xsl:when>
									<xsl:when test="position() = last()">
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>)</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>), (</xsl:text>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>, </xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:when>
						<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
							<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
								<xsl:variable name="entity_name" select="@ref"></xsl:variable>
								
								<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
								<xsl:variable name="entity_name3" select="position()"></xsl:variable>
								<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
								<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
									<xsl:if test="position() = ($entity_name3 + 1)">
										<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
										<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
											<xsl:text>(</xsl:text>
											<xsl:value-of select="$entity_name"/>
											<xsl:text>#</xsl:text>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>	
											<xsl:text>, </xsl:text>	
											<xsl:value-of select="$entity_name5"/>
											<xsl:text>#</xsl:text>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name5">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>	
											<xsl:text>), </xsl:text>
										</xsl:if>
									</xsl:if>
								</xsl:for-each>

							</xsl:for-each>
							<xsl:text>()</xsl:text>
						</xsl:when>
						<xsl:otherwise>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:text>}, </xsl:text>
				</xsl:for-each>
				<xsl:text>{</xsl:text>
				<xsl:for-each select="orm:RoleSequences">
					<xsl:variable name="entity_name3" select="../@id"></xsl:variable>
					<xsl:for-each select="orm:RoleSequence[2]/orm:Role">
						<xsl:variable name="entity_name" select="@ref"></xsl:variable>
						<xsl:variable name="entity_name1" select="position()"></xsl:variable>
						
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>
						<xsl:text>(</xsl:text>
						
						<xsl:value-of select="$entity_name"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
						
						<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:EqualityConstraint[@id = $entity_name3]/orm:RoleSequences/orm:RoleSequence[1]/orm:Role">
							<xsl:if test="position() = $entity_name1">
								<xsl:text>, </xsl:text>
								<xsl:variable name="entity_name2" select="@ref"></xsl:variable>
								<xsl:value-of select="$entity_name2"/>
								<xsl:text>#</xsl:text>
								<xsl:for-each select="//orm:Facts/orm:Fact">
									<xsl:for-each select="orm:FactRoles/orm:Role">
										<xsl:if test="@id = $entity_name2">
											<xsl:value-of select="../../@_Name"/>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="position()" />	
										</xsl:if>		
									</xsl:for-each>
								</xsl:for-each>	
							</xsl:if>
						</xsl:for-each>
						<xsl:text>)</xsl:text>
					</xsl:for-each>
				</xsl:for-each>
				<xsl:text>}</xsl:text>	
			<xsl:text>)</xsl:text>
			<xsl:text>&#xa;</xsl:text>
		</xsl:for-each>
		
		<!-- Exclusion Constraints-->
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:ExclusionConstraint">
			<xsl:if test="not(orm:ExclusiveOrMandatoryConstraint/@ref) and (orm:RoleSequences/orm:RoleSequence/JoinRule)">
				<xsl:text>R-SETex({</xsl:text>
					<xsl:for-each select="orm:RoleSequences/orm:RoleSequence">
						<xsl:choose>
							<xsl:when test="position() = 1">
								<xsl:for-each select="orm:Role">
									<xsl:if test="position() != 1">
										<xsl:text>, </xsl:text>
									</xsl:if>
									<xsl:variable name="entity_name" select="@ref"></xsl:variable>
									<xsl:value-of select="@ref"/>
									<xsl:text>#</xsl:text>
									<xsl:for-each select="//orm:Facts/orm:Fact">
										<xsl:for-each select="orm:FactRoles/orm:Role">
											<xsl:if test="@id = $entity_name">
												<xsl:value-of select="../../@_Name"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="position()" />	
											</xsl:if>		
										</xsl:for-each>
									</xsl:for-each>
								</xsl:for-each>
								<xsl:text>}, {</xsl:text>
								<xsl:choose>
									<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
										<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
											<xsl:choose>
												<xsl:when test="position() = 1">
													<xsl:text>(</xsl:text>
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>, </xsl:text>
												</xsl:when>
												<xsl:when test="position() = last()">
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>)</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>), (</xsl:text>
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>, </xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:for-each>
									</xsl:when>
									<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
										<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
											<xsl:variable name="entity_name" select="@ref"></xsl:variable>

											<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
											<xsl:variable name="entity_name3" select="position()"></xsl:variable>
											<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
											<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:ExclusionConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
												<xsl:if test="position() = ($entity_name3 + 1)">
													<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
													<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
														<xsl:text>(</xsl:text>
														<xsl:value-of select="$entity_name"/>
														<xsl:text>#</xsl:text>
														<xsl:for-each select="//orm:Facts/orm:Fact">
															<xsl:for-each select="orm:FactRoles/orm:Role">
																<xsl:if test="@id = $entity_name">
																	<xsl:value-of select="../../@_Name"/>
																	<xsl:text>.</xsl:text>
																	<xsl:value-of select="position()" />	
																</xsl:if>		
															</xsl:for-each>
														</xsl:for-each>	
														<xsl:text>, </xsl:text>	
														<xsl:value-of select="$entity_name5"/>
														<xsl:text>#</xsl:text>
														<xsl:for-each select="//orm:Facts/orm:Fact">
															<xsl:for-each select="orm:FactRoles/orm:Role">
																<xsl:if test="@id = $entity_name5">
																	<xsl:value-of select="../../@_Name"/>
																	<xsl:text>.</xsl:text>
																	<xsl:value-of select="position()" />	
																</xsl:if>		
															</xsl:for-each>
														</xsl:for-each>	
														<xsl:text>), </xsl:text>
													</xsl:if>
												</xsl:if>
											</xsl:for-each>

										</xsl:for-each>
										<xsl:text>()</xsl:text>
									</xsl:when>
									<xsl:otherwise>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:text>}, </xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>{</xsl:text>
								<xsl:for-each select="orm:Role">
									<xsl:if test="position() != 1">
										<xsl:text>, </xsl:text>
									</xsl:if>
									<xsl:variable name="entity_name" select="@ref"></xsl:variable>
									<xsl:value-of select="@ref"/>
									<xsl:text>#</xsl:text>
									<xsl:for-each select="//orm:Facts/orm:Fact">
										<xsl:for-each select="orm:FactRoles/orm:Role">
											<xsl:if test="@id = $entity_name">
												<xsl:value-of select="../../@_Name"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="position()" />	
											</xsl:if>		
										</xsl:for-each>
									</xsl:for-each>
								</xsl:for-each>
								<xsl:text>}, {</xsl:text>
								<xsl:choose>
									<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
										<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
											<xsl:choose>
												<xsl:when test="position() = 1">
													<xsl:text>(</xsl:text>
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>, </xsl:text>
												</xsl:when>
												<xsl:when test="position() = last()">
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>)</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>), (</xsl:text>
													<xsl:value-of select="@ref"/>
													<xsl:text>#</xsl:text>
													<xsl:variable name="entity_name" select="@ref"></xsl:variable>
													<xsl:for-each select="//orm:Facts/orm:Fact">
														<xsl:for-each select="orm:FactRoles/orm:Role">
															<xsl:if test="@id = $entity_name">
																<xsl:value-of select="../../@_Name"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="position()" />	
															</xsl:if>		
														</xsl:for-each>
													</xsl:for-each>
													<xsl:text>, </xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:for-each>
									</xsl:when>
									<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
										<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
											<xsl:variable name="entity_name" select="@ref"></xsl:variable>

											<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
											<xsl:variable name="entity_name3" select="position()"></xsl:variable>
											<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
											<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:ExclusionConstraint/orm:RoleSequences/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
												<xsl:if test="position() = ($entity_name3 + 1)">
													<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
													<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
														<xsl:text>(</xsl:text>
														<xsl:value-of select="$entity_name"/>
														<xsl:text>#</xsl:text>
														<xsl:for-each select="//orm:Facts/orm:Fact">
															<xsl:for-each select="orm:FactRoles/orm:Role">
																<xsl:if test="@id = $entity_name">
																	<xsl:value-of select="../../@_Name"/>
																	<xsl:text>.</xsl:text>
																	<xsl:value-of select="position()" />	
																</xsl:if>		
															</xsl:for-each>
														</xsl:for-each>	
														<xsl:text>, </xsl:text>	
														<xsl:value-of select="$entity_name5"/>
														<xsl:text>#</xsl:text>
														<xsl:for-each select="//orm:Facts/orm:Fact">
															<xsl:for-each select="orm:FactRoles/orm:Role">
																<xsl:if test="@id = $entity_name5">
																	<xsl:value-of select="../../@_Name"/>
																	<xsl:text>.</xsl:text>
																	<xsl:value-of select="position()" />	
																</xsl:if>		
															</xsl:for-each>
														</xsl:for-each>	
														<xsl:text>), </xsl:text>
													</xsl:if>
												</xsl:if>
											</xsl:for-each>

										</xsl:for-each>
										<xsl:text>()</xsl:text>
									</xsl:when>
									<xsl:otherwise>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:text>}, </xsl:text>
							</xsl:otherwise>
						</xsl:choose>	
					</xsl:for-each>
					<xsl:text>{</xsl:text>
					<xsl:for-each select="orm:RoleSequences">
						<xsl:variable name="entity_name3" select="../@id"></xsl:variable>
						<xsl:for-each select="orm:RoleSequence[1]/orm:Role">
							<xsl:variable name="entity_name" select="@ref"></xsl:variable>
							<xsl:variable name="entity_name1" select="position()"></xsl:variable>
							
							<xsl:if test="position() != 1">
								<xsl:text>, </xsl:text>
							</xsl:if>
							<xsl:text>(</xsl:text>
							
							<xsl:value-of select="$entity_name"/>
							<xsl:text>#</xsl:text>
							<xsl:for-each select="//orm:Facts/orm:Fact">
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<xsl:if test="@id = $entity_name">
										<xsl:value-of select="../../@_Name"/>
										<xsl:text>.</xsl:text>
										<xsl:value-of select="position()" />	
									</xsl:if>		
								</xsl:for-each>
							</xsl:for-each>	
							
							<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:ExclusionConstraint[@id = $entity_name3]/orm:RoleSequences/orm:RoleSequence[2]/orm:Role">
								<xsl:if test="position() = $entity_name1">
									<xsl:text>, </xsl:text>
									<xsl:variable name="entity_name2" select="@ref"></xsl:variable>
									<xsl:value-of select="$entity_name2"/>
									<xsl:text>#</xsl:text>
									<xsl:for-each select="//orm:Facts/orm:Fact">
										<xsl:for-each select="orm:FactRoles/orm:Role">
											<xsl:if test="@id = $entity_name2">
												<xsl:value-of select="../../@_Name"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="position()" />	
											</xsl:if>		
										</xsl:for-each>
									</xsl:for-each>	
								</xsl:if>
							</xsl:for-each>
							<xsl:text>)</xsl:text>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:text>}</xsl:text>	
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="orm:ExclusiveOrMandatoryConstraint/@ref">
				<xsl:variable name="entity_name22" select="orm:RoleSequences/orm:RoleSequence[1]/orm:Role/@ref"></xsl:variable>
				<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role/@id = $entity_name22">
					<xsl:for-each select="orm:RoleSequences/orm:RoleSequence">
						<xsl:variable name="entity_name2" select="orm:Role/@ref"></xsl:variable>
						<xsl:variable name="entity_name3" select="position()"></xsl:variable>
						<xsl:for-each select="//orm:ExclusionConstraint">
							<xsl:if test="orm:ExclusiveOrMandatoryConstraint/@ref">
								<xsl:for-each select="orm:RoleSequences/orm:RoleSequence">
									<xsl:if test="position() > $entity_name3">
										<xsl:text>R-SETex({</xsl:text>
										<xsl:value-of select="$entity_name2"/>
										<xsl:text>#</xsl:text>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name2">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>	
										<xsl:text>}, {}, {</xsl:text>
										<xsl:variable name="entity_name4" select="orm:Role/@ref"></xsl:variable>
										<xsl:value-of select="$entity_name4"/>
										<xsl:text>#</xsl:text>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name4">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>	
										<xsl:text>}, {}, {(</xsl:text>
										<xsl:value-of select="$entity_name2"/>
										<xsl:text>#</xsl:text>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name2">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>, </xsl:text>
										<xsl:variable name="entity_name4" select="orm:Role/@ref"></xsl:variable>
										<xsl:value-of select="$entity_name4"/>
										<xsl:text>#</xsl:text>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name4">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>	
										<xsl:text>)})&#xa;</xsl:text>
									</xsl:if>
								</xsl:for-each>		
							</xsl:if>
						</xsl:for-each>
					</xsl:for-each>
				</xsl:if>	
			</xsl:if>
		</xsl:for-each>
		
		<!-- SubType Relation (isa, ex, tot) -->
		<!-- O-SETtot({EntityType},EntityType) -->
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:MandatoryConstraint">
			<xsl:if test="not(@IsSimple='true') and not(@IsImplied = 'true')">
				<xsl:variable name="entity_name11" select="orm:RoleSequence/orm:Role[1]/@ref"></xsl:variable>
				<xsl:if test="//orm:Facts/orm:SubtypeFact/orm:FactRoles/orm:SupertypeMetaRole/@id = $entity_name11">	
					<xsl:text>O-SETtot({</xsl:text>		
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:variable name="entity_name" select="@ref"></xsl:variable>
							<xsl:variable name="entity_name2" select="position()"></xsl:variable>
							<xsl:for-each select="//orm:Facts/orm:SubtypeFact">
								<xsl:if test="orm:FactRoles/orm:SupertypeMetaRole/@id = $entity_name">
									<xsl:if test="$entity_name2 != 1">
										<xsl:text>, </xsl:text>
									</xsl:if>	
									<xsl:variable name="entity_name1" select="orm:FactRoles/orm:SubtypeMetaRole/orm:RolePlayer/@ref"></xsl:variable>
									<xsl:value-of select="$entity_name1" />	
									<xsl:text>#</xsl:text>
									<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name1]/@Name | //orm:Objects/orm:ObjectifiedType[@id = $entity_name1]/@Name" />
								</xsl:if>
							</xsl:for-each>
						</xsl:for-each>
					<xsl:text>}, </xsl:text>
						<xsl:variable name="entity_name" select="orm:RoleSequence/orm:Role[1]/@ref"></xsl:variable>					
						<xsl:variable name="entity_name1" select="//orm:Facts/orm:SubtypeFact/orm:FactRoles/orm:SupertypeMetaRole[@id = $entity_name]/orm:RolePlayer/@ref"></xsl:variable>
						<xsl:value-of select="$entity_name1" />	
						<xsl:text>#</xsl:text>
						<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name1]/@Name | //orm:Objects/orm:ObjectifiedType[@id = $entity_name1]/@Name" />
					<xsl:text>)</xsl:text>
					<xsl:text>&#xa;</xsl:text>	
				</xsl:if>	
			</xsl:if>
		</xsl:for-each>
		
		<!-- O-SETisa({EntityType},EntityType) -->
		<!-- For Entity -->
		<xsl:for-each select="//orm:Objects/orm:EntityType">
			<xsl:variable name="entity_name1" select="@id"></xsl:variable>
			<xsl:if test="orm:PlayedRoles/orm:SupertypeMetaRole">
				<xsl:text>O-SETisa({</xsl:text>
				<xsl:for-each select="orm:PlayedRoles/orm:SupertypeMetaRole">
					<xsl:variable name="entity_name2" select="@ref"></xsl:variable>
					<xsl:for-each select="//orm:Facts/orm:SubtypeFact">
						<xsl:if test="orm:FactRoles/orm:SupertypeMetaRole/@id = $entity_name2">	
							<xsl:variable name="entity_name3" select="orm:FactRoles/orm:SubtypeMetaRole/orm:RolePlayer/@ref"></xsl:variable>
							<xsl:value-of select="$entity_name3" />	
							<xsl:text>#</xsl:text>
							<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name3]/@Name" />
						</xsl:if>
					</xsl:for-each>
					<xsl:if test="position() != last()">
						<xsl:text>, </xsl:text>
					</xsl:if>
				</xsl:for-each>	
				<xsl:text>}, </xsl:text>
				<xsl:value-of select="$entity_name1" />	
				<xsl:text>#</xsl:text>
				<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name1]/@Name" />
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>	
			</xsl:if>
		</xsl:for-each>
		
		<!-- For ObjectifiedType -->
		<xsl:for-each select="//orm:Objects/orm:ObjectifiedType">
			<xsl:variable name="entity_name1" select="@id"></xsl:variable>
			<xsl:if test="orm:PlayedRoles/orm:SupertypeMetaRole">
				<xsl:text>O-SETisa({</xsl:text>
				<xsl:for-each select="orm:PlayedRoles/orm:SupertypeMetaRole">
					<xsl:variable name="entity_name2" select="@ref"></xsl:variable>
					<xsl:for-each select="//orm:Facts/orm:SubtypeFact">
						<xsl:if test="orm:FactRoles/orm:SupertypeMetaRole/@id = $entity_name2">	
							<xsl:variable name="entity_name3" select="orm:FactRoles/orm:SubtypeMetaRole/orm:RolePlayer/@ref"></xsl:variable>
							<xsl:value-of select="$entity_name3" />	
							<xsl:text>#</xsl:text>
							<xsl:value-of select="//orm:Objects/orm:ObjectifiedType[@id = $entity_name3]/@Name" />
						</xsl:if>
					</xsl:for-each>
					<xsl:if test="position() != last()">
						<xsl:text>, </xsl:text>
					</xsl:if>
				</xsl:for-each>	
				<xsl:text>}, </xsl:text>
				<xsl:value-of select="$entity_name1" />	
				<xsl:text>#</xsl:text>
				<xsl:value-of select="//orm:Objects/orm:ObjectifiedType[@id = $entity_name1]/@Name" />
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>	
			</xsl:if>
		</xsl:for-each>
		
		<!-- O-SETex({EntityType},EntityType) -->
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:ExclusionConstraint">
			<xsl:if test="not(orm:RoleSequences/orm:RoleSequence/JoinRule)">
				<xsl:variable name="entity_name11" select="orm:RoleSequences/orm:RoleSequence[1]/orm:Role/@ref"></xsl:variable>
				<xsl:if test="//orm:Facts/orm:SubtypeFact/orm:FactRoles/orm:SupertypeMetaRole/@id = $entity_name11">
					<xsl:text>O-SETex({</xsl:text>		
						<xsl:for-each select="orm:RoleSequences/orm:RoleSequence">
							<xsl:variable name="entity_name" select="orm:Role/@ref"></xsl:variable>
							<xsl:variable name="entity_name2" select="position()"></xsl:variable>
							<xsl:for-each select="//orm:Facts/orm:SubtypeFact">
								<xsl:if test="orm:FactRoles/orm:SupertypeMetaRole/@id = $entity_name">
									<xsl:if test="$entity_name2 != 1">
										<xsl:text>, </xsl:text>
									</xsl:if>	
									<xsl:variable name="entity_name1" select="orm:FactRoles/orm:SubtypeMetaRole/orm:RolePlayer/@ref"></xsl:variable>
									<xsl:value-of select="$entity_name1" />	
									<xsl:text>#</xsl:text>
									<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name1]/@Name | //orm:Objects/orm:ObjectifiedType[@id = $entity_name1]/@Name" />
								</xsl:if>
							</xsl:for-each>
						</xsl:for-each>
					<xsl:text>}, </xsl:text>
						<xsl:variable name="entity_name" select="orm:RoleSequences/orm:RoleSequence[1]/orm:Role[1]/@ref"></xsl:variable>					
						<xsl:variable name="entity_name1" select="//orm:Facts/orm:SubtypeFact/orm:FactRoles/orm:SupertypeMetaRole[@id = $entity_name]/orm:RolePlayer/@ref"></xsl:variable>
						<xsl:value-of select="$entity_name1" />	
						<xsl:text>#</xsl:text>
						<xsl:value-of select="//orm:Objects/orm:EntityType[@id = $entity_name1]/@Name | //orm:Objects/orm:ObjectifiedType[@id = $entity_name1]/@Name" />
					<xsl:text>)</xsl:text>
					<xsl:text>&#xa;</xsl:text>	
				</xsl:if>	
			</xsl:if>
		</xsl:for-each>
		
		<!-- Frequency Constraints-->		
		<!-- FREQ({ATTRIBUTE*},{JOINRELATION}, (min,max))-->		
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:FrequencyConstraint">
			<xsl:text>FREQ({</xsl:text>
				<xsl:for-each select="orm:RoleSequence">					
					<xsl:for-each select="orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>
						<xsl:variable name="entity_name" select="@ref"></xsl:variable>
						<xsl:value-of select="@ref"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:text>}, {</xsl:text>
					<xsl:choose>
						<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
							<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:SubPaths/orm:SubPath/orm:PathedRoles/orm:PathedRole[@Purpose='PostInnerJoin']">
								<xsl:choose>
									<xsl:when test="position() = 1">
										<xsl:text>(</xsl:text>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>, </xsl:text>
									</xsl:when>
									<xsl:when test="position() = last()">
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>)</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>), (</xsl:text>
										<xsl:value-of select="@ref"/>
										<xsl:text>#</xsl:text>
										<xsl:variable name="entity_name" select="@ref"></xsl:variable>
										<xsl:for-each select="//orm:Facts/orm:Fact">
											<xsl:for-each select="orm:FactRoles/orm:Role">
												<xsl:if test="@id = $entity_name">
													<xsl:value-of select="../../@_Name"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="position()" />	
												</xsl:if>		
											</xsl:for-each>
										</xsl:for-each>
										<xsl:text>, </xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:when>
						<xsl:when test="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
							<xsl:for-each select="orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath/orm:PathedRoles/orm:PathedRole">
								<xsl:variable name="entity_name" select="@ref"></xsl:variable>
								
								<xsl:variable name="entity_name2" select="../../@id"></xsl:variable>
								<xsl:variable name="entity_name3" select="position()"></xsl:variable>
								<xsl:variable name="entity_name4" select="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name]/../../@_Name"></xsl:variable>
								<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:FrequencyConstraint/orm:RoleSequence/orm:JoinRule/orm:JoinPath/orm:PathComponents/orm:RolePath[@id = $entity_name2]/orm:PathedRoles/orm:PathedRole">
									<xsl:if test="position() = ($entity_name3 + 1)">
										<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
										<xsl:if test="//orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id = $entity_name5]/../../@_Name != $entity_name4">
											<xsl:text>(</xsl:text>
											<xsl:value-of select="$entity_name"/>
											<xsl:text>#</xsl:text>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>	
											<xsl:text>, </xsl:text>	
											<xsl:value-of select="$entity_name5"/>
											<xsl:text>#</xsl:text>
											<xsl:for-each select="//orm:Facts/orm:Fact">
												<xsl:for-each select="orm:FactRoles/orm:Role">
													<xsl:if test="@id = $entity_name5">
														<xsl:value-of select="../../@_Name"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="position()" />	
													</xsl:if>		
												</xsl:for-each>
											</xsl:for-each>	
											<xsl:text>), </xsl:text>
										</xsl:if>
									</xsl:if>
								</xsl:for-each>

							</xsl:for-each>
							<xsl:text>()</xsl:text>
						</xsl:when>
						<xsl:otherwise>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:text>}, (</xsl:text>
				</xsl:for-each>
				<xsl:value-of select="@MinFrequency"/>
				<xsl:text>, </xsl:text>
				<xsl:value-of select="@MaxFrequency"/>
			<xsl:text>))</xsl:text>
			<xsl:text>&#xa;</xsl:text>
		</xsl:for-each>
		
		<!-- OBJECT CARDINALITY -->
		<xsl:for-each select="//orm:Objects/orm:EntityType">
			<xsl:if test="starts-with(orm:Notes/orm:Note/orm:Text, '#=') or starts-with(orm:Notes/orm:Note/orm:Text, '#&lt;=') or starts-with(orm:Notes/orm:Note/orm:Text, '#&gt;=')">
				<xsl:text>O-CARD(</xsl:text>
				<xsl:value-of select="@id"/>
				<xsl:text>#</xsl:text>
				<xsl:value-of select="@Name"/>
				<xsl:text>) = (</xsl:text>
					<xsl:choose>
					  <xsl:when test="starts-with(orm:Notes/orm:Note/orm:Text, '#=')">
						<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#=')"/>
						<xsl:text>,</xsl:text>
						<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#=')"/>
					  </xsl:when>
					  <xsl:when test="starts-with(orm:Notes/orm:Note/orm:Text, '#&lt;=')">
						<xsl:text>1,</xsl:text>
						<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#&lt;=')"/>
					  </xsl:when>
					  <xsl:when test="starts-with(orm:Notes/orm:Note/orm:Text, '#&gt;=')">
						<xsl:text>1,</xsl:text>
						<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#&gt;=')"/>
					  </xsl:when>
					  <xsl:otherwise>
					  </xsl:otherwise>
					</xsl:choose> 
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
		</xsl:for-each>
		<xsl:for-each select="//orm:Objects/orm:ObjectifiedType">
			<xsl:if test="not(orm:NestedPredicate/@IsImplied)">
				<xsl:if test="starts-with(orm:Notes/orm:Note/orm:Text, '#=') or starts-with(orm:Notes/orm:Note/orm:Text, '#&lt;=') or starts-with(orm:Notes/orm:Note/orm:Text, '#&gt;=')">
					<xsl:text>O-CARD(</xsl:text>
					<xsl:value-of select="@id"/>
					<xsl:text>#</xsl:text>
					<xsl:value-of select="@Name"/>
					<xsl:text>) = (</xsl:text>
						<xsl:choose>
						  <xsl:when test="starts-with(orm:Notes/orm:Note/orm:Text, '#=')">
							<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#=')"/>
							<xsl:text>,</xsl:text>
							<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#=')"/>
						  </xsl:when>
						  <xsl:when test="starts-with(orm:Notes/orm:Note/orm:Text, '#&lt;=')">
							<xsl:text>1,</xsl:text>
							<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#&lt;=')"/>
						  </xsl:when>
						  <xsl:when test="starts-with(orm:Notes/orm:Note/orm:Text, '#&gt;=')">
							<xsl:text>1,</xsl:text>
							<xsl:value-of select="substring-after(orm:Notes/orm:Note/orm:Text,'#&gt;=')"/>
						  </xsl:when>
						  <xsl:otherwise>
						  </xsl:otherwise>
						</xsl:choose> 
					<xsl:text>)</xsl:text>
					<xsl:text>&#xa;</xsl:text>
				</xsl:if>
			</xsl:if>
		</xsl:for-each>
		
		<!-- RING -->
		<xsl:for-each select="//orm:ORMModel/orm:Constraints/orm:RingConstraint">
			<xsl:if test="contains(@Type,'Reflexive') and not(contains(@Type,'PurelyReflexive'))">
				<xsl:text>RINGref(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'PurelyReflexive')">
				<xsl:text>RINGref(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
				<xsl:text>RINGsym(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'Irreflexive')">
				<xsl:text>RINGirr(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'Antisymmetric')">
				<xsl:text>RINGantisym(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'Asymmetric')">
				<xsl:text>RINGasym(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'Intransitive') and not(contains(@Type,'StronglyIntransitive'))">
				<xsl:text>RINGintr(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'StronglyIntransitive')">
				<xsl:text>RINGintr(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
				<xsl:text>RINGirr(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'Acyclic')">
				<xsl:text>RINGacyclic(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'Symmetric')">
				<xsl:text>RINGsym(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
			<xsl:if test="contains(@Type,'Transitive')">
				<xsl:text>RINGtrans(</xsl:text>
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:if test="position() != 1">
							<xsl:text>, </xsl:text>
						</xsl:if>		
						<xsl:variable name="entity_name5" select="@ref"></xsl:variable>
						<xsl:value-of select="$entity_name5"/>
						<xsl:text>#</xsl:text>
						<xsl:for-each select="//orm:Facts/orm:Fact">
							<xsl:for-each select="orm:FactRoles/orm:Role">
								<xsl:if test="@id = $entity_name5">
									<xsl:value-of select="../../@_Name"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="position()" />	
								</xsl:if>		
							</xsl:for-each>
						</xsl:for-each>	
					</xsl:for-each>
				<xsl:text>)</xsl:text>
				<xsl:text>&#xa;</xsl:text>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>