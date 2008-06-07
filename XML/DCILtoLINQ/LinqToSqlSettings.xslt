<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	exclude-result-prefixes="xsl">
	<xsl:output method="xml" indent="yes"/>
	<xsl:template match="/">
		<LinqToSqlSettings xmlns="http://schemas.neumont.edu/ORM/2008-04/LinqToSql/Settings">
			<xsl:comment>These settings affect LinqToSql generators, but modifications here
	have no effect until the .ORM file is regenerated. To force regenerationt, right
	click the	parent .ORM file in the Solution Explorer and choose 'Run Custom Tool'.
	The schema file at the end this settings file is designed to help you customize
	your settings. For any of the settings areas, click immediately after the element
	name and click space to see a list of individual attributes.</xsl:comment>
			<xsl:comment>Change connection string properties here</xsl:comment>
			<ConnectionString DataSource="." SettingsProperty="" DatabaseName=""/>
			<xsl:comment>Change basic name generation settings</xsl:comment>
			<NameParts/>
			<xsl:comment>Change WCF name generation settings</xsl:comment>
			<ServiceLayer/>
			<xsl:comment>Inline settings schema, do not modify</xsl:comment>
			<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
				targetNamespace="http://schemas.neumont.edu/ORM/2008-04/LinqToSql/Settings"
				elementFormDefault="qualified">
				<xs:element name="LinqToSqlSettings">
					<xs:complexType>
						<xs:all>
							<xs:element name="ConnectionString" minOccurs="0">
								<xs:annotation>
									<xs:documentation>Properties used to generate connection string for a DBML file.</xs:documentation>
								</xs:annotation>
								<xs:complexType>
									<xs:attribute name="DataSource" type="xs:string" use="required">
										<xs:annotation>
											<xs:documentation>The location of the database, generally a server name.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="SettingsProperty" type="xs:string" use="required">
										<xs:annotation>
											<xs:documentation>The name of the property in the PROJECTNAME.Properties.Settings file that provides the connection string. The default is {@DatabaseName}ConnectionString</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="DatabaseName" type="xs:string" use="required">
										<xs:annotation>
											<xs:documentation>The name of a database. The schema name is used if this is not set</xs:documentation>
										</xs:annotation>
									</xs:attribute>
								</xs:complexType>
							</xs:element>
							<xs:element name="NameParts" minOccurs="0">
								<xs:complexType>
									<xs:attribute name="DataContextClassSuffix" type="NamePartType" default="DataContext">
										<xs:annotation>
											<xs:documentation>The text appended to the name of the generated class that is derived from System.Data.Linq.DataContext</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="DataContextTableSuffix" type="NamePartType" default="Table">
										<xs:annotation>
											<xs:documentation>The text appended to the names of typed table getters in a DataContext class.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="CollectionSuffix" type="NamePartType" default="Collection">
										<xs:annotation>
											<xs:documentation>The text appended to the names of typed EntitySet properties.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="AssociationReferenceSuffix" type="NamePartType" default="Reference">
										<xs:annotation>
											<xs:documentation>The text appended to the names of typed properties used for association navigation (the source or target of a foreign key).</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="PrivateFieldPrefix" type="NamePartType" default="_">
										<xs:annotation>
											<xs:documentation>The text prepended to private field names.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
								</xs:complexType>
							</xs:element>
							<xs:element name="ServiceLayer" minOccurs="0">
								<xs:complexType>
									<xs:attribute name="Generate" type="xs:boolean" default="false">
										<xs:annotation>
											<xs:documentation>Determines whether or not the generator should include WCF DataContract attributes and ServiceContract calls.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="CreateKeyword" type="NamePartType" default="Insert">
										<xs:annotation>
											<xs:documentation>The text prepended to the name of 'create' service methods in the WCF service contract.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="ReadKeyword" type="NamePartType" default="Select">
										<xs:annotation>
											<xs:documentation>The text prepended to the name of 'read' service methods in the WCF service contract.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="UpdateKeyword" type="NamePartType" default="Update">
										<xs:annotation>
											<xs:documentation>The text prepended to the name of 'update' service methods in the WCF service contract.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="DeleteKeyword" type="NamePartType" default="Delete">
										<xs:annotation>
											<xs:documentation>The text prepended to the name of 'delete' service methods in the WCF service contract.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="PreferredIdKeyword" type="NamePartType" default="PreferredIdentifier">
										<xs:annotation>
											<xs:documentation>The text appended to the name of service methods used to select by the preferred identifier.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="ServiceNameSuffix" type="NamePartType" default="Service">
										<xs:annotation>
											<xs:documentation>The text appended to the name of service contract interfaces.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="InitializeFunctionName" type="NamePartType" default="Initialize">
										<xs:annotation>
											<xs:documentation>If WCF service is generated, this specifes the function name for initializing EntitySet members for serialization.</xs:documentation>
										</xs:annotation>
									</xs:attribute>
									<xs:attribute name="InstanceContextMode" default="PerCall">
										<xs:annotation>
											<xs:documentation>Determines the InstanceContextMode used with the ServiceBehavior attribute.</xs:documentation>
										</xs:annotation>
										<xs:simpleType>
											<xs:restriction base="xs:token">
												<xs:enumeration value="PerCall"/>
												<xs:enumeration value="PerSession"/>
												<xs:enumeration value="Single"/>
											</xs:restriction>
										</xs:simpleType>
									</xs:attribute>
								</xs:complexType>
							</xs:element>
						</xs:all>
					</xs:complexType>
				</xs:element>
				<xs:simpleType name="NamePartType">
					<xs:restriction base="xs:string">
						<xs:pattern value="[_\p{{L}}][_\p{{L}}\p{{Nd}}]*"/>
					</xs:restriction>
				</xs:simpleType>
			</xs:schema>
		</LinqToSqlSettings>
	</xsl:template>
</xsl:stylesheet>