using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Applications : System.Web.UI.Page
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
        GridView_Applications(Request.QueryString["computer"]);
    }
    protected void GridView_Applications(string system)
    {
        String connectionString = "Data Source=C:\\programdata\\Admin Arsenal\\Database.db";
        //String connectionString = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        String selectCommand = "SELECT name,publisher,version,installdate,uninstall,ApplicationId FROM applications WHERE ComputerId = " + system + " order by name asc";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, connectionString);

        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);

       
        GridView1.DataSource = dt;
        // txtValueA.Text = dt.Rows[0][0].ToString();
        GridView1.DataBind();

        connectionString = "Data Source=C:\\programdata\\Admin Arsenal\\Database.db";
        //String connectionString = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
         selectCommand = "SELECT * FROM computers WHERE ComputerId = '" + system + "'";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
         dataAdapter =
                    new SQLiteDataAdapter(selectCommand, connectionString);

         dt = new DataTable();
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
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow dr = ((DataRowView)e.Row.DataItem).Row;
            DataView dv = ((DataRowView)e.Row.DataItem).DataView;
            DataRowView drv = e.Row.DataItem as DataRowView;
            
            e.Row.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(this.form1, dr["ApplicationId"].ToString()));
            e.Row.Attributes.Add("class", "system" + dr["ApplicationId"].ToString());
            e.Row.Attributes.Add("onmouseover", "hvrstart('" + "system" + dr["ApplicationId"].ToString() + "')");
            e.Row.Attributes.Add("onmouseleave", "hvrleave('" + "system" + dr["ApplicationId"].ToString() + "')");
        }



        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                LinkButton LnkHeaderText = e.Row.Cells[i].Controls[0] as LinkButton;
                e.Row.Cells[i].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.GridView1, "Sort$" + LnkHeaderText.Text);


            }

        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Do wherever you want with grvSearch.SelectedIndex                
        }
        catch
        {
            //...throw
        }
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dataTable = GridView1.DataSource as DataTable;
        string sortExpression = e.SortExpression;
        string direction = string.Empty;

        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);

            if (SortDirection == SortDirection.Descending)
            {
                SortDirection = SortDirection.Ascending;
                direction = " ASC";
            }
            else
            {
                SortDirection = SortDirection.Descending;
                direction = " DESC";
            }

            DataTable table = GridView1.DataSource as DataTable;
            table.DefaultView.Sort = sortExpression + direction;

            GridView1.DataSource = table;
            GridView1.DataBind();

        }
    }
    public SortDirection SortDirection
    {
        get
        {
            if (ViewState["SortDirection"] == null)
            {
                ViewState["SortDirection"] = SortDirection.Descending;
            }
            return (SortDirection)ViewState["SortDirection"];
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }
}