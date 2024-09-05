using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;

namespace VelocityPillApp
{
    public class VelocityHelper
    {
        public enum State
        {
            Enabled = 0,
            Disabled = 1,
            Custom = 2
        }

        public class WNFState
        {
            public string DisplayableID
            {
                get
                {
                    string id = $"0x{ID:X8}";
                    foreach (string line in App.Lines)
                    {
                        if (line.Split(',').Last().ToLower() == id.ToLower())
                        {
                            id = line.Split(',').First() + " (" + id + ")";
                            break;
                        }
                    }

                    return id;
                }
            }

            public uint ID { get; internal set; }
            public State Enabled
            {
                get
                {
                    if (BitConverter.ToString(WNFStateData) == BitConverter.ToString(new byte[] { 01, 08, 00, 00, 00, 00, 00, 00 }))
                    {
                        return State.Enabled;
                    }
                    else if (BitConverter.ToString(WNFStateData) == BitConverter.ToString(new byte[] { 01, 04, 00, 00, 00, 00, 00, 00 }))
                    {
                        return State.Disabled;
                    }
                    return State.Custom;
                }
                set
                {
                    if (Enabled == value) return;
                    if (value == State.Custom) return;
                    if (value == State.Enabled)
                    {
                        WNFStateData = new byte[] { 01, 08, 00, 00, 00, 00, 00, 00 };
                    }
                    else
                    {
                        WNFStateData = new byte[] { 01, 04, 00, 00, 00, 00, 00, 00 };
                    }
                }
            }
            public byte[] WNFStateData { get; set; }
            public int DisplayableEnabled
            {
                get
                {
                    return (int)Enabled;
                }
                set
                {
                    try
                    {
                        Enabled = (State)value;
                    }
                    catch
                    {
                    }
                }
            }

