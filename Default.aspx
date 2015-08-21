<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<style>
    #document {
        display: none;
    }

    body, html {
        font-family: Helvetica,sans-serif;
        font-size: 14px;
        line-height: 1.45em;
        color: #3A3A3A;
        background-color: #EFEFEF;
    }

    .myGrid {
        background-color: #fff;
        border: solid 1px #525252;
        border-collapse: collapse;
    }

        .myGrid td {
            padding: 2px;
            border: solid 1px #c1c1c1;
            white-space: nowrap;
        }

        .myGrid th {
            padding: 4px 2px;
            color: #fff;
            background-color: #8F9393;
            border-left: solid 1px #525252;
            font-size: 0.9em;
        }

    #comp_dialog {
        font-family: Helvetica,sans-serif;
        font-size: 14px;
        line-height: 1.45em;
    }

    .imtip {
        position: absolute;
        bottom: 0;
        right: 0;
    }

    a:link {
        color: #000;
    }

    /* visited link */
    a:visited {
        color: #ff6a00;
    }

    /* mouse over link */
    a:hover {
        color: #000;
    }

    /* selected link */
    a:active {
        color: #0000FF;
    }

    th {
        padding: 4px 2px;
        border: solid 1px #c1c1c1;
        font-size: 0.9em;
        cursor: pointer;
    }

        th:hover {
            background-color: #717171;
        }

    tr:nth-child(even) {
        background-color: #EFEFEF;
    }


    /* Get Text Under navbar link pictures*/

    .navlinks {
        font-family: Tahoma;
        font-size: 10pt;
        line-height: 13px;
        width: 64px;
        padding-bottom: 2px;
        padding-top: 2px;
        line-height: 13px;
        text-align: center;
        height: 82px;
    }

        .navlinks img {
            display: block;
            margin: 0 auto;
            padding-bottom: 2px;
            padding-top: 2px;
        }

        .navlinks p {
            margin: 0px 0px 0px;
        }


    #FCol {
        width: auto;
        overflow-y: hidden;
        overflow-x: scroll;
    }

    .tree td img {
        vertical-align: bottom;
    }

    TD {
        font-size: 12px;
    }
