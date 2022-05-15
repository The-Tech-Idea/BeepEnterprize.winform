﻿using System;

namespace KOC.DHUB3.Models
{
    public class cls_Wide_workover
    {
        public cls_Wide_workover()
        {

        }
        public DateTime? WaterActionDate { get; set; }
        public string WaterAvailable { get; set; }
        public string WellBudgetName { get; set; }
        public string WellBudgetYear { get; set; }
        public string WellObjective { get; set; }
        public string WellProfileTarget { get; set; }
        public string ConnectionType { get; set; }
        public string ProgramReady { get; set; }
        public string WSRemarks { get; set; }
        public double ExpGasGainMSCFPD2 { get; set; }
        public double ExpGasRestoreMSCFPD { get; set; }
        public DateTime? ExpectedHookupDate { get; set; }
        public DateTime? ExpectedOpenDate { get; set; }
        public double ExpGasGainandRestore { get; set; }
        public double ExpWaterGainandRestore { get; set; }
        public DateTime? ScheduledFinishDate { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public double GainRestoreDifference { get; set; }
        public double GasRateBeforeMSCFPD { get; set; }
        public double MovementDuration { get; set; }
        public double OperationalDurationOverrun { get; set; }
        public string RigRelatedComments { get; set; }
        public double TestedGasRateAfter { get; set; }
        public string RigDrillingCategory { get; set; }
        public string RigCategory { get; set; }
        public string FieldOriginal { get; set; }
        public double DrillingWOTypeOriginal { get; set; }
        public string ReservoirOriginal { get; set; }
        public double CompletionTypeOriginal { get; set; }
        public double TrajectoryTypeOriginal { get; set; }
        public double WellObjectiveOriginal { get; set; }
        public double WorkoverNo { get; set; }
        public double CompletionActivityExists { get; set; }
        public string GPRemarks { get; set; }
        public string PORemarks { get; set; }
        public string ProgramStatus { get; set; }
        public string LocationConstraintsFeedback { get; set; }
        public string LocationFeedback { get; set; }
        public DateTime? RiglessJobRequestedDate { get; set; }
        public DateTime? RiglessJobFDPlanDate { get; set; }
        public DateTime? RiglessJobWSPlanDate { get; set; }
        public string RiglessJobRequester { get; set; }
        public string RiglessJobZone { get; set; }
        public string RiglessJobWellString { get; set; }
        public string RiglessJobServiceName { get; set; }
        public string RiglessJobServiceCategory { get; set; }
        public string RiglessJobRequestStatus { get; set; }
        public string RiglessJobRequestID { get; set; }
        public string RiglessJobContractorCode { get; set; }
        public double RepTestedOilRateGain { get; set; }
        public double RepOilRateBefore { get; set; }
        public double RepWaterRateBefore { get; set; }
        public double RepGORBeforeSCFSTB { get; set; }
        public double RepTestedOilRateAfter { get; set; }
        public double RepTestedWaterRateAfter { get; set; }
        public double RepTestedGORSCFSTB { get; set; }
        public DateTime? RepTestedRateAfterDate { get; set; }
        public double ActivityNo { get; set; }
        public string ASSET { get; set; }
        public string UWI { get; set; }
        public string OperationalStatus { get; set; }
        public string Activity { get; set; }
        public string SubActivity { get; set; }
        public DateTime? ActualFinishDate { get; set; }
        public DateTime? ActualHookupDate { get; set; }
        public DateTime? ActualOpenDate { get; set; }
        public DateTime? ActualSpudDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public string ALEquipmentStatus { get; set; }
        public string ArtificalLiftContractor { get; set; }
        public string CompletionType { get; set; }
        public string AdditionalWellDetails { get; set; }
        public DateTime? ConstructionActionDate { get; set; }
        public string ConstructionComplete { get; set; }
        public string CoringRequired { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string DrillingEngRemarks { get; set; }
        public string DrillingOpsRemarks { get; set; }
        public string DrillingWOType { get; set; }
        public string DrillPipeRequired { get; set; }
        public string DualComplRigRequired { get; set; }
        public double Easting { get; set; }
        public string ALRequired { get; set; }
        public DateTime? ALEquimentAvailableDate { get; set; }
        public double ExpHookuptoOpen { get; set; }
        public double ExpReleasetoHookup { get; set; }
        public double ExpPriortoStart { get; set; }
        public double ExpGasGainMCSFPD1 { get; set; }
        public double ExpGasRestoreMCSFPD { get; set; }
        public double ExpGORSTFSTB { get; set; }
        public double ExpWaterRestoreBWPD { get; set; }
        public double ScheduleDurationDays { get; set; }
        public double ExpectedOilGainBOPD { get; set; }
        public double ExpectedWaterGainBWPD { get; set; }
        public double ExpOilGainandRestore { get; set; }
        public double ExpOilRestore { get; set; }
        public double FDEstimatedScheduleDuration { get; set; }
        public string FDRemarks { get; set; }
        public string Field { get; set; }
        public string FlowlineContractor { get; set; }
        public string GainRestoreDifferenceReason { get; set; }
        public string GCArea { get; set; }
        public string GCTarget { get; set; }
        public double GORBeforeSCFSTB { get; set; }
        public string GP_Maintenance { get; set; }
        public string HeaderHPLP { get; set; }
        public DateTime? LocationClearanceDate { get; set; }
        public DateTime? LocationConstraintDate { get; set; }
        public string LocationConstraintDetails { get; set; }
        public string LocationConstraint { get; set; }
        public string LocationReady { get; set; }
        public string LocationReleased { get; set; }
        public string MaterialAvailable { get; set; }
        public string MaterialPendingforWells { get; set; }
        public string MaterialPendingforWorkovers { get; set; }
        public string MaterialStatus { get; set; }
        public DateTime? MaterialAvailableDate { get; set; }
        public DateTime? MOCApprovedDate { get; set; }
        public string MOCRemarks { get; set; }
        public DateTime? MOCRequestedDate { get; set; }
        public string MOCStatus { get; set; }
        public string MocType { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string MovementDurationReason { get; set; }
        public double NoofTests { get; set; }
        public double Northing { get; set; }
        public string Objectives { get; set; }
        public string OperationalDurationReason { get; set; }
        public string PDDPComplete { get; set; }
        public string PlannedWellCompletion { get; set; }
        public string TwoPPComplete { get; set; }
        public double OilRateBeforeBOPD { get; set; }
        public double WaterRateBeforeBWPD { get; set; }
        public string ProfileLocked { get; set; }
        public string Project { get; set; }
        public string ActivityQuarter { get; set; }
        public DateTime? ReleaseActionDate { get; set; }
        public string Reservoir { get; set; }
        public DateTime? RigExpiryDate { get; set; }
        public string Tracking { get; set; }
        public double RigHP { get; set; }
        public string RigStatus { get; set; }
        public double RigHPRequired { get; set; }
        public string RigName { get; set; }
        public string RigScheduleWellName { get; set; }
        public string RotationRequired { get; set; }
        public double Sequence { get; set; }
        public string Slot { get; set; }
        public string SystemWetDry { get; set; }
        public DateTime? ProgramSubmitted { get; set; }
        public DateTime? JOBNOTCOMPLETESUSPENDED { get; set; }
        public string MOCRequest { get; set; }
        public string CasingPolicy { get; set; }
        public string DIMSNo { get; set; }
        public double TestedGORSCFSTB { get; set; }
        public double TestedOilRateGainBOPD { get; set; }
        public DateTime? TestedRateAfterDate { get; set; }
        public double TestedOilRateAfterBOPD { get; set; }
        public double TestedWaterRateAfterBWPD { get; set; }


    }
}