namespace RuleArchitect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlSystems",
                c => new
                    {
                        ControlSystemId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ControlSystemId)
                .Index(t => t.Name, unique: true, name: "IX_ControlSystemName");
            
            CreateTable(
                "dbo.SoftwareOptions",
                c => new
                    {
                        SoftwareOptionId = c.Int(nullable: false, identity: true),
                        PrimaryName = c.String(nullable: false, maxLength: 255),
                        AlternativeNames = c.String(maxLength: 500),
                        SourceFileName = c.String(maxLength: 255),
                        PrimaryOptionNumberDisplay = c.String(maxLength: 100),
                        Notes = c.String(maxLength: 2147483647),
                        CheckedBy = c.String(maxLength: 100),
                        CheckedDate = c.DateTime(),
                        ControlSystemId = c.Int(),
                    })
                .PrimaryKey(t => t.SoftwareOptionId)
                .ForeignKey("dbo.ControlSystems", t => t.ControlSystemId)
                .Index(t => t.ControlSystemId);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        RequirementId = c.Int(nullable: false, identity: true),
                        SoftwareOptionId = c.Int(nullable: false),
                        RequirementType = c.String(nullable: false, maxLength: 100),
                        Condition = c.String(maxLength: 100),
                        GeneralRequiredValue = c.String(maxLength: 2147483647),
                        RequiredSoftwareOptionId = c.Int(),
                        RequiredSpecCodeDefinitionId = c.Int(),
                        OspFileName = c.String(maxLength: 255),
                        OspFileVersion = c.String(maxLength: 50),
                        Notes = c.String(maxLength: 2147483647),
                    })
                .PrimaryKey(t => t.RequirementId)
                .ForeignKey("dbo.SoftwareOptions", t => t.RequiredSoftwareOptionId)
                .ForeignKey("dbo.SpecCodeDefinitions", t => t.RequiredSpecCodeDefinitionId)
                .ForeignKey("dbo.SoftwareOptions", t => t.SoftwareOptionId, cascadeDelete: true)
                .Index(t => t.SoftwareOptionId)
                .Index(t => t.RequiredSoftwareOptionId)
                .Index(t => t.RequiredSpecCodeDefinitionId);
            
            CreateTable(
                "dbo.SpecCodeDefinitions",
                c => new
                    {
                        SpecCodeDefinitionId = c.Int(nullable: false, identity: true),
                        SpecCodeNo = c.String(nullable: false, maxLength: 50),
                        SpecCodeBit = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 255),
                        Category = c.String(nullable: false, maxLength: 50),
                        MachineTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SpecCodeDefinitionId)
                .ForeignKey("dbo.MachineTypes", t => t.MachineTypeId)
                .Index(t => new { t.SpecCodeNo, t.SpecCodeBit, t.MachineTypeId }, unique: true, name: "IX_SpecCodeNoBitMachineType");
            
            CreateTable(
                "dbo.MachineTypes",
                c => new
                    {
                        MachineTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.MachineTypeId)
                .Index(t => t.Name, unique: true, name: "IX_MachineTypeName");
            
            CreateTable(
                "dbo.SoftwareOptionSpecificationCodes",
                c => new
                    {
                        SoftwareOptionSpecificationCodeId = c.Int(nullable: false, identity: true),
                        SoftwareOptionId = c.Int(nullable: false),
                        SpecCodeDefinitionId = c.Int(nullable: false),
                        ActivationRule = c.String(maxLength: 255),
                        ValueInterpretation = c.String(maxLength: 2147483647),
                    })
                .PrimaryKey(t => t.SoftwareOptionSpecificationCodeId)
                .ForeignKey("dbo.SoftwareOptions", t => t.SoftwareOptionId, cascadeDelete: true)
                .ForeignKey("dbo.SpecCodeDefinitions", t => t.SpecCodeDefinitionId)
                .Index(t => t.SoftwareOptionId)
                .Index(t => t.SpecCodeDefinitionId);
            
            CreateTable(
                "dbo.ParameterMappings",
                c => new
                    {
                        ParameterMappingId = c.Int(nullable: false, identity: true),
                        RelatedSheetName = c.String(maxLength: 255),
                        ConditionIdentifier = c.String(maxLength: 255),
                        ConditionName = c.String(maxLength: 255),
                        SettingContext = c.String(maxLength: 255),
                        ConfigurationDetailsJson = c.String(maxLength: 2147483647),
                        SoftwareOptionId = c.Int(),
                    })
                .PrimaryKey(t => t.ParameterMappingId)
                .ForeignKey("dbo.SoftwareOptions", t => t.SoftwareOptionId)
                .Index(t => t.SoftwareOptionId);
            
            CreateTable(
                "dbo.OptionNumberRegistries",
                c => new
                    {
                        OptionNumberRegistryId = c.Int(nullable: false, identity: true),
                        OptionNumber = c.String(nullable: false, maxLength: 50),
                        SoftwareOptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OptionNumberRegistryId)
                .ForeignKey("dbo.SoftwareOptions", t => t.SoftwareOptionId, cascadeDelete: true)
                .Index(t => t.SoftwareOptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptionNumberRegistries", "SoftwareOptionId", "dbo.SoftwareOptions");
            DropForeignKey("dbo.ParameterMappings", "SoftwareOptionId", "dbo.SoftwareOptions");
            DropForeignKey("dbo.Requirements", "SoftwareOptionId", "dbo.SoftwareOptions");
            DropForeignKey("dbo.Requirements", "RequiredSpecCodeDefinitionId", "dbo.SpecCodeDefinitions");
            DropForeignKey("dbo.SoftwareOptionSpecificationCodes", "SpecCodeDefinitionId", "dbo.SpecCodeDefinitions");
            DropForeignKey("dbo.SoftwareOptionSpecificationCodes", "SoftwareOptionId", "dbo.SoftwareOptions");
            DropForeignKey("dbo.SpecCodeDefinitions", "MachineTypeId", "dbo.MachineTypes");
            DropForeignKey("dbo.Requirements", "RequiredSoftwareOptionId", "dbo.SoftwareOptions");
            DropForeignKey("dbo.SoftwareOptions", "ControlSystemId", "dbo.ControlSystems");
            DropIndex("dbo.OptionNumberRegistries", new[] { "SoftwareOptionId" });
            DropIndex("dbo.ParameterMappings", new[] { "SoftwareOptionId" });
            DropIndex("dbo.SoftwareOptionSpecificationCodes", new[] { "SpecCodeDefinitionId" });
            DropIndex("dbo.SoftwareOptionSpecificationCodes", new[] { "SoftwareOptionId" });
            DropIndex("dbo.MachineTypes", "IX_MachineTypeName");
            DropIndex("dbo.SpecCodeDefinitions", "IX_SpecCodeNoBitMachineType");
            DropIndex("dbo.Requirements", new[] { "RequiredSpecCodeDefinitionId" });
            DropIndex("dbo.Requirements", new[] { "RequiredSoftwareOptionId" });
            DropIndex("dbo.Requirements", new[] { "SoftwareOptionId" });
            DropIndex("dbo.SoftwareOptions", new[] { "ControlSystemId" });
            DropIndex("dbo.ControlSystems", "IX_ControlSystemName");
            DropTable("dbo.OptionNumberRegistries");
            DropTable("dbo.ParameterMappings");
            DropTable("dbo.SoftwareOptionSpecificationCodes");
            DropTable("dbo.MachineTypes");
            DropTable("dbo.SpecCodeDefinitions");
            DropTable("dbo.Requirements");
            DropTable("dbo.SoftwareOptions");
            DropTable("dbo.ControlSystems");
        }
    }
}