</style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="/css/bootstrap.css" media="screen">
    <link rel="stylesheet" href="/scripts/jquery-ui.css">
    <script src="/scripts/jquery-1.11.3.js"></script>
    <script src="/scripts/jquery-ui.js"></script>
    <script>
    $(document).ready(function () {
        // Clone our gridview first
                    var tab = $("#<%=GridView1.ClientID%>").clone(true);
                    // clone again for freeze
                    var tabFreeze = $("#<%=GridView1.ClientID%>").clone(true);
 
                    // set width (for scroll)
                    var totalWidth = $("#<%=GridView1.ClientID%>").outerWidth();
                    var firstColWidth = $("#<%=GridView1.ClientID%> th:first-child").outerWidth();
                    tabFreeze.width(firstColWidth);
                    tab.width(totalWidth - firstColWidth);
 
                    // here make 2 table 1 for freeze column 2 for all remain column
 
                    tabFreeze.find("th:gt(0)").remove();
                    tabFreeze.find("td:not(:first-child)").remove();
 
                    tab.find("th:first-child").remove();
                    tab.find("td:first-child").remove();
 
                    // create a container for these 2 table and make 2nd table scrollable
            
                    var container = $('<table border="0" cellpadding="0" cellspacing="0"><tr><td valign="top"><div id="FCol" style="border:solid 1px #525252;border-right:none;"></div></td><td valign="top"><div id="Col" style="border:solid 1px #525252;overflow:auto"></div></td></tr></table)');
                       $("#FCol", container).html($(tabFreeze));
                    $("#Col", container).html($(tab));
 
                    // clear all html
                    $("#gridContainer").html('');
                    $("#gridContainer").append(container);


                    $('#Col').on('scroll', function () {
                        $('#FCol').scrollTop($(this).scrollTop());
                        $("#h01").html($(this).scrollTop());
                    });
                    $('#Col').height($(window).height() - 150);
                    $('#FCol').height($(window).height() - 150);
                    $('#Col').width($(window).width() - 600);

                    $('#TreeView1').height($(window).height() - 100);

                     //Give the second GridView a unique ID

                    document.getElementById('GridView1').id = "GridView2";
                    $("#document").show();


        // Begin CSV Parse Script
        // The event listener for the file upload
                    document.getElementById('browse').addEventListener('change', upload, false);

        // Method that checks that the browser supports the HTML5 File API
                    function browserSupportFileUpload() {
                        var isCompatible = false;
                        if (window.File && window.FileReader && window.FileList && window.Blob) {
                            isCompatible = true;
                        }
                        return isCompatible;
                    }

        // Method that reads and processes the selected file
                    function upload(evt) {
                        if (!browserSupportFileUpload()) {
                            alert('The File APIs are not fully supported in this browser!');
                        } else {
                            var data = null;
                            var file = evt.target.files[0];
                            var reader = new FileReader();
                            reader.readAsText(file);
                            reader.onload = function (event) {
                                var csvData = event.target.result;
                                strDelimiter = (",");

                                // Create a regular expression to parse the CSV values.
                                var objPattern = new RegExp(
                                    (
                                        // Delimiters.
                                        "(\\" + strDelimiter + "|\\r?\\n|\\r|^)" +

                                        // Quoted fields.
                                        "(?:\"([^\"]*(?:\"\"[^\"]*)*)\"|" +

                                        // Standard fields.
                                        "([^\"\\" + strDelimiter + "\\r\\n]*))"
                                    ),
                                    "gi"
                                    );


                                // Create an array to hold our data. Give the array
                                // a default empty first row.
                                var data = [[]];

                                // Create an array to hold our individual pattern
                                // matching groups.
                                var arrMatches = null;


                                // Keep looping over the regular expression matches
                                // until we can no longer find a match.
                                while (arrMatches = objPattern.exec(csvData)) {

                                    // Get the delimiter that was found.
                                    var strMatchedDelimiter = arrMatches[1];

                                    // Check to see if the given delimiter has a length
                                    // (is not the start of string) and if it matches
                                    // field delimiter. If id does not, then we know
                                    // that this delimiter is a row delimiter.
                                    if (
                                        strMatchedDelimiter.length &&
                                        strMatchedDelimiter !== strDelimiter
                                        ) {

                                        // Since we have reached a new row of data,
                                        // add an empty row to our data array.
                                        data.push([]);

                                    }

                                    var strMatchedValue;

                                    // Now that we have our delimiter out of the way,
                                    // let's check to see which kind of value we
                                    // captured (quoted or unquoted).
                                    if (arrMatches[2]) {

                                        // We found a quoted value. When we capture
                                        // this value, unescape any double quotes.
                                        strMatchedValue = arrMatches[2].replace(
                                            new RegExp("\"\"", "g"),
                                            "\""
                                            );

                                    } else {

                                        // We found a non-quoted value.
                                        strMatchedValue = arrMatches[3];

                                    }


                                    // Now that we have our value string, let's add
                                    // it to the data array.
                                    data[data.length - 1].push(strMatchedValue);
                                }

                                // Return the parsed data.
                                var name = "";
                                if (data && data.length > 0) {
                                    alert('Imported -' + data.length + '- rows successfully!');
                                    

                                    for (var i = 0; i < data.length; i++)
                                    {
                                        addClick(data[i]);
                                        var name = data[i];
                                    }

                                } else {
                                    alert('No data to import!');
                                }
                                getDNS(name);
                            };
                            reader.onerror = function () {
                                alert('Unable to read ' + file.fileName);
                            };
                        }
                    }
    });

        // Cause HTML5 doesn't let us style file input control, we make a fake one.
        function HandleBrowseClick() {
            var fileinput = document.getElementById("browse");
            fileinput.click();
        }
        function Handlechange() {
            var fileinput = document.getElementById("browse");
            var textinput = document.getElementById("filename");
            textinput.value = fileinput.value;
        }
        // Row select hover functions:
        function hvrstart(sender) {
            $('.' + sender).css("background-color", "#8F9393");

        }
        function hvrleave(sender) {
            $('.' + sender).css("background-color", "");
        }

    </script>
    <link rel="stylesheet" href="//cdn.datatables.net/1.10.7/css/jquery.dataTables.min.css" />
    <script src="//cdn.datatables.net/1.10.7/js/jquery.dataTables.min.js"></script>
