using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
     const string CONNECTION = "Data Source=C:\\ProgramData\\Admin arsenal\\pdq inventory\\Database.db";
     protected void Page_Load(object sender, EventArgs e)
    {

        

        if (!IsPostBack)
        {

            GridView_All();
            Populate_TreeView1();
        }
        else
        {

            if (Request["__EVENTTARGET"] == "form1")
            {
                Response.Redirect("/computer.aspx?computer=" + Request["__EVENTARGUMENT"]);
            }
            else if (Request["__EVENTTARGET"] == "TreeView1")
            {
                string[] split = Request["__EVENTARGUMENT"].Split(new Char[] { '-' });
                //txtValueA.Text = split[split.Count() - 1].ToString();

                GridView_Tree(Int32.Parse(split[split.Count() - 1 ]));
            }
            else
            {
                GridView_All();
            }
        }

        
    }
    // Populate left Navbar TreeView
    protected void GridView_Tree(int node)
    {
        String selectCommand = "select Path,Type,ReportDefinitionId,ADDistinguishedName from collections WHERE CollectionId =" + node;
        SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectCommand, CONNECTION);

        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        if (dt.Rows[0][1].ToString() == "DynamicCollection") // Collection corrosponds to a report, Not AD Path or Group
        {
            int reportDefinitionId = Int32.Parse(dt.Rows[0][2].ToString());
            // Has a ReportDefinitionId, Get Comparison Operator of RootFilter
        //    txtValueA.Text = dt.Rows[0][1].ToString();
            
            selectCommand = "select Comparison from ReportDefinitionFilters WHERE Type = 'RootFilter' AND ReportDefinitionId =" + reportDefinitionId;
            dataAdapter = new SQLiteDataAdapter(selectCommand, CONNECTION);

            dt = new DataTable();
            dataAdapter.Fill(dt);
            string select;
            switch (dt.Rows[0][0].ToString())
            {
                case ("All"): select = "*";
                    break;
            }
            // Get Value Data
            
            selectCommand = "select TableName,ColumnName,Comparison,Value from ReportDefinitionFilters WHERE Type = 'ValueFilter' AND ReportDefinitionId =" + reportDefinitionId;
            dataAdapter = new SQLiteDataAdapter(selectCommand, CONNECTION);

            dt = new DataTable();
            dataAdapter.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            // Query with Value Data
            string table;
            string column;
            string operant;
            if (dt.Rows[0][0].ToString() != null)
            {
                table = " FROM " + dt.Rows[0][0].ToString();
                column = " WHERE " + dt.Rows[0][1].ToString();
                operant = dt.Rows[0][2].ToString();
                switch (operant)
                {
                    case ("IsTrue"): operant = " = 1";
                        break;
                    case ("IsFalse"):
                        operant = " = 0";
                        break;
                    case ("After"): operant = ">= date('now','-" + dt.Rows[0][3].ToString().Replace(" ago", "") + "')";
                        break;
                    case ("Before"):
                        operant = "<= date('now','-" + dt.Rows[0][3].ToString().Replace(" ago", "") + "')";
                        break;
                    case ("!Contains"):
                        operant = " NOT LIKE '%" + dt.Rows[0][3].ToString() + "%'";
                        break;
                    case ("Contains"):
                        operant = " LIKE  '%" + dt.Rows[0][3].ToString() + "%'";
                        break;
                    case ("Equals"):
                        operant = " = " + dt.Rows[0][3].ToString();
                        break;
                }
                dt = new DataTable();
                try {
                    selectCommand = "select *" + table + "s" + column + operant;
                    dataAdapter = new SQLiteDataAdapter(selectCommand, CONNECTION);
                    dataAdapter.Fill(dt);
                } catch
                {
                    try {
                        selectCommand = "select *" + table + "es" + column + operant;
                        dataAdapter = new SQLiteDataAdapter(selectCommand, CONNECTION);
                        dataAdapter.Fill(dt);
                    }
                    catch
                    {
                       // txtValueA.Text = selectCommand;
                    }
                }

               // txtValueA.Text = selectCommand;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }

            


        }
        else if (dt.Rows[0][1].ToString() == "ActiveDirectoryCollection") // Collection is an AD 
        {
            selectCommand = "select * from Computers WHERE ADParentDistinguishedName = '" + dt.Rows[0][3].ToString() + "'";
            dataAdapter = new SQLiteDataAdapter(selectCommand, CONNECTION);

            dt = new DataTable();
            dataAdapter.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
    protected void GridView_System(string system)
    {
        
        //String CONNECTION = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        String selectCommand = "select ComputerId, Name, HostName, IsOnline from computers WHERE Name = '" + system + "' order by Name asc";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, CONNECTION);

        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);

        dt.Columns["IsOnline"].ColumnName = "Online";
        GridView1.DataSource = dt;
        // txtValueA.Text = dt.Rows[0][0].ToString();
        GridView1.DataBind();
    }
    protected void GridView_All()
    {

        //String CONNECTION = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
//        String selectCommand = "select HostName,Name,IsOnline,IPAddress,ScanErrorMessage,OSName,Memory,SerialNumber,BootTime,Manufacturer,Model,Chassis,ComputerID from computers order by Name asc";

        String selectCommand = "select * from computers order by Name asc";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, CONNECTION);

        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);

        dt.Columns["IsOnline"].ColumnName = "Online";
        GridView1.DataSource = dt;
       // txtValueA.Text = dt.Rows[0][0].ToString();
        GridView1.DataBind();


        // Populate Dialog UI dropdown with scan users
        selectCommand = "select * from credentials order by IsDefault asc";

        dataAdapter =
                    new SQLiteDataAdapter(selectCommand, CONNECTION);

        dt = new DataTable();
        dataAdapter.Fill(dt);

        // Find the Default User
        int rIndex = 0;
        foreach (DataRow row in dt.Rows)
        {
            if (row["IsDefault"].ToString() == "1")
            {
                dt.Rows[rIndex]["UserName"] = "(Default) " + row["UserName"].ToString();
                dt.Rows[rIndex]["CredentialsId"] = -1;
                break;
            }
            rIndex++;
        }

        DropDownList1.DataTextField = "UserName";
        DropDownList1.DataValueField = "CredentialsId";

        DropDownList1.DataSource = dt;
        DropDownList1.DataBind();
        DropDownList1.SelectedIndex = rIndex;

        // Then add your first item
        DropDownList1.Items.Insert(0, "Select");

    }
    protected void Populate_TreeView1()
    {
        // Populate TreeView
        
        //String CONNECTION = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        String selectCommand = "select * from collections order by Name asc";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                   new SQLiteDataAdapter(selectCommand, CONNECTION);
        TreeView1.Nodes.Clear();
        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow node in dt.Rows)
        {
            if (node["ParentID"].ToString() == "")
            {
                TreeNode nodeParent = new TreeNode();
                nodeParent.Text = node["Name"].ToString();
                nodeParent.Value = node["CollectionId"].ToString();

                TreeView1.Nodes.Add(nodeParent);

                Populate_Nodes(dt, nodeParent);
                nodeParent.Value = "-" + node["CollectionId"].ToString();
                nodeParent.Collapse();
            }


        }
    }
    protected void Populate_Nodes(DataTable dt, TreeNode nodeParent)
    {
        foreach (DataRow cNode in dt.Rows)
        {
            if (cNode["ParentID"].ToString() == nodeParent.Value.ToString())
            {
                TreeNode nodeChild = new TreeNode();
                nodeChild.Text = cNode["Name"].ToString();
                nodeChild.Value = cNode["CollectionId"].ToString();
                nodeParent.ChildNodes.Add(nodeChild);
                Populate_Nodes(dt, nodeChild);
                nodeChild.Value = "-" + cNode["CollectionId"].ToString();
                nodeChild.Collapse();
            }
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            TableCell Cell = new TableCell();
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                LinkButton LnkHeaderText = e.Row.Cells[i].Controls[0] as LinkButton;
                
                BoundField field = (BoundField)((DataControlFieldCell)e.Row.Cells[i]).ContainingField;
                if (field.HeaderText == "ComputerId")
                {
                    e.Row.Cells[i].Visible = false;
                }
            }


        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow dr = ((DataRowView)e.Row.DataItem).Row;
            DataView dv = ((DataRowView)e.Row.DataItem).DataView;
            DataRowView drv = e.Row.DataItem as DataRowView;
         //   txtValueA.Text = " TEST " + txtValueA.Text;

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                BoundField field = (BoundField)((DataControlFieldCell)e.Row.Cells[i]).ContainingField;

                if (field.HeaderText == ("Online"))
                {
                    if (dr["Online"].ToString() == "1")
                    {
                        e.Row.Cells[i].Text = "<img src='img/44.png' height='15' width='15'>Yes</img>";
                    }
                    else
                    {
                        e.Row.Cells[i].Text = "<img src='img/41.png' height='15' width='15'>No</img>";
                    }
                }
                if (field.HeaderText == "ComputerId")
                {
                    e.Row.Cells[i].Visible = false;
                }
                if (field.HeaderText == "Name")
                {

                    switch (dr["Chassis"].ToString())
                    {
                        case "Laptop":
                        case "Portable":
                            e.Row.Cells[i].Text = "<img src='img/chassislaptop.png' style='vertical-align:middle;' height='15' %width='15'>  " + dr["Name"].ToString() + "</img>";
                            break;
                        case "Desktop":
                        case "Tower":
                            e.Row.Cells[i].Text = "<img src='img/chassisdesktop.png' style='vertical-align:middle;' height='15' width='15'>  " + dr["Name"].ToString() + "</img>";
                            break;
                        case "Mini Tower":
                        case "Space Saving":
                        case "Other":
                            e.Row.Cells[i].Text = "<img src='img/chassiscomputer.png' style='vertical-align:middle;' height='15' width='15'>  " + dr["Name"].ToString() + "</img>";
                            break;
                        case "Rack Mount Chassis":
                            e.Row.Cells[i].Text = "<img src='img/chassismain.png' style='vertical-align:middle;' height='15' width='15'>  " + dr["Name"].ToString() + "</img>";
                            break;
                        default:
                            e.Row.Cells[i].Text = "<img src='img/unknown.png' style='vertical-align:middle;' height='15' width='15'>  " + dr["Name"].ToString() + "</img>";
                            break;
                    }
                }
            }
            //e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
            //  e.Row.Cells[0].Text = dr["Name"].ToString();

            if (dr.Table.Columns.Contains("ComputerId"))
            { 
                e.Row.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(this.form1, dr["ComputerId"].ToString()));
                e.Row.Attributes.Add("id", "system" + dr["ComputerId"].ToString());
                e.Row.Attributes.Add("class", "system" + dr["ComputerId"].ToString());
                e.Row.Attributes.Add("onmouseover", "hvrstart('" + "system" + dr["ComputerId"].ToString() + "')");
                e.Row.Attributes.Add("onmouseleave", "hvrleave('" + "system" + dr["ComputerId"].ToString() + "')");
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
    [WebMethod]
    public static string GetHostName(string name)
    {
        try { return Dns.GetHostEntry(name).HostName; } catch { return ""; }
    }

    [WebMethod]
    public static int GetOrders()
    {
        int active_orders = 0;
        int previous_orders = 0;
        try {
            // Get the highest order number from all previous orders
            String selectCommand = "SELECT value FROM CustomComputerValues WHERE CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE Name = 'LoanPreviousOrder') ORDER by value DESC LIMIT 1";
            SQLiteConnection sqConnection = new SQLiteConnection(CONNECTION);
            SQLiteCommand sqCommand = new SQLiteCommand(selectCommand, sqConnection);
            sqConnection.Open();
            SQLiteDataReader sqReader = sqCommand.ExecuteReader();

            while (sqReader.Read())
            {
                previous_orders = Int32.Parse(sqReader.GetString(0));
            }
            // Get the highest order number from all active orders
            selectCommand = "SELECT value FROM CustomComputerValues WHERE CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE Name = 'LoanOrder') ORDER by value DESC LIMIT 1";
            sqConnection = new SQLiteConnection(CONNECTION);
            sqCommand = new SQLiteCommand(selectCommand, sqConnection);
            sqConnection.Open();
            sqReader = sqCommand.ExecuteReader();

            while (sqReader.Read())
            {
                active_orders = Int32.Parse(sqReader.GetString(0));
            }
            // Compare the results and send back highest order number as result
            if (active_orders >= previous_orders)
                return active_orders;
            else
                return previous_orders;

        }
        catch 
        { return 99; }

    }

    [WebMethod]
    public static void LoanDevice(string name, string loan_user, string date, string order)
    {
        // Register device as loaned
        String selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, 1
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'IsLoaned'";

        SQLiteConnection sqConnection = new SQLiteConnection(CONNECTION);
        SQLiteCommand sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        SQLiteDataReader sqReader = sqCommand.ExecuteReader();
        
        // Register user loaning device
        selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, '" + loan_user + @"'
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'LoanUser'";

        sqConnection = new SQLiteConnection(CONNECTION);
        sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        sqReader = sqCommand.ExecuteReader();

        // Register due date
        selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, '" + date + @"'
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'LoanDueDate'";

        sqConnection = new SQLiteConnection(CONNECTION);
        sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        sqReader = sqCommand.ExecuteReader();

        //Register order number
        selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, '" + order + @"'
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'LoanOrder'";

        sqConnection = new SQLiteConnection(CONNECTION);
        sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        sqReader = sqCommand.ExecuteReader();

    }
    [WebMethod]
    public static void AddComputer(string name, string hostname, string credentialid)
    {
        // PDQ will not attempt to scan a device with no hostname, it will try to re-resolve it though if it is available later
        // Oddly, if no hostname is provided PDQ will also force the IP address to be the PDQ server itself, and will not try to re-resolve
        if (hostname == "")
            hostname = name;

        DateTime time = DateTime.Now;

        string format = "yyyy-MM-dd HH:mm:ss";
        String selectCommand = "INSERT INTO Computers (ScanUserId, Name, HostName, Added, Memory,OSServicePack,OSArchitecture,ProcessorCount,ProcessorCores,ProcessorSpeed,IPAddress,IsOnline,RequiresDotNet,NeedsReboot) values('" + credentialid + "','" + name + "','" + hostname + "', '" + time.ToString(format) + "', 0, 0, 0,0,0,0,' ',0,0,0)";

        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteConnection sqConnection = new SQLiteConnection(CONNECTION);
        SQLiteCommand sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        SQLiteDataReader sqReader = sqCommand.ExecuteReader();
    }

    [WebMethod]
    public static string[] GetOnLoan()
        {
        var computerList = new List<string>();

        try
        {
            // Get a list of all items not on loan
            String selectCommand = @"select name from computers where name NOT IN(
                            select Computers.Name
                            FROM  CustomComputerValues
                            INNER JOIN Computers
                            ON Computers.ComputerId = CustomComputerValues.ComputerId
                            WHERE CustomComputerValues.CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'IsLoaned')
                            AND CustomComputerValues.Value = 1)";
            SQLiteConnection sqConnection = new SQLiteConnection(CONNECTION);
            SQLiteCommand sqCommand = new SQLiteCommand(selectCommand, sqConnection);
            sqConnection.Open();
            SQLiteDataReader sqReader = sqCommand.ExecuteReader();

            while (sqReader.Read())
            {
                computerList.Add(sqReader.GetString(0));
            }
        }
        catch { }

        string[] onLoan = computerList.ToArray();
        return onLoan;
        }

    [WebMethod]
    public static string ReturnList()
    {

        // Get a list of all items not on loan
        String selectCommand = @"
                            select Computers.Name, Computers.ComputerId
                            FROM  CustomComputerValues
                            INNER JOIN Computers
                            ON Computers.ComputerId = CustomComputerValues.ComputerId
                            WHERE CustomComputerValues.CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'IsLoaned')
                            AND CustomComputerValues.Value = 1";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, CONNECTION);

        DataTable dt = new DataTable();

        dataAdapter.Fill(dt);
        dt.Columns.Add("LoanedTo", typeof(System.String));
        dt.Columns.Add("DueDate", typeof(System.String));
        dt.Columns.Add("LoanOrder", typeof(System.String));

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        // Above Query gets just the computerID and name for each system currently on loan, go into each row and get additional info
        foreach (DataRow dr in dt.Rows)
        {

            // Query Loan user for this row
            try {
                string compID = dr["ComputerID"].ToString();
                selectCommand = @"
                            SELECT Value
                            FROM CustomComputerValues
                            WHERE ComputerID = " + compID + @"
                            AND CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'LoanUser')
                            ";
                dataAdapter =
                        new SQLiteDataAdapter(selectCommand, CONNECTION);

                DataTable dt2 = new DataTable();
                dataAdapter.Fill(dt2);
                dr["LoanedTo"] = dt2.Rows[0]["Value"].ToString();
            } catch
            {

            }

            // Query due date for this row
            try
            {
                string compID = dr["ComputerID"].ToString();
                selectCommand = @"
                            SELECT Value
                            FROM CustomComputerValues
                            WHERE ComputerID = " + compID + @"
                            AND CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'LoanDueDate')
                            ";
                dataAdapter =
                        new SQLiteDataAdapter(selectCommand, CONNECTION);

                DataTable dt3 = new DataTable();
                dataAdapter.Fill(dt3);
                dr["DueDate"] = dt3.Rows[0]["Value"].ToString();
            }
            catch
            {

            }

            try
            {
                string compID = dr["ComputerID"].ToString();
                selectCommand = @"
                            SELECT Value
                            FROM CustomComputerValues
                            WHERE ComputerID = " + compID + @"
                            AND CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'LoanOrder')
                            ";
                dataAdapter =
                        new SQLiteDataAdapter(selectCommand, CONNECTION);

                DataTable dt4 = new DataTable();
                dataAdapter.Fill(dt4);
                dr["LoanOrder"] = dt4.Rows[0]["Value"].ToString();
            }
            catch
            {

            }


            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(rows);

    }

    [WebMethod]
    public static void ReturnDevice(string name, string order)
    {
        // Mark Loan user as available
        String selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, 'Available'
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'LoanUser'";

        SQLiteConnection sqConnection = new SQLiteConnection(CONNECTION);
        SQLiteCommand sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        SQLiteDataReader sqReader = sqCommand.ExecuteReader();

        // Register previous order number
        selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, '" + order + @"'
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'LoanPreviousOrder'";

        sqConnection = new SQLiteConnection(CONNECTION);
        sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        sqReader = sqCommand.ExecuteReader();

        // Unregister Device as loaned
        selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, 0
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'IsLoaned'";

        sqConnection = new SQLiteConnection(CONNECTION);
        sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        sqReader = sqCommand.ExecuteReader();

        // Null order number
        selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, NULL
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'LoanOrder'";

        sqConnection = new SQLiteConnection(CONNECTION);
        sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        sqReader = sqCommand.ExecuteReader();

        // Null due date
        selectCommand = @"INSERT or REPLACE INTO CustomComputerValues (ComputerId, CustomComputerItemId, Value)
                                SELECT Computers.ComputerId, CustomComputerItems.CustomComputerItemId, NULL
                                FROM Computers, CustomComputerItems
                                WHERE Computers.Name = '" + name + @"'
                                AND CustomComputerItems.Name = 'LoanDueDate'";

        sqConnection = new SQLiteConnection(CONNECTION);
        sqCommand = new SQLiteCommand(selectCommand, sqConnection);
        sqConnection.Open();
        sqReader = sqCommand.ExecuteReader();
    }

    [WebMethod]
    public static string AllDevices()
    {
        // Get a list of all items not on loan
        String selectCommand = @"
                            select Computers.Name, Computers.ComputerId
                            FROM  CustomComputerValues
                            INNER JOIN Computers
                            ON Computers.ComputerId = CustomComputerValues.ComputerId
                            WHERE CustomComputerValues.CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'IsLoaned')
                            AND CustomComputerValues.Value = 1";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, CONNECTION);

        DataTable dt = new DataTable();

        dataAdapter.Fill(dt);
        dt.Columns.Add("LoanedTo", typeof(System.String));
        dt.Columns.Add("DueDate", typeof(System.String));
        dt.Columns.Add("LoanOrder", typeof(System.String));

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        // Above Query gets just the computerID and name for each system currently on loan, go into each row and get additional info
        foreach (DataRow dr in dt.Rows)
        {

            // Query Loan user for this row
            try
            {
                string compID = dr["ComputerID"].ToString();
                selectCommand = @"
                            SELECT Value
                            FROM CustomComputerValues
                            WHERE ComputerID = " + compID + @"
                            AND CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'LoanUser')
                            ";
                dataAdapter =
                        new SQLiteDataAdapter(selectCommand, CONNECTION);

                DataTable dt2 = new DataTable();
                dataAdapter.Fill(dt2);
                dr["LoanedTo"] = dt2.Rows[0]["Value"].ToString();
            }
            catch
            {

            }

            // Query due date for this row
            try
            {
                string compID = dr["ComputerID"].ToString();
                selectCommand = @"
                            SELECT Value
                            FROM CustomComputerValues
                            WHERE ComputerID = " + compID + @"
                            AND CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'LoanDueDate')
                            ";
                dataAdapter =
                        new SQLiteDataAdapter(selectCommand, CONNECTION);

                DataTable dt3 = new DataTable();
                dataAdapter.Fill(dt3);
                dr["DueDate"] = dt3.Rows[0]["Value"].ToString();
            }
            catch
            {

            }

            try
            {
                string compID = dr["ComputerID"].ToString();
                selectCommand = @"
                            SELECT Value
                            FROM CustomComputerValues
                            WHERE ComputerID = " + compID + @"
                            AND CustomComputerItemId = (SELECT CustomComputerItemId FROM CustomComputerItems WHERE name = 'LoanOrder')
                            ";
                dataAdapter =
                        new SQLiteDataAdapter(selectCommand, CONNECTION);

                DataTable dt4 = new DataTable();
                dataAdapter.Fill(dt4);
                dr["LoanOrder"] = dt4.Rows[0]["Value"].ToString();
            }
            catch
            {

            }


            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(rows);
        return "";
    }
}