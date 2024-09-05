﻿// Copyright 2015-2021 (c) Interop Tools Development Team
// This file is licensed to you under the MIT license.

using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

#if ARM
using LGRuntimeComponent;
using System;
using Windows.Security.ExchangeActiveSyncProvisioning;
#endif

namespace RegistryHelper
{
    public sealed class LGRPCProvider : IRegistryProvider
    {
#if ARM
        private RegistryWrapper _lgrpcreg;
        private RpcWrapper _lgrpc;
#endif

        public bool IsSupported() => Initialize();

        private bool Initialize()
        {
#if ARM
            if (_lgrpc != null)
            {
                return true;
            }

            EasClientDeviceInformation deviceInformation = new EasClientDeviceInformation();
            string SystemManufacturer = deviceInformation.SystemManufacturer;

            if (SystemManufacturer.ToLower().Contains("lg"))
            {
                try
                {
                    _lgrpc = RpcWrapper.GetRpcWrapper();
                    _lgrpcreg = new RegistryWrapper();
                    return true;
                }
                catch
                {
                    _lgrpc = null;
                    _lgrpcreg = null;
                    return false;
                }
            }
#endif
            return false;
        }

        public REG_STATUS RegDeleteKey(REG_HIVES hive, string key, bool recursive) => REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegDeleteValue(REG_HIVES hive, string key, string name) => REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegEnumKey(REG_HIVES? hive, string key, out IReadOnlyList<REG_ITEM> items)
        {
            items = new List<REG_ITEM>();
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegQueryMultiString(REG_HIVES hive, string key, string regvalue, out string[] data)
        {
            data = new string[0];
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegQueryVariableString(REG_HIVES hive, string key, string regvalue, out string data)
        {
            data = "";
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegQueryDword(REG_HIVES hive, string key, string regvalue, out uint data)
        {
#if ARM
            try
            {
                bool res = Initialize();
                if (!res)
                {
                    data = uint.MinValue;
                    return REG_STATUS.FAILED;
                }

                if (hive == REG_HIVES.HKEY_LOCAL_MACHINE)
                {
                    data =
 BitConverter.ToUInt32(BitConverter.GetBytes(_lgrpcreg.GetRegistryDWORDValue(key, regvalue)), 0);
                    return REG_STATUS.SUCCESS;
                }
            }
            catch
            {
            }
#endif
            data = uint.MinValue;
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_KEY_STATUS RegQueryKeyStatus(REG_HIVES hive, string key) => REG_KEY_STATUS.UNKNOWN;

        public REG_STATUS RegQueryQword(REG_HIVES hive, string key, string regvalue, out ulong data)
        {
            data = ulong.MinValue;
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegQueryString(REG_HIVES hive, string key, string regvalue, out string data)
        {
#if ARM
            try
            {
                bool res = Initialize();
                if (!res)
                {
                    data = "";
                    return REG_STATUS.FAILED;
                }

                if (hive == REG_HIVES.HKEY_LOCAL_MACHINE)
                {
                    data = _lgrpcreg.GetRegistryValue(key, regvalue);
                    return REG_STATUS.SUCCESS;
                }
            }
            catch
            {
            }
#endif
            data = "";
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegQueryValue(REG_HIVES hive, string key, string regvalue, REG_VALUE_TYPE valtype,
            out REG_VALUE_TYPE outvaltype, out byte[] data)
        {
            data = new byte[0];
            outvaltype = REG_VALUE_TYPE.REG_NONE;
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegSetDword(REG_HIVES hive, string key, string regvalue, uint data)
        {
#if ARM
            try
            {
                bool res = Initialize();
                if (!res)
                {
                    return REG_STATUS.FAILED;
                }

                if (hive == REG_HIVES.HKEY_LOCAL_MACHINE)
                {
                    _lgrpc.RpcSetRegistryDWORDValue(key, regvalue, BitConverter.ToInt32(BitConverter.GetBytes(data), 0));
                    return REG_STATUS.SUCCESS;
                }
            }
            catch
            {
            }
#endif
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS
            RegSetMultiString(REG_HIVES hive, string key, string regvalue, [ReadOnlyArray] string[] data) =>
            REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegSetQword(REG_HIVES hive, string key, string regvalue, ulong data) =>
            REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegSetString(REG_HIVES hive, string key, string regvalue, string data)
        {
#if ARM
            try
            {
                bool res = Initialize();
                if (!res)
                {
                    return REG_STATUS.FAILED;
                }

                if (hive == REG_HIVES.HKEY_LOCAL_MACHINE)
                {
                    _lgrpcreg.AddRegKey(key, regvalue, data);
                    return REG_STATUS.SUCCESS;
                }
            }
            catch
            {
            }
#endif
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegSetValue(REG_HIVES hive, string key, string regvalue, REG_VALUE_TYPE valtype,
            [ReadOnlyArray] byte[] data) => REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegSetVariableString(REG_HIVES hive, string key, string regvalue, string data) =>
            REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegRenameKey(REG_HIVES hive, string key, string newname) => REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegAddKey(REG_HIVES hive, string key) => REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegQueryKeyLastModifiedTime(REG_HIVES hive, string key, out long lastmodified)
        {
            lastmodified = long.MinValue;
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegQueryValue(REG_HIVES hive, string key, string regvalue, uint valtype, out uint outvaltype,
            out byte[] data)
        {
            outvaltype = 0;
            data = new byte[0];
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegSetValue(REG_HIVES hive, string key, string regvalue, uint valtype,
            [ReadOnlyArray] byte[] data) => REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegEnumKey(REG_HIVES? hive, string key, out IReadOnlyList<REG_ITEM_CUSTOM> items)
        {
            items = new List<REG_ITEM_CUSTOM>();
            return REG_STATUS.NOT_IMPLEMENTED;
        }

        public REG_STATUS RegLoadHive(string FilePath, string mountpoint, bool inUser) => REG_STATUS.NOT_IMPLEMENTED;

        public REG_STATUS RegUnloadHive(string mountpoint, bool inUser) => REG_STATUS.NOT_IMPLEMENTED;
    }
}
