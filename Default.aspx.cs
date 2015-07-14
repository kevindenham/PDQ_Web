using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{

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
               // txtValueA.Text = Request["__EVENTARGUMENT"];
               // txtValueA.Text = Request["__EVENTTARGET"];
                GridView_System(Request["__EVENTARGUMENT"]);

                
                Response.Redirect("/computer.aspx?computer=" + Request["__EVENTARGUMENT"]);
            }

            else
            {

                GridView_All();
            }
            string[] split = Request["__EVENTARGUMENT"].Split(new Char[] { '-' });
            txtValueA.Text = split[split.Count() - 1];
        }

    }

    protected void GridView_System(string system)
    {
        String connectionString = "Data Source=C:\\programdata\\Admin Arsenal\\Database.db";
        //String connectionString = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        String selectCommand = "select ComputerId, Name, HostName, IsOnline from computers WHERE Name = '" + system + "' order by Name asc";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, connectionString);

        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);

        dt.Columns["IsOnline"].ColumnName = "Online";
        GridView1.DataSource = dt;
        // txtValueA.Text = dt.Rows[0][0].ToString();
        GridView1.DataBind();
    }
    protected void GridView_All()
    {
        String connectionString = "Data Source=C:\\programdata\\Admin Arsenal\\Database.db";
        //String connectionString = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        String selectCommand = "select * from computers order by Name asc";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                    new SQLiteDataAdapter(selectCommand, connectionString);

        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);

        dt.Columns["IsOnline"].ColumnName = "Online";
        GridView1.DataSource = dt;
       // txtValueA.Text = dt.Rows[0][0].ToString();
        GridView1.DataBind();

    }
    protected void Populate_TreeView1()
    {
        // Populate TreeView
        String connectionString = "Data Source=C:\\programdata\\Admin Arsenal\\Database.db";
        //String connectionString = "Data Source=\\\\10.198.102.88\\c$\\ProgramData\\Admin Arsenal\\PDQ Inventory\\Database.db";
        String selectCommand = "select * from collections order by Name asc";
        //        String selectCommand = "select Name, IsOnline, Chassis from computers order by computerid desc";
        SQLiteDataAdapter dataAdapter =
                   new SQLiteDataAdapter(selectCommand, connectionString);
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
                e.Row.Cells[i].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.GridView1, "Sort$" + LnkHeaderText.Text);

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


            e.Row.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(this.form1, dr["ComputerId"].ToString()));
            e.Row.Attributes.Add("class", "system" + dr["ComputerId"].ToString());
            e.Row.Attributes.Add("onmouseover", "hvrstart('" + "system" + dr["ComputerId"].ToString() + "')");
            e.Row.Attributes.Add("onmouseleave", "hvrleave('" + "system" + dr["ComputerId"].ToString() + "')");
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