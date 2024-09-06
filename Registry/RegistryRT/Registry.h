/*
MIT License

Copyright (c) 2019 Gustave Monce - @gus33000 - gus33000.me

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#pragma once
#include <collection.h>
#include <windows.h>
#include "NativeRegistry.h"

namespace RegistryRT {

public
enum class RegistryType
{
    None = REG_NONE,
    String = REG_SZ,
    VariableString = REG_EXPAND_SZ,
    Binary = REG_BINARY,
    Integer = REG_DWORD,
    IntegerBigEndian = REG_DWORD_BIG_ENDIAN,
    SymbolicLink = REG_LINK,
    MultiString = REG_MULTI_SZ,
    ResourceList = REG_RESOURCE_LIST,
    HardwareResourceList = REG_FULL_RESOURCE_DESCRIPTOR,
    ResourceRequirement = REG_RESOURCE_REQUIREMENTS_LIST,
    Long = REG_QWORD
};

#undef HKEY_CLASSES_ROOT
#undef HKEY_CURRENT_USER
#undef HKEY_LOCAL_MACHINE
#undef HKEY_USERS
#undef HKEY_CURRENT_CONFIG
#undef HKEY_CURRENT_USER_LOCAL_SETTINGS

public
enum class RegistryHive
{
    HKEY_CLASSES_ROOT,
    HKEY_CURRENT_USER,
    HKEY_LOCAL_MACHINE,
    HKEY_USERS,
    HKEY_CURRENT_CONFIG,
    HKEY_CURRENT_USER_LOCAL_SETTINGS
};

public
ref class Registry sealed
{
private:
    property LPGETTOKENINFORMATION GetTokenInformation;
    property LPOPENPROCESSTOKEN OpenProcessToken;
    property LPOPENTHREADTOKEN OpenThreadToken;
    property LPNTCREATEKEY NtCreateKey;
    property LPNTOPENKEY NtOpenKey;
    property LPNTDELETEKEY NtDeleteKey;
    property LPNTFLUSHKEY NtFlushKey;
    property LPNTQUERYKEY NtQueryKey;
    property LPNTENUMERATEKEY NtEnumerateKey;
    property LPNTSETVALUEKEY NtSetValueKey;
    property LPNTSETINFORMATIONKEY NtSetInformationKey;
    property LPNTQUERYVALUEKEY NtQueryValueKey;
    property LPNTENUMERATEVALUEKEY NtEnumerateValueKey;
    property LPNTDELETEVALUEKEY NtDeleteValueKey;
    property LPNTQUERYMULTIPLEVALUEKEY NtQueryMultipleValueKey;
    property LPNTRENAMEKEY NtRenameKey;
    property LPNTCOMPACTKEYS NtCompactKeys;
    property LPNTCOMPRESSKEY NtCompressKey;
    property LPNTLOCKREGISTRYKEY NtLockRegistryKey;
    property LPNTQUERYOPENSUBKEYSEX NtQueryOpenSubKeysEx;
    property LPNTSAVEKEYEX NtSaveKeyEx;
    property LPNTLOADKEYEX NtLoadKeyEx;
    property LPNTUNLOADKEY2 NtUnloadKey2;
    property LPNTUNLOADKEYEX NtUnloadKeyEx;
    property LPNTSAVEKEY NtSaveKey;
    property LPNTRESTOREKEY NtRestoreKey;
    property LPNTLOADKEY NtLoadKey;
    property LPNTLOADKEY2 NtLoadKey2;
    property LPNTREPLACEKEY NtReplaceKey;
    property LPNTUNLOADKEY NtUnloadKey;
    property LPNTCLOSE NtClose;
    property LPNTNOTIFYCHANGEKEY NtNotifyChangeKey;
    property LPNTOPENTHREAD NtOpenThread;
    property LPNTCREATEFILE NtCreateFile;
    property LPNTOPENPROCESSTOKEN NtOpenProcessToken;
    property LPNTADJUSTPRIVILEGESTOKEN NtAdjustPrivilegesToken;
    property LPNTQUERYINFORMATIONTOKEN NtQueryInformationToken;
    property LPRTLINITSTRING RtlInitString;
    property LPRTLINITANSISTRING RtlInitAnsiString;
    property LPRTLINITUNICODESTRING RtlInitUnicodeString;
    property LPRTLANSISTRINGTOUNICODESTRING RtlAnsiStringToUnicodeString;
    property LPRTLUNICODESTRINGTOANSISTRING RtlUnicodeStringToAnsiString;
    property LPRTLFREEANSISTRING RtlFreeAnsiString;
    property LPRTLFREEUNICODESTRING RtlFreeUnicodeString;
    property LPRTLCONVERTSIDTOUNICODESTRING RtlConvertSidToUnicodeString;
    property LPRTLALLOCATEHEAP RtlAllocateHeap;
    property LPRTLFREEHEAP RtlFreeHeap;
    property LPNTQUERYOBJECT NtQueryObject;

public:
    Registry();

    RegistryType Registry::GetValueInfo(RegistryHive Hive, Platform::String ^ Key, Platform::String ^ Name, int nSize);

    Platform::Boolean Registry::GetSubKeyList(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::Array<Platform::String ^> ^ *csaSubkeys);
    Platform::Boolean
    Registry::GetValueList(RegistryHive Hive, Platform::String ^ Key, Platform::Array<Platform::String ^> ^ *csaValues);

    Platform::Boolean Registry::ValueExists(RegistryHive Hive, Platform::String ^ Key, Platform::String ^ Name);

    Platform::Boolean Registry::WriteValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        const Platform::Array<uint8> ^ csaValue,
        RegistryType type);
    Platform::Boolean Registry::QueryValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        RegistryType *RegType,
        Platform::Array<uint8> ^ *RetBuffer);

    Platform::Boolean Registry::RenameKey(RegistryHive Hive, Platform::String ^ Key, Platform::String ^ csNewKeyName);

    Platform::Boolean Registry::DeleteKey(RegistryHive Hive, Platform::String ^ Key);
    Platform::Boolean Registry::DeleteKeysRecursive(RegistryHive Hive, Platform::String ^ Key);

    Platform::Boolean Registry::CreateKey(RegistryHive Hive, Platform::String ^ Key);

    Platform::Boolean Registry::DeleteValue(RegistryHive Hive, Platform::String ^ Key, Platform::String ^ Name);
    unsigned int Registry::GetKeyStatus(RegistryHive Hive, Platform::String ^ Key);
    Platform::Boolean Registry::FindHiddenKeys(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::Array<Platform::String ^> ^ *csaSubkeys);
    Platform::Boolean Registry::IsKeyHidden(RegistryHive Hive, Platform::String ^ Key);
    Platform::Boolean Registry::GetKeyLastWriteTime(RegistryHive Hive, Platform::String ^ Key, int64 *LastWriteTime);

    // Customs
    Platform::Boolean Registry::WriteValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        const Platform::Array<uint8> ^ csaValue,
        uint32 type);
    unsigned int Registry::GetValueInfo2(RegistryHive Hive, Platform::String ^ Key, Platform::String ^ Name, int nSize);
    Platform::Boolean Registry::QueryValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        uint32 *RegType,
        Platform::Array<uint8> ^ *RetBuffer);

    Platform::Boolean
    Registry::LoadHive(Platform::String ^ HiveFile, Platform::String ^ MountName, Platform::Boolean InUser);
    Platform::Boolean Registry::UnloadHive(Platform::String ^ KeyPath, Platform::Boolean InUser);

private:
    Platform::String ^ Registry::GetCurrentUserSID();
    Platform::String ^ Registry::GetRootPathFor(RegistryHive hRoot);
    BOOL Registry::ReadValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        RegistryType RegType,
        KEY_VALUE_PARTIAL_INFORMATION **retInfo);
    Platform::Boolean Registry::WriteValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        PVOID pValue,
        ULONG ulValueLength,
        RegistryType dwRegType);

    // Customs
    Platform::Boolean Registry::WriteValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        PVOID pValue,
        ULONG ulValueLength,
        DWORD dwRegType);
    BOOL Registry::ReadValue(
        RegistryHive Hive,
        Platform::String ^ Key,
        Platform::String ^ Name,
        uint32 RegType,
        KEY_VALUE_PARTIAL_INFORMATION **retInfo);
};
} // namespace RegistryRT
