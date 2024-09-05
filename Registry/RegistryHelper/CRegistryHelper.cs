﻿// Copyright 2015-2021 (c) Interop Tools Development Team
// This file is licensed to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;

namespace RegistryHelper
{
    public sealed class CRegistryHelper
    {
        private readonly List<IRegistryProvider> providers = new();

        public CRegistryHelper()
        {
            providers.Add(new RegistryRTProvider());
#if !STORE
            providers.Add(new DevProgramProvider());
            providers.Add(new WinPRTUtilsProvider());

            NDTKRegistryProvider ndtkprov = new();
            if (ndtkprov.IsSupported())
            {
                providers.Add(ndtkprov);
            }

            SAMSUNGRPCProvider samprov = new();
            if (samprov.IsSupported())
            {
                providers.Add(samprov);
            }

            LGRPCProvider lgprov = new();
            if (lgprov.IsSupported())
            {
                providers.Add(lgprov);
            }
#endif
        }

        public REG_STATUS RegEnumKey(REG_HIVES? hive, string key, out IReadOnlyList<REG_ITEM> items)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegEnumKey(hive, key, out IReadOnlyList<REG_ITEM> regitems);
                if (result == REG_STATUS.SUCCESS)
                {
                    items = regitems;
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                items = new List<REG_ITEM>();
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                items = new List<REG_ITEM>();
                return REG_STATUS.FAILED;
            }

            items = new List<REG_ITEM>();
            return REG_STATUS.FAILED;
        }

        [Windows.Foundation.Metadata.DefaultOverload()]
        public REG_STATUS RegEnumKey(REG_HIVES? hive, string key, out IReadOnlyList<REG_ITEM_CUSTOM> items)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegEnumKey(hive, key, out IReadOnlyList<REG_ITEM_CUSTOM> regitems);
                if (result == REG_STATUS.SUCCESS)
                {
                    items = regitems;
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                items = new List<REG_ITEM_CUSTOM>();
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                items = new List<REG_ITEM_CUSTOM>();
                return REG_STATUS.FAILED;
            }