</head>
<body>
    <ul class="nav navbar-nav" style="float: none; border-bottom: 1px solid black">
        <li><a class="navlinks" style="border-right: 1px solid #3A3A3A; height: 82px" runat="server" href="~/">
            <img src="img/scans.png" /><p style="padding-top: 4px">Scan</p>
        </a></li>

        <li><a class="navlinks" style="width: 85px" runat="server" href="~/">
            <img src="img/dyn_coll.png" /><p>New Dynamic Collection</p>
        </a></li>
        <li><a class="navlinks" style="width: 85px" runat="server" href="~/">
            <img src="img/stat_coll.png" /><p>New Static Collection</p>
        </a></li>
        <li>
            <button class="navlinks" onclick="add_dev()" id="add_dev" style="width: 85px" runat="server">
                <img src="img/add_comp.png" /><p>Add Devices</p>
            </button>
        </li>
        <li><a class="navlinks" style="width: 85px; border-right: 1px solid #3A3A3A; line-height: 1px; height: 82px" runat="server" href="~/">
            <img src="img/new_rep.png" /><p>New Report</p>
        </a></li>
        <li>
            <button class="navlinks" onclick="loan_dev()" id="Button1" style="width: 85px" runat="server">
                <img src="img/67.png" /><p>Loan Device</p>
            </button>
        </li>

    </ul>
    <div id="document">
        <form id="form1" runat="server">
            <div style="border: 1px solid #000; display: inline-block; margin: 5px 2px 0px 2px">
                <asp:TreeView ID="TreeView1" Style="display: inline-block; overflow-y: scroll" ImageSet="Arrows" runat="server">
                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                    <Nodes>
                    </Nodes>
                    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                    <ParentNodeStyle Font-Bold="False" />
                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                </asp:TreeView>
            </div>
            <div id="gridContainer" style="display: inline-block; position: fixed; margin-top: 50px; margin-left: 10px;">
                <asp:GridView CssClass="myGrid" ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" AllowSorting="True" OnSorting="GridView1_Sorting" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" />

            </div>
            <div id="dialog" title="Add Devices" style="width: 500px; height: 500px;">
                <p id="comp_dialog" style="margin: 5px 5px 5px 0px">Use this window to add devices to your PDQ database.  Devices can be added by entering their names, importing a text file, or browsing Active Directory.</p>
                <p id="comp_dialog" style="margin: 5px 5px 5px 0px">Computer Name - type a computer name and press Enter</p>

                <input type="text" id="input_name" style="margin: 5px 5px 5px 0px; width: 550px" maxlength="255">
                <button id="get_hostname" onclick="getDNS($(document.getElementById('input_name'))[0].value)">Add</button>
                <div>
                    <label style="margin: 5px 5px 5px 0px">Scan User</label>

                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                </div>
                <div>
                    <div style="border: 1px solid; width: 602px; height: 260px; float: left;">
                        <table id="add_table" class="display compact nowrap" cellspacing="0" width="600px">
                            <thead>
                                <tr>
                                    <th>Device</th>
                                    <th>Hostname</th>
                                    <th>Scan User</th>
                                    <th>rHostName</th>
                                    <th>CredentialName</th>
                                    <th>CredentialID</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <input type="file" id="browse" name="fileupload" accept=".csv,.txt" style="display: none" onchange="Handlechange();" />
                    <input type="text" id="filename" style="display: none;" readonly="true" />
                    <input type="button" value="Import" id="fakeBrowse" style="margin: 5px 3px 5px 3px; width: 100px;" onclick="HandleBrowseClick();" />
                    <button style="margin: 5px 3px 5px 3px; width: 100px" id="rem_row">Remove</button>
                </div>
            </div>
            <div id="loan_dialog" title="Loan Devices" style="width: 500px; height: 500px;">
                <p id="comp_dialog" style="margin: 5px 5px 5px 0px">Use this window to loan devices.  You can search for devices in the database by typing their name or asset tag.</p>
                <p id="comp_dialog" style="margin: 5px 5px 5px 0px">Computer Name - type a computer name and press Enter</p>
                <div class="ui-widget">
  <input id="tags" style="margin: 5px 5px 5px 0px; width: 550px" maxlength="255"><button id="add_loan" onclick="loanClick($(document.getElementById('tags'))[0].value)">Add</button>