            public string DisplayableText
            {
                get
                {
                    return BitConverter.ToString(WNFStateData).Replace("-", "");
                }
                set
                {
                    try
                    {
                        string s = Regex.Replace(value, ".{2}", "$0-").TrimEnd('-');
                        String[] tempAry = s.Split('-');
                        byte[] buffer = new byte[tempAry.Length];
                        for (int i = 0; i < tempAry.Length; i++)
                        {
                            buffer[i] = Convert.ToByte(tempAry[i], 16);
                        }
                        if (buffer.Count() != 8)
                        {
                            WNFStateData = buffer;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private readonly VelocityPillRT.VelocityPill pill;

        public VelocityHelper()
        {
            pill = new VelocityPillRT.VelocityPill();
        }

        private static uint SwapEndianness(uint value)
        {
            uint b1 = (value >> 0) & 0xff;
            uint b2 = (value >> 8) & 0xff;
            uint b3 = (value >> 16) & 0xff;
            uint b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }

        public List<WNFState> GetFeatureStates()
        {
            List<WNFState> itemlist = new List<WNFState>();

            try
            {
                //byte[] result;
                //var ret = pill.GetFeatureState(featureStoreName, out result);

                RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);

                string binstr = data.Substring(8);

                //var binstr = BitConverter.ToString(result).Replace("-", "");

                string startids = binstr.Substring(32, binstr.Count() - 32);
                IEnumerable<string> idlist = Split(startids, 24);

                foreach (string id in idlist)
                {
                    if (id == "")
                        continue;

                    string s = Regex.Replace(id.Substring(8, 16), ".{2}", "$0-").TrimEnd('-');
                    String[] tempAry = s.Split('-');
                    byte[] buffer = new byte[tempAry.Length];
                    for (int i = 0; i < tempAry.Length; i++)
                    {
                        buffer[i] = Convert.ToByte(tempAry[i], 16);
                    }

                    WNFState state = new WNFState()
                    {
                        ID = SwapEndianness(uint.Parse(id.Substring(0, 8), System.Globalization.NumberStyles.HexNumber)),
                        WNFStateData = buffer
                    };
                    itemlist.Add(state);
                }
            }
            catch
            {

            }

            return itemlist;
        }

        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public void ClearFeatureStates()
        {
            try
            {
                byte[] result = new byte[] { 02, 02, 16, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };
                int retc = pill.SetFeatureState(App.featureStoreName, result);
                if (retc != 0)
                {
                    RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                    reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);
                    string countstr = data.Substring(0, 8);
                    uint count = SwapEndianness(uint.Parse(countstr, System.Globalization.NumberStyles.HexNumber));
                    count++;
                    string datatowrite = BitConverter.ToString(BitConverter.GetBytes(count)).Replace("-", "") + BitConverter.ToString(result).Replace("-", "");
                    RegistryHelper.REG_STATUS fret = reg.RegSetValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, datatowrite);

                    if (fret != RegistryHelper.REG_STATUS.SUCCESS)
                        ShowCommand(result);
                }
            }
            catch
            {

            }
        }

        public void AddMultipleFeatureStates(IList<WNFState> states, bool clean)
        {
            try
            {
                //byte[] result;
                //var ret = pill.GetFeatureState(featureStoreName, out result);


                RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);

                string binstr = data.Substring(8);


                string s = Regex.Replace(binstr, ".{2}", "$0-").TrimEnd('-');
                String[] tempAry = s.Split('-');
                byte[] result = new byte[tempAry.Length];
                for (int i = 0; i < tempAry.Length; i++)
                {
                    result[i] = Convert.ToByte(tempAry[i], 16);
                }
                
                if (clean)
                    result = new byte[] { 02, 02, 16, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };

                foreach (WNFState state in states)
                {
                    uint count = result.ElementAt(4);

                    byte[] buffer = BitConverter.GetBytes(state.ID);

                    List<byte> newlst = result.ToList();
                    newlst.AddRange(buffer);

                    newlst.AddRange(state.WNFStateData);

                    result = newlst.ToArray();

                    count++;
                    result[4] = BitConverter.GetBytes(count).First();
                }

                int retc = pill.SetFeatureState(App.featureStoreName, result);
                if (retc != 0)
                {
                    reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data);
                    string countstr = data.Substring(0, 8);
                    uint count2 = SwapEndianness(uint.Parse(countstr, System.Globalization.NumberStyles.HexNumber));
                    count2++;
                    string datatowrite = BitConverter.ToString(BitConverter.GetBytes(count2)).Replace("-", "") + BitConverter.ToString(result).Replace("-", "");
                    RegistryHelper.REG_STATUS fret = reg.RegSetValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, datatowrite);
                
                    if (fret != RegistryHelper.REG_STATUS.SUCCESS)
                        ShowCommand(result);
                }
            }
            catch
            {

            }
        }

        public void AddFeatureState(WNFState state)
        {
            try
            {
                //byte[] result;
                //var ret = pill.GetFeatureState(featureStoreName, out result);


                RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);

                string binstr = data.Substring(8);


                string s = Regex.Replace(binstr, ".{2}", "$0-").TrimEnd('-');
                String[] tempAry = s.Split('-');
                byte[] result = new byte[tempAry.Length];
                for (int i = 0; i < tempAry.Length; i++)
                {
                    result[i] = Convert.ToByte(tempAry[i], 16);
                }


                uint count = result.ElementAt(4);

                byte[] buffer = BitConverter.GetBytes(state.ID);

                List<byte> newlst = result.ToList();
                newlst.AddRange(buffer);

                newlst.AddRange(state.WNFStateData);

                result = newlst.ToArray();

                Debug.WriteLine(BitConverter.ToString(BitConverter.GetBytes(count)));
                
                count++;
                result[4] = BitConverter.GetBytes(count).First();

                Debug.WriteLine(BitConverter.ToString(BitConverter.GetBytes(count)));

                int retc = pill.SetFeatureState(App.featureStoreName, result);
                if (retc != 0)
                {
                    reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data);
                    string countstr = data.Substring(0, 8);
                    uint count2 = SwapEndianness(uint.Parse(countstr, System.Globalization.NumberStyles.HexNumber));
                    count2++;
                    string datatowrite = BitConverter.ToString(BitConverter.GetBytes(count2)).Replace("-", "") + BitConverter.ToString(result).Replace("-", "");
                    RegistryHelper.REG_STATUS fret = reg.RegSetValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, datatowrite);