            items = new List<REG_ITEM_CUSTOM>();
            return REG_STATUS.FAILED;
        }

        public REG_STATUS RegAddKey(REG_HIVES hive, string key)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegAddKey(hive, key);
                if (result == REG_STATUS.SUCCESS)
                {
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        public REG_STATUS RegDeleteKey(REG_HIVES hive, string key, bool recursive)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegDeleteKey(hive, key, recursive);
                if (result == REG_STATUS.SUCCESS)
                {
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        public REG_STATUS RegDeleteValue(REG_HIVES hive, string key, string name)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegDeleteValue(hive, key, name);
                if (result == REG_STATUS.SUCCESS)
                {
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        public REG_STATUS RegRenameKey(REG_HIVES hive, string key, string newname)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegRenameKey(hive, key, newname);
                if (result == REG_STATUS.SUCCESS)
                {
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        public REG_KEY_STATUS RegQueryKeyStatus(REG_HIVES hive, string key)
        {
            bool hadaccessdenied = false;
            bool hadunknown = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_KEY_STATUS result = prov.RegQueryKeyStatus(hive, key);
                if (result == REG_KEY_STATUS.FOUND)
                {
                    return result;
                }

                if (result == REG_KEY_STATUS.NOT_FOUND)
                {
                    return result;
                }

                if (result == REG_KEY_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_KEY_STATUS.UNKNOWN)
                {
                    hadunknown = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_KEY_STATUS.ACCESS_DENIED;
            }

            if (hadunknown)
            {
                return REG_KEY_STATUS.UNKNOWN;
            }

            return REG_KEY_STATUS.UNKNOWN;
        }

        public REG_STATUS RegQueryValue(REG_HIVES hive, string key, string regvalue, REG_VALUE_TYPE valtype,
            out REG_VALUE_TYPE outvaltype, out string data)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegQueryValue(hive, key, regvalue, valtype, out REG_VALUE_TYPE valtypetmp,
                    out byte[] datatmp);
                if (result == REG_STATUS.SUCCESS)
                {
                    outvaltype = valtypetmp;
                    data = Convert.RegBufferToString((uint)valtypetmp, datatmp);
                    return result;
                }

                REG_STATUS? singleresult = null;

                switch (valtype)
                {
                    case REG_VALUE_TYPE.REG_DWORD:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryDword(hive, key, regvalue, out uint datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2.ToString();
                                return result2;
                            }

                            break;
                        }
                    case REG_VALUE_TYPE.REG_QWORD:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryQword(hive, key, regvalue, out ulong datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2.ToString();
                                return result2;
                            }

                            break;
                        }
                    case REG_VALUE_TYPE.REG_MULTI_SZ:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryMultiString(hive, key, regvalue, out string[] datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = string.Join("\n", datatmp2);
                                return result2;
                            }

                            break;
                        }
                    case REG_VALUE_TYPE.REG_SZ:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryString(hive, key, regvalue, out string datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2;
                                return result2;
                            }

                            break;
                        }
                    case REG_VALUE_TYPE.REG_EXPAND_SZ:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryVariableString(hive, key, regvalue, out string datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2;
                                return result2;
                            }

                            break;
                        }
                }

                if (singleresult == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (singleresult == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (singleresult == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                    continue;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                outvaltype = REG_VALUE_TYPE.REG_NONE;
                data = "";
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                outvaltype = REG_VALUE_TYPE.REG_NONE;
                data = "";
                return REG_STATUS.FAILED;
            }

            outvaltype = REG_VALUE_TYPE.REG_NONE;
            data = "";
            return REG_STATUS.FAILED;
        }

        [Windows.Foundation.Metadata.DefaultOverload()]
        public REG_STATUS RegQueryValue(REG_HIVES hive, string key, string regvalue, uint valtype, out uint outvaltype,
            out string data)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegQueryValue(hive, key, regvalue, valtype, out uint valtypetmp,
                    out byte[] datatmp);
                if (result == REG_STATUS.SUCCESS)
                {
                    outvaltype = valtypetmp;
                    data = Convert.RegBufferToString((uint)valtypetmp, datatmp);
                    return result;
                }

                REG_STATUS? singleresult = null;

                switch (valtype)
                {
                    case (uint)REG_VALUE_TYPE.REG_DWORD:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryDword(hive, key, regvalue, out uint datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2.ToString();
                                return result2;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_QWORD:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryQword(hive, key, regvalue, out ulong datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2.ToString();
                                return result2;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_MULTI_SZ:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryMultiString(hive, key, regvalue, out string[] datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = string.Join("\n", datatmp2);
                                return result2;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_SZ:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryString(hive, key, regvalue, out string datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2;
                                return result2;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_EXPAND_SZ:
                        {
                            data = "";
                            outvaltype = valtype;
                            REG_STATUS result2 = prov.RegQueryVariableString(hive, key, regvalue, out string datatmp2);

                            singleresult = result2;
                            if (result2 == REG_STATUS.SUCCESS)
                            {
                                data = datatmp2;
                                return result2;
                            }

                            break;
                        }
                }

                if (singleresult == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (singleresult == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (singleresult == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                    continue;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                outvaltype = (uint)REG_VALUE_TYPE.REG_NONE;
                data = "";
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                outvaltype = (uint)REG_VALUE_TYPE.REG_NONE;
                data = "";
                return REG_STATUS.FAILED;
            }

            outvaltype = (uint)REG_VALUE_TYPE.REG_NONE;
            data = "";
            return REG_STATUS.FAILED;
        }

        public REG_STATUS RegSetValue(REG_HIVES hive, string key, string regvalue, REG_VALUE_TYPE valtype, string data)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            try
            {
                foreach (IRegistryProvider prov in providers)
                {
                    REG_STATUS? result = null;
                    switch (valtype)
                    {
                        case REG_VALUE_TYPE.REG_DWORD:
                            {
                                result = prov.RegSetDword(hive, key, regvalue, uint.Parse(data));

                                if (result == REG_STATUS.SUCCESS)
                                {
                                    return (REG_STATUS)result;
                                }

                                break;
                            }
                        case REG_VALUE_TYPE.REG_QWORD:
                            {
                                result = prov.RegSetQword(hive, key, regvalue, ulong.Parse(data));

                                if (result == REG_STATUS.SUCCESS)
                                {
                                    return (REG_STATUS)result;
                                }

                                break;
                            }
                        case REG_VALUE_TYPE.REG_MULTI_SZ:
                            {
                                result = prov.RegSetMultiString(hive, key, regvalue,
                                    data.Replace("\r", "\0").Split('\n'));

                                if (result == REG_STATUS.SUCCESS)
                                {
                                    return (REG_STATUS)result;
                                }

                                break;
                            }
                        case REG_VALUE_TYPE.REG_SZ:
                            {
                                result = prov.RegSetString(hive, key, regvalue, data);

                                if (result == REG_STATUS.SUCCESS)
                                {
                                    return (REG_STATUS)result;
                                }

                                break;
                            }
                        case REG_VALUE_TYPE.REG_EXPAND_SZ:
                            {
                                result = prov.RegSetVariableString(hive, key, regvalue, data);

                                if (result == REG_STATUS.SUCCESS)
                                {
                                    return (REG_STATUS)result;
                                }

                                break;
                            }
                        default:
                            {
                                byte[] buffer = Convert.StringToRegBuffer((uint)valtype, data);

                                result = prov.RegSetValue(hive, key, regvalue, valtype, buffer);
                                if (result == REG_STATUS.SUCCESS)
                                {
                                    return (REG_STATUS)result;
                                }

                                break;
                            }
                    }

                    if (result == REG_STATUS.NOT_IMPLEMENTED)
                    {
                        continue;
                    }

                    if (result == REG_STATUS.ACCESS_DENIED)
                    {
                        hadaccessdenied = true;
                        continue;
                    }

                    if (result == REG_STATUS.FAILED)
                    {
                        hadfailed = true;
                    }
                }
            }
            catch
            {
                hadfailed = true;
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        [Windows.Foundation.Metadata.DefaultOverload()]
        public REG_STATUS RegSetValue(REG_HIVES hive, string key, string regvalue, uint valtype, string data)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS? result = null;
                switch (valtype)
                {
                    case (uint)REG_VALUE_TYPE.REG_DWORD:
                        {
                            result = prov.RegSetDword(hive, key, regvalue, uint.Parse(data));

                            if (result == REG_STATUS.SUCCESS)
                            {
                                return (REG_STATUS)result;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_QWORD:
                        {
                            result = prov.RegSetQword(hive, key, regvalue, ulong.Parse(data));

                            if (result == REG_STATUS.SUCCESS)
                            {
                                return (REG_STATUS)result;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_MULTI_SZ:
                        {
                            result = prov.RegSetMultiString(hive, key, regvalue, data.Replace("\r", "\0").Split('\n'));

                            if (result == REG_STATUS.SUCCESS)
                            {
                                return (REG_STATUS)result;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_SZ:
                        {
                            result = prov.RegSetString(hive, key, regvalue, data);

                            if (result == REG_STATUS.SUCCESS)
                            {
                                return (REG_STATUS)result;
                            }

                            break;
                        }
                    case (uint)REG_VALUE_TYPE.REG_EXPAND_SZ:
                        {
                            result = prov.RegSetVariableString(hive, key, regvalue, data);

                            if (result == REG_STATUS.SUCCESS)
                            {
                                return (REG_STATUS)result;
                            }

                            break;
                        }
                    default:
                        {
                            byte[] buffer = Convert.StringToRegBuffer(valtype, data);

                            result = prov.RegSetValue(hive, key, regvalue, valtype, buffer);
                            if (result == REG_STATUS.SUCCESS)
                            {
                                return (REG_STATUS)result;
                            }

                            break;
                        }
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        public bool DoesFileExists(string path)
        {
            bool fileexists;
            try
            {
                fileexists = File.Exists(path);
            }
            catch (InvalidOperationException)
            {
                fileexists = true;
            }

            return fileexists;
        }

        public REG_STATUS RegLoadHive(string FilePath, string mountpoint, bool inUser)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegLoadHive(FilePath, mountpoint, inUser);
                if (result == REG_STATUS.SUCCESS)
                {
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        public REG_STATUS RegUnloadHive(string mountpoint, bool inUser)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegUnloadHive(mountpoint, inUser);
                if (result == REG_STATUS.SUCCESS)
                {
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                return REG_STATUS.FAILED;
            }

            return REG_STATUS.FAILED;
        }

        public REG_STATUS RegQueryKeyLastModifiedTime(REG_HIVES hive, string key, out long lastmodified)
        {
            bool hadaccessdenied = false;
            bool hadfailed = false;
            foreach (IRegistryProvider prov in providers)
            {
                REG_STATUS result = prov.RegQueryKeyLastModifiedTime(hive, key, out lastmodified);
                if (result == REG_STATUS.SUCCESS)
                {
                    return result;
                }

                if (result == REG_STATUS.NOT_IMPLEMENTED)
                {
                    continue;
                }

                if (result == REG_STATUS.ACCESS_DENIED)
                {
                    hadaccessdenied = true;
                    continue;
                }

                if (result == REG_STATUS.FAILED)
                {
                    hadfailed = true;
                }
            }

            if (hadaccessdenied)
            {
                lastmodified = long.MinValue;
                return REG_STATUS.ACCESS_DENIED;
            }

            if (hadfailed)
            {
                lastmodified = long.MinValue;
                return REG_STATUS.FAILED;
            }

            lastmodified = long.MinValue;
            return REG_STATUS.FAILED;
        }
    }
}
