﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PRM.Core.Protocol.FileTransmit.Host;
using PRM.Core.Protocol.FileTransmit.SFTP;
using PRM.Core.Protocol.Putty;
using PRM.Core.Protocol.Putty.Host;
using PRM.Core.Protocol.Putty.SSH;
using PRM.Core.Protocol.Putty.Telnet;
using PRM.Core.Protocol.RDP;
using PRM.Core.Protocol.RDP.Host;
using PRM.Core.Protocol.VNC;
using PRM.Core.Protocol.VNC.Host;
using Shawn.Utils.RDP;

namespace PRM.Core.Protocol
{
    public static class ProtocolHostFactory
    {
        public static ProtocolHostBase Get(ProtocolServerBase server, double width = 0, double height = 0)
        {
            switch (server)
            {
                case ProtocolServerRDP _:
                    {
                        var host = new AxMsRdpClient09Host(server, width, height);
                        return host;
                    }
                case ProtocolServerSSH ssh:
                    {
                        var host = new KittyHost(ssh);
                        return host;
                    }
                case ProtocolServerTelnet telnet:
                    {
                        var host = new KittyHost(telnet);
                        return host;
                    }
                case ProtocolServerVNC vnc:
                {
                    var host = new VncHost(vnc);
                    return host;
                    }
                case ProtocolServerSFTP sftp:
                {
                    var host = new FileTransmitHost(sftp);
                    return host;
                }
                default:
                    throw new NotImplementedException($"Host of {server.GetType()} is not implemented");
            }
        }

        public static bool IsConnWithFullScreen(this ProtocolServerBase server)
        {
            switch (server)
            {
                case ProtocolServerRDP rdp:
                    {
                        if (rdp.RdpFullScreenFlag == ERdpFullScreenFlag.EnableFullAllScreens)
                            return true;
                        if (rdp.IsConnWithFullScreen)
                            return true;
                        return rdp.AutoSetting?.FullScreenLastSessionIsFullScreen ?? false;
                    }
                case ProtocolServerVNC _:
                case ProtocolServerSSH _:
                case ProtocolServerTelnet _:
                case ProtocolServerSFTP _:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
