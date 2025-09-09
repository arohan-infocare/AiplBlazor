// File: AtmsEntityConfigurations.cs
// Namespace: ATMS.Infrastructure.Persistence.Configurations
// Contains IEntityTypeConfiguration<T> for all ATMS domain entities.

using AiplBlazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATMS.Infrastructure.Persistence.Configurations
{
    #region Topology & Fiber

    public class CorridorConfiguration : IEntityTypeConfiguration<Corridor>
    {
        public void Configure(EntityTypeBuilder<Corridor> builder)
        {
            builder.ToTable("Corridors");
            builder.Property(x => x.CorridorCode).HasMaxLength(64).IsRequired();
            builder.Property(x => x.CorridorName).HasMaxLength(256);
            builder.HasIndex(x => new { x.TenantId, x.CorridorCode }).IsUnique()
                .HasDatabaseName("IX_Corridors_TenantId_CorridorCode");
        }
    }

    public class PhysicalSiteConfiguration : IEntityTypeConfiguration<PhysicalSite>
    {
        public void Configure(EntityTypeBuilder<PhysicalSite> builder)
        {
            builder.ToTable("PhysicalSites");
            builder.Property(x => x.SiteCode).HasMaxLength(64).IsRequired();
            builder.Property(x => x.SiteName).HasMaxLength(256);
            builder.HasIndex(x => x.CorridorId).HasDatabaseName("IX_PhysicalSites_CorridorId");
            builder.HasIndex(x => x.SiteCode).HasDatabaseName("IX_PhysicalSites_SiteCode");
        }
    }

    public class EquipmentLocationConfiguration : IEntityTypeConfiguration<EquipmentLocation>
    {
        public void Configure(EntityTypeBuilder<EquipmentLocation> builder)
        {
            builder.ToTable("EquipmentLocations");
            builder.Property(x => x.LocationName).HasMaxLength(256);
            builder.HasIndex(x => new { x.CorridorId, x.ChainageKm }).HasDatabaseName("IX_EquipmentLocations_Corridor_Chainage");
            builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_EquipmentLocations_TenantId");
        }
    }

    public class FiberChamberConfiguration : IEntityTypeConfiguration<FiberChamber>
    {
        public void Configure(EntityTypeBuilder<FiberChamber> builder)
        {
            builder.ToTable("FiberChambers");
            builder.Property(x => x.ChamberCode).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => new { x.CorridorId, x.ChainageKm }).HasDatabaseName("IX_FiberChambers_Corridor_Chainage");
            builder.HasIndex(x => x.ChamberCode).HasDatabaseName("IX_FiberChambers_Code");
        }
    }

    public class OpticalFiberSegmentConfiguration : IEntityTypeConfiguration<OpticalFiberSegment>
    {
        public void Configure(EntityTypeBuilder<OpticalFiberSegment> builder)
        {
            builder.ToTable("OpticalFiberSegments");
            builder.Property(x => x.CableType).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => x.FromLocationId).HasDatabaseName("IX_OpticalFiberSegments_FromLocation");
            builder.HasIndex(x => x.ToLocationId).HasDatabaseName("IX_OpticalFiberSegments_ToLocation");
        }
    }

    #endregion

    #region Equipment & BOM

    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ToTable("Equipments");
            builder.Property(x => x.ModelNumber).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Manufacturer).HasMaxLength(128);
            builder.Property(x => x.EquipmentType).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => new { x.Category, x.ModelNumber }).HasDatabaseName("IX_Equipments_Category_Model");
        }
    }

    public class EquipmentBoMTemplateConfiguration : IEntityTypeConfiguration<EquipmentBoMTemplate>
    {
        public void Configure(EntityTypeBuilder<EquipmentBoMTemplate> builder)
        {
            builder.ToTable("EquipmentBoMTemplates");
            builder.Property(x => x.ComponentName).HasMaxLength(256).IsRequired();
            builder.HasIndex(x => x.EquipmentId).HasDatabaseName("IX_EquipmentBoMTemplates_EquipmentId");
            builder.HasIndex(x => x.SubEquipmentId).HasDatabaseName("IX_EquipmentBoMTemplates_SubEquipmentId");
        }
    }

    public class LocationBoMConfiguration : IEntityTypeConfiguration<LocationBoM>
    {
        public void Configure(EntityTypeBuilder<LocationBoM> builder)
        {
            builder.ToTable("LocationBoMs");
            builder.Property(x => x.ComponentName).HasMaxLength(256).IsRequired();
            builder.HasIndex(x => x.LocationId).HasDatabaseName("IX_LocationBoMs_LocationId");
            builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_LocationBoMs_TenantId");
        }
    }

    public class InstalledEquipmentConfiguration : IEntityTypeConfiguration<InstalledEquipment>
    {
        public void Configure(EntityTypeBuilder<InstalledEquipment> builder)
        {
            builder.ToTable("InstalledEquipments");
            builder.Property(x => x.AssetTag).HasMaxLength(64);
            builder.Property(x => x.IpAddress).HasMaxLength(64);
            builder.Property(x => x.MacAddress).HasMaxLength(64);
            builder.HasIndex(x => new { x.LocationId, x.EquipmentId }).HasDatabaseName("IX_InstalledEquipments_Location_Equipment");
            builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_InstalledEquipments_TenantId");
        }
    }

    public class AssetLifecycleEventConfiguration : IEntityTypeConfiguration<AssetLifecycleEvent>
    {
        public void Configure(EntityTypeBuilder<AssetLifecycleEvent> builder)
        {
            builder.ToTable("AssetLifecycleEvents");
            builder.Property(x => x.EventType).HasMaxLength(128);
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_AssetLifecycleEvents_InstalledEquipmentId");
        }
    }

    public class AssetTransferConfiguration : IEntityTypeConfiguration<AssetTransfer>
    {
        public void Configure(EntityTypeBuilder<AssetTransfer> builder)
        {
            builder.ToTable("AssetTransfers");
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_AssetTransfers_InstalledEquipmentId");
            builder.HasIndex(x => x.TransferDateUtc).HasDatabaseName("IX_AssetTransfers_TransferDateUtc");
        }
    }

    public class WarrantyConfiguration : IEntityTypeConfiguration<Warranty>
    {
        public void Configure(EntityTypeBuilder<Warranty> builder)
        {
            builder.ToTable("Warranties");
            builder.Property(x => x.Provider).HasMaxLength(256);
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_Warranties_InstalledEquipmentId");
        }
    }

    #endregion

    #region Procurement & BOQ

    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.Property(x => x.ProjectCode).HasMaxLength(64).IsRequired();
            builder.Property(x => x.ProjectName).HasMaxLength(256);
            builder.HasIndex(x => x.ProjectCode).HasDatabaseName("IX_Projects_ProjectCode");
        }
    }

    public class BoqConfiguration : IEntityTypeConfiguration<Boq>
    {
        public void Configure(EntityTypeBuilder<Boq> builder)
        {
            builder.ToTable("Boqs");
            builder.Property(x => x.Title).HasMaxLength(256);
            builder.HasIndex(x => x.ProjectId).HasDatabaseName("IX_Boqs_ProjectId");
        }
    }

    public class BoqItemConfiguration : IEntityTypeConfiguration<BoqItem>
    {
        public void Configure(EntityTypeBuilder<BoqItem> builder)
        {
            builder.ToTable("BoqItems");
            builder.Property(x => x.Section).HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.Property(x => x.Make).HasMaxLength(128);
            builder.HasIndex(x => x.BoqId).HasDatabaseName("IX_BoqItems_BoqId");
            builder.HasIndex(x => x.EquipmentId).HasDatabaseName("IX_BoqItems_EquipmentId");
        }
    }

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");
            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
            builder.Property(x => x.ContactPerson).HasMaxLength(128);
            builder.HasIndex(x => x.Name).HasDatabaseName("IX_Suppliers_Name");
        }
    }

    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders");
            builder.Property(x => x.OrderNumber).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => x.SupplierId).HasDatabaseName("IX_PurchaseOrders_SupplierId");
            builder.HasIndex(x => x.OrderNumber).HasDatabaseName("IX_PurchaseOrders_OrderNumber");
        }
    }

    public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
        {
            builder.ToTable("PurchaseOrderLines");
            builder.Property(x => x.Description).HasMaxLength(512).IsRequired();
            builder.Property(x => x.Uom).HasMaxLength(32);
            builder.HasIndex(x => x.PurchaseOrderId).HasDatabaseName("IX_PurchaseOrderLines_PurchaseOrderId");
        }
    }

    public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
    {
        public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
        {
            builder.ToTable("GoodsReceipts");
            builder.Property(x => x.ReceiptNumber).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => x.PurchaseOrderId).HasDatabaseName("IX_GoodsReceipts_PurchaseOrderId");
            builder.HasIndex(x => x.ReceiptNumber).HasDatabaseName("IX_GoodsReceipts_ReceiptNumber");
        }
    }

    public class ServiceContractConfiguration : IEntityTypeConfiguration<ServiceContract>
    {
        public void Configure(EntityTypeBuilder<ServiceContract> builder)
        {
            builder.ToTable("ServiceContracts");
            builder.Property(x => x.ContractNumber).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => x.SupplierId).HasDatabaseName("IX_ServiceContracts_SupplierId");
            builder.HasIndex(x => x.StartDateUtc).HasDatabaseName("IX_ServiceContracts_StartDateUtc");
        }
    }

    #endregion

    #region Installation & Maintenance

    public class InstallationWorkOrderConfiguration : IEntityTypeConfiguration<InstallationWorkOrder>
    {
        public void Configure(EntityTypeBuilder<InstallationWorkOrder> builder)
        {
            builder.ToTable("InstallationWorkOrders");
            builder.Property(x => x.WorkOrderNumber).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => x.PurchaseOrderId).HasDatabaseName("IX_InstallationWorkOrders_PurchaseOrderId");
        }
    }

    public class InstallationActivityConfiguration : IEntityTypeConfiguration<InstallationActivity>
    {
        public void Configure(EntityTypeBuilder<InstallationActivity> builder)
        {
            builder.ToTable("InstallationActivities");
            builder.Property(x => x.ActivityType).HasMaxLength(128).IsRequired();
            builder.HasIndex(x => x.WorkOrderId).HasDatabaseName("IX_InstallationActivities_WorkOrderId");
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_InstallationActivities_InstalledEquipmentId");
        }
    }

    public class InstallationChecklistItemConfiguration : IEntityTypeConfiguration<InstallationChecklistItem>
    {
        public void Configure(EntityTypeBuilder<InstallationChecklistItem> builder)
        {
            builder.ToTable("InstallationChecklistItems");
            builder.Property(x => x.StepCode).HasMaxLength(64).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(512).IsRequired();
            builder.HasIndex(x => x.WorkOrderId).HasDatabaseName("IX_InstallationChecklistItems_WorkOrderId");
        }
    }

    public class MaintenanceLogConfiguration : IEntityTypeConfiguration<MaintenanceLog>
    {
        public void Configure(EntityTypeBuilder<MaintenanceLog> builder)
        {
            builder.ToTable("MaintenanceLogs");
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_MaintenanceLogs_InstalledEquipmentId");
            builder.HasIndex(x => x.LogDateUtc).HasDatabaseName("IX_MaintenanceLogs_LogDateUtc");
        }
    }

    #endregion

    #region Incidents & Enforcement

    public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {
        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.ToTable("Incidents");
            builder.Property(x => x.IncidentNumber).HasMaxLength(64).IsRequired();
            builder.HasIndex(x => new { x.TenantId, x.Status }).HasDatabaseName("IX_Incidents_TenantId_Status");
            builder.HasIndex(x => new { x.DetectedAtUtc }).HasDatabaseName("IX_Incidents_DetectedAtUtc");
        }
    }

    public class IncidentMediaConfiguration : IEntityTypeConfiguration<IncidentMedia>
    {
        public void Configure(EntityTypeBuilder<IncidentMedia> builder)
        {
            builder.ToTable("IncidentMedias");
            builder.Property(x => x.FilePath).HasMaxLength(1024).IsRequired();
            builder.HasIndex(x => x.IncidentId).HasDatabaseName("IX_IncidentMedias_IncidentId");
            builder.HasIndex(x => x.CaptureStartUtc).HasDatabaseName("IX_IncidentMedias_CaptureStartUtc");
        }
    }

    public class ChallanConfiguration : IEntityTypeConfiguration<Challan>
    {
        public void Configure(EntityTypeBuilder<Challan> builder)
        {
            builder.ToTable("Challans");
            builder.Property(x => x.ChallanNumber).HasMaxLength(64);
            builder.Property(x => x.VehicleNumberMasked).HasMaxLength(64);
            builder.HasIndex(x => x.IncidentId).HasDatabaseName("IX_Challans_IncidentId");
            builder.HasIndex(x => x.ChallanNumber).HasDatabaseName("IX_Challans_ChallanNumber");
        }
    }

    public class EmergencyServiceVehicleConfiguration : IEntityTypeConfiguration<EmergencyServiceVehicle>
    {
        public void Configure(EntityTypeBuilder<EmergencyServiceVehicle> builder)
        {
            builder.ToTable("EmergencyServiceVehicles");
            builder.Property(x => x.VehicleType).HasMaxLength(64).IsRequired();
            builder.Property(x => x.VehicleNumber).HasMaxLength(64);
            builder.HasIndex(x => x.VehicleNumber).HasDatabaseName("IX_EmergencyServiceVehicles_VehicleNumber");
        }
    }

    public class IncidentDispatchConfiguration : IEntityTypeConfiguration<IncidentDispatch>
    {
        public void Configure(EntityTypeBuilder<IncidentDispatch> builder)
        {
            builder.ToTable("IncidentDispatches");
            builder.HasIndex(x => x.IncidentId).HasDatabaseName("IX_IncidentDispatches_IncidentId");
            builder.HasIndex(x => x.VehicleId).HasDatabaseName("IX_IncidentDispatches_VehicleId");
        }
    }

    public class EmergencyVehicleHeartbeatConfiguration : IEntityTypeConfiguration<EmergencyVehicleHeartbeat>
    {
        public void Configure(EntityTypeBuilder<EmergencyVehicleHeartbeat> builder)
        {
            builder.ToTable("EmergencyVehicleHeartbeats");
            builder.HasIndex(x => x.VehicleId).HasDatabaseName("IX_EmergencyVehicleHeartbeats_VehicleId");
            builder.HasIndex(x => x.TimestampUtc).HasDatabaseName("IX_EmergencyVehicleHeartbeats_TimestampUtc");
        }
    }

    #endregion

    #region Integration & Telemetry

    public class DeviceIntegrationLogConfiguration : IEntityTypeConfiguration<DeviceIntegrationLog>
    {
        public void Configure(EntityTypeBuilder<DeviceIntegrationLog> builder)
        {
            builder.ToTable("DeviceIntegrationLogs");
            builder.Property(x => x.MessageType).HasMaxLength(128);
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_DeviceIntegrationLogs_InstalledEquipmentId");
            builder.HasIndex(x => x.ReceivedAtUtc).HasDatabaseName("IX_DeviceIntegrationLogs_ReceivedAtUtc");
        }
    }

    public class DeviceHealthLogConfiguration : IEntityTypeConfiguration<DeviceHealthLog>
    {
        public void Configure(EntityTypeBuilder<DeviceHealthLog> builder)
        {
            builder.ToTable("DeviceHealthLogs");
            builder.Property(x => x.Status).HasMaxLength(64);
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_DeviceHealthLogs_InstalledEquipmentId");
            builder.HasIndex(x => x.TimestampUtc).HasDatabaseName("IX_DeviceHealthLogs_TimestampUtc");
        }
    }

    public class VmsMessageScheduleConfiguration : IEntityTypeConfiguration<VmsMessageSchedule>
    {
        public void Configure(EntityTypeBuilder<VmsMessageSchedule> builder)
        {
            builder.ToTable("VmsMessageSchedules");
            builder.Property(x => x.MessageText).HasMaxLength(1024).IsRequired();
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_VmsMessageSchedules_InstalledEquipmentId");
            builder.HasIndex(x => new { x.StartTimeUtc, x.EndTimeUtc }).HasDatabaseName("IX_VmsMessageSchedules_Window");
        }
    }

    public class AnprEventConfiguration : IEntityTypeConfiguration<AnprEvent>
    {
        public void Configure(EntityTypeBuilder<AnprEvent> builder)
        {
            builder.ToTable("AnprEvents");
            builder.Property(x => x.PlateNumberMasked).HasMaxLength(64);
            builder.Property(x => x.PlateNumberHash).HasMaxLength(128);
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_AnprEvents_InstalledEquipmentId");
            builder.HasIndex(x => x.CapturedAtUtc).HasDatabaseName("IX_AnprEvents_CapturedAtUtc");
        }
    }

    public class EcbCallLogConfiguration : IEntityTypeConfiguration<EcbCallLog>
    {
        public void Configure(EntityTypeBuilder<EcbCallLog> builder)
        {
            builder.ToTable("EcbCallLogs");
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_EcbCallLogs_InstalledEquipmentId");
            builder.HasIndex(x => x.CallStartUtc).HasDatabaseName("IX_EcbCallLogs_CallStartUtc");
        }
    }

    public class TrafficSensorDataConfiguration : IEntityTypeConfiguration<TrafficSensorData>
    {
        public void Configure(EntityTypeBuilder<TrafficSensorData> builder)
        {
            builder.ToTable("TrafficSensorData");
            builder.HasIndex(x => x.InstalledEquipmentId).HasDatabaseName("IX_TrafficSensorData_InstalledEquipmentId");
            builder.HasIndex(x => x.TimestampUtc).HasDatabaseName("IX_TrafficSensorData_TimestampUtc");
        }
    }

    #endregion

    #region Analytics & Admin

    public class SlaEventConfiguration : IEntityTypeConfiguration<SlaEvent>
    {
        public void Configure(EntityTypeBuilder<SlaEvent> builder)
        {
            builder.ToTable("SlaEvents");
            builder.HasIndex(x => x.EntityType).HasDatabaseName("IX_SlaEvents_EntityType");
            builder.HasIndex(x => x.TimestampUtc).HasDatabaseName("IX_SlaEvents_TimestampUtc");
        }
    }

    public class IncidentReportConfiguration : IEntityTypeConfiguration<IncidentReport>
    {
        public void Configure(EntityTypeBuilder<IncidentReport> builder)
        {
            builder.ToTable("IncidentReports");
            builder.HasIndex(x => x.IncidentId).HasDatabaseName("IX_IncidentReports_IncidentId");
        }
    }

    public class DisasterEventConfiguration : IEntityTypeConfiguration<DisasterEvent>
    {
        public void Configure(EntityTypeBuilder<DisasterEvent> builder)
        {
            builder.ToTable("DisasterEvents");
            builder.Property(x => x.EventType).HasMaxLength(128).IsRequired();
            builder.HasIndex(x => x.StartedAtUtc).HasDatabaseName("IX_DisasterEvents_StartedAtUtc");
        }
    }

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.Property(x => x.NotificationType).HasMaxLength(64);
            builder.Property(x => x.UserId).HasMaxLength(128);
            builder.HasIndex(x => x.UserId).HasDatabaseName("IX_Notifications_UserId");
            builder.HasIndex(x => x.TimestampUtc).HasDatabaseName("IX_Notifications_TimestampUtc");
        }
    }

    public class ShiftLogConfiguration : IEntityTypeConfiguration<ShiftLog>
    {
        public void Configure(EntityTypeBuilder<ShiftLog> builder)
        {
            builder.ToTable("ShiftLogs");
            builder.Property(x => x.UserId).HasMaxLength(128);
            builder.HasIndex(x => x.UserId).HasDatabaseName("IX_ShiftLogs_UserId");
            builder.HasIndex(x => x.ShiftStartUtc).HasDatabaseName("IX_ShiftLogs_ShiftStartUtc");
        }
    }

    public class SystemAuditLogConfiguration : IEntityTypeConfiguration<SystemAuditLog>
    {
        public void Configure(EntityTypeBuilder<SystemAuditLog> builder)
        {
            builder.ToTable("SystemAuditLogs");
            builder.Property(x => x.UserId).HasMaxLength(128);
            builder.Property(x => x.ActionType).HasMaxLength(64);
            builder.Property(x => x.EntityAffected).HasMaxLength(128);
            builder.Property(x => x.EntityId).HasMaxLength(64);
            builder.Property(x => x.CorrelationId).HasMaxLength(64);
            builder.Property(x => x.TenantId).HasMaxLength(64);

            builder.HasIndex(x => x.UserId).HasDatabaseName("IX_SystemAuditLogs_UserId");
            builder.HasIndex(x => x.ActionType).HasDatabaseName("IX_SystemAuditLogs_ActionType");
            builder.HasIndex(x => x.EntityAffected).HasDatabaseName("IX_SystemAuditLogs_EntityAffected");
            builder.HasIndex(x => x.TimestampUtc).HasDatabaseName("IX_SystemAuditLogs_TimestampUtc");
            builder.HasIndex(x => new { x.UserId, x.TimestampUtc }).HasDatabaseName("IX_SystemAuditLogs_UserId_TimestampUtc");
            builder.HasIndex(x => new { x.TenantId, x.TimestampUtc }).HasDatabaseName("IX_SystemAuditLogs_TenantId_TimestampUtc");
        }
    }

    public class ScheduledJobConfiguration : IEntityTypeConfiguration<ScheduledJob>
    {
        public void Configure(EntityTypeBuilder<ScheduledJob> builder)
        {
            builder.ToTable("ScheduledJobs");
            builder.Property(x => x.JobName).HasMaxLength(128).IsRequired();
            builder.HasIndex(x => x.JobName).HasDatabaseName("IX_ScheduledJobs_JobName");
        }
    }

    #endregion
}