</div>

                <div>
                    <asp:DropDownList ID="DropDownList2" runat="server" style="display:none"></asp:DropDownList>
                    <label style="margin: 5px 5px 5px 0px">Loan To: </label>

                <input type="text" id="loan_user" style="margin: 5px 5px 5px 0px; width:auto" maxlength="255">
                        <label style="margin: 5px 5px 5px 0px">Due Date: </label><input type="text" id="datepicker">
                </div>
                <div>
                    <div style="border: 1px solid; width: 602px; height: 260px; float: left;">
                        <table id="loan_table" class="display compact nowrap" cellspacing="0" width="600px">
                            <thead>
                                <tr>
                                    <th>Device</th>
                                    <th>Loaned To</th>
                                    <th>Due Date</th>                                   
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <button style="margin: 5px 3px 5px 3px; width: 100px" id="rem_loan">Remove</button>
                </div>
            </div>
            <asp:Literal runat="server" ID="txtValueA" EnableViewState="false">txtTest</asp:Literal>
        </form>
    </div>
</body>
</html>
<script>

    $(document).ready(function () {
        
        // Enter key submit for loan device input text box
        $("#tags").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#add_loan").click();
            }
        });
        // Enter key submit for add device input text box
        $("#input_name").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#get_hostname").click();
            }
        });

        
        // Add Datepicker for loan device
        $(function () {
            $("#datepicker").datepicker();
        });

        // Create table for adding devices
        var table = $('#add_table').DataTable(
        {
            "sScrollX": "100%",
            "sScrollY": "230px",
            "paging": false,
            "info": false,
            "bFilter": false,
            "table": "compact",
            "language": {
                "emptyTable": " "
            },
            "columnDefs": [
    {
        // hide the third column since this contains the credentialID we'll submit to the server later
        "targets": [3, 4, 5],
        "visible": false,
    }
            ]
        });
        // Make rows selectable
        $('#add_table tbody').on('click', 'tr', function () {
            $(this).toggleClass('selected');
        });
        // Make rows removable
        $('#rem_row').click(function () {
            table.row('.selected').remove().draw(false);
        });


    // Create table for loaning devices
        var loan_table = $('#loan_table').DataTable(
        {
            "sScrollX": "100%",
            "sScrollY": "230px",
            "paging": false,
            "info": false,
            "bFilter": false,
            "table": "compact",
            "language": {
                "emptyTable": " "
            },
        });
        // Make rows selectable
        $('#loan_table tbody').on('click', 'tr', function () {
            $(this).toggleClass('selected');
        });
        // Make rows removable
        $('#rem_loan').click(function () {
            loan_table.row('.selected').remove().draw(false);
        });
    });

    // Create Jquery UI Dialog for Loan Device
