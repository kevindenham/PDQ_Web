<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Computer.aspx.cs" Inherits="Computer" %>

<!DOCTYPE html>

<style>

.header{

}
body, html{
    font-family: Helvetica,sans-serif;
    font-size: 14px;
    line-height: 1.45em;
    color: #3A3A3A;
    background-color: #EFEFEF;
}
.data_block{
    display:inline;
    margin-left: 20px; 
    padding-left: 20px;
    width:220px;
    list-style-type: none;
    border:1px solid #000;
    }
.textbox_data ul
{
    padding: 0;
    list-style-type: none;
    border:1px solid #000;
    float:left;
    margin-left:5px;
    margin-top:10px;
    width:515px;
    height:670px;
    overflow-y:scroll;
    overflow-x:hidden;
    background-color:#fff;
}
.textbox_data ul li {
    margin-left:5px;
    width:495px;
    display:inline-block;
}
.textbox_data ul li label{
    width:150px;
}
.liHeaderBold {

}
.dTxBx {
    margin-left:10px;
    width:350px;
    float:right;
    margin-right:5px;

}

.menu_simple ul {
    margin: 0; 
    padding: 0;
    width:220px;
    list-style-type: none;
    border:1px solid #000;
    float:left;
    background-color:#fff;
}
.menu_simple ul li a {
    text-decoration: none;
    color: black; 
    padding: 5.5px 11px;
    display:block;
    width: 198px;
    cursor:default;
}
.menu_simple ul li a:visited {
    color: black;
} 
.menu_simple ul li a:hover, .menu_simple ul li .current {
    color: black;
    background-color: #caf1fa;
}
.sysinfotxt
{
    margin-left:10px;
    width:350px;
    float:right;
    margin-right:5px
}
.sysinfolabel
{

}
.sysinfoheader
{

}
</style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                
    <div>
                    <div class="menu_simple" ">
                        <ul>
                            <li><a runat="server" id="computer" href="/Computer.aspx?computer="><asp:image runat="server" id="computer_img" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Computer</a></li>
                             <li><a runat="server" id="ad_groups" href="/ad_groups.aspx?computer="><img src="img/icon-adgroups.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Active Directory Groups</a></li>
                             <li><a runat="server" id="applications" href="/Applications.aspx?computer="><img src="img/applications.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Applications</a></li>
                             <li><a runat="server" id="custom_items" href="/custom_items.aspx?computer="><img src="img/custom_items.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Custom Items</a></li>
                             <li><a runat="server" id="disk_drives" href="/disk_drives.aspx?computer="><img src="img/disk_drives.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Disk Drives</a></li>
                             <li><a runat="server" id="displays" href="/Applications.aspx?computer="><img src="img/displays.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Displays</a></li>
                             <li><a runat="server" id="environment" href="/environment.aspx?computer="><img src="img/environment.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Environment</a></li>
                             <li><a runat="server" id="files" href="/files.aspx?computer="><img src="img/62.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Files</a></li>
                             <li><a runat="server" id="hardware" href="/hardware.aspx?computer="><img src="img/hardware.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/> Hardware</a></li>
                             <li><a runat="server" id="hot_fixes" href="/hot_fixes.aspx?computer="><img src="img/hot_fixes.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Hot Fixes</a></li>
                             <li><a runat="server" id="local_groups" href="/local_groups.aspx?computer="><img src="img/local_groups.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Local Groups</a></li>
                             <li><a runat="server" id="local_users" href="/local_users.aspx?computer="><img src="img/local_users.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Local Users</a></li>
                             <li><a runat="server" id="memory" href="/memory.aspx?computer="><img src="img/memory.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Memory Modules</a></li>
                             <li><a runat="server" id="nics" href="/nics.aspx?computer="><img src="img/nics.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  NICs</a></li>
                             <li><a runat="server" id="printers" href="/printers.aspx?computer="><img src="img/unknown.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Printers</a></li>
                             <li><a runat="server" id="processes" href="/processes.aspx?computer="><img src="img/processes.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Processes</a></li>
                             <li><a runat="server" id="product_keys" href="/product_keys.aspx?computer="><img src="img/product_keys.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Product Keys</a></li>
                             <li><a runat="server" id="registry" href="/registry.aspx?computer="><img src="img/registry.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Registry</a></li>
                             <li><a runat="server" id="scans" href="/scans.aspx?computer="><img src="img/scans.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Scans</a></li>
                             <li><a runat="server" id="services" href="/services.aspx?computer="><img src="img/services.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/> Services</a></li>
                             <li><a runat="server" id="shares" href="/shares.aspx?computer="><img src="img/shares.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Shares</a></li>
                             <li><a runat="server" id="windows_features" href="/windows_features.aspx?computer="><img src="img/windows_features.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Windows Features</a></li>
                             <li><a runat="server" id="windows_task_schedules" href="/windows_task_schedules.aspx?computer="><img src="img/windows_task_schedules.png" style='vertical-align:middle;margin-bottom:3px;' height='15' width='15'/>  Windows Task Schedules</a></li>
