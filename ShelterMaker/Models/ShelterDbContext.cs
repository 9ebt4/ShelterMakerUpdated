using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShelterMaker.Models;

public partial class ShelterDbContext : DbContext
{
    public ShelterDbContext()
    {
    }

    public ShelterDbContext(DbContextOptions<ShelterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alteration> Alterations { get; set; }

    public virtual DbSet<AlterationType> AlterationTypes { get; set; }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<Associate> Associates { get; set; }

    public virtual DbSet<AssociateMaintenance> AssociateMaintenances { get; set; }

    public virtual DbSet<Ban> Bans { get; set; }

    public virtual DbSet<BanMaintenance> BanMaintenances { get; set; }

    public virtual DbSet<Bed> Beds { get; set; }

    public virtual DbSet<BedMaintenance> BedMaintenances { get; set; }

    public virtual DbSet<CaseWorkerPatron> CaseWorkerPatrons { get; set; }

    public virtual DbSet<Checklist> Checklists { get; set; }

    public virtual DbSet<ContactInfo> ContactInfos { get; set; }

    public virtual DbSet<ContactMaintenance> ContactMaintenances { get; set; }

    public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<GoogleUser> GoogleUsers { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<IncidentMaintenance> IncidentMaintenances { get; set; }

    public virtual DbSet<InfoRelease> InfoReleases { get; set; }

    public virtual DbSet<Initial> Initials { get; set; }

    public virtual DbSet<Intake> Intakes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<MedicalCondition> MedicalConditions { get; set; }

    public virtual DbSet<MedicalConditionMaintenance> MedicalConditionMaintenances { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<NoteMaintenance> NoteMaintenances { get; set; }

    public virtual DbSet<Patron> Patrons { get; set; }

    public virtual DbSet<PatronBan> PatronBans { get; set; }

    public virtual DbSet<PatronIncident> PatronIncidents { get; set; }

    public virtual DbSet<PatronInfoRelease> PatronInfoReleases { get; set; }

    public virtual DbSet<PatronNote> PatronNotes { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Relationship> Relationships { get; set; }

    public virtual DbSet<Requirement> Requirements { get; set; }

    public virtual DbSet<SexualOffender> SexualOffenders { get; set; }

    public virtual DbSet<TenRule> TenRules { get; set; }

    public virtual DbSet<TrackedTable> TrackedTables { get; set; }

    public virtual DbSet<WorkPass> WorkPasses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=shelterDB;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alteration>(entity =>
        {
            entity.HasKey(e => e.AlterationId).HasName("PK__Alterati__A06D0CED798BD255");

            entity.ToTable("Alteration");

            entity.Property(e => e.AlterationId).HasColumnName("alterationID");
            entity.Property(e => e.AlterationDate)
                .HasColumnType("datetime")
                .HasColumnName("alterationDate");
            entity.Property(e => e.AlterationTypeId).HasColumnName("alterationTypeID");
            entity.Property(e => e.AlteredKey).HasColumnName("alteredKey");
            entity.Property(e => e.AssociateId).HasColumnName("associateID");
            entity.Property(e => e.TrackedTableId).HasColumnName("trackedTableID");

            entity.HasOne(d => d.AlterationType).WithMany(p => p.Alterations)
                .HasForeignKey(d => d.AlterationTypeId)
                .HasConstraintName("FK__Alteratio__alter__5DCAEF64");

            entity.HasOne(d => d.Associate).WithMany(p => p.Alterations)
                .HasForeignKey(d => d.AssociateId)
                .HasConstraintName("FK__Alteratio__assoc__5CD6CB2B");

            entity.HasOne(d => d.TrackedTable).WithMany(p => p.Alterations)
                .HasForeignKey(d => d.TrackedTableId)
                .HasConstraintName("FK__Alteratio__track__5EBF139D");
        });

        modelBuilder.Entity<AlterationType>(entity =>
        {
            entity.HasKey(e => e.AlterationTypeId).HasName("PK__Alterati__7E44104B45B33178");

            entity.ToTable("AlterationType");

            entity.Property(e => e.AlterationTypeId).HasColumnName("alterationTypeID");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.AmenityId).HasName("PK__Amenity__5C8402D6526E55F7");

            entity.ToTable("Amenity");

            entity.Property(e => e.AmenityId).HasColumnName("amenityID");
            entity.Property(e => e.BedId).HasColumnName("bedID");
            entity.Property(e => e.Category)
                .HasMaxLength(30)
                .HasColumnName("category");

            entity.HasOne(d => d.Bed).WithMany(p => p.Amenities)
                .HasForeignKey(d => d.BedId)
                .HasConstraintName("FK__Amenity__bedID__47DBAE45");
        });

        modelBuilder.Entity<Associate>(entity =>
        {
            entity.HasKey(e => e.AssociateId).HasName("PK__Associat__D6EE574209BC1B33");

            entity.ToTable("Associate");

            entity.Property(e => e.AssociateId).HasColumnName("associateID");
            entity.Property(e => e.AssociateMaintenanceId).HasColumnName("associateMaintenanceID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");

            entity.HasOne(d => d.AssociateMaintenance).WithMany(p => p.Associates)
                .HasForeignKey(d => d.AssociateMaintenanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Associate__assoc__52593CB8");

            entity.HasOne(d => d.GoogleUser).WithMany(p => p.Associates)
                .HasForeignKey(d => d.GoogleUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Associate_GoogleUser");
        });

        modelBuilder.Entity<AssociateMaintenance>(entity =>
        {
            entity.HasKey(e => e.AssociateMaintenanceId).HasName("PK__Associat__27066BD5FABF5284");

            entity.ToTable("AssociateMaintenance");

            entity.Property(e => e.AssociateMaintenanceId).HasColumnName("associateMaintenanceID");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Ban>(entity =>
        {
            entity.HasKey(e => e.BanId).HasName("PK__Ban__9AF0474F1E8430C8");

            entity.ToTable("Ban");

            entity.Property(e => e.BanId).HasColumnName("banID");
            entity.Property(e => e.BanMaintenanceId).HasColumnName("banMaintenanceID");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.IncidentReportId).HasColumnName("incidentReportID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");

            entity.HasOne(d => d.BanMaintenance).WithMany(p => p.Bans)
                .HasForeignKey(d => d.BanMaintenanceId)
                .HasConstraintName("FK__Ban__banMaintena__0A9D95DB");

            entity.HasOne(d => d.IncidentReport).WithMany(p => p.Bans)
                .HasForeignKey(d => d.IncidentReportId)
                .HasConstraintName("FK__Ban__incidentRep__09A971A2");
        });

        modelBuilder.Entity<BanMaintenance>(entity =>
        {
            entity.HasKey(e => e.BanMaintenanceId).HasName("PK__BanMaint__63AFEEF0926707C0");

            entity.ToTable("BanMaintenance");

            entity.Property(e => e.BanMaintenanceId).HasColumnName("banMaintenanceID");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Bed>(entity =>
        {
            entity.HasKey(e => e.BedId).HasName("PK__Bed__3700DCCF58C741C9");

            entity.ToTable("Bed");

            entity.Property(e => e.BedId).HasColumnName("bedID");
            entity.Property(e => e.FacilityId).HasColumnName("facilityID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.HasOne(d => d.Facility).WithMany(p => p.Beds)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Bed__facilityID__4222D4EF");
        });

        modelBuilder.Entity<BedMaintenance>(entity =>
        {
            entity.HasKey(e => e.BedMaintenanceId).HasName("PK__BedMaint__1D5C243C0C01E1F2");

            entity.ToTable("BedMaintenance");

            entity.Property(e => e.BedMaintenanceId).HasColumnName("bedMaintenanceID");
            entity.Property(e => e.BedId).HasColumnName("bedID");
            entity.Property(e => e.Category)
                .HasMaxLength(30)
                .HasColumnName("category");

            entity.HasOne(d => d.Bed).WithMany(p => p.BedMaintenances)
                .HasForeignKey(d => d.BedId)
                .HasConstraintName("FK__BedMainte__bedID__44FF419A");
        });

        modelBuilder.Entity<CaseWorkerPatron>(entity =>
        {
            entity.HasKey(e => e.CaseWorkerPatronId).HasName("PK__CaseWork__7079DCB3316C710B");

            entity.ToTable("CaseWorkerPatron");

            entity.Property(e => e.CaseWorkerPatronId).HasColumnName("caseWorkerPatronID");
            entity.Property(e => e.CaseWorkerId).HasColumnName("caseWorkerID");
            entity.Property(e => e.PatronId).HasColumnName("patronID");

            entity.HasOne(d => d.CaseWorker).WithMany(p => p.CaseWorkerPatrons)
                .HasForeignKey(d => d.CaseWorkerId)
                .HasConstraintName("FK__CaseWorke__caseW__1F98B2C1");

            entity.HasOne(d => d.Patron).WithMany(p => p.CaseWorkerPatrons)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK__CaseWorke__patro__1EA48E88");
        });

        modelBuilder.Entity<Checklist>(entity =>
        {
            entity.HasKey(e => e.ChecklistId).HasName("PK__Checklis__15C1ACBCCE6B1788");

            entity.ToTable("Checklist");

            entity.Property(e => e.ChecklistId).HasColumnName("checklistID");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.FacilityId).HasColumnName("facilityID");
            entity.Property(e => e.Options)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.StartTime).HasColumnName("startTime");

            entity.HasOne(d => d.Facility).WithMany(p => p.Checklists)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Checklist__facil__398D8EEE");
        });

        modelBuilder.Entity<ContactInfo>(entity =>
        {
            entity.HasKey(e => e.ContactInfoId).HasName("PK__ContactI__6E77958AE63A486B");

            entity.ToTable("ContactInfo");

            entity.Property(e => e.ContactInfoId).HasColumnName("contactInfoID");
            entity.Property(e => e.ContactMaintenanceId).HasColumnName("contactMaintenanceID");
            entity.Property(e => e.Details)
                .HasMaxLength(255)
                .HasColumnName("details");
            entity.Property(e => e.FacilityId).HasColumnName("facilityID");
            entity.Property(e => e.PersonId).HasColumnName("personID");

            entity.HasOne(d => d.ContactMaintenance).WithMany(p => p.ContactInfos)
                .HasForeignKey(d => d.ContactMaintenanceId)
                .HasConstraintName("FK__ContactIn__conta__6383C8BA");

            entity.HasOne(d => d.Facility).WithMany(p => p.ContactInfos)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__ContactIn__facil__656C112C");

            entity.HasOne(d => d.Person).WithMany(p => p.ContactInfos)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__ContactIn__perso__6477ECF3");
        });

        modelBuilder.Entity<ContactMaintenance>(entity =>
        {
            entity.HasKey(e => e.ContactMaintenanceId).HasName("PK__ContactM__C69B741F01931772");

            entity.ToTable("ContactMaintenance");

            entity.Property(e => e.ContactMaintenanceId).HasColumnName("contactMaintenanceID");
            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<EmergencyContact>(entity =>
        {
            entity.HasKey(e => e.EmergencyContactId).HasName("PK__Emergenc__7394A13DBFD5A1F3");

            entity.ToTable("EmergencyContact");

            entity.Property(e => e.EmergencyContactId).HasColumnName("emergencyContactID");
            entity.Property(e => e.AssociateId).HasColumnName("associateID");
            entity.Property(e => e.PatronId).HasColumnName("patronID");
            entity.Property(e => e.PersonId).HasColumnName("personID");
            entity.Property(e => e.RelationshipId).HasColumnName("relationshipID");

            entity.HasOne(d => d.Associate).WithMany(p => p.EmergencyContacts)
                .HasForeignKey(d => d.AssociateId)
                .HasConstraintName("FK__Emergency__assoc__29221CFB");

            entity.HasOne(d => d.Patron).WithMany(p => p.EmergencyContacts)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK__Emergency__patro__2A164134");

            entity.HasOne(d => d.Person).WithMany(p => p.EmergencyContacts)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__Emergency__perso__2B0A656D");

            entity.HasOne(d => d.Relationship).WithMany(p => p.EmergencyContacts)
                .HasForeignKey(d => d.RelationshipId)
                .HasConstraintName("FK__Emergency__relat__2BFE89A6");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Facility__AA54818487400DA1");

            entity.ToTable("Facility");

            entity.Property(e => e.FacilityId).HasColumnName("facilityID");
            entity.Property(e => e.FacilityCode)
                .HasMaxLength(20)
                .HasColumnName("facilityCode");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Gender__306E222064F4262D");

            entity.ToTable("Gender");

            entity.Property(e => e.GenderId).HasColumnName("genderID");
            entity.Property(e => e.Category)
                .HasMaxLength(9)
                .HasColumnName("category");
        });

        modelBuilder.Entity<GoogleUser>(entity =>
        {
            entity.HasKey(e => e.GoogleUserId).HasName("PK__GoogleUs__AE034353C6BB727D");

            entity.ToTable("GoogleUser");

            entity.Property(e => e.GoogleUserId).HasColumnName("googleUserID");
            entity.Property(e => e.FacilityId).HasColumnName("facilityID");
            entity.Property(e => e.GoogleToken)
                .HasMaxLength(4000)
                .HasColumnName("googleToken");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.PersonId).HasColumnName("personId");

            entity.HasOne(d => d.Facility).WithMany(p => p.GoogleUsers)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK_GoogleUser_Facility");

            entity.HasOne(d => d.Person).WithMany(p => p.GoogleUsers)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_Associate_Person");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.IncidentId).HasName("PK__Incident__06A5D761021F51B0");

            entity.ToTable("Incident");

            entity.Property(e => e.IncidentId).HasColumnName("incidentID");
            entity.Property(e => e.ActionTaken).HasColumnName("actionTaken");
            entity.Property(e => e.AssociateId).HasColumnName("associateID");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.EmergencyServices).HasColumnName("emergencyServices");
            entity.Property(e => e.IncidentDate)
                .HasColumnType("datetime")
                .HasColumnName("incidentDate");
            entity.Property(e => e.IncidentMaintenanceId).HasColumnName("incidentMaintenanceID");

            entity.HasOne(d => d.Associate).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.AssociateId)
                .HasConstraintName("FK__Incident__associ__03F0984C");

            entity.HasOne(d => d.IncidentMaintenance).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.IncidentMaintenanceId)
                .HasConstraintName("FK__Incident__incide__04E4BC85");
        });

        modelBuilder.Entity<IncidentMaintenance>(entity =>
        {
            entity.HasKey(e => e.IncidentMaintenanceId).HasName("PK__Incident__B7CD4704CFFD7DBB");

            entity.ToTable("IncidentMaintenance");

            entity.Property(e => e.IncidentMaintenanceId).HasColumnName("incidentMaintenanceID");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
        });

        modelBuilder.Entity<InfoRelease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InfoRele__3214EC2736EF31D2");

            entity.ToTable("InfoRelease");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PatronId).HasColumnName("PatronID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.RelationshipId).HasColumnName("RelationshipID");

            entity.HasOne(d => d.Patron).WithMany(p => p.InfoReleases)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK_InfoRelease_Patron");

            entity.HasOne(d => d.Person).WithMany(p => p.InfoReleases)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_InfoRelease_Person");

            entity.HasOne(d => d.Relationship).WithMany(p => p.InfoReleases)
                .HasForeignKey(d => d.RelationshipId)
                .HasConstraintName("FK_InfoRelease_Relationship");
        });

        modelBuilder.Entity<Initial>(entity =>
        {
            entity.HasKey(e => e.InitialId).HasName("PK__Initial__DCF9974E310ECB6A");

            entity.ToTable("Initial");

            entity.Property(e => e.InitialId).HasColumnName("initialID");
            entity.Property(e => e.Covid).HasColumnName("covid");
            entity.Property(e => e.InitialAgreement).HasColumnName("initialAgreement");
            entity.Property(e => e.Locations).HasColumnName("locations");
            entity.Property(e => e.Medical).HasColumnName("medical");
        });

        modelBuilder.Entity<Intake>(entity =>
        {
            entity.HasKey(e => e.IntakeId).HasName("PK__Intake__0F6900690EACC6A6");

            entity.ToTable("Intake");

            entity.Property(e => e.IntakeId).HasColumnName("intakeID");
            entity.Property(e => e.InitialId).HasColumnName("initialID");
            entity.Property(e => e.RequirementsId).HasColumnName("requirementsID");
            entity.Property(e => e.SexualOffenderId).HasColumnName("sexualOffenderID");
            entity.Property(e => e.TenRulesId).HasColumnName("tenRulesID");

            entity.HasOne(d => d.Initial).WithMany(p => p.Intakes)
                .HasForeignKey(d => d.InitialId)
                .HasConstraintName("FK__Intake__initialI__6FE99F9F");

            entity.HasOne(d => d.Requirements).WithMany(p => p.Intakes)
                .HasForeignKey(d => d.RequirementsId)
                .HasConstraintName("FK__Intake__requirem__71D1E811");

            entity.HasOne(d => d.SexualOffender).WithMany(p => p.Intakes)
                .HasForeignKey(d => d.SexualOffenderId)
                .HasConstraintName("FK__Intake__sexualOf__70DDC3D8");

            entity.HasOne(d => d.TenRules).WithMany(p => p.Intakes)
                .HasForeignKey(d => d.TenRulesId)
                .HasConstraintName("FK__Intake__tenRules__72C60C4A");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__56A1284A83E72C72");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("itemID");
            entity.Property(e => e.CheckListId).HasColumnName("checkListId");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .IsFixedLength();
            entity.Property(e => e.IsChecked).HasColumnName("isChecked");

            entity.HasOne(d => d.CheckList).WithMany(p => p.Items)
                .HasForeignKey(d => d.CheckListId)
                .HasConstraintName("fk_checklist");
        });

        modelBuilder.Entity<MedicalCondition>(entity =>
        {
            entity.HasKey(e => e.MedicalConditionId).HasName("PK__MedicalC__A7E311BE0357E8EC");

            entity.ToTable("MedicalCondition");

            entity.Property(e => e.MedicalConditionId).HasColumnName("medicalConditionID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.MedicalConditionMaintenanceId).HasColumnName("medicalConditionMaintenanceID");
            entity.Property(e => e.PatronId).HasColumnName("patronID");

            entity.HasOne(d => d.MedicalConditionMaintenance).WithMany(p => p.MedicalConditions)
                .HasForeignKey(d => d.MedicalConditionMaintenanceId)
                .HasConstraintName("FK__MedicalCo__medic__3A4CA8FD");

            entity.HasOne(d => d.Patron).WithMany(p => p.MedicalConditions)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK__MedicalCo__patro__395884C4");
        });

        modelBuilder.Entity<MedicalConditionMaintenance>(entity =>
        {
            entity.HasKey(e => e.MedicalConditionMaintenanceId).HasName("PK__MedicalC__853397D48DFD23C0");

            entity.ToTable("MedicalConditionMaintenance");

            entity.Property(e => e.MedicalConditionMaintenanceId).HasColumnName("medicalConditionMaintenanceID");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__Notes__03C97EDDC7EA61F9");

            entity.Property(e => e.NoteId).HasColumnName("noteID");
            entity.Property(e => e.AssociateId).HasColumnName("associateID");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.NoteMaintenanceId).HasColumnName("noteMaintenanceID");

            entity.HasOne(d => d.Associate).WithMany(p => p.Notes)
                .HasForeignKey(d => d.AssociateId)
                .HasConstraintName("FK__Notes__associate__7E37BEF6");

            entity.HasOne(d => d.NoteMaintenance).WithMany(p => p.Notes)
                .HasForeignKey(d => d.NoteMaintenanceId)
                .HasConstraintName("FK__Notes__noteMaint__7F2BE32F");
        });

        modelBuilder.Entity<NoteMaintenance>(entity =>
        {
            entity.HasKey(e => e.NoteMaintenanceId).HasName("PK__NoteMain__A3F648E961E5ED9D");

            entity.ToTable("NoteMaintenance");

            entity.Property(e => e.NoteMaintenanceId).HasColumnName("noteMaintenanceID");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Patron>(entity =>
        {
            entity.HasKey(e => e.PatronId).HasName("PK__Patron__285F64A6925A5DDA");

            entity.ToTable("Patron");

            entity.Property(e => e.PatronId).HasColumnName("patronID");
            entity.Property(e => e.BedId).HasColumnName("bedID");
            entity.Property(e => e.FacilityId).HasColumnName("facilityID");
            entity.Property(e => e.IntakeId).HasColumnName("intakeID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.LastCheckIn)
                .HasColumnType("datetime")
                .HasColumnName("lastCheckIn");
            entity.Property(e => e.PassPhrase)
                .HasMaxLength(50)
                .HasColumnName("passPhrase");
            entity.Property(e => e.PersonId).HasColumnName("personID");
            entity.Property(e => e.WorkPassId).HasColumnName("workPassID");

            entity.HasOne(d => d.Bed).WithMany(p => p.Patrons)
                .HasForeignKey(d => d.BedId)
                .HasConstraintName("FK__Patron__bedID__0F624AF8");

            entity.HasOne(d => d.Facility).WithMany(p => p.Patrons)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Patron__facility__4D5F7D71");

            entity.HasOne(d => d.Intake).WithMany(p => p.Patrons)
                .HasForeignKey(d => d.IntakeId)
                .HasConstraintName("FK__Patron__intakeID__0E6E26BF");

            entity.HasOne(d => d.Person).WithMany(p => p.Patrons)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__Patron__personID__0D7A0286");

            entity.HasOne(d => d.WorkPass).WithMany(p => p.Patrons)
                .HasForeignKey(d => d.WorkPassId)
                .HasConstraintName("FK__Patron__workPass__10566F31");
        });

        modelBuilder.Entity<PatronBan>(entity =>
        {
            entity.HasKey(e => e.PatronBanId).HasName("PK__PatronBa__EC05836B5536A5D5");

            entity.ToTable("PatronBan");

            entity.Property(e => e.PatronBanId).HasColumnName("patronBanID");
            entity.Property(e => e.BanId).HasColumnName("banID");
            entity.Property(e => e.PatronId).HasColumnName("patronID");

            entity.HasOne(d => d.Ban).WithMany(p => p.PatronBans)
                .HasForeignKey(d => d.BanId)
                .HasConstraintName("FK__PatronBan__banID__1BC821DD");

            entity.HasOne(d => d.Patron).WithMany(p => p.PatronBans)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK__PatronBan__patro__1AD3FDA4");
        });

        modelBuilder.Entity<PatronIncident>(entity =>
        {
            entity.HasKey(e => e.PatronIncidentId).HasName("PK__PatronIn__EE9755D165F979E1");

            entity.Property(e => e.PatronIncidentId).HasColumnName("patronIncidentID");
            entity.Property(e => e.IncidentId).HasColumnName("incidentID");
            entity.Property(e => e.PatronId).HasColumnName("patronID");

            entity.HasOne(d => d.Incident).WithMany(p => p.PatronIncidents)
                .HasForeignKey(d => d.IncidentId)
                .HasConstraintName("FK__PatronInc__incid__17F790F9");

            entity.HasOne(d => d.Patron).WithMany(p => p.PatronIncidents)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK__PatronInc__patro__17036CC0");
        });

        modelBuilder.Entity<PatronInfoRelease>(entity =>
        {
            entity.HasKey(e => e.PatronInfoReleaseId).HasName("PK__PatronIn__4390A87B71185353");

            entity.ToTable("PatronInfoRelease");

            entity.Property(e => e.PatronInfoReleaseId).HasColumnName("patronInfoReleaseID");
            entity.Property(e => e.PatronId).HasColumnName("patronID");
            entity.Property(e => e.PersonId).HasColumnName("personID");
            entity.Property(e => e.RelationshipId).HasColumnName("relationshipID");

            entity.HasOne(d => d.Patron).WithMany(p => p.PatronInfoReleases)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK__PatronInf__patro__245D67DE");

            entity.HasOne(d => d.Person).WithMany(p => p.PatronInfoReleases)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__PatronInf__perso__25518C17");

            entity.HasOne(d => d.Relationship).WithMany(p => p.PatronInfoReleases)
                .HasForeignKey(d => d.RelationshipId)
                .HasConstraintName("FK__PatronInf__relat__2645B050");
        });

        modelBuilder.Entity<PatronNote>(entity =>
        {
            entity.HasKey(e => e.PatronNoteId).HasName("PK__PatronNo__6697118622D541FD");

            entity.Property(e => e.PatronNoteId).HasColumnName("patronNoteID");
            entity.Property(e => e.NoteId).HasColumnName("noteID");
            entity.Property(e => e.PatronId).HasColumnName("patronID");

            entity.HasOne(d => d.Note).WithMany(p => p.PatronNotes)
                .HasForeignKey(d => d.NoteId)
                .HasConstraintName("FK__PatronNot__noteI__14270015");

            entity.HasOne(d => d.Patron).WithMany(p => p.PatronNotes)
                .HasForeignKey(d => d.PatronId)
                .HasConstraintName("FK__PatronNot__patro__1332DBDC");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person__EC7D7D6D2C93FF43");

            entity.ToTable("Person");

            entity.Property(e => e.PersonId).HasColumnName("personID");
            entity.Property(e => e.BirthDay).HasColumnName("birthDay");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.GenderId).HasColumnName("genderID");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middleName");

            entity.HasOne(d => d.Gender).WithMany(p => p.People)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("FK__Person__genderID__4CA06362");
        });

        modelBuilder.Entity<Relationship>(entity =>
        {
            entity.HasKey(e => e.RelationshipId).HasName("PK__Relation__4BCCCEF7CD84F743");

            entity.ToTable("Relationship");

            entity.Property(e => e.RelationshipId).HasColumnName("relationshipID");
            entity.Property(e => e.Relationship1)
                .HasMaxLength(20)
                .HasColumnName("relationship");
        });

        modelBuilder.Entity<Requirement>(entity =>
        {
            entity.HasKey(e => e.RequirementsId).HasName("PK__Requirem__2D2EA45D2AB7E803");

            entity.Property(e => e.RequirementsId).HasColumnName("requirementsID");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Confirmed).HasColumnName("confirmed");
        });

        modelBuilder.Entity<SexualOffender>(entity =>
        {
            entity.HasKey(e => e.SexualOffenderId).HasName("PK__SexualOf__ED497BB64734C69B");

            entity.ToTable("SexualOffender");

            entity.Property(e => e.SexualOffenderId).HasColumnName("sexualOffenderID");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.IsOffender).HasColumnName("isOffender");
        });

        modelBuilder.Entity<TenRule>(entity =>
        {
            entity.HasKey(e => e.TenRulesId).HasName("PK__TenRules__F2F2B8EB2429697C");

            entity.Property(e => e.TenRulesId).HasColumnName("tenRulesID");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Confirmed).HasColumnName("confirmed");
        });

        modelBuilder.Entity<TrackedTable>(entity =>
        {
            entity.HasKey(e => e.TrackedTableId).HasName("PK__TrackedT__AAE2580AFE1B142B");

            entity.Property(e => e.TrackedTableId).HasColumnName("trackedTableID");
            entity.Property(e => e.TableName)
                .HasMaxLength(255)
                .HasColumnName("tableName");
        });

        modelBuilder.Entity<WorkPass>(entity =>
        {
            entity.HasKey(e => e.WorkPassId).HasName("PK__WorkPass__46770FA0D26E6DDC");

            entity.ToTable("WorkPass");

            entity.Property(e => e.WorkPassId).HasColumnName("workPassID");
            entity.Property(e => e.Confirmed).HasColumnName("confirmed");
            entity.Property(e => e.Needed).HasColumnName("needed");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