                    if (fret != RegistryHelper.REG_STATUS.SUCCESS)
                        ShowCommand(result);
                }
            }
            catch
            {

            }
        }

        public void SetFeatureState(WNFState state)
        {
            try
            {
                //byte[] result;
                //var ret = pill.GetFeatureState(featureStoreName, out result);


                RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);

                string binstr = data.Substring(8);


                string s = Regex.Replace(binstr, ".{2}", "$0-").TrimEnd('-');
                String[] tempAry = s.Split('-');
                byte[] result = new byte[tempAry.Length];
                for (int i = 0; i < tempAry.Length; i++)
                {
                    result[i] = Convert.ToByte(tempAry[i], 16);
                }

                string id = BitConverter.ToString(BitConverter.GetBytes(SwapEndianness(state.ID))).Replace("-", "");

                int location = BitConverter.ToString(result).Replace("-", "").IndexOf(id) / 2 + 4;

                int count = 0;
                foreach (byte byteitm in state.WNFStateData)
                {
                    result[location + count] = byteitm;
                    count++;
                }

                int retc = pill.SetFeatureState(App.featureStoreName, result);
                if (retc != 0)
                {
                    reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data);
                    string countstr = data.Substring(0, 8);
                    uint count2 = SwapEndianness(uint.Parse(countstr, System.Globalization.NumberStyles.HexNumber));
                    count2++;
                    string datatowrite = BitConverter.ToString(BitConverter.GetBytes(count2)).Replace("-", "") + BitConverter.ToString(result).Replace("-", "");
                    RegistryHelper.REG_STATUS fret = reg.RegSetValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, datatowrite);

                    if (fret != RegistryHelper.REG_STATUS.SUCCESS)
                        ShowCommand(result);
                }
            }
            catch
            {

            }
        }

        public async void SaveBin(byte[] buffer)
        {
            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Binary file", new List<string>() { ".bin" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New VanillaPill Feature definition file";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteBufferAsync(file, buffer.AsBuffer());
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        public async void ShowCommand(byte[] buffer)
        {
            string cmd = $"reg add \"HKLM\\{App.key}\" /v {App.featureStoreName} /t REG_BINARY /d 01000000{BitConverter.ToString(buffer).Replace("-", "")} /f";
            await new CommandContentDialog(cmd).ShowAsync();
        }

        public void RemoveFeatureState(WNFState state)
        {
            try
            {
                //byte[] result;
                //var ret = pill.GetFeatureState(featureStoreName, out result);


                RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);

                string binstr = data.Substring(8);
                
                string s = Regex.Replace(binstr, ".{2}", "$0-").TrimEnd('-');
                String[] tempAry = s.Split('-');
                byte[] result = new byte[tempAry.Length];
                for (int i = 0; i < tempAry.Length; i++)
                {
                    result[i] = Convert.ToByte(tempAry[i], 16);
                }

                uint count = result.ElementAt(4);

                string id = $"{SwapEndianness(state.ID):X8}";

                int location = BitConverter.ToString(result).Replace("-", "").IndexOf(id) / 2;

                List<byte> newlst = result.ToList();
                newlst.RemoveRange(location, 12);
                result = newlst.ToArray();

                count--;
                result[4] = BitConverter.GetBytes(count).First();

                int retc = pill.SetFeatureState(App.featureStoreName, result);
                if (retc != 0)
                {
                    reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data);
                    string countstr = data.Substring(0, 8);
                    uint count2 = SwapEndianness(uint.Parse(countstr, System.Globalization.NumberStyles.HexNumber));
                    count2++;
                    string datatowrite = BitConverter.ToString(BitConverter.GetBytes(count2)).Replace("-", "") + BitConverter.ToString(result).Replace("-", "");
                    RegistryHelper.REG_STATUS fret = reg.RegSetValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, datatowrite);

                    if (fret != RegistryHelper.REG_STATUS.SUCCESS)
                        ShowCommand(result);
                }
            }
            catch
            {

            }
        }
    }
}
