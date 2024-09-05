#pragma once

namespace VelocityPillRT
{
	typedef struct _WNF_STATE_NAME
	{
		ULONG Data[2];
	} WNF_STATE_NAME, *PWNF_STATE_NAME;

	typedef const WNF_STATE_NAME *PCWNF_STATE_NAME;

	typedef enum _WNF_STATE_NAME_LIFETIME
	{
		WnfWellKnownStateName,
		WnfPermanentStateName,
		WnfPersistentStateName,
		WnfTemporaryStateName
	} WNF_STATE_NAME_LIFETIME;

	typedef enum _WNF_STATE_NAME_INFORMATION
	{
		WnfInfoStateNameExist,
		WnfInfoSubscribersPresent,
		WnfInfoIsQuiescent
	} WNF_STATE_NAME_INFORMATION;

	typedef enum _WNF_DATA_SCOPE
	{
		WnfDataScopeSystem,
		WnfDataScopeSession,
		WnfDataScopeUser,
		WnfDataScopeProcess
	} WNF_DATA_SCOPE;

	typedef struct _WNF_TYPE_ID
	{
		GUID TypeId;
	} WNF_TYPE_ID, *PWNF_TYPE_ID;

	typedef const WNF_TYPE_ID *PCWNF_TYPE_ID;

	// rev
	typedef ULONG WNF_CHANGE_STAMP, *PWNF_CHANGE_STAMP;

	typedef struct _WNF_DELIVERY_DESCRIPTOR
	{
		ULONGLONG SubscriptionId;
		WNF_STATE_NAME StateName;
		WNF_CHANGE_STAMP ChangeStamp;
		ULONG StateDataSize;
		ULONG EventMask;
		WNF_TYPE_ID TypeId;
		ULONG StateDataOffset;
	} WNF_DELIVERY_DESCRIPTOR, *PWNF_DELIVERY_DESCRIPTOR;

	// NtUpdateWnfStateData
	typedef NTSTATUS(STDAPICALLTYPE NTUPDATEWNFSTATEDATA)
		(
			_In_ PCWNF_STATE_NAME StateName,
			_In_reads_bytes_opt_(Length) const VOID *Buffer,
			_In_opt_ ULONG Length,
			_In_opt_ PCWNF_TYPE_ID TypeId,
			_In_opt_ const VOID *ExplicitScope,
			_In_ WNF_CHANGE_STAMP MatchingChangeStamp,
			_In_ ULONG CheckStamp
			);
	typedef NTUPDATEWNFSTATEDATA FAR * LPNTUPDATEWNFSTATEDATA;

	// NtQueryWnfStateData
	typedef NTSTATUS(STDAPICALLTYPE NTQUERYWNFSTATEDATA)
		(
			_In_ PCWNF_STATE_NAME StateName,
			_In_opt_ PCWNF_TYPE_ID TypeId,
			_In_opt_ const VOID *ExplicitScope,
			_Out_ PWNF_CHANGE_STAMP ChangeStamp,
			_Out_writes_bytes_to_opt_(*BufferSize, *BufferSize) PVOID Buffer,
			_Inout_ PULONG BufferSize
			);
	typedef NTQUERYWNFSTATEDATA FAR * LPNTQUERYWNFSTATEDATA;

    public ref class VelocityPill sealed
    {

	private:
		LPNTUPDATEWNFSTATEDATA NtUpdateWnfStateData;
		LPNTQUERYWNFSTATEDATA NtQueryWnfStateData;

    public:
        VelocityPill();
		int VelocityPill::GetFeatureState(Platform::String^ featurestate, Platform::Array<uint8>^* result);
		int VelocityPill::SetFeatureState(Platform::String^ featurestate, const Platform::Array<uint8>^ state);
    };
}