$("#loan_dialog").dialog({
    autoOpen: false,
    resizable: false,
    width: 750,
    height: 535,

    modal: true,
    buttons: {
        "Submit": function () {        
            $(this).dialog("close");

            var table = document.getElementById("loan_table");
            oTable = $('#loan_table').dataTable();
            for (var i = 0, row; row = table.rows[i]; i++) {
                var dName = oTable.fnGetData(i, 0);
                if (dName) {
                    $.ajax({
                        type: "POST",
                        url: "Default.aspx/LoanDevice",
                        data: '{name: "' + dName + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        failure: function (response) {
                            alert(response.d);
                        }

                    });
                }
            }
        }
    },
    close: function () {
    }
});

    
    // Create Jquery UI Dialog for add device
    
    $("#dialog").dialog({
        autoOpen: false,
        resizable: false,
    width: 750,
    height: 535,
    
    modal: true,
    buttons: {
        "OK": function () {

            var table = document.getElementById("add_table");
            oTable = $('#add_table').dataTable();
            for (var i = 0, row; row = table.rows[i]; i++) {
                var dName = oTable.fnGetData(i, 0);
                var hName = oTable.fnGetData(i, 3);
                var credId = oTable.fnGetData(i, 5);
                if (credId == -1)
                {
                    credId = "NULL";
                }
                if (dName) {
                    $.ajax({
                        type: "POST",
                        url: "Default.aspx/AddComputer",
                        data: '{name: "' + dName + '", hostname: "' + hName + '", credentialid: "' + credId + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        failure: function (response) {
                            alert(response.d);
                        }

                    });
                }
            }


            
            $(this).dialog("close");
        },
        Cancel: function() {
            $(this).dialog("close");
        }
    },
    close: function() {
    }
    });
    
    // Open Jquery UI DIalog for add device
    function add_dev()
    {
        $("#dialog").dialog("open");
    }
    function loan_dev()
    {
        // Create an array of systems currently on loan
        var loaned = new Array();
        var dd2 = document.getElementById('DropDownList2');
        for (d = 1; d < dd2.options.length; d++) {
            loaned[d] = dd2.options[d].value;
        }

        var i = $('#GridView1 tr th a').filter(
        function () {
            return $(this).text() == 'Name';
        }).parent().prevAll().length;

        // Create an array of all computer names
        var colArray = $("#GridView1 tr td:nth-child(" + (i + 1) + ")").map(function () {
            // If name is currently in our loaned array, we exclude it
            if ($.inArray($(this).text().trim(), loaned) === -1) {
                return $(this).text().trim();
            } 
        }).get();

        $("#tags").autocomplete({
            source: colArray,
            select: function (e, ui) {
                $(this).next().val(ui.item.id);
            },
            change: function (ev, ui) {
                if (!ui.item)
                    $(this).val("");
            }
        });

        $("#loan_dialog").dialog("open");
    }
    // Send Jquery request with add device to fetch hostname from server
    function loanClick(theName)
    {
        var required = 0;
        if ($('#loan_user').val() == '') {
            required = 1;
            $('#loan_user').css('border-color', 'red');
        }
        else {
            $('#loan_user').css('border-color', '');
        }
        if ($('#datepicker').val() == '') {
            required = 1;
            $('#datepicker').css('border-color', 'red');
        }
        else {
            $('#datepicker').css('border-color', '');
        }
        if ($('#tags').val() == '') {
            required = 1;
            $('#tags').css('border-color', 'red');
        }
        else {
            $('#tags').css('border-color', '');
        }
        // All required fields populated, add values to table

        var table = document.getElementById("loan_table");
        oTable = $('#loan_table').dataTable();

        for (var i = 0, row; row = table.rows[i]; i++) {
            var data = oTable.fnGetData(i, 0)
            if (data) {
                if (typeof data === 'number') {
                    data = "" + data;
                }
                if (data.toUpperCase() === $('#tags').val() || data === "")
                    required = 1;
            }
        }
        if (required != 1)
        {
            var table = $('#loan_table').DataTable();
            table.row.add([
                $('#tags').val(),
                $('#loan_user').val(),
                $('#datepicker').val()
            ]).draw();
        }

    }

    function addClick(theName) {
        theName = "" + theName;
        var chunk = theName.split(".");
        var dName = chunk[0].toUpperCase() + "";
        var t = document.getElementById("DropDownList1");
        var selectedText = t.options[t.selectedIndex].text;
        var selectedValue = t.options[t.selectedIndex].value;
        var placeHolder = "<img src='img/ajax-loader.gif' style='vertical-align:middle;' height='15' width='15'><img src='img/warning.png' style='vertical-align:middle;' height='15' width='15'></img>";
        var x = 0;
        // Verify this device is not already in the add_table
        var table = document.getElementById("add_table");
        oTable = $('#add_table').dataTable();

        for (var i = 0, row; row = table.rows[i]; i++) {
            var data = oTable.fnGetData(i, 0)
            if (data) {
                if (typeof data === 'number') {
                    data = "" + data;
                }
                if (data.toUpperCase() === dName || data === "")
                    x = 1;
            }
        }

        // Verify this device is not in the database
        // Get the index of the Name column
        var i = $('#GridView1 tr th a').filter(
        function () {
            return $(this).text() == 'Name';
        }).parent().prevAll().length;

        // Check if the name is duplicate
        $("#GridView1 tr td:nth-child(" + (i + 1) + ")").each(function () {
            // Need those extra spaces because the name column includes spaces to seperate name from icon

            if ($(this).text().toUpperCase() == "  " + dName) {
                placeHolder = "<img src='img/warning.png' style='vertical-align:middle;' height='15' width='15'> Duplicate computer name: " + theName + "</img>";

            }
        });

        // Name not found in add_table or database, add to table
        if (x === 0) {
            // Create the extra row
            var table = $('#add_table').DataTable();
            table.row.add([
                dName,
                placeHolder,
                selectedText,
                "",
                selectedText,
                selectedValue
            ]).draw();
            }
    };

    function getDNS(dName) {
        if (dName != "") {
            addClick(dName);
            $.ajax({
                type: "POST",
                url: "Default.aspx/GetHostName",
                data: '{name: "' + dName + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    OnSuccess(response, dName);
                },
                failure: function (response) {
                    alert(response.d);
                },
                complete: function () {
                    // Because our Ajax requests fail when we send to many, check if there are any HostName cells unchecked, then resend a single request.
                    var table = document.getElementById("add_table");
                    oTable = $('#add_table').dataTable();
                    for (var i = 0, row; row = table.rows[i]; i++) {
                        if (oTable.fnGetData(i, 1) === "<img src='img/ajax-loader.gif' style='vertical-align:middle;' height='15' width='15'><img src='img/warning.png' style='vertical-align:middle;' height='15' width='15'></img>") {
                            getDNS(oTable.fnGetData(i, 0));
                            break;
                        }
                    }
                }

            });
        }
        function OnSuccess(response, dName) {
            // Add server response for hostname into datatable
            var table = document.getElementById("add_table");
            oTable = $('#add_table').dataTable();
            for (var i = 0, row; row = table.rows[i]; i++) {
                if (dName === oTable.fnGetData(i, 0)) {
                    // Ignore adding DNS for those with error condition
                    if (oTable.fnGetData(i, 1) === "<img src='img/ajax-loader.gif' style='vertical-align:middle;' height='15' width='15'><img src='img/warning.png' style='vertical-align:middle;' height='15' width='15'></img>")
                    {
                        if (response.d === "") {
                            oTable.fnUpdate("<img src='img/warning.png' style='vertical-align:middle;' height='15' width='15'> " + dName + " cannot be found</img>", i, 1);
                        } else {
                            oTable.fnUpdate("<img src='img/ok.png' style='vertical-align:middle;' height='15' width='15'> " + response.d + "</img>", i, 1);
                            oTable.fnUpdate(response.d, i, 3);
                        }
                    }
                }
            }
        }
    }
</script>

