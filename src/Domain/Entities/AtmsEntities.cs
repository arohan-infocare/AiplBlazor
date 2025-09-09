// File: AtmsEntities.cs
// Namespace: ATMS.Domain.Entities
// Authoritative POCOs for ATMS (RolePermission removed)

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AiplBlazor.Domain.Common.Entities;

namespace AiplBlazor.Domain.Entities

{
    #region --- Value Objects & Enums ---

    public class GeoPoint
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? GeometryWkt { get; set; }
    }

    public enum RoadSide : byte { Unknown = 0, LHS = 1, RHS = 2, Median = 3 }

    public enum EquipmentCategory
    {
        ITSDevice,
        Tolling,
        Civil,
        Safety,
        Communication,
        Power,
        Sensor,
        ControlRoom,
        ITInfrastructure,
        Other
    }

    public enum EquipmentStatus
    {
        InStock,
        Installed,
        Active,
        Standby,
        Faulty,
        InMaintenance,
        Decommissioned
    }

    public enum IncidentType
    {
        Overspeed,
        WrongWay,
        Stoppage,
        IllegalUTurn,
        LaneViolation,
        RedLightViolation,
        Tailgating,
        Overloading,
        IllegalParking,
        OverHeight,
        NoHelmet,
        MobileUse,
        Accident,
        HazardousSpill,
        Fire,
        Other
    }

    public enum IncidentStatus { New, Acknowledged, Verified, Dispatched, InProgress, Resolved, Closed }
    public enum PriorityLevel { Low, Medium, High, Critical }
    public enum MediaType { Image, Video, Audio, Other }
    public enum VehicleStatus { Available, Dispatched, OnScene, Returning, OutOfService }
    public enum MaintenanceType { Fault, Repair, Replacement, Scheduled, Preventive }
    public enum DeviceProtocol { ONVIF, SNMP, MQTT, HTTP, Proprietary }
    public enum AnprEventType { Overspeed, WrongWay, Stoppage, Other }
    public enum DetectionSource { Manual, ANPR, Radar, CameraAI, TrafficSensor, ECB, ThirdParty }

    #endregion

    #region --- Core Topology & Fiber ---

    public class Corridor : BaseAuditableEntityGuid
    {
        [Required] public string CorridorCode { get; set; } = default!;
        public string? CorridorName { get; set; }
        public double? StartChainageKm { get; set; }
        public double? EndChainageKm { get; set; }
        public string? GeoBoundaryWkt { get; set; }
        public string? TenantId { get; set; }
    }

    public class PhysicalSite : BaseAuditableEntityGuid
    {
        [Required] public string SiteCode { get; set; } = default!;
        public string? SiteName { get; set; }
        public string? SiteType { get; set; } // TollPlaza, PoP, Warehouse
        public Guid CorridorId { get; set; }
        public string? TenantId { get; set; }
    }

    public class EquipmentLocation : BaseAuditableEntityGuid
    {
        [Required] public Guid CorridorId { get; set; }
        public double ChainageKm { get; set; }
        public RoadSide RoadSide { get; set; } = RoadSide.Unknown;
        public GeoPoint Position { get; set; } = new GeoPoint();
        public string? LocationName { get; set; }
        public string? TenantId { get; set; }
    }

    public class FiberChamber : BaseAuditableEntityGuid
    {
        [Required] public string ChamberCode { get; set; } = default!;
        public Guid CorridorId { get; set; }
        public double ChainageKm { get; set; }
        public string? ChamberType { get; set; } // StraightJoint / FieldChamber
        public int? CapacityCores { get; set; }
        public int? UsedCores { get; set; }
        public string? TenantId { get; set; }
    }

    public class OpticalFiberSegment : BaseAuditableEntityGuid
    {
        [Required] public string CableType { get; set; } = default!;
        public decimal LengthMeters { get; set; }
        public Guid? FromLocationId { get; set; }
        public Guid? ToLocationId { get; set; }
        public int SpliceCount { get; set; }
        public string? TenantId { get; set; }
    }

    #endregion

    #region --- Equipment & BOM ---

    public class Equipment : BaseAuditableEntity
    {
        [Required] public string ModelNumber { get; set; } = default!;
        public string? Manufacturer { get; set; }
        [Required] public string EquipmentType { get; set; } = default!;
        public EquipmentCategory Category { get; set; } = EquipmentCategory.ITSDevice;
        public double? OperatingVoltageDesign { get; set; }
        public double? PowerConsumptionDesignW { get; set; }
        public string? Description { get; set; }
    }

    public class EquipmentBoMTemplate : BaseAuditableEntity
    {
        [Required] public int EquipmentId { get; set; }
        [Required] public string ComponentName { get; set; } = default!;
        public int Quantity { get; set; } = 1;
        public int? SubEquipmentId { get; set; } // hierarchical BOM
        public double? OperatingVoltageDesign { get; set; }
        public double? PowerConsumptionDesignW { get; set; }
        public bool SurgeProtectionRequired { get; set; } = false;
    }

    public class LocationBoM : BaseAuditableEntityGuid
    {
        public Guid LocationId { get; set; }
        public int EquipmentId { get; set; }
        public int? EquipmentBoMTemplateId { get; set; }
        [Required] public string ComponentName { get; set; } = default!;
        public int Quantity { get; set; } = 1;
        public string Status { get; set; } = "Planned";
        public string? TenantId { get; set; }
    }

    public class InstalledEquipment : BaseAuditableSoftDeleteEntityGuid
    {
        [Required] public int EquipmentId { get; set; }
        public Guid LocationId { get; set; }
        public RoadSide RoadSide { get; set; }
        public DateTime? InstallationDateUtc { get; set; }
        public string? InstalledByUserId { get; set; }
        public EquipmentStatus Status { get; set; }
        public string? IpAddress { get; set; }
        public string? MacAddress { get; set; }
        public string? AssetTag { get; set; }
        public string? TenantId { get; set; }
    }

    public class AssetLifecycleEvent : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public string? EventType { get; set; }
        public DateTime EventDateUtc { get; set; }
        public string? Details { get; set; }
        public string? TenantId { get; set; }
    }

    public class AssetTransfer : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public Guid FromLocationId { get; set; }
        public Guid ToLocationId { get; set; }
        public DateTime TransferDateUtc { get; set; }
        public string? PerformedByUserId { get; set; }
        public string? TenantId { get; set; }
    }

    public class Warranty : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public string? Provider { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime ExpiryUtc { get; set; }
        public string? WarrantyDocUrl { get; set; }
        public string? TenantId { get; set; }
    }

    #endregion

    #region --- Procurement & BOQ ---

    public class Project : BaseAuditableEntityGuid
    {
        public string ProjectCode { get; set; } = default!;
        public string? ProjectName { get; set; }
        public string? TenantId { get; set; }
    }

    public class Boq : BaseAuditableEntityGuid
    {
        public Guid ProjectId { get; set; }
        public string? Title { get; set; }
        public string? DocumentRef { get; set; }
        public string? Currency { get; set; } = "INR";
        public string? TenantId { get; set; }
    }

    public class BoqItem : BaseAuditableEntityGuid
    {
        public Guid BoqId { get; set; }
        public string Section { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Make { get; set; }
        public string Uom { get; set; } = default!;
        public decimal Quantity { get; set; }
        public int? EquipmentId { get; set; }
        public int? EquipmentBoMTemplateId { get; set; }
        public Guid? TargetLocationId { get; set; }
        public decimal? UnitRate { get; set; }
        public decimal? TotalRate { get; set; }
        public string? TenantId { get; set; }
    }

    public class Supplier : BaseAuditableEntityGuid
    {
        [Required] public string Name { get; set; } = default!;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? TenantId { get; set; }
    }

    public class PurchaseOrder : BaseAuditableEntityGuid
    {
        [Required] public string OrderNumber { get; set; } = default!;
        public Guid SupplierId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime OrderedAtUtc { get; set; }
        public string? Status { get; set; }
        public string? TenantId { get; set; }
    }

    public class PurchaseOrderLine : BaseAuditableEntityGuid
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid? BoqItemId { get; set; }
        public string Description { get; set; } = default!;
        public string Uom { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal? UnitRate { get; set; }
    }

    public class GoodsReceipt : BaseAuditableEntityGuid
    {
        public string ReceiptNumber { get; set; } = default!;
        public Guid PurchaseOrderId { get; set; }
        public DateTime ReceivedAtUtc { get; set; }
        public string? TenantId { get; set; }
    }

    public class ServiceContract : BaseAuditableEntityGuid
    {
        public string ContractNumber { get; set; } = default!;
        public Guid? ProjectId { get; set; }
        public Guid? SupplierId { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public decimal? TotalValue { get; set; }
        public string? TenantId { get; set; }
    }

    #endregion

    #region --- Installation & Maintenance ---

    public class InstallationWorkOrder : BaseAuditableEntityGuid
    {
        public string WorkOrderNumber { get; set; } = default!;
        public Guid? PurchaseOrderId { get; set; }
        public DateTime ScheduledStartUtc { get; set; }
        public string? Status { get; set; }
        public string? TenantId { get; set; }
    }

    public class InstallationActivity : BaseAuditableEntityGuid
    {
        public Guid WorkOrderId { get; set; }
        public Guid? InstalledEquipmentId { get; set; }
        public string ActivityType { get; set; } = default!;
        public DateTime ActivityStartUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ActivityEndUtc { get; set; }
        public string? PerformedByUserId { get; set; }
        public string? TenantId { get; set; }
    }

    public class InstallationChecklistItem : BaseAuditableEntityGuid
    {
        public Guid WorkOrderId { get; set; }
        public string StepCode { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAtUtc { get; set; }
        public string? TenantId { get; set; }
    }

    public class MaintenanceLog : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public MaintenanceType LogType { get; set; }
        public DateTime LogDateUtc { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public string? TenantId { get; set; }
    }

    #endregion

    #region --- Incidents & Enforcement ---

    public class Incident : BaseAuditableEntityGuid
    {
        public string IncidentNumber { get; set; } = default!;
        public Guid? LocationId { get; set; }
        public IncidentType IncidentType { get; set; }
        public IncidentStatus Status { get; set; } = IncidentStatus.New;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        public DateTime DetectedAtUtc { get; set; } = DateTime.UtcNow;
        public Guid? DetectedByDeviceId { get; set; }
        public string? TenantId { get; set; }
    }

    public class IncidentMedia : BaseAuditableEntityGuid
    {
        public Guid IncidentId { get; set; }
        public MediaType MediaType { get; set; }
        public string FilePath { get; set; } = default!;
        public long FileSizeBytes { get; set; }
        public string? Sha256HashHex { get; set; }
        public DateTime CaptureStartUtc { get; set; }
        public DateTime CaptureEndUtc { get; set; }
        public bool LegalHold { get; set; }
        public string? TenantId { get; set; }
    }

    public class Challan : BaseAuditableEntityGuid
    {
        public Guid IncidentId { get; set; }
        public string? ChallanNumber { get; set; }
        public string? VehicleNumberMasked { get; set; }
        public string? ViolationType { get; set; }
        public DateTime? IssueDateUtc { get; set; }
        public string? Status { get; set; }
        public string? TenantId { get; set; }
    }

    public class EmergencyServiceVehicle : BaseAuditableEntityGuid
    {
        public string VehicleType { get; set; } = default!;
        public string? VehicleNumber { get; set; }
        public VehicleStatus Status { get; set; }
        public string? DriverName { get; set; }
        public string? TenantId { get; set; }
    }

    public class IncidentDispatch : BaseAuditableEntityGuid
    {
        public Guid IncidentId { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime DispatchTimestampUtc { get; set; }
        public string? TenantId { get; set; }
    }

    public class EmergencyVehicleHeartbeat : BaseAuditableEntityGuid
    {
        public Guid VehicleId { get; set; }
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public double? SpeedKmph { get; set; }
        public VehicleStatus Status { get; set; }
        public string? TenantId { get; set; }
    }

    #endregion

    #region --- Integration & Telemetry ---

    public class DeviceIntegrationLog : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public DeviceProtocol Protocol { get; set; }
        public string? MessageType { get; set; }
        public string? Payload { get; set; }
        public DateTime ReceivedAtUtc { get; set; }
        public string? TenantId { get; set; }
    }

    public class DeviceHealthLog : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = default!;
        public double? CpuUsagePercent { get; set; }
        public double? MemoryUsagePercent { get; set; }
        public string? TenantId { get; set; }
    }

    public class VmsMessageSchedule : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public string MessageText { get; set; } = default!;
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public Guid? IncidentId { get; set; }
        public string? TenantId { get; set; }
    }

    public class AnprEvent : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public string? PlateNumberMasked { get; set; }
        public string? PlateNumberHash { get; set; }
        public double? SpeedKmph { get; set; }
        public AnprEventType EventType { get; set; }
        public DateTime CapturedAtUtc { get; set; }
        public Guid? IncidentId { get; set; }
        public string? TenantId { get; set; }
    }

    public class EcbCallLog : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public DateTime CallStartUtc { get; set; }
        public DateTime? CallEndUtc { get; set; }
        public string? Status { get; set; }
        public Guid? LinkedIncidentId { get; set; }
        public string? TenantId { get; set; }
    }

    public class TrafficSensorData : BaseAuditableEntityGuid
    {
        public Guid InstalledEquipmentId { get; set; }
        public DateTime TimestampUtc { get; set; }
        public int VehicleCount { get; set; }
        public double? AverageSpeedKmph { get; set; }
        public string? TenantId { get; set; }
    }

    #endregion

    #region --- Analytics & Admin ---

    public class SlaEvent : BaseAuditableEntityGuid
    {
        public string? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string? EventName { get; set; }
        public DateTime TimestampUtc { get; set; }
        public long? DurationMs { get; set; }
        public string? TenantId { get; set; }
    }

    public class IncidentReport : BaseAuditableEntityGuid
    {
        public Guid IncidentId { get; set; }
        public DateTime ReportCreatedAtUtc { get; set; }
        public string? Summary { get; set; }
        public string? TenantId { get; set; }
    }

    public class DisasterEvent : BaseAuditableEntityGuid
    {
        public string EventType { get; set; } = default!;
        public int SeverityLevel { get; set; }
        public Guid? LinkedIncidentId { get; set; }
        public DateTime StartedAtUtc { get; set; }
        public DateTime? ClearedAtUtc { get; set; }
        public string? TenantId { get; set; }
    }

    public class Notification : BaseAuditableEntityGuid
    {
        public string? UserId { get; set; }
        public string? NotificationType { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string? TenantId { get; set; }
    }

    public class ShiftLog : BaseAuditableEntityGuid
    {
        public string? UserId { get; set; }
        public DateTime ShiftStartUtc { get; set; }
        public DateTime? ShiftEndUtc { get; set; }
        public string? RoleDuringShift { get; set; }
        public string? TenantId { get; set; }
    }

    public class SystemAuditLog : BaseAuditableEntityGuid
    {
        [MaxLength(128)]
        public string? UserId { get; set; }

        [MaxLength(64)]
        public string? ActionType { get; set; }

        [MaxLength(128)]
        public string? EntityAffected { get; set; }

        [MaxLength(64)]
        public string? EntityId { get; set; }

        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        public string? Details { get; set; }

        [MaxLength(64)]
        public string? CorrelationId { get; set; }

        [MaxLength(64)]
        public string? TenantId { get; set; }
    }

    public class ScheduledJob : BaseAuditableEntityGuid
    {
        public string JobName { get; set; } = default!;
        public DateTime? LastRunAt { get; set; }
        public DateTime? NextRunAt { get; set; }
        public string? Status { get; set; }
        public string? LastResult { get; set; }
    }

    #endregion
}
