using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Computer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        computer.HRef = computer.HRef + Request.QueryString["computer"];
        ad_groups.HRef = ad_groups.HRef + Request.QueryString["computer"];
        applications.HRef = applications.HRef + Request.QueryString["computer"];
        custom_items.HRef = custom_items.HRef + Request.QueryString["computer"];
        disk_drives.HRef = disk_drives.HRef + Request.QueryString["computer"];
        displays.HRef = displays.HRef + Request.QueryString["computer"];
        environment.HRef = environment.HRef + Request.QueryString["computer"];
        files.HRef = files.HRef + Request.QueryString["computer"];
        hardware.HRef = hardware.HRef + Request.QueryString["computer"];
        hot_fixes.HRef = hot_fixes.HRef + Request.QueryString["computer"];
        local_groups.HRef = local_groups.HRef + Request.QueryString["computer"];
        local_users.HRef = local_users.HRef + Request.QueryString["computer"];
        memory.HRef = memory.HRef + Request.QueryString["computer"];
        nics.HRef = nics.HRef + Request.QueryString["computer"];
        printers.HRef = printers.HRef + Request.QueryString["computer"];
        processes.HRef = processes.HRef + Request.QueryString["computer"];
        registry.HRef = registry.HRef + Request.QueryString["computer"];
        scans.HRef = scans.HRef + Request.QueryString["computer"];
        services.HRef = services.HRef + Request.QueryString["computer"];
        shares.HRef = shares.HRef + Request.QueryString["computer"];
        windows_features.HRef = windows_features.HRef + Request.QueryString["computer"];
        windows_task_schedules.HRef = windows_task_schedules.HRef + Request.QueryString["computer"];

        computer_img.ImageUrl = "img/chassislaptop.png";
        GridView_System(Request.QueryString["computer"]);
    }
    protected void GridView_System(string system)
    {
        String connectionString = "Data Source=C:\\programdata\\Admin Arsenal\\Database.db";
        //String connectionString = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        String selectCommand = "SELECT * FROM computers WHERE ComputerId = '" + system + "'";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, connectionString);

        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);
        
        Label1.Text = dt.Rows[0][0].ToString();

        switch (dt.Rows[0]["Chassis"].ToString())
        {
            case "Laptop":
            case "Portable":
                Label1.Text = "<img src='img/chassislaptop.png' style='vertical-align:middle;' height = '40' width='40'>  " + dt.Rows[0]["Name"].ToString() + "</img>";
                break;
            case "Desktop":
            case "Tower":
                Label1.Text = "<img src='img/chassisdesktop.png' style='vertical-align:middle;' height='40' width='40'>  " + dt.Rows[0]["Name"].ToString() + "</img>";
                break;
            case "Mini Tower":
            case "Space Saving":
            case "Other":
                Label1.Text = "<img src='img/chassiscomputer.png' style='vertical-align:middle;' height='40' width='40'>  " + dt.Rows[0]["Name"].ToString() + "</img>";
                break;
            case "Rack Mount Chassis":
                Label1.Text = "<img src='img/chassismain.png' style='vertical-align:middle;' height='40' width='40'>  " + dt.Rows[0]["Name"].ToString() + "</img>";
                break;
            default:
                Label1.Text = "<img src='img/unknown.png' style='vertical-align:middle;' height='40' width='40'>  " + dt.Rows[0]["Name"].ToString() + "</img>";
                break;
        }

        tbName.Text = dt.Rows[0]["Name"].ToString();
        tbDesc.Text = dt.Rows[0]["Description"].ToString();
        tbHost.Text = dt.Rows[0]["HostName"].ToString();
        tbAdded.Text = DateTime.Parse(dt.Rows[0]["Added"].ToString()).ToString();
        if (dt.Rows[0]["IsOnline"].ToString() == "1")
            tbUptime.Text = ((DateTime.Now - DateTime.Parse(dt.Rows[0]["BootTime"].ToString()))).Days.ToString() + " days";
        tbBoot.Text = DateTime.Parse(dt.Rows[0]["BootTime"].ToString()).ToString();
        tbCurUser.Text = dt.Rows[0]["CurrentUser"].ToString();
        tbIPAddr.Text = dt.Rows[0]["IPAddress"].ToString();
        tbMacAddr.Text = dt.Rows[0]["MACAddress"].ToString();
        tbTimeZn.Text = dt.Rows[0]["TimeZone"].ToString();
        tbOS.Text = dt.Rows[0]["OS"].ToString();
        tbOSName.Text = dt.Rows[0]["OSName"].ToString();
        tbVersion.Text = dt.Rows[0]["OSVersion"].ToString();
        if (dt.Rows[0]["OSServicePack"].ToString() != "")
            tbServPack.Text = dt.Rows[0]["OSServicePack"].ToString();
        tbInstalled.Text = DateTime.Parse(dt.Rows[0]["OSInstallDate"].ToString()).ToString();
        tbSerNum.Text = dt.Rows[0]["OSSerialNumber"].ToString();
        tbSysDrive.Text = dt.Rows[0]["SystemDrive"].ToString();
        tbIEVer.Text = dt.Rows[0]["IEVersion"].ToString();
        tbArch.Text = dt.Rows[0]["OSArchitecture"].ToString() + "-bit";
        tbNet.Text = dt.Rows[0]["DotNetVersions"].ToString();
        if (dt.Rows[0]["NeedsReboot"].ToString() == "1")
            tbReboot.Text = "Yes";
        else
            tbReboot.Text = "No";
        tbPath.Text = dt.Rows[0]["ADParentPath"].ToString();
        tbADDesc.Text = dt.Rows[0]["ADDescription"].ToString();
        tbDomain.Text = dt.Rows[0]["ADDomain"].ToString();
        tbLastLogon.Text = DateTime.Parse(dt.Rows[0]["ADLastLogon"].ToString()).ToString();
        tbCreated.Text = DateTime.Parse(dt.Rows[0]["ADWhenCreated"].ToString()).ToString();

        tbManuf.Text = dt.Rows[0]["Manufacturer"].ToString();
        tbModel.Text = dt.Rows[0]["Model"].ToString();
        Int64 mem = Int64.Parse(dt.Rows[0]["Memory"].ToString());
        tbMemory.Text = SizeSuffix(mem);
        tbProc.Text = dt.Rows[0]["ProcessorName"].ToString();
        tbSerNum.Text = dt.Rows[0]["SerialNumber"].ToString();
        tbBiosVer.Text = dt.Rows[0]["BIOSVersion"].ToString();
        tbBiosManu.Text = dt.Rows[0]["BIOSManufacturer"].ToString();
        tbBiosAsset.Text = dt.Rows[0]["BIOSAssetTag"].ToString();
        tbFamily.Text = dt.Rows[0]["SystemFamily"].ToString();
        tbSysVer.Text = dt.Rows[0]["SystemVersion"].ToString();
        tbSKU.Text = dt.Rows[0]["SystemSKU"].ToString();

        tbLastScan.Text = DateTime.Parse(dt.Rows[0]["SuccessfulScanDate"].ToString()).ToString();

        
        connectionString = "Data Source=C:\\programdata\\Admin Arsenal\\Database.db";
        //String connectionString = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        selectCommand = "SELECT UserName FROM credentials WHERE CredentialsId = '" + dt.Rows[0]["ScanUserId"].ToString() + "'";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        dataAdapter =
                    new SQLiteDataAdapter(selectCommand, connectionString);
        dt = new DataTable();
        dataAdapter.Fill(dt);
            tbScanUser.Text = dt.Rows[0]["UserName"].ToString();



    }
    static readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    static string SizeSuffix(Int64 value)
    {
        if (value < 0) { return "-" + SizeSuffix(-value); }

        int i = 0;
        decimal dValue = (decimal)value;
        while (Math.Round(dValue / 1024) >= 1)
        {
            dValue /= 1024;
            i++;
        }

        return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
    }

}