</ul>
                       <asp:Label ID="Label1" runat="server" style="margin-left:20px;font-size:18px;" Text="Label"></asp:Label>

                            
                    </div>
    <div>
                    <div class="textbox_data" >
                    <ul>
                                                <li style="margin-left:12px;margin-top:10px;font-weight:bold;"><asp:Label ID="Label2" runat="server" Text="General"></asp:Label></li>
                        <li><asp:Label ID="Label3" runat="server" Text="Name"></asp:Label> <asp:TextBox ID="tbName" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label4"  runat="server" Text="Description"></asp:Label><asp:TextBox ID="tbDesc" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label5"  runat="server" Text="Hostname"></asp:Label><asp:TextBox ID="tbHost" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label6"  runat="server" Text="Added"></asp:Label><asp:TextBox ID="tbAdded" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label7"  runat="server" Text="Online"></asp:Label></li>
                        <li ><asp:Label ID="Label8"  runat="server" Text="Uptime"></asp:Label><asp:TextBox ID="tbUptime" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label9"  runat="server" Text="Boot Time"></asp:Label><asp:TextBox ID="tbBoot" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label10"  runat="server" Text="Current User"></asp:Label><asp:TextBox ID="tbCurUser" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label11"  runat="server" Text="IP Address"></asp:Label><asp:TextBox ID="tbIPAddr" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label12"  runat="server" Text="MAC Address"></asp:Label><asp:TextBox ID="tbMacAddr" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label13"  runat="server" Text="Time Zone"></asp:Label><asp:TextBox ID="tbTimeZn" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                                                <li style="margin-left:12px;font-weight:bold;"><asp:Label ID="Label17" style="margin-top:10px" runat="server" Text="Operating System"></asp:Label></li>
                        <li ><asp:Label ID="Label14"  runat="server" Text="O/S"></asp:Label><asp:TextBox ID="tbOS" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label15"  runat="server" Text="Name"></asp:Label><asp:TextBox ID="tbOSName" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label16"  runat="server" Text="Version"></asp:Label><asp:TextBox ID="tbVersion" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label18"  runat="server" Text="Service Pack"></asp:Label><asp:TextBox ID="tbServPack" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label19"  runat="server" Text="Installed"></asp:Label><asp:TextBox ID="tbInstalled" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label20"  runat="server" Text="Serial Number"></asp:Label><asp:TextBox ID="tbSerNum" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label21"  runat="server" Text="System Drive"></asp:Label><asp:TextBox ID="tbSysDrive" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label22"  runat="server" Text="IE Version"></asp:Label><asp:TextBox ID="tbIEVer" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label23"  runat="server" Text="Architecture"></asp:Label><asp:TextBox ID="tbArch" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label24"  runat="server" Text=".NET Versions"></asp:Label><asp:TextBox ID="tbNet" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label25"  runat="server" Text="Needs Reboot"></asp:Label><asp:TextBox ID="tbReboot" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                                                <li style="margin-left:12px;font-weight:bold;"><asp:Label ID="Label41" style="margin-top:10px" runat="server" Text="Active Directory"></asp:Label></li>
                        <li ><asp:Label ID="Label43"  runat="server" Text="Path"></asp:Label><asp:TextBox ID="tbPath" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label44"  runat="server" Text="Description"></asp:Label><asp:TextBox ID="tbADDesc" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label45"  runat="server" Text="Domain"></asp:Label><asp:TextBox ID="tbDomain" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label46"  runat="server" Text="Location"></asp:Label><asp:TextBox ID="tbLocation" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label47"  runat="server" Text="Last Logon"></asp:Label><asp:TextBox ID="tbLastLogon" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label48"  runat="server" Text="Created"></asp:Label><asp:TextBox ID="tbCreated" CssClass="dTxBx" runat="server"></asp:TextBox></li>                                         
                                                <li style="margin-left:12px;font-weight:bold;"><asp:Label ID="Label27" style="margin-top:10px" runat="server" Text="System"></asp:Label></li>
                        <li ><asp:Label ID="Label26"  runat="server" Text="Manufacturer"></asp:Label><asp:TextBox ID="tbManuf" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label28"  runat="server" Text="Model"></asp:Label><asp:TextBox ID="tbModel" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label29"  runat="server" Text="Memory"></asp:Label><asp:TextBox ID="tbMemory" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label30"  runat="server" Text="Processor"></asp:Label><asp:TextBox ID="tbProc" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label31"  runat="server" Text="Chassis"></asp:Label><asp:TextBox ID="tbChassis" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label32"  runat="server" Text="BIOS Version"></asp:Label><asp:TextBox ID="tbBiosVer" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label33"  runat="server" Text="BIOS Manufacturer"></asp:Label><asp:TextBox ID="tbBiosManu" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label34"  runat="server" Text="BIOS Asset Tag"></asp:Label><asp:TextBox ID="tbBiosAsset" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label35"  runat="server" Text="Family"></asp:Label><asp:TextBox ID="tbFamily" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label36"  runat="server" Text="Version"></asp:Label><asp:TextBox ID="tbSysVer" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label37"  runat="server" Text="SKU"></asp:Label><asp:TextBox ID="tbSKU" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                                                <li style="margin-left:12px;font-weight:bold;"><asp:Label ID="Label42" style="margin-top:10px" runat="server" Text="Scanning"></asp:Label></li>
                        <li ><asp:Label ID="Label38"  runat="server" Text="Last Scan"></asp:Label><asp:TextBox ID="tbLastScan" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label39"  runat="server" Text="Last Scan Profile"></asp:Label><asp:TextBox ID="tbScanProf" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        <li ><asp:Label ID="Label40"  runat="server" Text="Scan User"></asp:Label><asp:TextBox ID="tbScanUser" CssClass="dTxBx" runat="server"></asp:TextBox></li>
                        
                    </ul>
                </div>
         </div>
        
    
    </div>
    </form>
</body>
</html>
