#include "pch.h"
#include "VelocityPill.h"
#include <windows.h>
#include <vector>

using namespace VelocityPillRT;
using namespace Platform;

VelocityPill::VelocityPill()
{
	HMODULE ntdll = LoadLibraryW(L"ntdll");

	NtUpdateWnfStateData = (LPNTUPDATEWNFSTATEDATA)GetProcAddress(ntdll, "NtUpdateWnfStateData");
	if (!NtUpdateWnfStateData) {
		throw ref new Platform::COMException(HRESULT_FROM_WIN32(GetLastError()));
	}

	NtQueryWnfStateData = (LPNTQUERYWNFSTATEDATA)GetProcAddress(ntdll, "NtQueryWnfStateData");
	if (!NtQueryWnfStateData) {
		throw ref new Platform::COMException(HRESULT_FROM_WIN32(GetLastError()));
	}
}

int VelocityPill::GetFeatureState(Platform::String^ featurestate, Platform::Array<uint8>^* result)
{
	WNF_STATE_NAME name;

	std::wstring fooW(featurestate->Begin());
	std::string fooA(fooW.begin(), fooW.end());
	const char* featurestatechar = fooA.c_str();

	sscanf_s(featurestatechar, "%llx", (uint64_t*)&name);

	WNF_CHANGE_STAMP a;

	ULONG bufferSize = 0;
	NTSTATUS status = NtQueryWnfStateData(&name, 0, nullptr, &a, nullptr, &bufferSize);

	if (status != 0xc0000023 && status != 0) {
		return status;
	}

	std::vector<uint8_t> buffer(bufferSize);
	status = NtQueryWnfStateData(&name, 0, nullptr, &a, buffer.data(), &bufferSize);

	if (status == 0)
	{
		*result = ref new Platform::Array<uint8>(buffer.data(), buffer.size());
	}

	return 0;
}

#define NT_SUCCESS(Status) (((NTSTATUS)(Status)) >= 0)

int VelocityPill::SetFeatureState(Platform::String^ featurestate, const Platform::Array<uint8>^ state)
{
	WNF_STATE_NAME name;

	std::wstring fooW(featurestate->Begin());
	std::string fooA(fooW.begin(), fooW.end());
	const char* featurestatechar = fooA.c_str();

	sscanf_s(featurestatechar, "%llx", (uint64_t*)&name);

	NTSTATUS status = NtUpdateWnfStateData(&name, state->Data, state->Length, nullptr, nullptr, 0, 0);

	if (!NT_SUCCESS(status))
	{
		return status;
	}

	return 0;
}
